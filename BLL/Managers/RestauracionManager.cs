using Ferpuser.BLL.Helpers;
using Ferpuser.BLL.Services;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class RestauracionManager
    {
        public ApplicationDbContext db { get; set; }
        
        public RestauracionManager(ApplicationDbContext db)
        {
            this.db = db;
        }

        public Registration GetInscripcion(string nif, Guid CongressId)
        {
            //Quitar los espacios y pasarlo a minúsculas
            nif = nif.Replace(" ", "").ToUpper().Trim();

            //Buscar el cliente por nif            
            Registration inscripcion = db.Registrations.AsNoTracking()
                .Include(f => f.Registrant)
                .Include(f => f.Congress)
                .FirstOrDefault(f => f.CongressId == CongressId && f.Registrant.NIF.Trim().ToUpper() == nif);

            return inscripcion;
        }

        public IEnumerable<Encuentro> GetEncuentrosActivos(Guid CongressId)
        {
            return db.Encuentros.AsNoTracking().Include(f => f.Congress)
                .Where(f =>
                    (f.CongressId == CongressId) &&
                    (f.Fecha >= DateTime.Now)
                );
        }

        public async Task CreateReservas(string[] NIFs, int EncuentroId)
        {
            DateTime Now = DateTime.Now;

            Encuentro encuentro = db.Encuentros.AsNoTracking().SingleOrDefault(f => f.Id == EncuentroId);
            using (var transaction = db.Database.BeginTransaction())
            {
                foreach (var item in NIFs)
                {
                    //Quitar los espacios y pasarlo a minúsculas
                    string NIF = item.Replace(" ", "").ToUpper().Trim();
                    Registration Inscripcion = GetInscripcion(NIF, encuentro.CongressId);

                    int? NumeroMesa = ObtenerNumeroMesaDisponible(encuentro.Id);
                    Restauracion restauracion = new Restauracion()
                    {
                        EncuentroId = EncuentroId,
                        FechaHoraReserva = Now,
                        NIF = NIF,
                        NumeroMesa = NumeroMesa.Value,
                        RegistrationId = Inscripcion.Id
                    };
                    db.Restauraciones.Add(restauracion);
                    encuentro.Libres = encuentro.Libres - 1;
                    encuentro.Reservados = encuentro.Reservados + 1;
                }

                await db.SaveChangesAsync();
                transaction.Commit();
            }            
        }


        public async Task DeleteReserva(Guid InscripcionId, int EncuentroId)
        {   
            Encuentro encuentro = await db.Encuentros.FirstAsync(f => f.Id == EncuentroId);
            Restauracion restauracion = await db.Restauraciones.FirstAsync(f => f.EncuentroId == EncuentroId && f.RegistrationId == InscripcionId);
            encuentro.Libres = encuentro.Libres + 1;
            encuentro.Reservados = encuentro.Reservados - 1;
            db.Restauraciones.Remove(restauracion);
            db.SaveChanges();
        }
        
        /// <summary>
        /// Obtiene el primer número de mesa libre disponible y si no hay devuelve null
        /// </summary>
        /// <param name="EncuentroId"></param>
        /// <returns></returns>
        public int? ObtenerNumeroMesaDisponible(int EncuentroId)
        {
            int? resultado = null;

            Encuentro encuentro = db.Encuentros.AsNoTracking().FirstOrDefault(f => f.Id == EncuentroId);
            var reservas = db.Restauraciones.Where(f => f.EncuentroId == EncuentroId);

            for (int i = 1; i <= encuentro.NumeroMesas; i++)
            {
                int ocupadas = reservas.Count(f => f.NumeroMesa == i);
                if (ocupadas < encuentro.ComensalesMesa)
                {
                    resultado = i;
                    break;
                }
            }

            return resultado;
        }

        public IEnumerable<string> EnviarMailReservas(Guid CongressId, string[] NIFs, ControllerContext controller_context, AppSettings appSettings)
        {
            List<string> NIFsErroresEnvio = new List<string>();
            foreach (var nif in NIFs)
            {
                var inscripcion = GetInscripcion(nif, CongressId);
                if (!EnviarMailReservas(CongressId, inscripcion.Id, controller_context, appSettings))
                    NIFsErroresEnvio.Add(nif);
            }
            return NIFsErroresEnvio;
        }

        public bool EnviarMailReservas(Guid CongressId, Guid InscripcionId, ControllerContext controller_context, AppSettings appSettings)
        {
            Congress congress = db.Congresses.AsNoTracking().Single(f => f.Id == CongressId);
            Registration inscripcion = db.Registrations.Include(f => f.Registrant.Treatment).AsNoTracking().Single(f => f.Id == InscripcionId);
            IEnumerable<int> EncuentrosReservados = db.Restauraciones.Where(f => 
                f.RegistrationId == InscripcionId && 
                f.Encuentro.CongressId == CongressId)
                .Select(f => f.EncuentroId).ToList();

            //Enviar el mail con el código QR para cada una de las inscripciones            
            try
            {
                SmtpConfig smtpConfig = new SmtpConfig()
                {
                    SmtpPassword = congress.MailPassword,
                    SmtpPort = congress.OutgoingMailPort,
                    SmtpServer = congress.OutgoingMailServer,
                    SmtpUser = congress.MailUser
                };
                EmailService service = new EmailService(smtpConfig);

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Path.Combine("HtmlTemplates", "Reserva.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("#NOMBRE_EVENTO#", congress.Name);
                body = body.Replace("#TRATAMIENTO_INSCRITO#", inscripcion.Registrant.Treatment.Name);
                body = body.Replace("#NOMBRE_INSCRITO#", inscripcion.Registrant.Name);
                body = body.Replace("#APELLIDOS_INSCRITO#", inscripcion.Registrant.Surnames);

                var encuentros = db.Encuentros.AsNoTracking().Include(f => f.Congress).Where(f => EncuentrosReservados.Contains(f.Id));
                string info_encuentros = string.Empty;
                foreach (var encuentro in encuentros)
                {
                    var restauraciones = db.Restauraciones.Where(f => f.EncuentroId == encuentro.Id);
                    var restauracion = restauraciones.FirstOrDefault(f => f.RegistrationId == inscripcion.Id);

                    var acompaniantes = db.Registrations.Include(f => f.Registrant.Treatment).Where(f => restauraciones.Select(g => g.RegistrationId).Contains(f.Id));
                    string sAcompaniantes = string.Empty;
                    foreach (var acompaniante in acompaniantes)
                    {
                        if (acompaniante.Id == restauracion.RegistrationId) //Si es uno mismo no se pone como acompañante
                            continue;
                        sAcompaniantes += $"{acompaniante.Registrant.Treatment.Name} {acompaniante.Registrant.Name} {acompaniante.Registrant.Surnames}<br/>";
                    }
                    if (string.IsNullOrWhiteSpace(sAcompaniantes))
                        sAcompaniantes = "De momento usted es el primer inscrito en esta mesa.";

                    string mesa = string.Empty;
                    if (encuentro.ReservaMesa)
                        mesa = $"<b>Mesa:</b> {restauracion.NumeroMesa}<br />";

                    info_encuentros += $"<p>" +
                        $"<b>{encuentro.Nombre}</b><br />" +
                        $"<b>Fecha:</b> {encuentro.Fecha.ToString("dd/MM/yyyy HH:mm")}<br />" +
                        $"<b>Lugar:</b> {encuentro.Lugar}<br />" +
                        mesa +
                        $"<b>Acompañantes:</b><br />" +
                        sAcompaniantes +
                        $"</p>";
                }
                body = body.Replace("#INFO_ENCUENTROS#", info_encuentros);

                //Componer la url que irá en el qr
                var urlHelperFactory = controller_context.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
                var u = urlHelperFactory.GetUrlHelper(controller_context);
                string qr_url = u.Action("EncuentrosReservados", "Restauracion", new { congressId = CongressId, nif = inscripcion.Registrant.NIF.Trim() }, controller_context.HttpContext.Request.Scheme);
                body = body.Replace("#QR_URL#", qr_url.Replace("&", "%26")); //Había un problema si no se sustituye, no cogía bien el segundo parámetro


                string[] mails_en_copia = appSettings.EncuentrosMailsCopia.Split(",");
                if (appSettings.EncuentrosMailsPrueba)
                {
                    service.Send(
                        "Su reserva ha sido actualizada [Pruebas => EncuentrosMailsPrueba (AppSettings)].",
                        body,
                        true,
                        mails_en_copia,
                        congress.MailUser
                        );
                }
                else
                {
                    List<string> mails = new string[] { inscripcion.Registrant.Email, inscripcion.Registrant.Email2 }.ToList();
                    mails.AddRange(mails_en_copia);

                    service.Send(
                        "Su reserva ha sido actualizada.",
                        body,
                        true,
                        mails.ToArray(),
                        congress.MailUser
                        );
                }


            }
            catch (Exception ex)
            {
                //logger.LogError(ex, ex.Message);
                return false;
            }
            return true;
        }
    }    
}


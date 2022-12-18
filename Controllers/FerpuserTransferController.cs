using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Enums;
using Ferpuser.Models.Sage;
using Ferpuser.Models.Transfer;
using Ferpuser.Transfer;
using Ferpuser.ViewFunctions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.RandomExtension;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Ferpuser.Controllers
{
    public class FerpuserTransferController : Controller
    {
        private readonly ILogger<FerpuserTransferController> _logger;
        private readonly FerpuserContext _ferpuserContext;
        private readonly FerpuserContextADONet ferpuserContextAdoNet;
        private readonly ApplicationDbContext db;
        private readonly SageContext _sageContext;
        private FerpuserContextADONetFactory ferpuserContextFactory;

        public FerpuserTransferController(
            ILogger<FerpuserTransferController> logger, 
            ApplicationDbContext context, 
            FerpuserContext ferpuserContext,
            FerpuserContextADONet ferpuserContextAdoNet,
            SageContext sageContext,
            FerpuserContextADONetFactory ferpuserContextFactory)
        {
            _logger = logger;
            db = context;
            _ferpuserContext = ferpuserContext;
            _sageContext = sageContext;
            this.ferpuserContextAdoNet = ferpuserContextAdoNet;
            this.ferpuserContextFactory = ferpuserContextFactory;

        }

        public async Task<IActionResult> FromWeb(Guid? id)
        {
            if (!id.HasValue)
                return BadRequest();
            
            if (db.Congresses.Find(id.Value) == null)           
                return NotFound();
            
            #region IMPORT REGISTRATIONS

            List<RegistrationTransfer> registrations;
            try
            {
                //registrations = _ferpuserContext.RegistrationTransfers.AsNoTracking().ToList();
                registrations = ferpuserContextAdoNet.GetRegistrationTransfer();
            }
            catch (InvalidCastException e)
            {
                _logger.LogError(e, "Error de conexión.");
                return BadRequest("Invalid cast exception: " + e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error de conexión.");
                return BadRequest(e.Message + " " + e.GetType());
            }
            var account = db.Accounts.AsNoTracking().FirstOrDefault(a => a.Name.Equals(User.Identity.Name));
            foreach (var r in registrations)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9]");
                //r.FacNif = rgx.Replace(r.FacNif, "").ToUpper();
                if (r.Uid.HasValue && !r.Uid.Value.Equals(Guid.Empty))
                {
                    continue;
                }
                var regTypeTrans = _ferpuserContext.RegistrationTypeTransfers.Find(r.TipoCuota);
                if (regTypeTrans == null)
                {
                    regTypeTrans = new RegistrationTypeTransfer
                    {
                        Nombre = "No indicado"
                    };
                }
                var regType = db.RegistrationTypes.FirstOrDefault(rt => rt.Name.ToLower().Trim().Equals(regTypeTrans.Nombre.ToLower().Trim()));
                if (regType == null)
                {
                    regType = new RegistrationType
                    {
                        Name = regTypeTrans.Nombre.Trim(),
                        Description = ""
                    };
                    db.RegistrationTypes.Add(regType);
                    db.SaveChanges();
                }

                var provTrans = _ferpuserContext.ProvinceTransfers.Find(r.Provincia);
                var clientProvTrans = _ferpuserContext.ProvinceTransfers.Find(r.FacProvincia);
                var workplace = _ferpuserContext.WorkplaceTransfers.FirstOrDefault(wt => wt.Id.Equals(r.CentroTrabajo));
                var treatment = db.Treatments.FirstOrDefault();
                var max = 1;
                if (db.Registrations.Any(r => r.Deleted == null && r.CongressId.Equals(id)))
                {
                    max = db.Registrations.Where(r => r.Deleted == null && r.CongressId.Equals(id)).Max(r => r.Number) + 1;
                }
                try
                {
                    var registration = new Registration()
                    {
                        Number = max,
                        Created = r.FechaInscripcion,
                        CongressId = id.Value,
                        RegistrationTypeId = regType.Id,
                        Fee = r.PrecioCuota ?? 0,
                        Notes = r.Comentarios,
                        Imported = true,
                        AccountId = account.Id,
                        Registrant = new Registrant
                        {
                            NIF = r.Nif,
                            Email = r.Mail,
                            Email2 = r.Mail2,
                            Name = r.Nombre,
                            Surnames = r.Apellidos,
                            TreatmentId = treatment.Id,
                            Category = r.Categoria,
                            Location = new RegistrantLocation
                            {
                                Address = r.Direccion ?? "",
                                City = r.Poblacion ?? "",
                                ZipCode = r.CodigoPostal ?? "",
                                Province = provTrans?.Nombre ?? "",
                                Country = string.IsNullOrWhiteSpace(r.Pais) ? "España" : r.Pais,
                                Phone = r.Movil ?? "",
                                Phone2 = r.Telefono ?? "",
                            },
                            Position = r.Cargo,
                            ProfessionalCategory = r.CategoriaProfesional,
                            Workplace = workplace?.Nombre ?? (r.CentroTrabajo2 ?? ""),
                            Laboratorio = r.Laboratorio,
                            Especialidad = r.Especialidad
                        },
                    };
                    r.Uid = registration.Id;

                    if (r.FacNombre != null && r.FacNif != null && r.FacNif.Length > 0)
                    {
                        //= r.FacNif.Replace(" ", "").Replace("-", "").Trim().ToUpper();
                        var sageClient = _sageContext.Clientes.FirstOrDefault(c =>
                            c.CIF.Trim().ToUpper().Equals(r.FacNif.Trim().ToUpper()) ||
                            c.CIF.Trim().ToUpper().Equals(r.FacNif)
                        );
                        if (sageClient != null)
                        {
                            var client = await SageController.UpdateSageClient(db, sageClient);
                            registration.Client = client;
                        }
                        else
                        {
                            var client = db.Clients.Include(c => c.Locations).FirstOrDefault(c => c.NIF.Trim().ToLower().Equals(r.FacNif.Trim().ToLower()));
                            if (client == null)
                            {
                                registration.Client = new Client
                                {
                                    BusinessName = r.FacNombre,
                                    NIF = r.FacNif,
                                    Email = r.Mail,
                                    Email2 = r.FacMail
                                };
                            }
                            else
                            {
                                registration.Client = client;
                                if (string.IsNullOrWhiteSpace(registration.Client.Email))
                                {
                                    registration.Client.Email = r.FacMail;
                                    db.Update(registration.Client);
                                }
                            }
                        }
                        List<ClientLocation> clientLocations;
                        if (registration.Client.Locations == null)
                        {
                            clientLocations = db.ClientLocations.Where(cl => cl.ClientId.Equals(registration.ClientId)).ToList();
                        }
                        else
                        {
                            clientLocations = registration.Client.Locations.ToList();
                        }

                        var billingLocation = clientLocations.FirstOrDefault(l =>
                            (rgx.Replace(l.Address, "").ToUpper().Equals(rgx.Replace(r.FacDireccion, "").ToUpper()) ||
                             rgx.Replace(l.Address, "").ToUpper().Contains(rgx.Replace(r.FacDireccion, "").ToUpper()) ||
                             rgx.Replace(r.FacDireccion, "").ToUpper().Contains(rgx.Replace(l.Address, "").ToUpper())
                             ) && rgx.Replace(l.Phone, "").ToUpper().Equals(rgx.Replace(r.Movil, "").ToUpper()));
                        if (billingLocation == null)
                        {
                            billingLocation = new ClientLocation
                            {
                                Client = registration.Client,
                                Address = r.FacDireccion,
                                City = r.FacLocalidad,
                                ZipCode = r.FacCodigoPostal,
                                Country = r.FacPais ?? "España",
                                Province = clientProvTrans?.Nombre ?? "",
                                Phone = r.Movil,
                            };
                        }
                        registration.BillingLocation = billingLocation;
                    }

                    _ferpuserContext.Attach<RegistrationTransfer>(r);
                    _ferpuserContext.Update(r);
                    _ferpuserContext.SaveChanges();
                    db.Add(registration);
                    db.SaveChanges();
                    //break;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error al realizar la sincronización.");
                    return BadRequest(e.StackTrace);
                }
            }
            #endregion

            #region IMPORT ACCOMMODATIONS
            List<AccommodationTransfer> accommodations;
            try
            {
                accommodations = _ferpuserContext.AccommodationTransfers.ToList();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("alojamientos' doesn't exist"))
                {
                    return Ok();
                }
                return BadRequest(e.Message);
            }


            foreach (var r in accommodations)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9]");
                r.FacNif = rgx.Replace(r.FacNif, "").ToUpper();
                if (db.Accommodations.Any(rg =>
                    rg.CongressId.Equals(id.Value) &&
                    rg.Registrant.NIF.ToLower().Trim().Equals(r.Nif.ToLower().Trim()) &&
                    rg.Created.Equals(r.FechaInscripcion)
                    ))
                {
                    continue;
                }

                var provTrans = _ferpuserContext.ProvinceTransfers.Find(r.Provincia);
                var compProv = _ferpuserContext.ProvinceTransfers.Find(r.ProvinciaAcompanya);
                var clientProvTrans = _ferpuserContext.ProvinceTransfers.Find(r.FacProvincia);

                var workplace = _ferpuserContext.WorkplaceTransfers.FirstOrDefault(wt => wt.Id.Equals(r.CentroTrabajo));

                var treatment = db.Treatments.FirstOrDefault();

                var roomType = db.RoomTypes.FirstOrDefault(rt => rt.Name.ToLower().Trim().Equals(r.TipoHabitacion.ToLower().Trim()));
                if (roomType == null)
                {
                    roomType = new RoomType
                    {
                        Name = r.TipoHabitacion.Trim(),
                        Description = ""
                    };
                    db.RoomTypes.Add(roomType);
                    db.SaveChanges();
                }
                var HotelId = r.IdHotel;
                var hotelTrans = _ferpuserContext.HotelTransfers.FirstOrDefault(h => h.Id == HotelId);
                if (hotelTrans == null)
                {
                    hotelTrans = new HotelTransfer
                    {
                        Nombre = "No indicado"
                    };
                }
                var max = 1;
                if (db.Accommodations.Any(ac => ac.Deleted == null && ac.CongressId.Equals(id)))
                {
                    try
                    {
                        max = db.Accommodations.Where(ac => ac.Deleted == null && ac.CongressId.Equals(id)).Max(ac => ac.Number) + 1;
                    }
                    catch
                    {

                    }
                }
                try
                {

                    var accommodation = new Accommodation()
                    {
                        Number = max,
                        Created = r.FechaInscripcion,
                        CongressId = id.Value,
                        Notes = r.Comentarios,
                        Imported = true,
                        AccountId = account.Id,
                        RoomType = roomType,
                        StartDate = r.FechaLlegada,
                        EndDate = r.FechaSalida,
                        Fee = r.Precio ?? 0,
                        Hotel = hotelTrans.Nombre,
                        Registrant = new Registrant
                        {
                            NIF = r.Nif,
                            Email = r.Mail,
                            Email2 = r.Mail2,
                            Name = r.Nombre,
                            Surnames = r.Apellidos,
                            TreatmentId = treatment.Id,
                            Location = new RegistrantLocation
                            {
                                Address = r.Direccion ?? "",
                                City = r.Poblacion ?? "",
                                ZipCode = r.CodigoPostal ?? "",
                                Province = provTrans?.Nombre ?? "",
                                Country = "España",
                                Phone = r.Movil ?? "",
                                Phone2 = r.Telefono ?? "",
                            },
                            Position = r.Cargo,
                            ProfessionalCategory = r.CategoriaProfesional,
                            Workplace = workplace?.Nombre ?? (r.CentroTrabajo2 ?? "")
                        },
                    };

                    if (r.NifAcompanya != null && r.NombreAcompanya != null && r.NifAcompanya.Length > 0)
                    {
                        accommodation.Companion = new Registrant
                        {
                            NIF = r.NifAcompanya,
                            Email = r.MailAcompanya,
                            Name = r.NombreAcompanya,
                            Surnames = r.ApellidosAcompanya,
                            TreatmentId = treatment.Id,
                            Location = new RegistrantLocation
                            {
                                Address = r.DireccionAcompanya ?? "",
                                City = r.PoblacionAcompanya ?? "",
                                ZipCode = r.CodigoPostalAcompanya ?? "",
                                Province = compProv?.Nombre ?? "",
                                Country = "España",
                                Phone = r.TelefonoAcompanya ?? "",
                            },
                        };
                    }

                    if (r.FacNombre != null && r.FacNif != null && r.FacNif.Length > 0)
                    {
                        var sageClient = _sageContext.Clientes.FirstOrDefault(c =>
                            c.CIF.Trim().ToLower().Equals(r.FacNif.Trim().ToLower()) ||
                            c.CIF.Trim().ToLower().Equals(r.FacNif.Replace(" ", "").Replace("-", "").Trim().ToLower())
                        );
                        if (sageClient != null)
                        {
                            var client = await SageController.UpdateSageClient(db, sageClient);
                            accommodation.Client = client;
                        }
                        else
                        {
                            var client = db.Clients.FirstOrDefault(c => c.NIF.Trim().ToLower().Equals(r.FacNif.Trim().ToLower()));
                            if (client == null)
                            {
                                accommodation.Client = new Client
                                {
                                    BusinessName = r.FacNombre,
                                    NIF = r.FacNif,
                                    Email = r.Mail,
                                    Email2 = r.FacMail
                                };
                            }
                            else
                            {
                                accommodation.Client = client;
                            }
                        }

                        List<ClientLocation> clientLocations;
                        if (accommodation.Client.Locations == null)
                        {
                            clientLocations = db.ClientLocations.Where(cl => cl.ClientId.Equals(accommodation.ClientId)).ToList();
                        }
                        else
                        {
                            clientLocations = accommodation.Client.Locations.ToList();
                        }
                        var billingLocation = clientLocations.FirstOrDefault(l =>
                            (rgx.Replace(l.Address, "").ToUpper().Equals(rgx.Replace(r.FacDireccion, "").ToUpper()) ||
                             rgx.Replace(l.Address, "").ToUpper().Contains(rgx.Replace(r.FacDireccion, "").ToUpper()) ||
                             rgx.Replace(r.FacDireccion, "").ToUpper().Contains(rgx.Replace(l.Address, "").ToUpper())
                             ) && rgx.Replace(l.Phone, "").ToUpper().Equals(rgx.Replace(r.Movil, "").ToUpper()));
                        if (billingLocation == null)
                        {
                            billingLocation = new ClientLocation
                            {
                                Client = accommodation.Client,
                                Address = r.FacDireccion,
                                City = r.FacLocalidad,
                                ZipCode = r.FacCodigoPostal,
                                Country = r.FacPais ?? "España",
                                Province = clientProvTrans?.Nombre ?? "",
                                Phone = r.Movil,
                                Phone2 = r.Telefono,
                            };
                        }

                        accommodation.BillingLocation = billingLocation;
                    }

                    db.Add(accommodation);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            #endregion

            var Result = ImportPonentes(id.Value);

            db.SaveChanges();
            return Ok();
        }

        private ImportResult ImportPonentes(Guid CongressId)
        {
            ImportResult result = new ImportResult();
            List<PonenteTransfer> ponentes;

            var dbFerpUserComite = ferpuserContextFactory.CreateApplicationDbContextComite(CongressId);
            if (dbFerpUserComite == null)
            {
                result.Success = false;
                result.Message = "Error. No se ha podido crear la conexión.";
                return result;
            }

            //Conexión con la base de datos mysql
            try
            {
                ponentes = dbFerpUserComite.GetPonenteTransfer().ToList();
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("miembros_comites' doesn't exist"))
                { 
                    result.Success = false;
                    result.Message = "Error de conexión a la base de datos: " + ex.Message;
                }
                else
                {
                    result.Success = true;
                    result.Message = "La tabla de donde se importan los ponentes no existe en esta base de datos, por lo tanto se ha desestimado la importación.";
                }
                return result;
            }

            //Actualización de registros
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    //Borramos todos los ponentes del congreso que hayan sido importados previamente
                    db.RemoveRange(db.Ponentes.Where(f => f.CongressId == CongressId && !string.IsNullOrWhiteSpace(f.IdExterno)));

                    //Recorremos los registros para ir añadiendolos
                    foreach (var p in ponentes)
                    {
                        Ponente nuevo = new Ponente()
                        {
                            CongressId = CongressId,
                            IdExterno = p.Id.ToString(),
                            Login = p.Login,
                            Password = p.Password,
                            Nombre = p.Nombre,
                            Apellidos = p.Apellidos,
                            NIF = p.Nif,
                            Localidad = p.Localidad,
                            Provincia = p.Provincia,
                            Cargo = p.Cargo,
                            CentroTrabajo = p.CentroTrabajo,
                            Telefono = p.Telefono,
                            Movil = p.Movil,
                            Mail = p.Mail,
                            Mail2 = p.Mail2,
                            Tratamiento = GetTratamiento(p.Tratamiento1), 
                            AmbitoComite = GetAmbitoComite(p.AmbitoComite),
                            Activo = GetBoolFromString(p.Activo) ?? true,
                            Visible = GetBoolFromString(p.Visible) ?? true,
                            UltimoAcceso = p.UltimoAcceso,
                            Comentarios = p.Comentarios,
                            Superevaluador = GetBoolFromString(p.SuperEvaluador) ?? false,
                            Visualizador = GetBoolFromString(p.Visualizador) ?? false,
                            JuntaDirectiva = GetBoolFromString(p.JuntaDirectiva) ?? false,
                            TipoComiteId = p.TipoComite,
                            PuestoComiteId = p.PuestoComite,
                            FechaRegistro = p.FechaRegistro
                        };

                        //Si el nif es vacío o nulo entonces continúa
                        if (string.IsNullOrWhiteSpace(nuevo.NIF))
                            continue;

                        db.Ponentes.Add(nuevo);
                    }

                    db.SaveChanges();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error de actualización de registros de ponentes: " + ex.Message;
            }

            result.Success = true;
            return result;
        }

        private Tratamiento? GetTratamiento(int? tratamiento)
        {
            if (!tratamiento.HasValue)
                return null;

            switch (tratamiento.Value)
            {
                case 1:
                    return Tratamiento.D;
                case 2:
                    return Tratamiento.Da;
                case 3:
                    return Tratamiento.DrD;
                case 4:
                    return Tratamiento.DraDa;
            }
            return null;
        }

        private AmbitoComite? GetAmbitoComite(string ambito_comite)
        {
            if (string.IsNullOrWhiteSpace(ambito_comite))
                return null;

            switch (ambito_comite.Trim().ToLower())
            {
                case "local":
                    return AmbitoComite.Local;
                case "nacional":
                    return AmbitoComite.Nacional;
            }
            return null;
        }

        private bool? GetBoolFromString(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return null;

            switch (valor.Trim().ToLower())
            {
                case "si":
                    return true;
                case "no":
                    return false;
            }
            return null;
        }

    }

    public class ImportResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

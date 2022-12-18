using CsvHelper;
using CsvHelper.Configuration;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    public class InscripcionesController : Controller
    {
        private readonly ApplicationDbContext db;

        public InscripcionesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Reports/PaidPending
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            SetViewData();
            return View();
        }

        [HttpPost, ActionName("Index")]
        public IActionResult IndexConfirmed(InscripcionesViewModel model)
        {
            if (ModelState.IsValid)
                model.Items = GetItems(model.Filter);
            SetViewData();
            return View(model);
        }

        public async Task<IActionResult> ExportCsv(InscripcionesViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            model.Items = GetItems(model.Filter);

            var first = model.Items.First();
            string fileName = first.Congress.Number + "-" + first.Congress.Name + "-inscripciones.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.GetCultureInfo("es-ES")))
            {
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<InscripcionesMap>();
                await csv.WriteRecordsAsync<Registration>(model.Items);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);            
        }

        private List<Registration> GetItems(InscripcionesFilter Filter)
        {
            var list = db.Registrations
                    .Include(f => f.BillingLocation)
                    .Include(f => f.Client)
                    .Include(f => f.Congress)
                    .Include(f => f.RegistrationType)
                    .Include(f => f.Registrant)
                        .ThenInclude(c => c.Treatment)
                    .Include(f => f.Registrant)
                        .ThenInclude(c => c.Location)
                    .Where(f =>
                        f.Deleted == null &&
                        !f.Congress.HideRegistrations &&
                        Filter.CongressId == f.CongressId &&
                        (Filter.EstadoPagada.HasValue ? f.Paid == Filter.EstadoPagada : true));

            if (Filter.FiltroFacturacion.HasValue)
            {
                if (Filter.FiltroFacturacion.Value)

                    list = list.Where(a => a.OnlyBilling);
                else
                    list = list.Where(a => !a.OnlyBilling);
            }
            return list.OrderBy(f => f.Registrant.Surnames).ToList();
        }

        /// <summary>
        /// Rellena los datos necesarios para las vistas
        /// </summary>
        private void SetViewData()
        {
            var congresses = db.Congresses.Where(c =>
                c.Deleted == null &&
                c.IsCongress &&
                !c.HideRegistrations
            );

            if (User.Claims.Any(c => c.Type.Equals(Consts.CLAIM_PERFIL_USUARIO_CLIENTE)))
                congresses = congresses.Where(f => f.Number == Consts.NUMBER_EVENTO_HARDCODEADO);

            ViewBag.Eventos = congresses;
        }
    }

    public class InscripcionesViewModel
    { 
        public InscripcionesFilter Filter { get; set; } = new InscripcionesFilter();
        public List<Registration> Items { get; set; } = new List<Registration>();
    }

    public class InscripcionesFilter
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Congreso")]
        public Guid CongressId { get; set; }

        [Display(Name ="Estado pago")]
        public bool? EstadoPagada { get; set; }

        [Display(Name = "Filtro facturación")]
        public bool? FiltroFacturacion { get; set; }
    }

    public class InscripcionesMap : ClassMap<Registration>
    {
        public InscripcionesMap()
        {
            Map(r => r.Registrant.NIF).Name("NIF").Index(0);
            Map(r => r.Registrant.Surnames).Name("Apellidos").Index(1);
            Map(r => r.Registrant.Name).Name("Nombre").Index(2);
            Map(r => r.Registrant.Especialidad).Name("Especialidad").Index(3);
            Map(r => r.Registrant.Location.Province).Name("Provincia").Index(4);
            Map(r => r.Registrant.Email).Name("Email").Index(5);
            Map(r => r.Registrant.Treatment.Name).Name("Tratamiento").Index(6);
            Map(r => r.RegistrationType.Name).Name("Tipo de inscripción").Index(7);
            Map(r => r.InvoiceNumber).Name("Nº Factura").Index(8);
            Map(r => r.FeeDisplay).Name("Precio").Index(9);
            Map(r => r.Registrant.Laboratorio).Name("Laboratorio").Index(10);
            Map(r => r.Client.BusinessName).Name("Cliente").Index(11);
            Map(r => r.Paid).Name("Pagado").Index(12);
        }
    }
}

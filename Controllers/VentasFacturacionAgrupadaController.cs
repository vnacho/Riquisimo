using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Ventas")]
    public class VentasFacturacionAgrupadaController : VentasBaseController
    {
        private readonly SageContext sage_db;
        private readonly ApplicationDbContext db;
        private readonly SageComuContext sage_comu_db;
        private VentaFacturaManager manager; 
        public VentasFacturacionAgrupadaController(SageContext sageContext, ApplicationDbContext dbContext, SageComuContext sage_comu_db_context, VentaFacturaManager venta_factura_manager)
        {
            sage_db = sageContext;
            db = dbContext;
            manager = venta_factura_manager;
            sage_comu_db = sage_comu_db_context;
        }

        /// <summary>
        /// Ejemplo para sacar datos:
        /// Evento: VII CONGRESO SENFE            
        /// Tipo: Beca
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            SetViewBag();
            return View();
        }

        [HttpPost, ActionName("Index")]
        public async Task<IActionResult> IndexConfirmed(bool facturar, bool traspasar, string CodigoOperario, string Serie)
        {
            SetViewBag();
            ViewBag.Operarios = sage_comu_db.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Series = sage_db.Serie.Where(f => f.Empresa == Consts.CODIGO_EMPRESA && f.Tipodoc == Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_FACTURA).OrderBy(f => f.Serie);

            VentasFacturacionAgrupadaViewModel model = new VentasFacturacionAgrupadaViewModel();
            await TryUpdateModelAsync<VentasFacturacionAgrupadaFilter>(model.Filter, "Filter");

            if (ModelState.IsValid)
            {
                Congress congress = await db.Congresses.FindAsync(model.Filter.CongressId);
                model.Items = await db.Registrations.Include(f => f.Registrant).Include(f => f.Client).Where(model.Filter.ExpressionFilter()).AsNoTracking().ToListAsync();
                model.Items.ForEach(f => f.Congress = congress);
            }

            if (facturar)
            {
                string[] selected = (string[])Request.Form["Selected"];
                model.Items.Where(f => selected.Contains(f.Id.ToString())).ToList().ForEach(f => f.IsCheckedHelper = true);

                if (selected.Any())
                {
                    var tuple = manager.CreateFacturaDesdeInscripciones(selected, CodigoOperario, Serie);
                    if (tuple.Item2.Any())
                    {
                        ModelState.AddModelError("", tuple.Item2.First().ErrorMessage);
                    }
                    else
                    {
                        TempData["Message"] = $"{selected.Count()} inscripciones se han procesado correctamente. {tuple.Item1} facturas generadas.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Debe seleccionar algún evento para facturar");
                }   
            }
            return View(model);
        }

        //public async Task<IActionResult> Traspasar(int[] idfacturas)
        //{
        //    return View();
        //}

        private void SetViewBag()
        {
            ViewBag.Eventos = db.Congresses.Where(rt => rt.Deleted == null).OrderBy(f => f.Name);
            ViewBag.TiposInscripcion = db.RegistrationTypes.Where(rt => rt.Deleted == null).OrderBy(f => f.Name);
            ViewBag.Clientes = db.Clients.OrderBy(f => f.Deleted == null).OrderBy(f => f.BusinessName);
            
        }
    }

    public class VentasFacturacionAgrupadaViewModel
    {
        public VentasFacturacionAgrupadaFilter Filter { get; set; }
        public List<Registration> Items { get; set; }

        [Display(Name = "Serie")]
        [MaxLength(2)]
        public string Serie { get; set; }

        [Display(Name = "Operario")]
        public string CodigoOperario { get; set; }

        public VentasFacturacionAgrupadaViewModel()
        {
            Filter = new VentasFacturacionAgrupadaFilter();
            Items = new List<Registration>();
        }
    }    

    public class VentasFacturacionAgrupadaFilter
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Evento")]
        public Guid? CongressId { get; set; }

        //[Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Tipo de inscripción")]
        public Guid? TipoInscripcionId { get; set; }

        [Display(Name = "Cliente")]
        public Guid? ClientId { get; set; }

        public Expression<Func<Registration, bool>> ExpressionFilter()
        {
            return f => 
                !f.Exported &&
                !f.Deleted.HasValue &&
                f.ClientId.HasValue &&                
                f.CongressId == CongressId && 
                (TipoInscripcionId.HasValue ? f.RegistrationTypeId == TipoInscripcionId : true) &&                
                (ClientId.HasValue ? f.ClientId == ClientId : true);
        }
    }
}

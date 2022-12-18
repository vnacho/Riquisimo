using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Core;
using Ferpuser.Models.SessionObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using CsvHelper;
using System.Globalization;
using Ferpuser.BLL.Services;
using Microsoft.AspNetCore.Authorization;

namespace Ferpuser.Controllers
{

    [Authorize(Policy = "Almacen")]
    public class MovimientosArticulosAlmacenController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public MovimientosArticulosAlmacenController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int? pag, string sort = "", string currentsort = "", bool reset = false, bool export = false)
        {
            if (reset)
            {
                HttpContext.Session.Remove(Consts.SESSION_REGISTRATION_LIST_STATE);
                return RedirectToAction("Index");
            }

            Pager pager;
            MovimientosArticulosAlmacenFilter filter;
            string sSesion = HttpContext.Session.GetString(Consts.SESSION_REGISTRATION_LIST_STATE);
            var previous = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            MovimientosArticulosAlmacenSession objSesion;

            if (!string.IsNullOrWhiteSpace(previous) && previous.Contains("/MovimientosArticulosAlmacen/Edit", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(sSesion))
            {
                objSesion = JsonConvert.DeserializeObject<MovimientosArticulosAlmacenSession>(sSesion);
                sort = objSesion.sort;
                filter = objSesion.filter;
                pager = new Pager(await _dbContext.MovimientosArticulosAlmacens.Where(r => r.Deleted == null).CountAsync(filter.ExpressionFilter()), objSesion.page, 50, 5);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(sort))
                {
                    if (string.IsNullOrWhiteSpace(currentsort))
                        sort = "Id desc";
                    else
                        sort = currentsort;
                }

                filter = new MovimientosArticulosAlmacenFilter();

                await TryUpdateModelAsync<MovimientosArticulosAlmacenFilter>(filter, "filter");

                pager = new Pager(await _dbContext.MovimientosArticulosAlmacens.CountAsync(filter.ExpressionFilter()), pag, 100, 5);
            }

            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;
            ViewData["Filter"] = filter;

            objSesion = new MovimientosArticulosAlmacenSession()
            {
                filter = filter,
                sort = sort,
                page = pager.Page
            };

            HttpContext.Session.SetString(Consts.SESSION_REGISTRATION_LIST_STATE, JsonConvert.SerializeObject(objSesion));

            IEnumerable<MovimientosArticulosAlmacen> list = await _dbContext.MovimientosArticulosAlmacens.Where(filter.ExpressionFilter())
                .Include(f => f.ArticulosAlmacen)
                .Include(f => f.CentroCoste)
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            CargarCombos();

            return View(list);
        }

        // GET: MovimientosArticulosAlmacenController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Create()
        {
            CargarCombos();
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateConfirmed(string guardar)
        {
            MovimientosArticulosAlmacen model = new MovimientosArticulosAlmacen();

            await TryUpdateModelAsync<MovimientosArticulosAlmacen>(model, "");

            //Validación de negocio            
            MovimientosArticulosAlmacenValidator validator = new MovimientosArticulosAlmacenValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                MovimientosArticulosAlmacenManager manager = new MovimientosArticulosAlmacenManager(_dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                if (guardar == "continuar")
                    return RedirectToAction("Create");
                return RedirectToAction("Index");
            }

            IEnumerable<ArticulosAlmacen> ArticuloSeleccionado = _dbContext.ArticulosAlmacen
                .Where(f => f.Id == model.ArticulosAlmacenId);

            if (ArticuloSeleccionado.Count() > 0)
                ActualizarCampos(ArticuloSeleccionado.First());
            else
                CargarCombos();


            return View(model);
        }

        public IActionResult Edit(Guid id)
        {
            var model = _dbContext.MovimientosArticulosAlmacens.Single(f => f.Id.Equals(id));

            IEnumerable<ArticulosAlmacen> ArticuloSeleccionado = _dbContext.ArticulosAlmacen
                .Where(f => f.Id == model.ArticulosAlmacenId);

            ActualizarCampos(ArticuloSeleccionado.First());

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(Guid id)
        {
            MovimientosArticulosAlmacen model = _dbContext.MovimientosArticulosAlmacens.Find(id);

            await TryUpdateModelAsync<MovimientosArticulosAlmacen>(model, "");

            MovimientosArticulosAlmacenValidator validator = new MovimientosArticulosAlmacenValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                MovimientosArticulosAlmacenManager manager = new MovimientosArticulosAlmacenManager(_dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";


                return RedirectToAction("Index");
            }
            CargarCombos();
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            MovimientosArticulosAlmacenValidator validator = new MovimientosArticulosAlmacenValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                MovimientosArticulosAlmacenManager manager = new MovimientosArticulosAlmacenManager(_dbContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<PartialViewResult> Refrescar()
        {
            MovimientosArticulosAlmacen model = new MovimientosArticulosAlmacen();
            RefrescarComun(model);

            return PartialView("~/Views/Shared/EditorTemplates/MovimientosArticulosAlmacen.cshtml", model);

        }
        public async Task<PartialViewResult> RefrescarPrecio()
        {
            MovimientosArticulosAlmacen model = new MovimientosArticulosAlmacen();
            RefrescarComun(model);

            return PartialView("~/Views/Shared/EditorTemplates/MovimientosArticulosAlmacen.cshtml", model);
        }

        private async void RefrescarComun(MovimientosArticulosAlmacen model)
        {
            await TryUpdateModelAsync<MovimientosArticulosAlmacen>(model, "");

            IEnumerable<ArticulosAlmacen> ArticuloSeleccionado = _dbContext.ArticulosAlmacen.Where(f => f.Id == model.ArticulosAlmacenId);

            if (ArticuloSeleccionado != null && ArticuloSeleccionado.Count()>0 )
                ActualizarCampos(ArticuloSeleccionado.First());
            else
                CargarCombos();

            ModelState.Clear();
        }

        public async Task<FileResult> ExportCsv(string currentsort = "")
        {
            MovimientosArticulosAlmacenFilter filter = new MovimientosArticulosAlmacenFilter();
            await TryUpdateModelAsync<MovimientosArticulosAlmacenFilter>(filter, "filter");

            IEnumerable<MovimientosArticulosAlmacen> list = await _dbContext.MovimientosArticulosAlmacens.Where(filter.ExpressionFilter())
                .Include(f => f.ArticulosAlmacen)
                .Include(f => f.CentroCoste)
                .OrderBy("ID")
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoMovimientosArticulosAlmacen.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<MovimientosArticulosAlmacenMap>();
                await csv.WriteRecordsAsync<MovimientosArticulosAlmacen>(list);
            }

            return File(memoryStream.ToArray(), "text/csv", fileName);
        }

        public async Task<FileResult> ExportPdf(string currentsort = "")
        {
            MovimientosArticulosAlmacenFilter filter = new MovimientosArticulosAlmacenFilter();
            await TryUpdateModelAsync<MovimientosArticulosAlmacenFilter>(filter, "filter");

            IEnumerable<MovimientosArticulosAlmacen> list = await _dbContext.MovimientosArticulosAlmacens.Where(filter.ExpressionFilter())
                .Include(f => f.ArticulosAlmacen)
                .Include(f => f.CentroCoste)
                .OrderBy("ID")
                .ToListAsync();

            string table = "<table class='w-100'><tr class='font-weight-bold'>" +
                "<td>Fecha</td>" +
                "<td>ENTRADA/SALIDA</td>" +
                "<td>Código producto</td>" +
                "<td class='text - right'>TIPO UNIDAD</td>" + 
                "<td class='text - right'>UNIDADES</td>" +
                "<td class='text - right'>Centro de coste</td>" +
                "</tr>{0}</table>";

            string rows = string.Empty;
            foreach (var item in list)
            {
                rows += $"<tr><td>{item.FechaMovimiento.ToShortDateString()}</td>" +
                        $"<td>{item.TipoMovimiento}</td>" +
                        $"<td>{item.ArticulosAlmacen.ProductCode}</td>" +
                        $"<td>{item.ArticulosAlmacen.Rate}</td>" +
                        $"<td>{item.Unidades}</td>" +
                        $"<td>{item.CentroCoste.Display}</td>" +
                        $"</tr>";
            }

            var html = PrintService.GetHtmlTheme("Listado de partes internos almacen", string.Format(table, rows));
            var pdf = PrintService.GetBytesLandscape(html);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_MovimientosArticulosAlmacen.pdf";
            return File(pdf, "application/pdf", fileName);
        }
        private void CargarCombos()
        {
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            ViewBag.ArticulosAlmacen = _dbContext.ArticulosAlmacen.OrderBy(f => f.ProductCode);
            ViewBag.TiposTaria = _dbContext.ArticulosAlmacen.GroupBy(f => f.Rate).Select(g => new { Rate = g.Key }); ;
        }
        private void ActualizarCampos(ArticulosAlmacen? ArticuloSeleccionado)
        {
            CargarCombos();
            ViewBag.Tarifa = ArticuloSeleccionado.Rate;
        }

    }
}

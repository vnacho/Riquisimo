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

    public class PartesInternosAlmacenController : ControlAlmacenBaseController
    {
        private readonly ApplicationDbContext _dbContext;

        public PartesInternosAlmacenController(ApplicationDbContext dbContext)
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
            PartesInternosAlmacenFilter filter;
            string sSesion = HttpContext.Session.GetString(Consts.SESSION_REGISTRATION_LIST_STATE);
            var previous = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            PartesInternosAlmacenSession objSesion;

            if (!string.IsNullOrWhiteSpace(previous) && previous.Contains("/PartesInternosAlmacen/Edit", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(sSesion))
            {
                objSesion = JsonConvert.DeserializeObject<PartesInternosAlmacenSession>(sSesion);
                sort = objSesion.sort;
                filter = objSesion.filter;
                pager = new Pager(await _dbContext.PartesInternosAlmacen.Where(r => r.Deleted == null).CountAsync(filter.ExpressionFilter()), objSesion.page, 50, 5);
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

                filter = new PartesInternosAlmacenFilter();

                await TryUpdateModelAsync<PartesInternosAlmacenFilter>(filter, "filter",
                    f => f.FechaDesde,
                    f => f.FechaHasta,
                    f => f.ArticulosAlmacenId,
                    f => f.DestinoId);

                pager = new Pager(await _dbContext.PartesInternosAlmacen.CountAsync(filter.ExpressionFilter()), pag, 100, 5);
            }

            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;
            ViewData["Filter"] = filter;

            objSesion = new PartesInternosAlmacenSession()
            {
                filter = filter,
                sort = sort,
                page = pager.Page
            };

            HttpContext.Session.SetString(Consts.SESSION_REGISTRATION_LIST_STATE, JsonConvert.SerializeObject(objSesion));

            IEnumerable<PartesInternosAlmacen> list = await _dbContext.PartesInternosAlmacen.Where(filter.ExpressionFilter())
                .Include(f => f.ArticulosAlmacen)
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            CargarCombos();

            return View(list);
        }

        // GET: PartesInternosAlmacenController/Details/5
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
            PartesInternosAlmacen model = new PartesInternosAlmacen();
            await TryUpdateModelAsync<PartesInternosAlmacen>(model, "",
                f => f.DestinoId,
                f => f.fecha,
                f => f.ArticulosAlmacenId,
                f => f.TariffTypeUnits,
                f => f.TariffTypeUnits2,
                f => f.Price,
                f => f.DestinoId,
                f => f.Amount
                );

            model.Calcular();

            //Validación de negocio            
            PartesInternosAlmacenValidator validator = new PartesInternosAlmacenValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                PartesInternosAlmacenManager manager = new PartesInternosAlmacenManager(_dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                if (guardar == "continuar")
                    return RedirectToAction("Create");
                return RedirectToAction("Index");
            }

            IEnumerable<ArticulosAlmacen> ArticuloSeleccionado = _dbContext.ArticulosAlmacen
                .Where(f => f.Id == model.ArticulosAlmacenId);

            if (ArticuloSeleccionado.Count() > 0)
                ActualizarCampos(model, ArticuloSeleccionado.First()); 
            else
                CargarCombos();


            return View(model);
        }

        public IActionResult Edit(Guid id)
        {
            var model = _dbContext.PartesInternosAlmacen.Single(f => f.Id.Equals(id));

            IEnumerable<ArticulosAlmacen> ArticuloSeleccionado = _dbContext.ArticulosAlmacen
                .Where(f => f.Id == model.ArticulosAlmacenId);

            ActualizarCampos(model, ArticuloSeleccionado.First());

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(Guid id)
        {
            PartesInternosAlmacen model = _dbContext.PartesInternosAlmacen.Find(id);

            await TryUpdateModelAsync<PartesInternosAlmacen>(model, "",
                f => f.DestinoId,
                f => f.fecha,
                f => f.ArticulosAlmacenId,
                f => f.TariffTypeUnits,
                f => f.TariffTypeUnits2,
                f => f.Price,
                f => f.Amount
                );

            model.Calcular();

            PartesInternosAlmacenValidator validator = new PartesInternosAlmacenValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                PartesInternosAlmacenManager manager = new PartesInternosAlmacenManager(_dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";


                return RedirectToAction("Index");
            }
            CargarCombos();
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            PartesInternosAlmacenValidator validator = new PartesInternosAlmacenValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                PartesInternosAlmacenManager manager = new PartesInternosAlmacenManager(_dbContext);
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
            PartesInternosAlmacen model = new PartesInternosAlmacen();
            RefrescarComun(model, true);
            
            return PartialView("~/Views/Shared/EditorTemplates/PartesInternosAlmacen.cshtml", model);

        }
        public async Task<PartialViewResult> RefrescarPrecio()
        {
            PartesInternosAlmacen model = new PartesInternosAlmacen();
            RefrescarComun(model, false);
            
            return PartialView("~/Views/Shared/EditorTemplates/PartesInternosAlmacen.cshtml", model);
        }

        private async void RefrescarComun(PartesInternosAlmacen model, bool cargarCombo)
        {
            await TryUpdateModelAsync<PartesInternosAlmacen>(model, "",
                 f => f.Id,
                 f => f.ArticulosAlmacenId);

            IEnumerable<ArticulosAlmacen> ArticuloSeleccionado = _dbContext.ArticulosAlmacen.Where(f => f.Id == model.ArticulosAlmacenId);

            if (ArticuloSeleccionado != null && ArticuloSeleccionado.Count() > 0)
                ActualizarCampos(model, ArticuloSeleccionado.First(), cargarCombo);
            else if (cargarCombo) CargarCombos();

            ModelState.Clear();
        }

        public async Task<FileResult> ExportCsv(string currentsort = "")
        {
            PartesInternosAlmacenFilter filter = new PartesInternosAlmacenFilter();
            await TryUpdateModelAsync<PartesInternosAlmacenFilter>(filter, "filter",
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.ArticulosAlmacenId,
                f => f.DestinoId
                );

            IEnumerable<PartesInternosAlmacen> list = await _dbContext.PartesInternosAlmacen
                .Include(f => f.ArticulosAlmacen)
                .Include(f => f.Destino)
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoPartesInternosAlmacen.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<PartesInternosAlmacenMap>();
                await csv.WriteRecordsAsync<PartesInternosAlmacen>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

        public async Task<FileResult> ExportPdf(string currentsort = "")
        {
            PartesInternosAlmacenFilter filter = new PartesInternosAlmacenFilter();
            await TryUpdateModelAsync<PartesInternosAlmacenFilter>(filter, "filter",
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.ArticulosAlmacenId,
                f => f.DestinoId
                );

            IEnumerable<PartesInternosAlmacen> list = await _dbContext.PartesInternosAlmacen
                .Include(f => f.ArticulosAlmacen)
                .Include(f => f.Destino)
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string table = "<table class='w-100'><tr class='font-weight-bold'>" +
                "<td>Fecha</td><td>Código producto</td>" +
                "<td class='text - right'>TTF_1</td><td class='text - right'>TTF_2</td>" +
                "<td class='text - right'>Precio</td><td class='text - right'>Importe</td><td>Centro de coste</td>" +
                "</tr>{0}</table>";

            string rows = string.Empty;
            foreach (var item in list)
            {
                rows += $"<tr><td>{item.fecha.ToShortDateString()}</td><td>{item.ArticulosAlmacen.ProductCode}</td>" +
                        $"<td class='text - right'>{item.ArticulosAlmacen.Rate} - {item.TariffTypeUnits}</td><td class='text - right'>{item.ArticulosAlmacen.Rate2} - {item.TariffTypeUnits2}</td>" +
                        $"<td class='text - right'>{item.Price}</td><td class='text - right'>{item.Amount}</td><td>{item.Destino.Display}</td>" +
                        $"</tr>";
            }

            var html = PrintService.GetHtmlTheme("Listado de partes internos almacen", string.Format(table, rows));
            var pdf = PrintService.GetBytesLandscape(html);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_PartesInternosAlmacen.pdf";
            return File(pdf, "application/pdf", fileName);
        }
        private void CargarCombos() 
        {
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            ViewBag.ArticulosAlmacen = _dbContext.ArticulosAlmacen.OrderBy(f => f.ProductCode);
            ViewBag.Tarifa = null;
            ViewBag.Tarifa2 = null;
        }
        private void ActualizarCampos(PartesInternosAlmacen model, ArticulosAlmacen? ArticuloSeleccionado, bool ActualizaPrecio = false)
        {
            CargarCombos();

            ViewBag.Tarifa = ArticuloSeleccionado.Rate;
            ViewBag.Tarifa2 = ArticuloSeleccionado.Rate2;

            if (ActualizaPrecio)
                model.Price = ArticuloSeleccionado.Price;

            if (ViewBag.Tarifa == null)
                model.TariffTypeUnits = 0;

            if (ViewBag.Tarifa2 == null)
                model.TariffTypeUnits2 = 0;

            model.Calcular();
        }
    }
}

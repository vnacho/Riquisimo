using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Core;
using Ferpuser.Models.SessionObjects;
using Microsoft.AspNetCore.Authorization;
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

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Almacen")]
    public class ArticulosAlmacenController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ArticulosAlmacenController(ApplicationDbContext dbContext)
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
            ArticulosAlmacenFilter filter;
            string sSesion = HttpContext.Session.GetString(Consts.SESSION_REGISTRATION_LIST_STATE);
            var previous = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            ArticulosAlmacenSession objSesion;

            if (!string.IsNullOrWhiteSpace(previous) && previous.Contains("/ArticulosAlmacen/Edit", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(sSesion))
            {
                objSesion = JsonConvert.DeserializeObject<ArticulosAlmacenSession>(sSesion);
                sort = objSesion.sort;
                filter = objSesion.filter;
                pager = new Pager(await _dbContext.ArticulosAlmacen.Where(r => r.Deleted == null).CountAsync(filter.ExpressionFilter()), objSesion.page, 50, 5);
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

                filter = new ArticulosAlmacenFilter();

                await TryUpdateModelAsync<ArticulosAlmacenFilter>(filter, "filter",
                    f => f.ProductCode,
                    f => f.ProductDescription,
                    f => f.CentroCosteId);

                pager = new Pager(await _dbContext.ArticulosAlmacen.CountAsync(filter.ExpressionFilter()), pag, 100, 5);
            }

            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;
            ViewData["Filter"] = filter;

            objSesion = new ArticulosAlmacenSession()
            {
                filter = filter,
                sort = sort,
                page = pager.Page
            };

            HttpContext.Session.SetString(Consts.SESSION_REGISTRATION_LIST_STATE, JsonConvert.SerializeObject(objSesion));

            IEnumerable<ArticulosAlmacen> list = await _dbContext.ArticulosAlmacen.Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            CargarCombo();

            return View(list);
        }

        // GET: ArticulosAlmacenController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Create()
        {
            CargarCombo();
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateConfirmed(string guardar)
        {
            ArticulosAlmacen model = new ArticulosAlmacen();
            await TryUpdateModelAsync<ArticulosAlmacen>(model, "",
                f => f.CentroCosteId,
                f => f.ProductDescription,
                f => f.ProductDescriptionExt,
                f => f.ProductCode,
                f => f.Price,
                f => f.Rate,
                f => f.Rate2
                );

            //Validación de negocio            
            ArticulosAlmacenValidator validator = new ArticulosAlmacenValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                ArticulosAlmacenManager manager = new ArticulosAlmacenManager(_dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                if (guardar == "continuar")
                    return RedirectToAction("Create");
                return RedirectToAction("Index");
            }
            CargarCombo();
            return View(model);
        }

        public IActionResult Edit(Guid id)
        {
            var model = _dbContext.ArticulosAlmacen.Single(f => f.Id.Equals(id));
            CargarCombo();
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(Guid id)
        {
            ArticulosAlmacen model = _dbContext.ArticulosAlmacen.Find(id);

            await TryUpdateModelAsync<ArticulosAlmacen>(model, "",
                f => f.CentroCosteId,
                f => f.ProductDescription,
                f => f.ProductDescriptionExt,
                f => f.ProductCode,
                f => f.Price,
                f => f.Rate,
                f => f.Rate2
                );

            ArticulosAlmacenValidator validator = new ArticulosAlmacenValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                ArticulosAlmacenManager manager = new ArticulosAlmacenManager(_dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }

            CargarCombo();

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (_dbContext.PartesInternosAlmacen.Where(f => f.ArticulosAlmacenId == id).Count() > 0)
            {
                TempData["ErrorMessage"] = "El articulo no se puede borrar porque existen partes de almacen.";
                return RedirectToAction("Index");
            }       


            ArticulosAlmacenValidator validator = new ArticulosAlmacenValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                ArticulosAlmacenManager manager = new ArticulosAlmacenManager(_dbContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

       private void CargarCombo()
        {
            ViewBag.CentrosCosteArtAlma = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
        }

        public async Task<FileResult> ExportCsv(string currentsort = "")
        {
            ArticulosAlmacenFilter filter = new ArticulosAlmacenFilter();
            await TryUpdateModelAsync<ArticulosAlmacenFilter>(filter, "filter",
                f => f.ProductCode,
                f => f.ProductDescription,
                f => f.CentroCosteId
                );

            IEnumerable<ArticulosAlmacen> list = await _dbContext.ArticulosAlmacen
                .Include(f => f.CentroCoste)
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoArticulosAlmacen.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<ArticulosAlmacenMap>();
                await csv.WriteRecordsAsync<ArticulosAlmacen>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

        public async Task<FileResult> ExportPdf(string currentsort = "")
        {
            ArticulosAlmacenFilter filter = new ArticulosAlmacenFilter();
            await TryUpdateModelAsync<ArticulosAlmacenFilter>(filter, "filter",
                f => f.ProductCode,
                f => f.ProductDescription,
                f => f.CentroCosteId
                );

            IEnumerable<ArticulosAlmacen> list = await _dbContext.ArticulosAlmacen
                .Include(f => f.CentroCoste)
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string table = "<table class='w-100'><tr class='font-weight-bold'>" +
                "<td>Código producto</td><td>Descripción producto</td><td>Descripción ext.</td>" +
                "<td>Tipo tarifa</td><td>Tipo tarifa 2</td>" +
                "<td>Precio</td><td>Centro de coste</td>" +
                "</tr>{0}</table>";

            string rows = string.Empty;
            foreach (var item in list)
            {
                rows += $"<tr><td>{item.ProductCode}</td><td>{item.ProductDescription}</td><td>{item.ProductDescriptionExt}</td>" +
                        $"<td>{item.Rate}</td><td>{item.Rate2}</td>" +
                        $"<td>{item.Price}</td><td>{item.CentroCoste.Display}</td>" +
                        $"</tr>";
            }

            var html = PrintService.GetHtmlTheme("Listado de articulos almacen", string.Format(table, rows));
            var pdf = PrintService.GetBytesLandscape(html);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ArticulosAlmacen.pdf";
            return File(pdf, "application/pdf", fileName);
        }

    }
}

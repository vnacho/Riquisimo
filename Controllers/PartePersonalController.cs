using CsvHelper;
using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Core;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Collaborations")]
    public class PartePersonalController : ControlPresupuestarioBaseController
    {
        private readonly ApplicationDbContext _dbContext;

        public PartePersonalController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            PartePersonalViewModel model = new PartePersonalViewModel();
            model.Filter = new PartePersonalFilter();
            if (HttpContext.Session.GetString(Consts.PRESERVE_PARTE_PERSONAL_URL) != null)
            {
                model.Filter = JsonSerializer.Deserialize<PartePersonalFilter>(HttpContext.Session.GetString(Consts.PRESERVE_PARTE_PERSONAL_URL));
            }

            model.Pager = new Pager(await _dbContext.PartePersonal.CountAsync(model.Filter.ExpressionFilter()), null, 10, 5);
            ViewData["Pager"] = model.Pager;
            ViewData["Sort"] = model.Sort;

            model.Items = await _dbContext.PartePersonal.Where(model.Filter.ExpressionFilter())
               .Include(f => f.Personal)
               .Include(f => f.CentroCoste)
               .OrderBy(model.Sort)
               .Skip((model.Pager.Page - 1) * model.Pager.PageSize)
               .Take(model.Pager.PageSize)
               .ToListAsync();

            ViewBag.Personal = _dbContext.Personal.OrderBy(f => f.Nombre);
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);

            return View(model);
        }

        [HttpPost, ActionName("Index")]
        public async Task<IActionResult> IndexConfirmed(int? pag, string sort = "", string currentsort = "", bool reset = false, bool recalcular = false)
        {   
            if (reset)
            {
                HttpContext.Session.Remove(Consts.PRESERVE_PARTE_PERSONAL_URL);
                return RedirectToAction("Index");
            }

            PartePersonalViewModel model = new PartePersonalViewModel();
            if (string.IsNullOrWhiteSpace(sort))
            {
                if (!string.IsNullOrWhiteSpace(currentsort))
                    model.Sort = currentsort;
            }
            else
                model.Sort = sort;

            model.Filter = new PartePersonalFilter();

            if (HttpContext.Session.GetString(Consts.PRESERVE_PARTE_PERSONAL_URL) != null)
                model.Filter = JsonSerializer.Deserialize<PartePersonalFilter>(HttpContext.Session.GetString(Consts.PRESERVE_PARTE_PERSONAL_URL));

            await TryUpdateModelAsync<PartePersonalFilter>(model.Filter, "filter",
                f => f.PersonalId,
                f => f.CentroCosteId,
                f => f.FechaDesde,
                f => f.FechaHasta
                );

            HttpContext.Session.SetString(Consts.PRESERVE_PARTE_PERSONAL_URL, JsonSerializer.Serialize(model.Filter));

            model.Pager = new Pager(await _dbContext.PartePersonal.CountAsync(model.Filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = model.Pager;
            ViewData["Sort"] = sort;

            model.Items = await _dbContext.PartePersonal.Where(model.Filter.ExpressionFilter())
                .Include(f => f.Personal)
                .Include(f => f.CentroCoste)
                .OrderBy(model.Sort)
                .Skip((model.Pager.Page - 1) * model.Pager.PageSize)
                .Take(model.Pager.PageSize)
                .ToListAsync();

            if (recalcular)
            {
                PartePersonalManager manager = new PartePersonalManager(_dbContext);
                int total = await manager.Recalcular(model.Filter);
                TempData["Message"] = $"Se han recalculado {total} partes de personal.";
            }

            ViewBag.Personal = _dbContext.Personal.OrderBy(f => f.Nombre);
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);

            return View(model);
        }

        //public async Task<IActionResult> Recalcular(int? pag, string sort = "", string currentsort = "")
        //{
        //    PartePersonalFilter filter = new PartePersonalFilter();
        //    await TryUpdateModelAsync<PartePersonalFilter>(filter, "filter",
        //        f => f.PersonalId,
        //        f => f.CentroCosteId,
        //        f => f.FechaDesde,
        //        f => f.FechaHasta
        //        );

        //    PartePersonalManager manager = new PartePersonalManager(_dbContext);
        //    int total = await manager.Recalcular(filter);
        //    TempData["Message"] = $"Se han recalculado {total} partes de personal.";

        //    if (string.IsNullOrWhiteSpace(sort))
        //    {
        //        if (string.IsNullOrWhiteSpace(currentsort))
        //            sort = "Id desc";
        //        else
        //            sort = currentsort;
        //    }

        //    Pager pager = new Pager(await _dbContext.PartePersonal.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
        //    ViewData["Pager"] = pager;
        //    ViewData["Sort"] = sort;

        //    IEnumerable<PartePersonal> list = await _dbContext.PartePersonal.Where(filter.ExpressionFilter())
        //        .Include(f => f.Personal)
        //        .Include(f => f.CentroCoste)
        //        .OrderBy(sort)
        //        .Skip((pager.Page - 1) * pager.PageSize)
        //        .Take(pager.PageSize)
        //        .ToListAsync();

        //    ViewBag.Personal = _dbContext.Personal.OrderBy(f => f.Nombre);
        //    ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);

        //    return RedirectToAction("Index", list);
        //}

        public async Task<FileResult> ExportCsv(string currentsort = "")
        {
            PartePersonalFilter filter = new PartePersonalFilter();
            await TryUpdateModelAsync<PartePersonalFilter>(filter, "filter",
                f => f.PersonalId,
                f => f.CentroCosteId,
                f => f.FechaDesde,
                f => f.FechaHasta
                );

            IEnumerable<PartePersonal> list = await _dbContext.PartePersonal
                .Include(f => f.Personal)
                .Include(f => f.CentroCoste)
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoPartesPersonal.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<PartePersonalMap>();
                await csv.WriteRecordsAsync<PartePersonal>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

        public IActionResult Create()
        {
            ViewBag.Personal = _dbContext.Personal.OrderBy(f => f.Nombre);
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed(string guardar)
        {
            PartePersonal model = new PartePersonal();
            model.TipoParte = "P";
            await TryUpdateModelAsync<PartePersonal>(model, "",
                f => f.Fecha,
                f => f.Unidades,
                f => f.Precio,
                f => f.Importe,
                f => f.PersonalId,
                f => f.CentroCosteId);

            model.Calcular();

            //Validación de negocio            
            PartePersonalValidator validator = new PartePersonalValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                PartePersonalManager manager = new PartePersonalManager(_dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                if (guardar == "continuar")
                    return RedirectToAction("Create");
                return RedirectToAction("Index");
            }
            ViewBag.Personal = _dbContext.Personal.OrderBy(f => f.Nombre);
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = _dbContext.PartePersonal.Single(f => f.Id == id);
            ViewBag.Personal = _dbContext.Personal.OrderBy(f => f.Nombre);
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            PartePersonal model = _dbContext.PartePersonal.Find(id);

            await TryUpdateModelAsync<PartePersonal>(model, "",
                f => f.TipoParte,
                f => f.Fecha,
                f => f.Unidades,
                f => f.Precio,
                f => f.Importe,
                f => f.PersonalId,
                f => f.CentroCosteId);

            model.Calcular();

            PartePersonalValidator validator = new PartePersonalValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                PartePersonalManager manager = new PartePersonalManager(_dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }
            ViewBag.Personal = _dbContext.Personal.OrderBy(f => f.Nombre);
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            PartePersonalValidator validator = new PartePersonalValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                PartePersonalManager manager = new PartePersonalManager(_dbContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<PartialViewResult> Refresh()
        {
            PartePersonal model = new PartePersonal();
            await TryUpdateModelAsync<PartePersonal>(model, "",
                f => f.Id,
                f => f.TipoParte,
                f => f.Fecha,
                f => f.Unidades,
                f => f.Precio,
                f => f.Importe,
                f => f.PersonalId,
                f => f.CentroCosteId);

            if (model.PersonalId != 0 && model.Precio == 0)
            {
                var personal = _dbContext.Personal.Find(model.PersonalId);
                if (personal != null && personal.CosteEstandar.HasValue)
                    model.Precio = personal.CosteEstandar.Value;
            }

            model.Calcular();

            ViewBag.Personal = _dbContext.Personal.OrderBy(f => f.Nombre);
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);

            ModelState.Clear();
            return PartialView("~/Views/Shared/EditorTemplates/PartePersonal.cshtml", model);

        }
    }
}

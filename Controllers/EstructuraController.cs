using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Collaborations")]
    public class EstructuraController : ControlPresupuestarioBaseController
    {
        private readonly ApplicationDbContext _dbContext;

        public EstructuraController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int? pag, string sort = "", string currentsort = "", bool reset = false, bool export = false)
        {
            if (reset)
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(sort))
            {
                if (string.IsNullOrWhiteSpace(currentsort))
                    sort = "Id desc";
                else
                    sort = currentsort;
            }

            EstructuraFilter filter = new EstructuraFilter();
            await TryUpdateModelAsync<EstructuraFilter>(filter, "filter",
                f => f.CentroCosteId);

            Pager pager = new Pager(await _dbContext.Estructura.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<Estructura> list = await _dbContext.Estructura.Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed()
        {
            Estructura model = new Estructura();
            await TryUpdateModelAsync<Estructura>(model, "",
                f => f.PorcentajeReparto,
                f => f.CentroCosteId);

            //Validación de negocio            
            EstructuraValidator validator = new EstructuraValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                EstructuraManager manager = new EstructuraManager(_dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = _dbContext.Estructura.Single(f => f.Id == id);
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            Estructura model = _dbContext.Estructura.Find(id);

            await TryUpdateModelAsync<Estructura>(model, "",
                f => f.PorcentajeReparto,
                f => f.CentroCosteId);

            EstructuraValidator validator = new EstructuraValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                EstructuraManager manager = new EstructuraManager(_dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            EstructuraValidator validator = new EstructuraValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                EstructuraManager manager = new EstructuraManager(_dbContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }
    }
}

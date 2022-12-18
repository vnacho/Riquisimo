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
    public class TipoTarifaController : ControlPresupuestarioBaseController
    {
        private readonly ApplicationDbContext _dbContext;

        public TipoTarifaController(ApplicationDbContext dbContext)
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

            TipoTarifaFilter filter = new TipoTarifaFilter();
            await TryUpdateModelAsync<TipoTarifaFilter>(filter, "filter",
                f => f.Nombre,
                f => f.Codigo);

            Pager pager = new Pager(await _dbContext.TiposTarifa.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<TipoTarifa> list = await _dbContext.TiposTarifa.Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed()
        {
            TipoTarifa model = new TipoTarifa();
            await TryUpdateModelAsync<TipoTarifa>(model, "",
                f => f.Codigo,
                f => f.Nombre);

            //Validación de negocio            
            TipoTarifaValidator validator = new TipoTarifaValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                TipoTarifaManager manager = new TipoTarifaManager(_dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = _dbContext.TiposTarifa.Single(f => f.Id == id);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            TipoTarifa model = _dbContext.TiposTarifa.Find(id);

            await TryUpdateModelAsync<TipoTarifa>(model, "",
                f => f.Codigo,
                f => f.Nombre);

            TipoTarifaValidator validator = new TipoTarifaValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                TipoTarifaManager manager = new TipoTarifaManager(_dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            TipoTarifaValidator validator = new TipoTarifaValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                TipoTarifaManager manager = new TipoTarifaManager(_dbContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }
    }
}

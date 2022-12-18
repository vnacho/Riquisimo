using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Core;
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
    public class TipoContratoController : ControlPresupuestarioBaseController
    {
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComuContext;
        private readonly ApplicationDbContext _dbContext;

        public TipoContratoController(SageContext sageContext, SageComuContext sageComuContext, ApplicationDbContext dbContext)
        {
            _sageContext = sageContext;
            _sageComuContext = sageComuContext;
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

            TipoContratoFilter filter = new TipoContratoFilter();
            await TryUpdateModelAsync<TipoContratoFilter>(filter, "filter",
                f => f.Nombre);

            Pager pager = new Pager(await _dbContext.TipoContrato.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<TipoContrato> list = await _dbContext.TipoContrato.Where(filter.ExpressionFilter())
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
            TipoContrato model = new TipoContrato();
            await TryUpdateModelAsync<TipoContrato>(model, "",
                f => f.Nombre);

            //Validación de negocio            
            TipoContratoValidator validator = new TipoContratoValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                TipoContratoManager manager = new TipoContratoManager(_dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = _dbContext.TipoContrato.Single(f => f.Id == id);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            TipoContrato model = _dbContext.TipoContrato.Find(id);

            await TryUpdateModelAsync<TipoContrato>(model, "",
                f => f.Nombre);

            TipoContratoValidator validator = new TipoContratoValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                TipoContratoManager manager = new TipoContratoManager(_dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            TipoContratoValidator validator = new TipoContratoValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                TipoContratoManager manager = new TipoContratoManager(_dbContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }
    }
}

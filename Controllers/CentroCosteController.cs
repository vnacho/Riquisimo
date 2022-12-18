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
    public class CentroCosteController : ControlPresupuestarioBaseController
    {
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComuContext;
        private readonly ApplicationDbContext _dbContext;

        public CentroCosteController(SageContext sageContext, SageComuContext sageComuContext, ApplicationDbContext dbContext)
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

            CentroCosteFilter filter = new CentroCosteFilter();
            await TryUpdateModelAsync<CentroCosteFilter>(filter, "filter",
                f => f.Nombre,
                f => f.NivelAnalitico1,
                f => f.NivelAnalitico2,
                f => f.TipoCentroCosteId);

            Pager pager = new Pager(await _dbContext.CentrosCoste.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<CentroCoste> list = await _dbContext.CentrosCoste.Include(f => f.TipoCentroCoste).Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            ViewBag.TiposCentroCoste = _dbContext.TiposCentroCoste.OrderBy(f => f.Descripcion);
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.TiposCentroCoste = _dbContext.TiposCentroCoste.OrderBy(f => f.Descripcion);
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed()
        {
            CentroCoste model = new CentroCoste();
            await TryUpdateModelAsync<CentroCoste>(model, "",
                f => f.Nombre,
                f => f.NivelAnalitico1,
                f => f.NivelAnalitico2,
                f => f.TipoCentroCosteId);

            //Validación de negocio            
            CentroCosteValidator validator = new CentroCosteValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            int controlRepetidos = _dbContext.CentrosCoste.Where(f => f.NivelAnalitico2.Equals(model.NivelAnalitico2)).Count();

            if (ModelState.IsValid && controlRepetidos==0)
            {
                CentroCosteManager manager = new CentroCosteManager(_dbContext, _sageContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }

            if (controlRepetidos > 0)
                TempData["ErrorMessage"] = string.Concat("Existen ", controlRepetidos.ToString(), " registro/s con el numero Analítico2 ", model.NivelAnalitico2, ".");

            ViewBag.TiposCentroCoste = _dbContext.TiposCentroCoste.OrderBy(f => f.Descripcion);
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = _dbContext.CentrosCoste.Single(f => f.Id == id);
            ViewBag.TiposCentroCoste = _dbContext.TiposCentroCoste.OrderBy(f => f.Descripcion);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            CentroCoste model = _dbContext.CentrosCoste.Find(id);

            await TryUpdateModelAsync<CentroCoste>(model, "",
                f => f.Nombre,
                f => f.NivelAnalitico1,
                f => f.NivelAnalitico2,
                f => f.TipoCentroCosteId);

            int controlRepetidos = _dbContext.CentrosCoste.Where(f => f.NivelAnalitico2.Equals(model.NivelAnalitico2) && f.Id!=id ).Count();

            CentroCosteValidator validator = new CentroCosteValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid && controlRepetidos==0)
            {
                CentroCosteManager manager = new CentroCosteManager(_dbContext, _sageContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }
            if (controlRepetidos>0)
                TempData["ErrorMessage"] = string.Concat("Existen ", controlRepetidos.ToString(), " registro/s con el numero Analítico2 ", model.NivelAnalitico2, ".");

            ViewBag.TiposCentroCoste = _dbContext.TiposCentroCoste.OrderBy(f => f.Descripcion);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            CentroCosteValidator validator = new CentroCosteValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                CentroCosteManager manager = new CentroCosteManager(_dbContext, _sageContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }
        public async Task<string> NivelAnalitico2ExistsCreate(string NivelAnalitico2)
        {
            CentroCoste model = new CentroCoste();
            await TryUpdateModelAsync<CentroCoste>(model, "");

            if (NivelAnalitico2.Length < 4)
                return "false";

            var centroCosteLoncal = await _dbContext.CentrosCoste.FirstOrDefaultAsync(c => c.NivelAnalitico2.Trim().ToLower().Equals(NivelAnalitico2.ToLower().Trim()));

            if (centroCosteLoncal != null)
                return "borrar";
            else
                return "false";

        }

    }
}

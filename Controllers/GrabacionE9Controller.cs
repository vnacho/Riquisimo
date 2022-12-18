using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
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
    public class GrabacionE9Controller : ControlPresupuestarioBaseController
    {
        private readonly ApplicationDbContext _dbContext;

        public GrabacionE9Controller(ApplicationDbContext dbContext)
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

            GrabacionE9Filter filter = new GrabacionE9Filter();
            await TryUpdateModelAsync<GrabacionE9Filter>(filter, "filter",
                f => f.CentroCosteId,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EntradaSalida,
                f => f.DestinoId);

            Pager pager = new Pager(await _dbContext.GrabacionE9.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<GrabacionE9> list = await _dbContext.GrabacionE9.Where(filter.ExpressionFilter())
                .Include(f => f.CentroCoste)
                .Include(f => f.Destino)
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();
            
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            ViewBag.CentrosCosteE9 = _dbContext.CentrosCoste.Where(f => f.TipoCentroCoste.Tipo == Consts.CODIGO_TIPO_CENTRO_COSTE_E9).OrderBy(f => f.Nombre);
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            ViewBag.CentrosCosteE9 = _dbContext.CentrosCoste.Where(f => f.TipoCentroCoste.Tipo == Consts.CODIGO_TIPO_CENTRO_COSTE_E9).OrderBy(f => f.Nombre);
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed()
        {
            GrabacionE9 model = new GrabacionE9();
            await TryUpdateModelAsync<GrabacionE9>(model, "",
                f => f.Fecha,
                f => f.Descripcion,
                f => f.EntradaSalida,
                f => f.Importe,
                f => f.CentroCosteId,
                f => f.DestinoId
                );

            //Validación de negocio            
            GrabacionE9Validator validator = new GrabacionE9Validator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                GrabacionE9Manager manager = new GrabacionE9Manager(_dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            ViewBag.CentrosCosteE9 = _dbContext.CentrosCoste.Where(f => f.TipoCentroCoste.Tipo == Consts.CODIGO_TIPO_CENTRO_COSTE_E9).OrderBy(f => f.Nombre);
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = _dbContext.GrabacionE9.Single(f => f.Id == id);
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            ViewBag.CentrosCosteE9 = _dbContext.CentrosCoste.Where(f => f.TipoCentroCoste.Tipo == Consts.CODIGO_TIPO_CENTRO_COSTE_E9).OrderBy(f => f.Nombre);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            GrabacionE9 model = _dbContext.GrabacionE9.Find(id);

            await TryUpdateModelAsync<GrabacionE9>(model, "",
                f => f.Fecha,
                f => f.Descripcion,
                f => f.EntradaSalida,
                f => f.Importe,
                f => f.CentroCosteId,
                f => f.DestinoId
                );

            GrabacionE9Validator validator = new GrabacionE9Validator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                GrabacionE9Manager manager = new GrabacionE9Manager(_dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }
            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            ViewBag.CentrosCosteE9 = _dbContext.CentrosCoste.Where(f => f.TipoCentroCoste.Tipo == Consts.CODIGO_TIPO_CENTRO_COSTE_E9).OrderBy(f => f.Nombre);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            GrabacionE9Validator validator = new GrabacionE9Validator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                GrabacionE9Manager manager = new GrabacionE9Manager(_dbContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }
    }
}

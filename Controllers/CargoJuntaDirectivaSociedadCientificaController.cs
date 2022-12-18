using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Congress")]
    public class CargoJuntaDirectivaSociedadCientificaController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CargoJuntaDirectivaSociedadCientificaController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
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

            CargoJuntaDirectivaSociedadCientificaFilter filter = new CargoJuntaDirectivaSociedadCientificaFilter();
            await TryUpdateModelAsync<CargoJuntaDirectivaSociedadCientificaFilter>(filter, "filter",
                f => f.Nombre);

            Pager pager = new Pager(await dbContext.CargosJuntaDirectivaSociedadCientifica.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<CargoJuntaDirectivaSociedadCientifica> list = await dbContext.CargosJuntaDirectivaSociedadCientifica.AsNoTracking().Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .Select(y => new CargoJuntaDirectivaSociedadCientifica
                {
                    Id = y.Id,
                    Nombre = y.Nombre
                })
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
            CargoJuntaDirectivaSociedadCientifica model = new CargoJuntaDirectivaSociedadCientifica();
            await TryUpdateModelAsync<CargoJuntaDirectivaSociedadCientifica>(model, "",
                f => f.Nombre
            );

            //Validación de negocio            
            CargoJuntaDirectivaSociedadCientificaValidator validator = new CargoJuntaDirectivaSociedadCientificaValidator(dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                CargoJuntaDirectivaSociedadCientificaManager manager = new CargoJuntaDirectivaSociedadCientificaManager(dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = dbContext.CargosJuntaDirectivaSociedadCientifica.AsNoTracking().Single(f => f.Id == id);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            CargoJuntaDirectivaSociedadCientifica model = dbContext.CargosJuntaDirectivaSociedadCientifica.Find(id);

            await TryUpdateModelAsync<CargoJuntaDirectivaSociedadCientifica>(model, "",
                f => f.Nombre
            );

            CargoJuntaDirectivaSociedadCientificaValidator validator = new CargoJuntaDirectivaSociedadCientificaValidator(dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                CargoJuntaDirectivaSociedadCientificaManager manager = new CargoJuntaDirectivaSociedadCientificaManager(dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            CargoJuntaDirectivaSociedadCientificaValidator validator = new CargoJuntaDirectivaSociedadCientificaValidator(dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                CargoJuntaDirectivaSociedadCientificaManager manager = new CargoJuntaDirectivaSociedadCientificaManager(dbContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }
    }
}

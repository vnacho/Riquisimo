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
    public class SocioSociedadCientificaController : Controller
    {
        private readonly ApplicationDbContext db;

        public SocioSociedadCientificaController(ApplicationDbContext db)
        {
            this.db = db;
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

            SocioSociedadCientificaFilter filter = new SocioSociedadCientificaFilter();
            await TryUpdateModelAsync<SocioSociedadCientificaFilter>(filter, "filter",
                f => f.Nombre);

            Pager pager = new Pager(await db.SociosSociedadCientifica.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<SocioSociedadCientifica> list = await db.SociosSociedadCientifica.AsNoTracking()
                .Include(f => f.SociedadCientifica)
                .Include(f => f.CargoJuntaDirectivaSociedadCientifica)
                .Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            return View(list);
        }

        public IActionResult Create()
        {
            LoadViewBag();
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed()
        {
            SocioSociedadCientifica model = new SocioSociedadCientifica();
            await TryUpdateModelAsync<SocioSociedadCientifica>(model, "",
                f => f.SociedadCientificaId,
                f => f.CargoJuntaDirectivaSociedadCientificaId,
                f => f.NIF,
                f => f.Nombre,
                f => f.Apellidos,
                f => f.JuntaDirectiva,
                f => f.FechaInicioCargo,
                f => f.FechaFinCargo
            );

            //Validación de negocio            
            SocioSociedadCientificaValidator validator = new SocioSociedadCientificaValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                SocioSociedadCientificaManager manager = new SocioSociedadCientificaManager(db);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }
            LoadViewBag();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = db.SociosSociedadCientifica.AsNoTracking().Single(f => f.Id == id);
            LoadViewBag();
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            SocioSociedadCientifica model = db.SociosSociedadCientifica.Find(id);

            await TryUpdateModelAsync<SocioSociedadCientifica>(model, "",
                f => f.SociedadCientificaId,
                f => f.CargoJuntaDirectivaSociedadCientificaId,
                f => f.NIF,
                f => f.Nombre,
                f => f.Apellidos,
                f => f.JuntaDirectiva,
                f => f.FechaInicioCargo,
                f => f.FechaFinCargo
            );


            SocioSociedadCientificaValidator validator = new SocioSociedadCientificaValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                SocioSociedadCientificaManager manager = new SocioSociedadCientificaManager(db);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }
            LoadViewBag();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            SocioSociedadCientificaValidator validator = new SocioSociedadCientificaValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                SocioSociedadCientificaManager manager = new SocioSociedadCientificaManager(db);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        private void LoadViewBag()
        {
            ViewBag.SociedadesCientificas = db.SociedadesCientificas.OrderBy(f => f.Nombre);
            ViewBag.Cargos = db.CargosJuntaDirectivaSociedadCientifica.OrderBy(f => f.Nombre);
        }
    }
}


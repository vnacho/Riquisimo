using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Helpers;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Congress")]
    public class AsistenteController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SageContext _sage;
        private readonly SageComuContext _sagecomu;

        public AsistenteController(ApplicationDbContext dbContext, SageContext sage, SageComuContext sagecomu)
        {
            _dbContext = dbContext;
            _sage = sage;
            _sagecomu = sagecomu;
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

            AsistenteFilter filter = new AsistenteFilter();
            await TryUpdateModelAsync<AsistenteFilter>(filter, "filter",
                f => f.NIF,
                f => f.Nombre,
                f => f.Apellidos);

            Pager pager = new Pager(await _dbContext.Asistente.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<Asistente> list = await _dbContext.Asistente.Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Paises = _sagecomu.paises.OrderBy(f => f.NOMBRE);
            ViewBag.Treatments = _dbContext.Treatments.ToList();
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed()
        {
            Asistente model = new Asistente();
            await TryUpdateModelAsync<Asistente>(model, "",
                f => f.NIF,
                f => f.TreatmentId,
                f => f.Nombre,
                f => f.Apellidos,
                f => f.CentroTrabajo,
                f => f.CategoriaProfesional,
                f => f.Cargo,
                f => f.FechaActualizacionCargo,
                f => f.Direccion,
                f => f.Poblacion,
                f => f.Ciudad,
                f => f.Telefono1,
                f => f.Telefono2,
                f => f.Email1,
                f => f.Email2,
                f => f.Asociacion,
                f => f.CodigoPais,
                f => f.CodigoPostal);

            model.NIF = NIFHelper.Format(model.NIF);

            //Validación de negocio            
            AsistenteValidator validator = new AsistenteValidator(_dbContext, _sage);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.CodigoPais))
                    model.NombrePais = _sagecomu.paises.Find(model.CodigoPais).NOMBRE;

                AsistenteManager manager = new AsistenteManager(_dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.Paises = _sagecomu.paises.OrderBy(f => f.NOMBRE);
            ViewBag.Treatments = _dbContext.Treatments.ToList();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = _dbContext.Asistente.Single(f => f.Id == id);
            ViewBag.Paises = _sagecomu.paises.OrderBy(f => f.NOMBRE);
            ViewBag.Treatments = _dbContext.Treatments.ToList();
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            Asistente model = _dbContext.Asistente.Find(id);

            await TryUpdateModelAsync<Asistente>(model, "",
               f => f.NIF,
               f => f.TreatmentId,
               f => f.Nombre,
               f => f.Apellidos,
               f => f.CentroTrabajo,
               f => f.CategoriaProfesional,
               f => f.Cargo,
               f => f.FechaActualizacionCargo,
               f => f.Direccion,
               f => f.Poblacion,
               f => f.Ciudad,
               f => f.Telefono1,
               f => f.Telefono2,
               f => f.Email1,
               f => f.Email2,
               f => f.Asociacion,
               f => f.CodigoPais,
               f => f.CodigoPostal);

            model.NIF = NIFHelper.Format(model.NIF);

            AsistenteValidator validator = new AsistenteValidator(_dbContext, _sage);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.CodigoPais))
                    model.NombrePais = _sagecomu.paises.Find(model.CodigoPais).NOMBRE;
                else
                    model.NombrePais = string.Empty;

                AsistenteManager manager = new AsistenteManager(_dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }
            ViewBag.Paises = _sagecomu.paises.OrderBy(f => f.NOMBRE);
            ViewBag.Treatments = _dbContext.Treatments.ToList();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            AsistenteValidator validator = new AsistenteValidator(_dbContext, _sage);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                AsistenteManager manager = new AsistenteManager(_dbContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }
    }
}

using Ferpuser.BLL.Filters;
using Ferpuser.Data;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Core;
using Ferpuser.Models.SessionObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Ferpuser.Models;
using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Managers;
using System.ComponentModel.DataAnnotations;
//using System.Data.Entity;

namespace Ferpuser.Controllers
{
    //[Authorize(Policy = "Compras")]

    public class TiendasController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public TiendasController(ApplicationDbContext dbContext)
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

            Pager pager = null;
            TiendaFilter filter;
            //string sSesion = HttpContext.Session.GetString(Consts.SESSION_REGISTRATION_LIST_STATE);
            //var previous = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            //TiendaSession objSesion;

            //if (!string.IsNullOrWhiteSpace(previous) && previous.Contains("/Tiendas/Edit", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(sSesion))
            //{
            //    objSesion = JsonConvert.DeserializeObject<TiendaSession>(sSesion);
            //    sort = objSesion.sort;
            //    filter = objSesion.filter;
            //    pager = new Pager(await _dbContext.Tiendas.Where(r => r.Deleted == null).CountAsync(filter.ExpressionFilter()), objSesion.page, 50, 5);
            //}
            //else
            //{
                if (string.IsNullOrWhiteSpace(sort))
                {
                    if (string.IsNullOrWhiteSpace(currentsort))
                        sort = "nombre desc";
                    else
                        sort = currentsort;
                }

                filter = new TiendaFilter();

                await TryUpdateModelAsync<TiendaFilter>(filter, "filter",
                    f => f.nombre);

                //pager = new Pager(await _dbContext.Tiendas.CountAsync(filter.ExpressionFilter()), pag, 100, 5);
            //}

            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;
            ViewData["Filter"] = filter;

            //objSesion = new TiendaSession()
            //{
            //    filter = filter,
            //    sort = sort,
            //    page = pager.Page
            //};

            //HttpContext.Session.SetString(Consts.SESSION_REGISTRATION_LIST_STATE, JsonConvert.SerializeObject(objSesion));

            IEnumerable<Tienda> list = await _dbContext.Tiendas.Where(filter.ExpressionFilter())
                .OrderBy(f=>f.nombre)
                .ToListAsync();
                //.Skip((pager.Page - 1) * pager.PageSize)
                //.Take(pager.PageSize)

            //CargarCombo();

            return View(list);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateConfirmed(string guardar)
        {
            Tienda model = new Tienda();
            await TryUpdateModelAsync<Tienda>(model, "");

            //Validación de negocio            
            TiendaValidator validator = new TiendaValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            //if (ModelState.IsValid)
            //{
                TiendaManager manager = new TiendaManager(_dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                if (guardar == "continuar")
                    return RedirectToAction("Create");
                return RedirectToAction("Index");
            //}

            return View(model);
        }

        public IActionResult Edit(Guid id)
        {
            var model = _dbContext.Tiendas.Single(f => f.Id.Equals(id));
            //CargarCombo();
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(Guid id)
        {
            Tienda model = _dbContext.Tiendas.Find(id);

            await TryUpdateModelAsync<Tienda>(model, "" );

            TiendaValidator validator = new TiendaValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            //if (ModelState.IsValid)
            //{
                TiendaManager manager = new TiendaManager(_dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            //}

            //CargarCombo();

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (_dbContext.Tiendas.Where(f => f.Id == id).Count() > 0)
            {
                TempData["ErrorMessage"] = "El articulo no se puede borrar porque existen partes de almacen.";
                return RedirectToAction("Index");
            }


            TiendaValidator validator = new TiendaValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                TiendaManager manager = new TiendaManager(_dbContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}

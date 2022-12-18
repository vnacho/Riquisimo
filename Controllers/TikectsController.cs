using DocumentFormat.OpenXml.ExtendedProperties;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Ferpuser.BLL.BusinessValidators;
using System.ComponentModel.DataAnnotations;
using System;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Core;
using Ferpuser.BLL.Filters;
using Microsoft.AspNetCore.Http;

namespace Ferpuser.Controllers
{
    public class TikectsController : ControlPresupuestarioBaseController
    {
        private readonly ApplicationDbContext _context;

        public TikectsController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(int? pag, string sort = "", string currentsort = "", bool reset = false, bool export = false)
        {
            //Tikect model = new Tikect();// { Year = DateTime.Now.Year };
            //CargarCombo();

            if (reset)
            {
                HttpContext.Session.Remove(Consts.SESSION_REGISTRATION_LIST_STATE);
                return RedirectToAction("Index");
            }

            Pager pager = null;
            TikectFilter filter;
            //string sSesion = HttpContext.Session.GetString(Consts.SESSION_REGISTRATION_LIST_STATE);
            //var previous = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            //TikectSession objSesion;

            //if (!string.IsNullOrWhiteSpace(previous) && previous.Contains("/Tikects/Edit", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(sSesion))
            //{
            //    objSesion = JsonConvert.DeserializeObject<TikectSession>(sSesion);
            //    sort = objSesion.sort;
            //    filter = objSesion.filter;
            //    pager = new Pager(await _context.Tikects.Where(r => r.Deleted == null).CountAsync(filter.ExpressionFilter()), objSesion.page, 50, 5);
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

            filter = new TikectFilter();

            await TryUpdateModelAsync<TikectFilter>(filter, "filter");

            //pager = new Pager(await _context.Tikects.CountAsync(filter.ExpressionFilter()), pag, 100, 5);
            //}

            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;
            ViewData["Filter"] = filter;

            //objSesion = new TikectSession()
            //{
            //    filter = filter,
            //    sort = sort,
            //    page = pager.Page
            //};

            //HttpContext.Session.SetString(Consts.SESSION_REGISTRATION_LIST_STATE, JsonConvert.SerializeObject(objSesion));

            IEnumerable<Tikect> list = await _context.Tikects.Where(filter.ExpressionFilter())
                .Include(f=>f.tienda)
                .ToListAsync();
            //.Skip((pager.Page - 1) * pager.PageSize)
            //.Take(pager.PageSize)

            CargarCombo();

            return View(list);
        }

        public IActionResult Create()
        {
            CargarCombo();
            Tikect modelo = new Tikect();
            return View(modelo);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateConfirmed(string guardar)
        {
            Tikect model = new Tikect();
            await TryUpdateModelAsync<Tikect>(model, "");

            //Validación de negocio            
            TikectValidator validator = new TikectValidator(_context);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                TikectManager manager = new TikectManager(_context);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                if (guardar == "continuar")
                    return RedirectToAction("Create");
                return RedirectToAction("Index");
            }

            CargarCombo();
            return View(model);
        }



        public IActionResult Edit(Guid id)
        {
            var model = _context.Tikects.Single(f => f.Id.Equals(id));
            CargarCombo();
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(Guid id)
        {
            Tikect model = _context.Tikects.Find(id);

            await TryUpdateModelAsync<Tikect>(model, "");

            //TikectValidator validator = new TikectValidator(_context);
            //IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            //this.AddValidationErrors(businessErrors);

            //if (ModelState.IsValid)
            //{
                TikectManager manager = new TikectManager(_context);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
            //if (guardar == "continuar")
                return RedirectToAction("Index");
            //}

            CargarCombo();

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            //if (_context.Tikects.Where(f => f.Id == id).Count() > 0)
            //{
            //    TempData["ErrorMessage"] = "El tikect no se puede borrar porque existen.";
            //    return RedirectToAction("Index");
            //}


            TikectValidator validator = new TikectValidator(_context);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                TikectManager manager = new TikectManager(_context);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        private void CargarCombo()
        {
            ViewBag.Tiendas = _context.Tiendas.OrderBy(f => f.nombre);
        }
    }
}

using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ParametroController : Controller
    {
        private readonly ApplicationDbContext db;
        public ParametroController(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public async Task<IActionResult> Index(int? pag, string sort = "", string currentsort = "", bool reset = false)
        {
            //Guardar estado de listado con paginación, ordenación y filtros
            string url = HttpContext.Session.GetString(Consts.PRESERVE_PARAMETRO_URL);
            if (!string.IsNullOrWhiteSpace(url))
            {
                HttpContext.Session.Remove(Consts.PRESERVE_PARAMETRO_URL);
                if (url.Contains("Parametro")) //Puede venir de otra página por lo que realizamos esa comprobación
                    return Redirect(url);
            }
            if (reset)
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(sort))
            {
                if (string.IsNullOrWhiteSpace(currentsort))
                    sort = "Codigo asc";
                else
                    sort = currentsort;
            }

            ParametroFilter filter = new ParametroFilter()
            {
                ExcluirDatosEmpresa = true
            };
            await TryUpdateModelAsync<ParametroFilter>(filter, "filter",
                f => f.Codigo);

            Pager pager = new Pager(await db.Parametros.CountAsync(filter.ExpressionFilter()), pag, 100, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<Parametro> list = await db.Parametros                
                .Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            return View(list);
        }

        public IActionResult Edit(string id)
        {
            var url = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            if (url.EndsWith(nameof(Parametro)) || url.EndsWith($"{nameof(Parametro)}/") || url.Contains("Index") || url.Contains("Parametro?")) //Guardar filtros y paginación de index
                HttpContext.Session.SetString(Consts.PRESERVE_PERSONAL_URL, url);
            var model = db.Parametros.Single(f => f.Codigo == id);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(string id)
        {
            Parametro model = db.Parametros.Find(id);

            await TryUpdateModelAsync<Parametro>(model, "", f => f.Valor);

            ParametroValidator validator = new ParametroValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                ParametroManager manager = new ParametroManager(db);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}

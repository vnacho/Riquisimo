using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Collaborations")]
    public class ControlPresupuestarioController : ControlPresupuestarioBaseController
    {
        public ControlPresupuestarioManager _manager { get; set; }
        public SecundarManager _secundarManager { get; set; }        

        public ControlPresupuestarioController(ControlPresupuestarioManager manager,  SecundarManager secundarManager)
        {
            _manager = manager;
            _secundarManager = secundarManager;            
        }

        public IActionResult Index()
        {            
            ViewBag.TiposCoste = _secundarManager.GetTiposCosteSelectList();
            ViewBag.CentrosCoste = _secundarManager.GetCentrosCosteSelectList(string.Empty);
            ModelState.Clear();
            return View();
        }

        [HttpPost, ActionName("Index")]
        public async Task<IActionResult> IndexConfirmed()
        {
            ControlPresupuestarioViewModel model = new ControlPresupuestarioViewModel();
            ControlPresupuestarioFilter filter = new ControlPresupuestarioFilter();
            await TryUpdateModelAsync<ControlPresupuestarioFilter>(filter, "",
                f => f.Tipo,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.CentroCosteCode,
                f => f.TipoCosteCode,
                f => f.PrefijoCuenta);

            model.Filter = filter;

            ViewBag.TiposCoste = _secundarManager.GetTiposCosteSelectList();
            ViewBag.CentrosCoste = _secundarManager.GetCentrosCosteSelectList(filter.TipoCosteCode);

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(filter.TipoCosteCode))
                    ViewBag.TipoCosteNombre = ((ViewBag.TiposCoste) as List<SelectListItem>).SingleOrDefault(f => f.Value == filter.TipoCosteCode).Text;
                if (!string.IsNullOrWhiteSpace(filter.CentroCosteCode))
                    ViewBag.CentroCosteNombre = ((ViewBag.CentrosCoste) as List<SelectListItem>).SingleOrDefault(f => f.Value == filter.CentroCosteCode).Text;

                var list = _manager.Get(filter);
                model.Items = list;
                return View(model);
            }

            return View(model);
        }

        public async Task<PartialViewResult> Filtros()
        {
            ControlPresupuestarioFilter filter = new ControlPresupuestarioFilter();
            await TryUpdateModelAsync<ControlPresupuestarioFilter>(filter, "",
                f => f.Tipo,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.CentroCosteCode,
                f => f.TipoCosteCode);

            ViewBag.TiposCoste = _secundarManager.GetTiposCosteSelectList();
            ViewBag.CentrosCoste = _secundarManager.GetCentrosCosteSelectList(filter.TipoCosteCode);

            ModelState.ClearValidationState("Tipo");
            return PartialView("_Filtros");
        }
    }
}

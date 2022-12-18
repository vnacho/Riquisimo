using Ferpuser.BLL.Managers;
using Ferpuser.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Admin")]
    public class DatosEmpresaController : Controller
    {
        public ParametroManager manager { get; set; }

        public DatosEmpresaController(ParametroManager manager)
        {
            this.manager = manager;
        }

        public IActionResult Index()
        {
            DatosEmpresaDto model = manager.GetDatosEmpresa();
            return View(model);
        }

        [HttpPost, ActionName(nameof(Index))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexConfirmed()
        {
            DatosEmpresaDto model = new DatosEmpresaDto();
            await TryUpdateModelAsync<DatosEmpresaDto>(model);

            if (ModelState.IsValid)
            {
                await manager.EditDatosEmpresa(model);
                TempData["Message"] = "Los datos de empresa se han modificado correctamente.";
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}

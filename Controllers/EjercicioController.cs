using Ferpuser.Data;
using Ferpuser.Models.Consts;
using Ferpuser.Models.SageComu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    public class EjercicioController : Controller
    {
        public IActionResult Cambiar(int ejercicio)
        {
            HttpContext.Session.SetInt32(Consts.SESSION_EJERCICIO, ejercicio);
            TempData["Message"] = "Se ha cambiado el ejercicio correctamente.";
            return RedirectToAction("Index","Home");
        }

        public PartialViewResult SelectorEjercicio([FromServices]SageComuContext _sageComuContext)
        {
            List<ejercici> ejercios = _sageComuContext.Ejercicios.OrderByDescending(f => f.ANY).ToList();
            ejercios.First().Selected = true;

            int? seleccionado = HttpContext.Session.GetInt32(Consts.SESSION_EJERCICIO);
            if (seleccionado.HasValue)
            {
                ejercios.ForEach(f => f.Selected = false);
                string sAnio = seleccionado.ToString().Trim();
                ejercios.Single(f => f.ANY.Trim() == sAnio).Selected = true;
            }            

            return PartialView("_EjercicioSelector", ejercios);
        }
    }

}

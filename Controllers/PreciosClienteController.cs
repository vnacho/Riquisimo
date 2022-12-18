using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    public class PreciosClienteController : Controller
    {
        private readonly SageContext _sageContext;
        public PreciosClienteController(SageContext sageContext)
        {
            _sageContext = sageContext;
        }

        [HttpPost]
        public IActionResult GetPVP(string codigoArticulo, string codigoCliente)
        {
            PreciosClienteManager manager = new PreciosClienteManager(_sageContext);
            var pvp = manager.GetPrecio(codigoArticulo, codigoCliente);            
            return Json(pvp);
        }
    }
}

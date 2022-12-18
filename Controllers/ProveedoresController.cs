using Ferpuser.Data;
using Ferpuser.Models.Sage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize]
    public class ProveedoresController : Controller
    {
        private readonly SageContext _sageContext;

        public ProveedoresController(SageContext sageContext)
        {
            _sageContext = sageContext;
        }

        public async Task<IActionResult> Buscador(string value)
        {
            IEnumerable<ProveedorSage> model = null;
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.ToLower();
                model = await _sageContext.Proveedores.Where(f => f.CODIGO.ToLower().Contains(value) || f.NOMBRE.ToLower().Contains(value)).OrderBy(f => f.NOMBRE).ToListAsync();
            }
            return View(model);
        }
    }
}

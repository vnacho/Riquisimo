using Ferpuser.Data;
using Ferpuser.Models.Sage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize]
    public class EventosController : Controller
    {
        private readonly SageContext _sageContext;

        public EventosController(SageContext sageContext)
        {
            _sageContext = sageContext;
        }

        public async Task<IActionResult> Buscador(string value)
        {
            IEnumerable<Almacen> model = null;
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.ToLower();
                model = await _sageContext.Almacen.Where(f => f.Codigo.ToLower().Contains(value) || f.Nombre.ToLower().Contains(value)).OrderBy(f => f.Codigo).ToListAsync();
            }
            return View(model);
        }

        public async Task<JsonResult> BuscadorJson(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.ToLower();
                var model = await _sageContext
                    .Almacen
                    .Where(f => f.Codigo.ToLower().Contains(query) || f.Nombre.ToLower().Contains(query))
                    .OrderBy(f => f.Codigo)
                    .Select(f => new { value = f.Codigo, label = f.DisplayName })
                    .ToListAsync();

                var json = JsonConvert.SerializeObject(model,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                return Json(json);
            }
            return Json(null);
        }
    }
}

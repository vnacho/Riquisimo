using Ferpuser.Data;
using Ferpuser.Models.Sage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Session;
using System.Linq;
using System.Threading.Tasks;
using Ferpuser.BLL.Filters;

namespace Ferpuser.Controllers
{
    [Authorize]
    public class ArticulosController : Controller
    {
        private readonly SageContext _sageContext;

        public ArticulosController(SageContext sageContext)
        {
            _sageContext = sageContext;
        }
       
        public JsonResult BuscadorJson(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.ToLower();
              var model = _sageContext
                    .Articulo
                    .Where(f => f.Codigo.ToLower().Contains(query) || f.Nombre.ToLower().Contains(query))
                    .OrderBy(f => f.Codigo)
                    .Select(f => new { value = f.Codigo, label = f.Display })
                    .ToList();

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

        public JsonResult GetArticuloJson(string codigoarticulo)
        {
            var articulo = _sageContext.Articulo.FirstOrDefault(f => f.Codigo == codigoarticulo);
            return Json(articulo);
        }

        //public async Task<IActionResult> GetIVAArticulo()
        //{
        //    ArticuloFilter filter = new ArticuloFilter();
        //    await TryUpdateModelAsync<ArticuloFilter>(filter, "",
        //        f => f.CodigoArticulo);
        //    Articulo model = _sageContext.Articulo.Where(filter.ExpressionFilter())?.FirstOrDefault();
        //    Tipo_IVA tipoIVA = _sageContext.Tipo_IVA.FirstOrDefault(f => f.Codigo == articulo.TIPO_IVA);
        //    return Json(new { codigotipoiva = model.TIPO_IVA, ivaporcentaje = tipoIVA.IVA });
        //}
    }
}

using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models.Sage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    public class TipoIVAController : Controller
    {
        private readonly SageContext _sageContext;
        public TipoIVAController(SageContext sageContext)
        {
            _sageContext = sageContext;
        }

        public async Task<IActionResult> GetFirstOrDefault(string codigoArticulo)
        {
            ArticuloFilter filter = new ArticuloFilter();
            await TryUpdateModelAsync<ArticuloFilter>(filter, "",
                f => f.CodigoArticulo);

            ArticuloManager manager = new ArticuloManager(_sageContext);
            Articulo articulo = _sageContext.Articulo.Where(filter.ExpressionFilter())?.FirstOrDefault();
            var tipoIVA = _sageContext.Tipo_IVA.SingleOrDefault(f => f.Codigo == articulo.TIPO_IVA)?.IVA;
            return Json(new { codigotipoiva = articulo.TIPO_IVA, ivaporcentaje = (int)tipoIVA.Value });
        }
        
    }
}

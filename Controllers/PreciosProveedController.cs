using Ferpuser.BLL.Filters;
using Ferpuser.Data;
using Ferpuser.Models.Sage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    public class PreciosProveedController : Controller
    {
        private readonly SageContext _sageContext;
        public PreciosProveedController(SageContext sageContext)
        {
            _sageContext = sageContext;
        }

        public async Task<IActionResult> GetFirstOrDefault()
        {
            PreciosProveedFilter filter = new PreciosProveedFilter();
            await TryUpdateModelAsync<PreciosProveedFilter>(filter, "",
                f => f.CodigoArticulo,
                f => f.CodigoProveedor);
            referpro model = _sageContext.PreciosProveed.Where(filter.ExpressionFilter())?.FirstOrDefault();

            Articulo articulo = _sageContext.Articulo.FirstOrDefault(f => f.Codigo == filter.CodigoArticulo);
            Tipo_IVA tipoIVA = _sageContext.Tipo_IVA.FirstOrDefault(f => f.Codigo == articulo.TIPO_IVA);
            return Json(new { pcompra = model?.PCOMPRA, codigotipoiva = articulo.TIPO_IVA, ivaporcentaje = tipoIVA.IVA });
        }
    }
}

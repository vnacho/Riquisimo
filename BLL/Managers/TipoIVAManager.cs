using Ferpuser.Data;
using Ferpuser.Models.Sage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class TipoIVAManager
    {
        private SageContext _sageContext;

        public TipoIVAManager(SageContext sageContext)
        {
            _sageContext = sageContext;
        }

        public Tipo_IVA GetIVA(string codigoProducto)
        {
            Articulo articulo = _sageContext.Articulo.Find(codigoProducto);
            if (articulo != null)
            {
                var tipoIVA = _sageContext.Tipo_IVA.SingleOrDefault(f => f.Codigo == articulo.TIPO_IVA);
                return tipoIVA;
            }
            return null;
        }
    }
}

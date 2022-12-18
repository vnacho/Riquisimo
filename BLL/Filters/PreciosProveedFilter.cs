using Ferpuser.Models.Sage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ferpuser.Models;

namespace Ferpuser.BLL.Filters
{
    public class PreciosProveedFilter
    {
        public string CodigoArticulo { get; set; }
        public string CodigoProveedor { get; set; }

        public Expression<Func<referpro, bool>> ExpressionFilter()
        {            
            return f =>
                (string.IsNullOrWhiteSpace(CodigoArticulo) ? true : f.ARTICULO.Trim() == CodigoArticulo.Trim()) &&
                (string.IsNullOrWhiteSpace(CodigoProveedor) ? true : f.PROVEEDOR.Trim() == CodigoProveedor.Trim());
        }

    }
}

using Ferpuser.Models.Sage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class ArticuloFilter
    {
        public string CodigoArticulo { get; set; }
        public Expression<Func<Articulo, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(CodigoArticulo) ? true : f.Codigo.Trim() == CodigoArticulo.Trim());
        }
    }
}

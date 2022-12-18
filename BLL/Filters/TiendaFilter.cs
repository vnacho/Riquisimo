using System.Linq.Expressions;
using System;
using Ferpuser.Models;

namespace Ferpuser.BLL.Filters
{
    public class TiendaFilter
    {
        public string nombre{ get; set; }
        public Expression<Func<Tienda, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(nombre) ? true : f.nombre.Trim() == nombre.Trim());
        }
    }
}

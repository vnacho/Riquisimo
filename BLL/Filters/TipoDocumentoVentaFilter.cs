using Ferpuser.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Ferpuser.BLL.Filters
{
    public class TipoDocumentoVentaFilter
    {
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public Expression<Func<TipoDocumentoVenta, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(Descripcion) ? true : f.Descripcion.Contains(Descripcion.Trim()));
        }
    }
}

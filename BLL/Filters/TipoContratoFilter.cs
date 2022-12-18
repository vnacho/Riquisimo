using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class TipoContratoFilter
    {
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        public Expression<Func<TipoContrato, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(Nombre) ? true : f.Nombre.Contains(Nombre.Trim()));
        }
    }
}

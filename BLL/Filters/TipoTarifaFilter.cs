using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class TipoTarifaFilter
    {
        [Display(Name = "Código")]
        [MaxLength(1)]
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        
        public Expression<Func<TipoTarifa, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(Codigo) ? true : f.Codigo.ToLower() == Codigo.ToLower()) &&
                (string.IsNullOrWhiteSpace(Nombre) ? true : f.Nombre.Contains(Nombre.Trim()));
        }
    }
}

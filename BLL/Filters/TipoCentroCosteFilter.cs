using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class TipoCentroCosteFilter
    {
        [Display(Name="Descripción")]
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public Expression<Func<TipoCentroCoste, bool>> ExpressionFilter()
        {
            return f => 
                (string.IsNullOrWhiteSpace(Descripcion) ? true : f.Descripcion.Contains(Descripcion.Trim())) && 
                (string.IsNullOrWhiteSpace(Tipo) ? true : f.Tipo == Tipo);
        }
    }
}

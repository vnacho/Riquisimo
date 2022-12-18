using Ferpuser.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Ferpuser.BLL.Filters
{
    public class PuestoComiteFilter
    {
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        public Expression<Func<PuestoComite, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(Nombre) ? true : f.Nombre.Contains(Nombre.Trim()));
        }
    }
}

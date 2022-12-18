using Ferpuser.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Ferpuser.BLL.Filters
{
    public class PonenteFilter
    {
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Congreso")]
        public Guid? CongressId { get; set; }
        public Expression<Func<Ponente, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(Nombre) ? true : f.Nombre.Contains(Nombre.Trim())) &&
                (CongressId.HasValue ? f.CongressId == CongressId : true);
        }
    }
}

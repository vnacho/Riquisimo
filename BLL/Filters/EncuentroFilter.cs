using Ferpuser.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Ferpuser.BLL.Filters
{
    public class EncuentroFilter
    {
        [Display(Name = "Evento")]
        public Guid? CongressId { get; set; }

        public Expression<Func<Encuentro, bool>> ExpressionFilter()
        {
            return f =>
                (CongressId.HasValue ? f.CongressId == CongressId : true);
        }
    }
}

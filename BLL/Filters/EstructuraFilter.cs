using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class EstructuraFilter
    {
        [Display(Name = "Centro de coste")]
        public int? CentroCosteId { get; set; }

        public Expression<Func<Estructura, bool>> ExpressionFilter()
        {
            return f =>
                (CentroCosteId.HasValue ? f.CentroCosteId == CentroCosteId.Value : true);
        }
    }
}

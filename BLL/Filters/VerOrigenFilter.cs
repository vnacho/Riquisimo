using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class VerOrigenFilter
    {
        [Display(Name = "Código nivel analítico 1")]
        public string NivelAnalitico1 { get; set; }

        public Expression<Func<VerOrigen, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(NivelAnalitico1) ? true : f.NivelAnalitico1.ToLower() == NivelAnalitico1.ToLower());
        }
    }
}

using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class CongressFilter
    {
        public int? Number { get; set; }

        public Expression<Func<Congress, bool>> ExpressionFilter()
        {
            return f =>
                (Number.HasValue ? f.Number == Number : true);
        }
    }
}

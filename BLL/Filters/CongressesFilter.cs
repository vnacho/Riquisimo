using Ferpuser.Models;
using System;
using System.Linq.Expressions;

namespace Ferpuser.BLL.Filters
{
    public class CongressesFilter
    {
        //public int? Number { get; set; }

        //public string? Code { get; set; }

        //public string? Place { get; set; }

        //public string? Name { get; set; }

        public string? TipoCongreso { get; set; }

        public Expression<Func<Congress, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(TipoCongreso) ? true : f.TipoCongress.Contains(TipoCongreso.Trim())) &&
                (f.Deleted == null);
        }
        //public Expression<Func<Congress, bool>> ExpressionFilter()
        //{
        //    return f =>
        //        (Number.HasValue ? f.Number == Number : true) &&
        //        (string.IsNullOrWhiteSpace(Code) ? true : f.Code.Contains(Code.Trim())) &&
        //        (string.IsNullOrWhiteSpace(Place) ? true : f.Place.Contains(Place.Trim())) &&
        //        (string.IsNullOrWhiteSpace(Name) ? true : f.Name.Contains(Name.Trim())) &&
        //        (string.IsNullOrWhiteSpace(TipoCongreso) ? true : f.TipoCongress.Contains(TipoCongreso.Trim())) &&
        //        (f.Deleted == null);
        //}
    }
}

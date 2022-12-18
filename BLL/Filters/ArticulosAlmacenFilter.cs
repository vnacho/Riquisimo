using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Ferpuser.BLL.Filters
{
    public class ArticulosAlmacenFilter
    {
        [Display(Name = "Centro de coste")]
        public int? CentroCosteId { get; set; }

        [Display(Name = "Código Artículo")]
        public string ProductCode { get; set; }

        [Display(Name = "Descripcción")]
        public string ProductDescription { get; set; }

        public Expression<Func<ArticulosAlmacen, bool>> ExpressionFilter()
        {
            return f => (CentroCosteId.HasValue ? f.CentroCosteId == CentroCosteId : true) &&
               (string.IsNullOrWhiteSpace(ProductCode) ? true : f.ProductCode.Contains(ProductCode.Trim())) &&
               (string.IsNullOrWhiteSpace(ProductDescription) ? true : f.ProductDescription.Contains(ProductDescription.Trim()));
        }
    }
}

using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Ferpuser.BLL.Filters
{
    public class InventarioArticulosAlmacenFilter
    {
        //[Display(Name = "Fecha")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //public DateTime? fecha { get; set; }

        [Display(Name = "Fecha desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaDesde { get; set; }

        [Display(Name = "Fecha hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaHasta { get; set; }

        [Display(Name = "Articulo Almacen")]
        public Guid? ArticulosAlmacenId { get; set; }

        [Display(Name = "Tipo tarifa")]
        public string? RateFiltro { get; set; }

        public Expression<Func<InventarioArticulosAlmacen, bool>> ExpressionFilter()
        {
            return f =>
                (ArticulosAlmacenId.HasValue ? f.ArticulosAlmacenId == ArticulosAlmacenId : true) &&
                (FechaDesde.HasValue ? f.FechaUltiActu >= FechaDesde : true) &&
                (FechaHasta.HasValue ? f.FechaUltiActu <= FechaHasta : true) &&
                (string.IsNullOrWhiteSpace(RateFiltro) ? true : f.ArticulosAlmacen.Rate.Contains(RateFiltro));
        }
    }
}

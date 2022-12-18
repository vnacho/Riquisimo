using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Ferpuser.BLL.Filters
{
    public class PartesInternosAlmacenFilter
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

        [Display(Name = "Destino")]
        public int? DestinoId { get; set; }

        public Expression<Func<PartesInternosAlmacen, bool>> ExpressionFilter()
        {
            return f => (DestinoId.HasValue ? f.DestinoId == DestinoId : true) &&
                (ArticulosAlmacenId.HasValue ? f.ArticulosAlmacenId == ArticulosAlmacenId : true) &&
                (FechaDesde.HasValue ? f.fecha >= FechaDesde : true) &&
                (FechaHasta.HasValue ? f.fecha <= FechaHasta : true);
        }

        //public static implicit operator PartesInternosAlmacenFilter(CongressFilter v)
        //{
        //    throw new NotImplementedException();
        //}
        //public Expression<Func<PartesInternosAlmacen, bool>> ExpressionFilter()
        //{
        //    return f =>
        //        (fecha.HasValue ? f.fecha >= fecha.Value : true) &&
        //        (ArticulosAlmacenId.HasValue ? f.ArticulosAlmacenId == ArticulosAlmacenId : true) &&
        //        (DestinoId.HasValue ? f.DestinoId == DestinoId : true);
        //}
    }
}

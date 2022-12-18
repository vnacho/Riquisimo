using Ferpuser.Models;
using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class TikectFilter
    {
        [Display(Name = "Tienda")]
        public Guid? idTienda { get; set; }

        [Display(Name = "Fecha desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaDesde { get; set; }

        [Display(Name = "Fecha hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaHasta { get; set; }

        public Expression<Func<Tikect, bool>> ExpressionFilter()
        {
            return f =>
                (idTienda == null ? true : f.tiendaID == idTienda) &&
                (FechaDesde.HasValue ? f.FechaTikect >= FechaDesde : true) &&
                (FechaHasta.HasValue ? f.FechaTikect <= FechaHasta : true);
        }
    }
}

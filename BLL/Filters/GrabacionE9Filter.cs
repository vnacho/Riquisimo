using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class GrabacionE9Filter
    {
        [Display(Name = "Centro de coste")]
        public int? CentroCosteId { get; set; }

        [Display(Name = "Fecha desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaDesde { get; set; }

        [Display(Name = "Fecha hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaHasta { get; set; }

        [Display(Name = "Entrada/Salida")]
        [MaxLength(1)]
        public string EntradaSalida { get; set; }

        [Display(Name = "Destino")]
        public int? DestinoId { get; set; }


        public Expression<Func<GrabacionE9, bool>> ExpressionFilter()
        {
            return f => 
                (CentroCosteId.HasValue ? f.CentroCosteId == CentroCosteId.Value : true) &&
                (FechaDesde.HasValue ? f.Fecha >= FechaDesde.Value : true) &&
                (FechaHasta.HasValue ? f.Fecha <= FechaHasta.Value : true) &&
                (string.IsNullOrWhiteSpace(EntradaSalida) ? true : f.EntradaSalida == EntradaSalida) && 
                (DestinoId.HasValue ? f.DestinoId == DestinoId.Value : true);
        }
    }
}

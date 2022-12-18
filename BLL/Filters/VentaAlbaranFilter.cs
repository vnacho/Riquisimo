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
    public class VentaAlbaranFilter
    {
        [Display(Name = "Cliente")]
        public string CodigoCliente { get; set; }

        [Display(Name = "Evento")]
        public string CodigoEvento { get; set; }

        [Display(Name = "Fecha desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaDesde { get; set; }

        [Display(Name = "Fecha hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaHasta { get; set; }

        [Display(Name = "Estado")]
        public EstadoAlbaran? EstadoAlbaran { get; set; }

        [Display(Name = "Vendedor")]
        public string CodigoVendedor { get; set; }

        public Expression<Func<VentaAlbaran, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(CodigoVendedor) ? true : f.CodigoVendedor.Trim() == CodigoVendedor.Trim()) &&
                (string.IsNullOrWhiteSpace(CodigoCliente) ? true : f.CodigoCliente.Trim() == CodigoCliente.Trim()) &&
                (string.IsNullOrWhiteSpace(CodigoEvento) ? true : f.AlbaranLineas.Any(g => g.CodigoEvento.Trim() == CodigoEvento.Trim())) &&
                (FechaDesde.HasValue ? f.Fecha >= FechaDesde : true) &&
                (FechaHasta.HasValue ? f.Fecha <= FechaHasta : true) && 
                (EstadoAlbaran.HasValue ? f.EstadoAlbaran == EstadoAlbaran : true);
        }
    }
}

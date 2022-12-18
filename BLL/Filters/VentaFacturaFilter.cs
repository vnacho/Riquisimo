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
    public class VentaFacturaFilter
    {
        [Display(Name = "Cliente")]
        public string CodigoCliente { get; set; }

        [Display(Name = "Evento")]
        public string CodigoEvento { get; set; }

        [Display(Name = "Vendedor")]
        public string CodigoVendedor { get; set; }

        [Display(Name = "Fecha desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaDesde { get; set; }

        [Display(Name = "Fecha hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaHasta { get; set; }

        [Display(Name = "Estado")]
        public EstadoFactura? EstadoFactura { get; set; }

        [Display(Name = "Pagada")]
        public bool? Pagada { get; set; }

        
        public Expression<Func<VentaFactura, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(CodigoCliente) ? true : f.CodigoCliente.Trim() == CodigoCliente.Trim()) &&
                (string.IsNullOrWhiteSpace(CodigoEvento) ? true : f.FacturaLineas.Any(g => g.CodigoEvento.Trim() == CodigoEvento.Trim())) &&
                (string.IsNullOrWhiteSpace(CodigoVendedor) ? true : f.CodigoVendedor.Trim() == CodigoVendedor.Trim()) &&
                (FechaDesde.HasValue ? f.Fecha >= FechaDesde : true) &&
                (FechaHasta.HasValue ? f.Fecha <= FechaHasta : true) &&
                (Pagada.HasValue ? f.Pagada == Pagada.Value : true) &&
                (EstadoFactura.HasValue ? f.EstadoFactura == EstadoFactura : true);
        }
    }
}

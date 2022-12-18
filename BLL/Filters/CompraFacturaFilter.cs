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
    public class CompraFacturaFilter
    {
        [Display(Name = "Proveedor")]
        public string CodigoProveedor { get; set; }

        [Display(Name = "Evento")]
        public string CodigoEvento { get; set; }

        [Display(Name = "Operario")]
        public string CodigoOperario { get; set; }

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

        public bool? TieneRetencion { get; set; }

        public string Registro { get; set; }

        [Display(Name = "Nº Factura")]
        public string numFactura { get; set; }

        public Expression<Func<CompraFactura, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(CodigoProveedor) ? true : f.CodigoProveedor.Trim() == CodigoProveedor.Trim()) &&
                (string.IsNullOrWhiteSpace(CodigoEvento) ? true : f.FacturaLineas.Any(g => g.CodigoEvento.Trim() == CodigoEvento.Trim())) &&
                (string.IsNullOrWhiteSpace(CodigoOperario) ? true : f.CodigoOperario.Trim() == CodigoOperario.Trim()) &&
                (string.IsNullOrWhiteSpace(Registro) ? true : f.Registro.Trim() == Registro.Trim()) &&
                (string.IsNullOrWhiteSpace(numFactura) ? true : f.NumeroFactura.Trim().Contains(numFactura.Trim())) &&
                (FechaDesde.HasValue ? f.Fecha >= FechaDesde : true) &&
                (FechaHasta.HasValue ? f.Fecha <= FechaHasta : true) &&
                (Pagada.HasValue ? f.Pagada == Pagada.Value : true) &&
                (TieneRetencion.HasValue ? f.TieneRetencion == TieneRetencion.Value : true) &&
                (EstadoFactura.HasValue ? f.EstadoFactura == EstadoFactura : true);
        }
    }
}

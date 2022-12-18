using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Ferpuser.Models;
using System.ComponentModel.DataAnnotations;
using Ferpuser.Models.Enums;

namespace Ferpuser.BLL.Filters
{
    public class CompraPedidoFilter
    {
        [Display(Name = "Proveedor")]
        public string CodigoProveedor { get; set; }

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
        public EstadoPedido? EstadoPedido { get; set; }

        public EstadoPedido[] EstadosPedido { get; set; }
        
        [Display(Name = "Operario")]
        public string CodigoOperario { get; set; }


        public Expression<Func<CompraPedido, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(CodigoProveedor) ? true : f.CodigoProveedor.Trim() == CodigoProveedor.Trim()) &&
                (string.IsNullOrWhiteSpace(CodigoEvento) ? true : f.PedidoLineas.Any(g => g.CodigoEvento.Trim() == CodigoEvento.Trim())) &&
                (string.IsNullOrWhiteSpace(CodigoOperario) ? true : f.CodigoOperario.Trim() == CodigoOperario.Trim()) &&
                (FechaDesde.HasValue? f.Fecha >= FechaDesde : true) &&
                (FechaHasta.HasValue ? f.Fecha <= FechaHasta : true) &&
                (EstadoPedido.HasValue ? f.EstadoPedido == EstadoPedido : true) && 
                (
                    EstadosPedido == null || !EstadosPedido.Any() ? true : (EstadosPedido.Contains(f.EstadoPedido))
                );
        }

    }
}

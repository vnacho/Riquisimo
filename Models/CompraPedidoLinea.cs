using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class CompraPedidoLinea
    {
        [Key]
        public int IdPedidoLinea { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public int PedidoId { get; set; }
        public CompraPedido Pedido { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Artículo")]
        public string CodigoArticulo { get; set; }

        [Display(Name = "Artículo")]
        //[Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string NombreArticulo { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones artículo")]
        public string ObservacionesPedidoLinea { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Evento")]
        public string CodigoEvento { get; set; }

        [Display(Name = "Evento")]
        //[Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string NombreEvento { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public decimal Unidades { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Unidades pendientes")]
        public decimal UnidadesPendientes { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio unitario")]
        [UIHint("Currency")]
        public decimal PrecioUnitario { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Currency)]
        [Display(Name = "Importe")]
        [UIHint("Currency")]
        public decimal TotalPedidoLinea { get; set; }
        public int Orden { get; set; }

        [Display(Name = "% IVA")]
        [Range(0, 100, ErrorMessage = "El campo '{0}' debe estar entre 0 y 100")]
        [UIHint("Percentage")]
        public int? IVA_Porcentaje { get; set; }

        [Display(Name = "% IVA")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string CodigoTipoIVA { get; set; }

        [NotMapped]
        public decimal ImporteIVA => Math.Round(TotalPedidoLinea * (IVA_Porcentaje ?? 0) / 100, 2);
        [NotMapped]
        public decimal Total => TotalPedidoLinea + ImporteIVA;

        /// <summary>
        /// Una línea de pedido puede estar satisfecha en varios albaranes
        /// </summary>
        public ICollection<CompraAlbaranLinea> AlbaranLineas { get; set; }

        /// <summary>
        /// Método para cargar los datos calculados
        /// </summary>
        public void Calcular()
        {
            TotalPedidoLinea = Unidades * PrecioUnitario;
        }

        
    }
}

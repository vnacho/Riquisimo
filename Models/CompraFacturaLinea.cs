using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class CompraFacturaLinea : IValidatableObject
    {
        [Key]
        public int IdFacturaLinea { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public int CompraFacturaId { get; set; }
        public CompraFactura CompraFactura { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Artículo")]
        public string CodigoArticulo { get; set; }

        [Display(Name = "Artículo")]
        //[Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string NombreArticulo { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones artículo")]
        public string ObservacionesFacturaLinea { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Evento")]
        public string CodigoEvento { get; set; }

        [Display(Name = "Evento")]
        //[Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string NombreEvento { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public decimal Unidades { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio unitario")]
        [UIHint("Currency")]
        public decimal BaseImponiblePrecioUnitario { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]       
        [Display(Name = "Base imponible")]
        [DataType(DataType.Currency)]
        [UIHint("Currency")]
        public decimal BaseImponibleTotal { get; set; }

        [Display(Name = "% IVA")]
        [Range(0,100, ErrorMessage = "El campo '{0}' debe estar entre 0 y 100")]
        [UIHint("Percentage")]
        public int? IVA_Porcentaje { get; set; }

        [Display(Name = "% IVA")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string CodigoTipoIVA { get; set; }

        public int Orden { get; set; }

        public int? CompraAlbaranLineaId { get; set; }
        public CompraAlbaranLinea CompraAlbaranLinea { get; set; }

        public int? CompraPedidoLineaId { get; set; }
        public CompraPedidoLinea CompraPedidoLinea { get; set; }

        [NotMapped]
        public string CodigoAlbaran { get; set; }

        [NotMapped]
        public string CodigoPedido { get; set; }

        [NotMapped]
        public decimal? UnidadesPendientes { get; set; }

        [NotMapped]
        public decimal ImporteIVA => Math.Round(BaseImponibleTotal * (IVA_Porcentaje ?? 0) / 100, 2);

        [NotMapped]
        public decimal Total => BaseImponibleTotal + ImporteIVA;


        /// <summary>
        /// Método para cargar los datos calculados
        /// </summary>
        public void Calcular()
        {
            BaseImponibleTotal = Unidades * BaseImponiblePrecioUnitario;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            if (UnidadesPendientes.HasValue && UnidadesPendientes.Value < Unidades)
                results.Add(new ValidationResult("No puede añadir más unidades que las que hay pendientes.", new string[] { "Unidades" }));
            return results;
        }
    }
}

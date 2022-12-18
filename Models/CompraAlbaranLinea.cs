using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class CompraAlbaranLinea : IValidatableObject
    {
        [Key]
        public int IdAlbaranLinea { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public int AlbaranId { get; set; }
        public CompraAlbaran Albaran { get; set; }

        public int? CompraPedidoLineaId { get; set; }
        public CompraPedidoLinea CompraPedidoLinea { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Artículo")]
        public string CodigoArticulo { get; set; }

        [Display(Name = "Artículo")]        
        public string NombreArticulo { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones artículo")]
        public string ObservacionesAlbaranLinea { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Evento")]
        public string CodigoEvento { get; set; }

        [Display(Name = "Evento")]        
        public string NombreEvento { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public decimal Unidades { get; set; }        

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio unitario")]
        [UIHint("Currency")]
        public decimal PrecioUnitario { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Currency)]
        [Display(Name = "Importe")]
        [UIHint("Currency")]
        public decimal TotalAlbaranLinea { get; set; }

        public int Orden { get; set; }

        [Display(Name = "% IVA")]
        [Range(0, 100, ErrorMessage = "El campo '{0}' debe estar entre 0 y 100")]
        [UIHint("Percentage")]
        public int? IVA_Porcentaje { get; set; }

        [Display(Name = "% IVA")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string CodigoTipoIVA { get; set; }

        [NotMapped]
        public decimal ImporteIVA => Math.Round(TotalAlbaranLinea * (IVA_Porcentaje ?? 0) / 100, 2);
        [NotMapped]
        public decimal Total => TotalAlbaranLinea + ImporteIVA;

        [NotMapped]
        public decimal? UnidadesPendientes { get; set; }

        [NotMapped]
        public string CodigoPedido { get; set; }

        /// <summary>
        /// Método para cargar los datos calculados
        /// </summary>
        public void Calcular()
        {
            TotalAlbaranLinea = Unidades * PrecioUnitario;
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

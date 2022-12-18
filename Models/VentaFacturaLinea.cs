using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class VentaFacturaLinea : IValidatableObject
    {
        [Key]
        public int IdFacturaLinea { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public int VentaFacturaId { get; set; }
        public VentaFactura VentaFactura { get; set; }

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

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "% Descuento")]
        [UIHint("Percentage")]
        [Range(0, 100, ErrorMessage = "El campo '{0}' debe estar entre 0 y 100")]
        public decimal Descuento { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Currency)]
        [Display(Name = "Importe descuento")]
        [UIHint("Currency")]
        public decimal ImporteDescuento { get; set; }

        public int Orden { get; set; }

        [Display(Name = "Mostrar descripción ampliada")]
        public bool DescripcionAmpliada { get; set; }

        [Display(Name = "Descripción ampliada")]
        [DataType(DataType.MultilineText)]
        public string TextoDescripcionAmpliada { get; set; }

        [Display(Name = "Mostrar tiempo")]
        public bool TieneTiempo { get; set; }

        [Display(Name = "Tiempo")]
        public decimal? Tiempo { get; set; }

        public int? VentaAlbaranLineaId { get; set; }
        public VentaAlbaranLinea VentaAlbaranLinea { get; set; }

        public int? VentaPedidoLineaId { get; set; }
        public VentaPedidoLinea VentaPedidoLinea { get; set; }

        [NotMapped]
        public string CodigoAlbaranDisplay { get; set; }

        [NotMapped]
        public string CodigoPedidoDisplay { get; set; }

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
            decimal importe_antes_descuentos = Unidades * BaseImponiblePrecioUnitario;
            if (Tiempo.HasValue)
                importe_antes_descuentos = importe_antes_descuentos * Tiempo.Value;

            ImporteDescuento = Math.Round(importe_antes_descuentos * Descuento / 100, 2);
            BaseImponibleTotal = Math.Round(importe_antes_descuentos - ImporteDescuento, 2);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            if (UnidadesPendientes.HasValue && UnidadesPendientes.Value < Unidades)
                results.Add(new ValidationResult("No puede añadir más unidades que las que hay pendientes.", new string[] { "Unidades" }));
            if (TieneTiempo && !Tiempo.HasValue)
                results.Add(new ValidationResult("El campo Tiempo es obligatorio.", new string[] { nameof(Tiempo) }));
            return results;
        }
    }
}

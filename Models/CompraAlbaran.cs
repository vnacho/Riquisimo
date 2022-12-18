using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class CompraAlbaran : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Código")]
        public string CodigoAlbaran { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [DataType(DataType.MultilineText)]
        public string Observaciones { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Currency)]
        [Display(Name = "Base Imponible")]
        [UIHint("Currency")]
        public decimal BaseImponible { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Currency)]
        [Display(Name = "Total")]
        [UIHint("Currency")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Proveedor")]
        public string CodigoProveedor { get; set; }

        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string NombreProveedor { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Operario")]
        public string CodigoOperario { get; set; }

        [Display(Name = "Operario")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string NombreOperario { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Evento")]
        public string CodigoEvento { get; set; }

        [Display(Name = "Evento")]
        public string NombreEvento { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public EstadoAlbaran EstadoAlbaran { get; set; }

        [Display(Name = "Albarán generado automáticamente")]
        public bool GeneradoAutomaticamente { get; set; }
        
        /// <summary>
        /// Líneas de albarán
        /// </summary>
        public ICollection<CompraAlbaranLinea> AlbaranLineas { get; set; }

        /// <summary>
        /// Relación con factura de compra, no es obligatorio. El albarán se creará primero y la factura después.
        /// </summary>
        public int? CompraFacturaId { get; set; }
        public CompraFactura CompraFactura { get; set; }        

        public bool TienePedidoRelacionado => AlbaranLineas != null && AlbaranLineas.Any(f => f.CompraPedidoLineaId != null);

        /// <summary>
        /// Método para cargar los datos calculados
        /// </summary>
        public void Calcular()
        {
            BaseImponible = AlbaranLineas.Sum(f => f.TotalAlbaranLinea);
            Total = AlbaranLineas.Sum(f => f.Total);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (this.AlbaranLineas == null || !this.AlbaranLineas.Any())
            {
                list.Add(new ValidationResult("El albarán debe contener al menos una línea."));
                return list;
            }
            
            // Valida que un albarán no tenga líneas que referencian a la misma línea de pedido
            int lineasVenidasPedido = this.AlbaranLineas.Where(f => f.CompraPedidoLineaId.HasValue).Count();
            int lineasDistintas = this.AlbaranLineas.Where(f => f.CompraPedidoLineaId.HasValue).Select(f => f.CompraPedidoLineaId).Distinct().Count();
            if (lineasDistintas != lineasVenidasPedido)                
                list.Add(new ValidationResult("Existen varias líneas de albarán que pertenecen a la misma línea de pedido. Por favor revise la información."));

            return list;
        }
    }    
}

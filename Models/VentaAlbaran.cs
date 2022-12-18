using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class VentaAlbaran : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Serie")]
        [MaxLength(2)]
        public string Serie { get; set; }

        [Display(Name = "Código")]
        public int CodigoAlbaran { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [DataType(DataType.MultilineText)]
        public string Observaciones { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Currency)]
        [Display(Name = "Total")]
        [UIHint("Currency")]
        public decimal BaseImponible { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Cliente")]
        public string CodigoCliente { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Vendedor")]
        public string CodigoVendedor { get; set; }

        [Display(Name = "Vendedor")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string NombreVendedor { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Evento")]
        public string CodigoEvento { get; set; }

        [Display(Name = "Evento")]
        public string NombreEvento { get; set; }

        [NotMapped]
        [Display(Name = "Total IVA")]
        public decimal? TotalIVA { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Currency)]
        [Display(Name = "Total")]
        [UIHint("Currency")]
        public decimal Total { get; set; }


        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public EstadoAlbaran EstadoAlbaran { get; set; }

        [Display(Name = "Dirección")]
        public int? LineaEnvCli { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Código postal")]
        public string CodigoPostal { get; set; }

        [Display(Name = "Población")]
        public string Poblacion { get; set; }

        [Display(Name = "Provincia")]
        public string Provincia { get; set; }

        [Display(Name = "Albarán generado automáticamente")]
        public bool GeneradoAutomaticamente { get; set; }

        /// <summary>
        /// Líneas de albarán
        /// </summary>
        public ICollection<VentaAlbaranLinea> AlbaranLineas { get; set; }

        /// <summary>
        /// Relación con factura de compra, no es obligatorio. El albarán se creará primero y la factura después.
        /// </summary>
        public int? VentaFacturaId { get; set; }
        public VentaFactura VentaFactura { get; set; }        

        public bool TienePedidoRelacionado => AlbaranLineas != null && AlbaranLineas.Any(f => f.VentaPedidoLineaId != null);
        public string CodigoDisplay => $"{Serie} - {CodigoAlbaran}";

        /// <summary>
        /// Método para cargar los datos calculados
        /// </summary>
        public void Calcular()
        {
            BaseImponible = AlbaranLineas.Sum(f => f.TotalAlbaranLinea);
            TotalIVA = AlbaranLineas.Sum(f => f.TotalAlbaranLinea * f.IVA_Porcentaje.Value / 100);
            Total = BaseImponible + TotalIVA.Value;
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
            int lineasVenidasPedido = this.AlbaranLineas.Where(f => f.VentaPedidoLineaId.HasValue).Count();
            int lineasDistintas = this.AlbaranLineas.Where(f => f.VentaPedidoLineaId.HasValue).Select(f => f.VentaPedidoLineaId).Distinct().Count();
            if (lineasDistintas != lineasVenidasPedido)                
                list.Add(new ValidationResult("Existen varias líneas de albarán que pertenecen a la misma línea de pedido. Por favor revise la información."));

            return list;
        }
    }
}

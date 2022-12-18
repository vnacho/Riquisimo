using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class VentaPedido : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Serie")]
        [MaxLength(2)]
        public string Serie { get; set; }
        
        [Display(Name = "Código")]
        public int CodigoPedido { get; set; }

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
        [Display(Name = "Centro de coste")]
        public string CodigoEvento { get; set; }

        [Display(Name = "Obra/Evento")]
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
        public EstadoPedido EstadoPedido { get; set; }

        [Display(Name = "Tipo de documento")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public int TipoDocumentoVentaId { get; set; }

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

        [Display(Name = "Tipo de documento")]
        public TipoDocumentoVenta TipoDocumentoVenta { get; set; }

        [NotMapped]
        public ICollection<DocumentoCompraVenta> documentos { get; set; } = new List<DocumentoCompraVenta>();
        public ICollection<VentaPedidoLinea> PedidoLineas { get; set; }

        public string CodigoDisplay => $"{Serie} - {CodigoPedido}";

        /// <summary>
        /// Método para cargar los datos calculados
        /// </summary>
        public void Calcular()
        {
            BaseImponible = PedidoLineas.Sum(f => f.TotalPedidoLinea);
            TotalIVA = PedidoLineas.Sum(f => f.TotalPedidoLinea * f.IVA_Porcentaje.Value / 100);
            Total = BaseImponible + TotalIVA.Value;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (this.PedidoLineas == null || !this.PedidoLineas.Any())
                list.Add(new ValidationResult("El pedido debe contener al menos una línea."));

            return list;
        }

    }    
}

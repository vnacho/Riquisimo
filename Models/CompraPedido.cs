using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class CompraPedido : IValidatableObject
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings =true)]
        [Display(Name = "Código")]
        public string CodigoPedido { get; set; }

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
        public EstadoPedido EstadoPedido { get; set; }

        //***********************************************Inicio fichero*************************************************/        
        [Display(Name = "Url del fichero")]
        [UIHint("File")]
        public string FicheroUrl { get; set; }

        [Display(Name = "Fichero")]
        public string FicheroNombre { get; set; }

        [Display(Name = "Tiene fichero")]
        public bool TieneFichero => !string.IsNullOrWhiteSpace(FicheroUrl);

        //***********************************************Fin fichero*************************************************/

        [NotMapped]
        public ICollection<DocumentoCompraVenta> documentos { get; set; } = new List<DocumentoCompraVenta>(); 
        public ICollection<CompraPedidoLinea> PedidoLineas { get; set; }

        /// <summary>
        /// Método para cargar los datos calculados
        /// </summary>
        public void Calcular()
        {
            BaseImponible = PedidoLineas.Sum(f => f.TotalPedidoLinea);
            Total = PedidoLineas.Sum(f => f.Total);
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

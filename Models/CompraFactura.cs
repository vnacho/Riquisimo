using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class CompraFactura : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Registro")]
        [MaxLength(6)]
        public string Registro { get; set; }

        [Display(Name = "Código SAGE")]
        public int CodigoSage { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Nº de factura")]
        [MaxLength(24)]
        public string NumeroFactura { get; set; }

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
        [Display(Name = "Centro de coste")]
        public string CodigoEvento { get; set; }

        [Display(Name = "Evento")]
        public string NombreEvento { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public EstadoFactura EstadoFactura { get; set; }

        [Display(Name = "Tiene retención")]
        public bool TieneRetencion { get; set; }

        [Display(Name = "% Retención")]
        [Range(0, 100, ErrorMessage = "El campo '{0}' debe estar entre 0 y 100")]
        [UIHint("Percentage")]
        public decimal? Retencion_Porcentaje { get; set; }

        [NotMapped]
        [Display(Name = "Total retención")]
        public decimal? TotalRetencion { get; set; }        

        [Display(Name = "Pagada")]
        public bool Pagada { get; set; }
        

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
        public ICollection<DocumentoCompraVenta> documentos { get; set; } =  new List<DocumentoCompraVenta>();

        public ICollection<CompraFacturaLinea> FacturaLineas { get; set; }

        /// <summary>
        /// Una factura puede estar relacionada con 1 a n albaranes
        /// </summary>
        public ICollection<CompraAlbaran> CompraAlbaranes { get; set; }


        /// <summary>
        /// Método para cargar los datos calculados
        /// </summary>
        public void Calcular()
        {
            BaseImponible = FacturaLineas.Sum(f => f.BaseImponibleTotal);
            Total = BaseImponible + FacturaLineas.Sum(f => f.BaseImponibleTotal * f.IVA_Porcentaje.Value / 100);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (this.FacturaLineas == null || !this.FacturaLineas.Any())
            {
                list.Add(new ValidationResult("El albarán debe contener al menos una línea."));
                return list;
            }

            // Valida que un albarán no tenga líneas que referencian a la misma línea de pedido
            int lineasVenidasAlbaran = this.FacturaLineas.Where(f => f.CompraAlbaranLineaId.HasValue).Count();
            int lineasDistintas = this.FacturaLineas.Where(f => f.CompraAlbaranLineaId.HasValue).Select(f => f.CompraAlbaranLineaId).Distinct().Count();
            if (lineasDistintas != lineasVenidasAlbaran)
                list.Add(new ValidationResult("Existen varias líneas de factura que pertenecen al mismo albarán. Por favor revise la información."));

            return list;
        }
    }    
}
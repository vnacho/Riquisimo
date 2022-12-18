using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class VentaFactura : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Serie")]
        [MaxLength(2)]
        public string Serie { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Nº de factura")]
        public int CodigoFactura { get; set; }        

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

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public EstadoFactura EstadoFactura { get; set; }

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

        #region Datos retención

        [Display(Name = "Tiene retención")]
        public bool TieneRetencion { get; set; }

        [Display(Name = "Es retención fiscal")]
        public bool? EsRetencionFiscal { get; set; }

        [Display(Name = "Es retención no fiscal")]
        public bool? EsRetencionNoFiscal { get; set; }

        [Display(Name = "Modo retención")]
        public ModoRetencion? ModoRetencion { get; set; }

        [Display(Name = "% Retención")]
        [Range(0, 100, ErrorMessage = "El campo '{0}' debe estar entre 0 y 100")]
        [UIHint("Percentage")]
        public decimal? Retencion_Porcentaje { get; set; }

        [NotMapped]
        [Display(Name = "Total retención")]
        public decimal? TotalRetencion { get; set; }

        #endregion

        [NotMapped]
        [Display(Name = "Total IVA")]
        public decimal? TotalIVA { get; set; }

        [NotMapped]
        [Display(Name = "Total antes de retención")]
        public decimal? TotalAntesRetencion { get; set; }

        [Display(Name = "Pagada")]
        public bool Pagada { get; set; }

        [Display(Name = "Importada desde Sage")]
        public bool ImportadaSage { get; set; }

        [Display(Name = "Incluir datos del evento")]
        public bool IncluirDatosEvento { get; set; } = true;


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

        //***********************************************Datos de origen*****************************************************/

        [Display(Name = "Origen")]
        public bool Origen { get; set; }

        [Display(Name = "Artículo")]
        public string OrigenCodigoArticulo { get; set; }

        [Display(Name = "Artículo")]
        public string OrigenNombreArticulo { get; set; }

        [Display(Name = "Importe")]
        public decimal? OrigenImporte { get; set; }

        //***********************************************Fin datos de origen*****************************************************/

        public string CodigoDisplay => $"{Serie?.PadRight(4, ' ')}{CodigoFactura.ToString().PadLeft(6, '0')}";


        public ICollection<VentaFacturaLinea> FacturaLineas { get; set; }

        /// <summary>
        /// Una factura puede estar relacionada con 1 a n albaranes
        /// </summary>
        public ICollection<VentaAlbaran> VentaAlbaranes { get; set; }


        /// <summary>
        /// Método para cargar los datos calculados
        /// </summary>
        public void Calcular()
        {
            BaseImponible = FacturaLineas.Sum(f => f.BaseImponibleTotal);
            BaseImponible = Math.Round(BaseImponible, 2);

            if (Origen && OrigenImporte.HasValue)
                BaseImponible = BaseImponible - OrigenImporte.Value;

            TotalIVA = FacturaLineas.Sum(f => f.BaseImponibleTotal * f.IVA_Porcentaje.Value / 100);
            if (!TotalIVA.HasValue)
                TotalIVA = 0;
            TotalIVA = Math.Round(TotalIVA.Value, 2);

            Total = BaseImponible + TotalIVA.Value;
            TotalAntesRetencion = Total;

            TotalRetencion = 0;
            if (TieneRetencion && ModoRetencion.HasValue && Retencion_Porcentaje.HasValue && Retencion_Porcentaje > 0)
            {
                switch (ModoRetencion.Value)
                {
                    case Enums.ModoRetencion.SobreBase:
                        TotalRetencion = BaseImponible * Retencion_Porcentaje / 100;
                        break;
                    case Enums.ModoRetencion.SobreTotal:
                        TotalRetencion = Total * Retencion_Porcentaje / 100;
                        break;
                }
            }

            TotalRetencion = Math.Round(TotalRetencion.Value, 2);
            Total = Total - TotalRetencion.Value;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> list = new List<ValidationResult>();

            if (Origen)
            {
                if (string.IsNullOrWhiteSpace(OrigenCodigoArticulo) && string.IsNullOrWhiteSpace(OrigenNombreArticulo))
                {
                    list.Add(new ValidationResult(
                        "Este campo es obligatorio.",
                        new string[] { nameof(OrigenCodigoArticulo) }));
                }
                if (!OrigenImporte.HasValue)
                {
                    list.Add(new ValidationResult(
                        "Este campo es obligatorio.",
                        new string[] { nameof(OrigenImporte) }));
                }
            }

            if (this.FacturaLineas == null || !this.FacturaLineas.Any())
            {
                list.Add(new ValidationResult("El albarán debe contener al menos una línea."));
                return list;
            }

            // Valida que un albarán no tenga líneas que referencian a la misma línea de pedido
            int lineasVenidasAlbaran = this.FacturaLineas.Where(f => f.VentaAlbaranLineaId.HasValue).Count();
            int lineasDistintas = this.FacturaLineas.Where(f => f.VentaAlbaranLineaId.HasValue).Select(f => f.VentaAlbaranLineaId).Distinct().Count();
            if (lineasDistintas != lineasVenidasAlbaran)
                list.Add(new ValidationResult("Existen varias líneas de factura que pertenecen al mismo albarán. Por favor revise la información."));

            return list;
        }
    }

    
}
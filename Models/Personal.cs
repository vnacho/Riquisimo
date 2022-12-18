using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Personal : IValidatableObject
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string NIF { get; set; }

        [Display(Name = "Fecha de validez del NIF")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaValidezNIF { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Categoría")]
        [MaxLength(40)]
        public string Categoria { get; set; }

        [Display(Name = "Coste éstandar")]
        [DataType(DataType.Currency)]
        [UIHint("Currency")]
        public decimal? CosteEstandar { get; set; }

        [Display(Name = "Venta")]
        [DataType(DataType.Currency)]
        [UIHint("Currency")]
        public decimal? Venta { get; set; }

        [Display(Name = "Fecha de alta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [UIHint("Date")]
        public DateTime? FechaAlta { get; set; }

        [Display(Name = "Fecha de última revisión médica")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [UIHint("Date")]
        public DateTime? FechaUltimaRevisionMedica { get; set; }

        [Display(Name = "Revisión médica")]
        public string RevisionMedica { get; set; }

        [Display(Name = "Fecha de baja")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [UIHint("Date")]
        public DateTime? FechaBaja { get; set; }

        [Display(Name = "Fecha apto")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [UIHint("Date")]
        public DateTime? FechaApto { get; set; }

        [Display(Name = "Fecha EPI")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [UIHint("Date")]
        public DateTime? FechaEntregaEpi { get; set; }

        public string IBAN { get; set; }

        public string IBAN_Display => IBAN?.Length > 4 ? IBAN.Substring(0, 4) : IBAN;

        [MaxLength(3)]
        public string SAP { get; set; }

        public bool CT { get; set; }

        [Display(Name = "Precio/Hora")]
        [DataType(DataType.Currency)]
        [UIHint("Currency")]
        public decimal? PrecioHora { get; set; }

        [Display(Name = "Fecha último contrato")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [UIHint("Date")]
        public DateTime? FechaUltimoContrato { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Tipo de tafifa")]
        public int TipoTarifaId { get; set; }

        [Display(Name = "Tipo de tafifa")]
        public TipoTarifa TipoTarifa { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Centro de coste")]
        public int CentroCosteId { get; set; }

        [Display(Name = "Centro de coste")]
        public CentroCoste CentroCoste { get; set; }

        [Display(Name = "Obra")]
        public int? ObraId { get; set; }

        [Display(Name = "Obra")]
        public CentroCoste Obra { get; set; }

        [Display(Name = "Tipo último contrato")]
        public int? TipoUltimoContratoId { get; set; }

        [Display(Name = "Tipo último contrato")]
        public TipoContrato TipoUltimoContrato { get; set; }

        public ICollection<PartePersonal> Partes { get; set; }
        public ICollection<Documento> Documentos { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errores = new List<ValidationResult>();
            if (FechaBaja.HasValue && FechaBaja.Value <= FechaAlta)
                errores.Add(new ValidationResult("La fecha de baja no puede ser menor que la fecha de alta", new string[] { "FechaBaja" }));
            return errores;
        }
    }
}

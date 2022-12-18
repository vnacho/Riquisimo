using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class PartePersonal : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Tipo de parte")]
        [MaxLength(1)]
        public string TipoParte { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = true)]
        public decimal Unidades { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = true)]
        [DataType(DataType.Currency)]
        [UIHint("Currency")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = true)]
        [DataType(DataType.Currency)]
        [UIHint("Currency")]
        public decimal Importe { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Personal")]
        public int PersonalId { get; set; }

        [Display(Name = "Personal")]
        public Personal Personal { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Centro de coste")]
        public int CentroCosteId { get; set; }

        [Display(Name = "Centro de coste")]
        public CentroCoste CentroCoste { get; set; }

        public void Calcular()
        {
            Importe = Math.Round(Unidades * Precio, 2);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errores = new List<ValidationResult>();
            if (TipoParte.Trim() != "M" && TipoParte.Trim() != "P")
                errores.Add(new ValidationResult("El tipo de parte tiene un valor no válido", new string[] { "TipoParte" }));
            return errores;
        }
    }
}

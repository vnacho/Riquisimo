using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class CentroCoste : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nivel analítico 1")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(3)]
        public string NivelAnalitico1 { get; set; }

        [Display(Name = "Nivel analítico 2")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(5)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El campo '{0}' debe ser numérico")]
        public string NivelAnalitico2 { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(80)]
        public string Nombre { get; set; }

        [Display(Name = "Tipo de centro de coste")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public int TipoCentroCosteId { get; set; }
        [Display(Name = "Tipo de centro de coste")]
        public TipoCentroCoste TipoCentroCoste { get; set; }

        public string Display => $"{NivelAnalitico2}-{Nombre}";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (NivelAnalitico1.Trim().Length != 3)
                list.Add(new ValidationResult("El campo Nivel analítico 1 debe tener 3 caracteres", new string[] { "NivelAnalitico1" }));
            if (NivelAnalitico2.Trim().Length != 5)
                list.Add(new ValidationResult("El campo Nivel analítico 2 debe tener 5 caracteres", new string[] { "NivelAnalitico2" }));
            return list;
            
        }
    }
}

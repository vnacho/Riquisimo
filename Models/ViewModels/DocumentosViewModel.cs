using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.ViewModels
{
    public class DocumentosViewModel : IValidatableObject
    {
        //[Display(Name = "Plantilla")]
        //[Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = false)]
        //public PlantillaDocumento? Plantilla { get; set; }

        [Display(Name = "Trabajador")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = false)]
        public int? IdPersonal { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Fecha { get; set; }

        [Display(Name = "Obra")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = false)]
        public int? IdObra { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            //if (Plantilla.HasValue && 
            //    (Plantilla == PlantillaDocumento.MAQUINARIA || Plantilla == PlantillaDocumento.INF_PS) && 
            //    !IdObra.HasValue)
            //    errors.Add(new ValidationResult("La obra es obligatoria para el tipo de documento seleccionado.", new string[] { "IdObra" }));
            return errors;
        }
    }
}

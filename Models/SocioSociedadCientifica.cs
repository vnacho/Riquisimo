using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class SocioSociedadCientifica : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        #region Relaciones

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Sociedad científica")]
        public int SociedadCientificaId { get; set; }
        [Display(Name = "Sociedad científica")]
        public SociedadCientifica SociedadCientifica { get; set; }

        [Display(Name = "Cargo en la junta directiva")]
        public int? CargoJuntaDirectivaSociedadCientificaId { get; set; }
        [Display(Name = "Cargo en la junta directiva")]
        public CargoJuntaDirectivaSociedadCientifica CargoJuntaDirectivaSociedadCientifica { get; set; }

        #endregion

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "NIF")]
        public string NIF { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Display(Name = "Junta directiva")]
        public bool JuntaDirectiva { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Fecha de inicio en el cargo")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaInicioCargo { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Fecha de fin en el cargo")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaFinCargo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errores = new List<ValidationResult>();
            if (JuntaDirectiva && !CargoJuntaDirectivaSociedadCientificaId.HasValue)
                errores.Add(new ValidationResult("Debe seleccionar un cargo en la junta directiva.", new string[] { nameof(CargoJuntaDirectivaSociedadCientificaId) }));
            if (FechaFinCargo <= FechaInicioCargo)
                errores.Add(new ValidationResult("La fecha de fin del cargo debe ser mayor al fecha de inicio.", new string[] { nameof(FechaFinCargo) }));

            return errores;
        }
    }
}

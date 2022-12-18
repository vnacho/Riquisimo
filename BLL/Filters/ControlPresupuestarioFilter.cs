using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class ControlPresupuestarioFilter : IValidatableObject
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public TipoInformeControlPresupuestario? Tipo { get; set; }

        [Display(Name = "Fecha desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public DateTime? FechaDesde { get; set; }

        [Display(Name = "Fecha hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaHasta { get; set; }

        [Display(Name = "Tipo de coste")]
        public string TipoCosteCode { get; set; }

        [Display(Name = "Centro de coste")]
        public string CentroCosteCode { get; set; }

        //[Display(Name = "Cuenta (prefijo hasta 5 caracteres)")]
        [Display(Name = "Cuenta")]
        [MaxLength(8)]
        public string PrefijoCuenta { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> list = new List<ValidationResult>();

            if (!FechaDesde.HasValue && !FechaHasta.HasValue)
                list.Add(new ValidationResult("Debe introducir algún valor en alguna de las fechas.", new string[] { "FechaDesde", "FechaHasta" }));

            if (FechaDesde.HasValue && FechaHasta.HasValue && FechaDesde.Value > FechaHasta.Value)
                list.Add(new ValidationResult("El campo 'Fecha hasta' no puede ser menor que el campo 'Fecha desde'.", new string[] { "FechaHasta" }));

            return list;
            
        }
    }
}

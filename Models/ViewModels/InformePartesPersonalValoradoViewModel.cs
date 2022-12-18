using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.Models.ViewModels
{
    public class InformePartesPersonalValoradoViewModel : IValidatableObject
    {
        [Display(Name = "Centro de coste")]
        public int? CentroCosteId { get; set; }

        [Display(Name = "Fecha desde")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = false)]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]        
        public DateTime? FechaDesde { get; set; }

        [Display(Name = "Fecha hasta")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaHasta { get; set; }

        [Display(Name = "Desglosado por obra")]
        public bool DesglosadoPorObra { get; set; }

        public IEnumerable<PartePersonal> Items { get; set; }

        public Expression<Func<PartePersonal, bool>> ExpressionFilter()
        {
            return f =>
                (CentroCosteId.HasValue ? f.CentroCosteId == CentroCosteId.Value : true) &&
                (f.Fecha >= FechaDesde) &&
                (f.Fecha <= FechaHasta);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errores = new List<ValidationResult>();
            if (FechaDesde.HasValue && FechaDesde.HasValue)
            {
                //int Dias = FechaHasta.Value.Subtract(FechaDesde.Value).Days;
                //if (Dias > 31)
                //    errores.Add(new ValidationResult("No se pueden consultar más de 31 días de diferencia.", new string[] { "FechaDesde", "FechaHasta" }));

                if (FechaDesde.Value > FechaHasta.Value)
                    errores.Add(new ValidationResult("La fecha hasta no puede ser menor que la fecha desde.", new string[] { "FechaHasta" }));
            }
            return errores;
        }
    }
}

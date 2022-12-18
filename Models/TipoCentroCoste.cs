using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class TipoCentroCoste
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Tipo")]
        [MaxLength(1)]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Descripción")]
        [MaxLength(80)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = true)]
        [Display(Name = "Porcentaje de distribución")]
        [UIHint("Percentage")]
        [Range(0,100, ErrorMessage = "El campo {0} debe ser un valor entre {1} y {2}")]
        public decimal PorcentajeDistribucion { get; set; }        
    }
}

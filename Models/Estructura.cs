using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Estructura
    {
        [Key]
        public int Id { get; set; }        

        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = true)]
        [Display(Name = "Porcentaje de reparto")]
        [Range(0, 100, ErrorMessage = "El campo {0} debe ser un valor entre {1} y {2}")]
        public decimal PorcentajeReparto { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Centro de coste")]
        public int CentroCosteId { get; set; }

        [Display(Name = "Centro de coste")]
        public CentroCoste CentroCoste { get; set; }
    }
}

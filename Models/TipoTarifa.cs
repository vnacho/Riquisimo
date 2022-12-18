using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class TipoTarifa
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Código")]
        [MaxLength(1)]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Nombre")]
        [MaxLength(40)]
        public string Nombre { get; set; }
    }
}

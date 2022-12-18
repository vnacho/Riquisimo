using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class VerOrigen
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Código nivel analítico 1")]
        [MaxLength(3)]
        [MinLength(3)]
        public string NivelAnalitico1 { get; set; }
    }
}

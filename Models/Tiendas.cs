using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Tienda : BaseModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string nombre { get; set; }

        [Display(Name = "Icono")]
        public string icono { get; set; }
    }
}

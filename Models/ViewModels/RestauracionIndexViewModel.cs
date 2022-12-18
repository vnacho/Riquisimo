using System;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models.ViewModels
{
    public class RestauracionIndexViewModel
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Evento")]
        public Guid CongressId { get; set; }

        public Congress Congress { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "NIF")]
        public string NIF { get; set; }

        
    }
}

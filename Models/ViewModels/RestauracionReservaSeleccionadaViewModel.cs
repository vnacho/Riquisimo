using System;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models.ViewModels
{
    public class RestauracionReservaSeleccionadaViewModel
    {
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Evento")]
        public Guid CongressId { get; set; }

        [Display(Name = "Evento")]
        public Congress Congress { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Restauración")]
        public int EncuentroId { get; set; }

        [Display(Name = "Restauración")]
        public Encuentro Encuentro { get; set; }

        [Required(ErrorMessage = "Debe introducir algún NIF")]
        [Display(Name = "NIFs")]
        public string NIFs { get; set; }

    }
}

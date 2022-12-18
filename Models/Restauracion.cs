using System;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class Restauracion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Encuentro")]
        public int EncuentroId { get; set; }
        [Display(Name = "Encuentro")]
        public Encuentro Encuentro { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Persona inscrita")]
        public Guid RegistrationId { get; set; }
        [Display(Name = "Inscrito")]
        public Registration Registration { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string NIF { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Fecha y hora de reserva")]
        public DateTime FechaHoraReserva { get; set; }

        [Display(Name = "Nº de mesa")]
        public int NumeroMesa { get; set; }


    }
}

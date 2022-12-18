using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ferpuser.Models
{
    public class Encuentro
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [ForeignKey(nameof(Congress))]
        [Display(Name = "Evento")]
        public Guid CongressId { get; set; }

        [Display(Name = "Evento")]
        public Congress Congress { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Lugar")]
        public string Lugar { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Número de mesas")]
        public int NumeroMesas { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Comensales por mesa")]
        public int ComensalesMesa { get; set; }

        [Display(Name = "Total de comensales")]
        public int TotalComensales { get; set; }

        [Display(Name = "Reservados")]
        public int Reservados { get; set; }

        [Display(Name = "Libres")]
        public int Libres { get; set; }

        [Display(Name = "Reserva mesa")]
        public bool ReservaMesa { get; set; }
    }
}

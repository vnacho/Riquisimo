using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class CargoJuntaDirectivaSociedadCientifica
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(50)]
        public string Nombre { get; set; }
    }
}

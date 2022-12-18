using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class SociedadCientifica
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; }
    }
}

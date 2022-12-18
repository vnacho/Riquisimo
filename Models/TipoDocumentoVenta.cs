using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class TipoDocumentoVenta
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(40)]
        public string Descripcion { get; set; }
    }
}

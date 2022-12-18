using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class PuestoComite
    {
        [Key]
        public int Id { get; set; }

        //[Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
    }
}

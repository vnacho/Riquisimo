using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class CategoriaInscrito
    {
        /// <summary>
        /// Se utiliza un string como clave porque tiene concordancia con datos externos.
        /// El usuario debe poder asignar esa clave manualmente.
        /// </summary>
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
    }
}
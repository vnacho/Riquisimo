using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class Parametro
    {
        [Key]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Formato")]
        public string Formato { get; set; }

        [Display(Name = "Valor")]
        public string Valor { get; set; }
    }
}

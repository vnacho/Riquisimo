using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models.Enums
{
    public enum Tratamiento
    {
        [Display(Name="D.")]
        D = 1,
        [Display(Name = "Dª.")]
        Da = 2,
        [Display(Name = "Dr. D.")]
        DrD = 3,
        [Display(Name = "Dra. Dª")]
        DraDa = 4
    }
}

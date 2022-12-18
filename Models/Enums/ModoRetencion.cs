using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models.Enums
{
    public enum ModoRetencion
    {
        [Display(Name="Sobre total")]
        SobreTotal = 0,
        [Display(Name = "Sobre base")]
        SobreBase = 1
    }
}

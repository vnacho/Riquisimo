using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models.Enums
{
    public enum EstadoFactura
    {
        [Display(Name = "No traspasado")]
        NoTaspasadoSAGE,
        [Display(Name = "Traspasado")]
        TaspasadoSAGE
    }
}

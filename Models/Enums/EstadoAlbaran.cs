using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models.Enums
{
    public enum EstadoAlbaran
    {
        [Display(Name = "No facturado")]
        NoFacturado,
        Facturado
    }
}

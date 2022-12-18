using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Enums
{
    public enum EstadoPedido
    {
        Pendiente = 0,
        [Display(Name = "Parcial")]
        PendienteParcial = 1,
        Servido = 2
    }
}

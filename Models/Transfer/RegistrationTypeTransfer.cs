using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Transfer
{
    public class RegistrationTypeTransfer
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int PrecioAntes { get; set; }
        public int PrecioDespues { get; set; }
        public string Activo { get; set; }
        public sbyte Orden { get; set; }
    }
}

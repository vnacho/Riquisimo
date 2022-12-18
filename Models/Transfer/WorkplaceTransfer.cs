using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Transfer
{
    public class WorkplaceTransfer
    {
        public uint Id { get; set; }
        public string CodCentro { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Telefono2 { get; set; }
        public string Fax { get; set; }
        public string Mail { get; set; }
        public sbyte Provincia { get; set; }
        public string CodPostal { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Transfer
{
    public class HotelTransfer
    {
        public byte Id { get; set; }
        public string Activo { get; set; }
        public byte Orden { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Mail { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public string Estrellas { get; set; }
        public float? PrecioIndividual { get; set; }
        public float? PrecioDoble { get; set; }
        public string CompletoIndividual { get; set; }
        public string CompletoDoble { get; set; }
        public float CoordenadasX { get; set; }
        public float CoordenadasY { get; set; }
        public string Observaciones { get; set; }
    }
}

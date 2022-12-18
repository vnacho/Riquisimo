using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Dtos
{
    public class ControlPresupuestarioDto
    {
        public string TipoCosteCode { get; set; }
        public string TipoCosteNombre { get; set; }
        public string CentroCosteCode { get; set; }
        public string CentroCosteNombre { get; set; }
        public string CuentaCode { get; set; }
        public string CuentaNombre { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public decimal RTDO => Haber - Debe; //ReadOnly

        //Informe N4
        public string DefApunte { get; set; }
        public DateTime FechaApunte { get; set; }

        public string Factura { get; set; }
        public string CodigoProveedor { get; set; }
        public string UrlDocumento { get; set; }
        public string NombreDocumento { get; set; }
    }
}

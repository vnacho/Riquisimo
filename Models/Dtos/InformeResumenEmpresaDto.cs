using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Dtos
{
    public class InformeResumenEmpresaDto
    {
        public string TipoCosteCode { get; set; }
        public string TipoCosteNombre { get; set; }
        public string CentroCosteCode { get; set; }
        public string CentroCosteNombre { get; set; }
        public string CuentaCode { get; set; }
        public string CuentaNombre { get; set; }

        public decimal DebeMes { get; set; }
        public decimal HaberMes { get; set; }
        public decimal RTDOMes => HaberMes - DebeMes; 
        public decimal TasaRendimientoMes => HaberMes == 0 ? 0 : RTDOMes / HaberMes * 100;

        public decimal DebeAnio { get; set; }
        public decimal HaberAnio { get; set; }
        public decimal RTDOAnio => HaberAnio - DebeAnio;
        public decimal TasaRendimientoAnio => HaberAnio == 0 ? 0 : RTDOAnio / HaberAnio * 100;

        public decimal DebeOrigen { get; set; }
        public decimal HaberOrigen { get; set; }
        public decimal RTDOOrigen => HaberOrigen - DebeOrigen;
        public decimal TasaRendimientoOrigen => HaberOrigen == 0 ? 0 : RTDOOrigen / HaberOrigen * 100;

        public bool AddOrigen { get; set; }
        ////Informe N4
        //public string DefApunte { get; set; }
        //public DateTime FechaApunte { get; set; }

        //public string Factura { get; set; }
        //public string CodigoProveedor { get; set; }
        //public string UrlDocumento { get; set; }
        //public string NombreDocumento { get; set; }
    }
}

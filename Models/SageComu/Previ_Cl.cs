using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models.SageComu
{
    public class Previ_Cl
    {
        //[Key]
        public string Empresa { get; set; }
        public string Cliente { get; set; }
        [Key]
        public string Factura { get; set; }
        //[Key]
        public int Orden { get; set; }
        public DateTime? Vencim { get; set; }
        public decimal Importe { get; set; }
        public decimal ImporteDiv { get; set; }
        public string Divisa { get; set; }
        
        public string Banco { get; set; }
        public DateTime? Emision { get; set; }
        public bool Asiento { get; set; }
        public DateTime? FeCreme {get; set;}
        public DateTime? Cobro {get; set;}
        //[Key]
        public int Impagado { get; set; }
        //[Key]
        public int Periodo { get; set; }
        //[Key]
        public decimal Pendiente { get; set; }
        //[Key]
        public string Doccargo { get; set; }
        public string Asi { get; set; }

        //[Column(TypeName = "smalldatetime")]
        public DateTime? FEC_OPER { get; set; }
        
    }
}

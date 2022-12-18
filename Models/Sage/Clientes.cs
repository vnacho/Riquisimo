using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public class Clientes
    {
        [Key]
        public string Codigo { get; set; }
        public string CIF { get; set; }
        public string Nombre { get; set; }
        public string Nombre2 { get; set; }
        public string Direccion { get; set; }
        public string CodPost { get; set; }
        public string Poblacion { get; set; }
        public string Provincia { get; set; }
        public string Pais { get; set; }
        public string Email { get; set; }
        public string Email_f { get; set; }
        public bool RETENCION { get; set; }
        public bool RETNOFISC { get; set; }
        public bool MODO_RET { get; set; }
        public int MODRETNOFI { get; set; }
        public decimal TPCRETNOFI { get; set; }
        public string TARIFA { get; set; }
        public string TIPO_RET { get; set; }
        public string VENDEDOR { get; set; }
        public string FPAG { get; set; }
        public bool RECARGO { get; set; }

        public string DisplayName => $"{Codigo.Trim()} {Nombre.Trim()}";
        public string DisplayNameCIF => $"{Nombre.Trim()} {CIF.Trim()}";
    }
}

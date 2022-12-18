using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public class Env_Cli
    {
        [Key]
        public int Linea { get; set; }
        [Key]
        public string Cliente { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string CodPos { get; set; }
        public string Poblacion { get; set; }
        public string Provincia { get; set; }
        public string Pais { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public int TIPO { get; set; }
    }
}

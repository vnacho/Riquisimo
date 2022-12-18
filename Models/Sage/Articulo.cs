using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public class Articulo
    {
        [Key]
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string NOMBRE2 { get; set; }
        public int Tipo_Art { get; set; }
        public string Familia { get; set; }
        public string TIPO_IVA { get; set; }

        public string Display
        {
            get
            {
                return Codigo.Trim() + " " + Nombre.Trim();
            }
        }
    }
}

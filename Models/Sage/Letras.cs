using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public class Letras
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Grupo { get; set; }

        [NotMapped]
        public string Display
        {
            get
            {
                return Codigo + " " + Nombre;
            }
        }
    }
}

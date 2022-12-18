using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public class Vendedor
    {
        [Key]
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Display
        {
            get
            {
                return Codigo + " " + Nombre;
            }
        }
    }
}

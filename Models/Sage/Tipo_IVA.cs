using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public class Tipo_IVA
    {
        [Key]
        public string Codigo { get; set; } = "03";
        public string Nombre { get; set; }
        public decimal IVA { get; set; }
    }
}

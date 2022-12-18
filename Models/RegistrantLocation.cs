using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class RegistrantLocation : Location
    {
        [Display(Name = "Persona inscrita")]
        public Guid? RegistrantId { get; set; }
        public Registrant Registrant { get; set; }
    }
}

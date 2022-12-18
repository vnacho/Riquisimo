using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models.ViewModels
{
    public class RestauracionEncuentrosViewModel
    {
        [Display(Name = "Evento")]
        public Guid CongressId { get; set; }

        [Display(Name = "Evento")]
        public Congress Congress { get; set; }  
        
        [Display(Name = "NIF")]
        public string NIF { get; set; }
        
        [Display(Name = "Inscripción")]
        public Registration Inscripcion { get; set; }

        [Display(Name = "Encuentros")]
        public IEnumerable<Encuentro> Encuentros { get; set; }
    }
}

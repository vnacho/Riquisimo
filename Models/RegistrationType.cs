using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class RegistrationType : BaseModel
    {

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
    }
}

using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Treatment : BaseModel
    {
        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
}

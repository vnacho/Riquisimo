using LazZiya.ExpressLocalization.Messages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ferpuser.Models
{
    public class Client : BaseModel
    {
        #nullable enable
        [Display(Name = "Código Sage")]
        public string? SageCode { get; set; }
        #nullable restore
        
        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Razón Social")]
        public string BusinessName { get; set; }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Index(IsUnique = true)]
        [Display(Name = "NIF")]
        public string NIF { get; set; }

        [Display(Name = "Correo electrónico")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Correo electrónico alternativo")]
        [DataType(DataType.EmailAddress)]
        public string Email2 { get; set; }

        [Display(Name = "Direcciones")]
        public List<ClientLocation> Locations { get; set; }

    }
}

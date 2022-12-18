using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Accommodation : RegistrationBase
    {
        public Accommodation()
        {
            Serie = "A ";
            Product = "70800";
        }

        //[Display(Name = "Serie")]
        //new public string Serie { get; set; } = "A ";
        //[Display(Name = "Artículo")]
        //new public string Product { get; set; } = "70800";

        [Display(Name = "Acompañante")]
        public Guid? CompanionId { get; set; }
        [Display(Name = "Acompañante")]
        public Registrant Companion { get; set; }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Fecha de entrada")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Fecha de salida")]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Hotel")]
        public string Hotel { get; set; }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Tipo de habitación")]
        public Guid RoomTypeId { get; set; }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Tipo de habitación")]
        public RoomType RoomType { get; set; }

        [Display(Name = "Inscripción")]
        public Registration Registration { get; set; }
        [Display(Name = "Inscripción")]
        [ForeignKey("Registration")]
        public Guid? RegistrationId { get; set; }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "IVA")]
        public override string VATId { get; set; } = "02";

        [Display(Name = "Es invitado")]
        public bool EsInvitado { get; set; }

    }
}

using Ferpuser.Models.Sage;
using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Registration : RegistrationBase
    {
        public Registration()
        {
            Serie = "I ";
            Product = "70100";
        }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Tipo de inscripción")]
        public Guid RegistrationTypeId { get; set; }
        [Display(Name = "Tipo de inscripción")]
        public RegistrationType RegistrationType { get; set; }

        [Display(Name = "Alojamiento")]
        public Accommodation Accommodation { get; set; }
        [Display(Name = "Alojamiento")]
        public Guid? AccommodationId { get; set; }

        [Display(Name = "Ha asistido")]
        [UIHint("NullableBool")]
        public bool? HaAsistido { get; set; }

        [Display(Name = "Créditos")]
        [UIHint("NullableBool")]
        public bool? Creditos { get; set; }

        [NotMapped]
        public bool IsCheckedHelper { get; set; }

    }
}

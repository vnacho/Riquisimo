using Ferpuser.Models.Sage;
using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class RegistrationBase : CostCenterProduct
    {
        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Persona inscrita")]
        public Guid RegistrantId { get; set; }
        [Display(Name = "Persona inscrita")]
        public Registrant Registrant { get; set; }
        public bool IsInvited
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Registrant.Category))
                {
                    return false;
                }
                var cat = Registrant.Category.ToLower().Trim();
                return cat.StartsWith("invit");
            }
        }
        public string DisplayName
        {
            get
            {
                string s = "" + Number;
                if (Registrant != null)
                {
                    s += " " + Registrant.FullName;
                }
                if (Client != null)
                {
                    s += " " + Client.BusinessName;
                }
                return s;
            }
        }
    }
}

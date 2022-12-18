using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Location : BaseModel
    {

        //[Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        //[Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Población")]
        public string City { get; set; }

        //[Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Código Postal")]
        public string ZipCode { get; set; }

        //[Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Provincia")]
        public string Province { get; set; }

        //[Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "País")]
        public string Country { get; set; }

        //[Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Número de teléfono móvil")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Número de teléfono fijo")]
        [DataType(DataType.PhoneNumber)]
        public string Phone2 { get; set; }

        public string FullAddress
        {
            get
            {
                return Address + " " + ZipCode + " " + Province;
            }
        }

        public string FullAddressWithPhone
        {
            get
            {
                return FullAddress + " (" + Phone + ")";
            }
        }
    }
}

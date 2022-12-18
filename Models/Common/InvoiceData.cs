using IbanNet.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class InvoiceData : BaseModel
    {

        [Display(Name = "Logo")]
        public string LogoBase64 { get; set; }

        [Display(Name = "Faldón")]
        public string TailBase64 { get; set; }

        [Iban]
        [Display(Name = "Banco del congreso")]
        public string IBAN { get; set; }

        [Display(Name = "Cuenta contable")]
        public string SageAccount { get; set; }
    }
}

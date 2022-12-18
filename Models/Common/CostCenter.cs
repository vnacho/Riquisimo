using IbanNet.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class CostCenter : BaseModel
    {

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Es congreso")]
        public bool IsCongress { get; set; }

        #region InvoiceData
        public InvoiceData InvoiceData { get; set; }
        public Guid? InvoiceDataId { get; set; }
        [Iban]
        [Display(Name = "Banco del congreso")]
        public string IBAN { get; set; }

        [Display(Name = "Cuenta contable")]
        public string SageAccount { get; set; }

        [Display(Name = "Código SWIFT")]
        public string SwiftCode { get; set; }

        [Display(Name = "Logo")]
        public string LogoBase64 { get; set; }

        [Display(Name = "Faldón")]
        public string TailBase64 { get; set; }
        #endregion

        #region Mail Fields
        [Display(Name = "Servidor entrante")]
        public string IncomingMailServer { get; set; }
        [Display(Name = "Servidor saliente")]
        public string OutgoingMailServer { get; set; }
        [Display(Name = "Puerto entrante")]
        public int IncomingMailPort { get; set; }
        [Display(Name = "Puerto saliente")]
        public int OutgoingMailPort { get; set; }

        [Display(Name = "Usuario")]
        public string MailUser { get; set; }
        [Display(Name = "Contraseña")]
        public string MailPassword { get; set; }

        [Display(Name = "Pie de página antes imagen")]
        public string SignatureBefore { get; set; }

        [Display(Name = "Imagen de pie de página")]
        public string Signature { get; set; }

        [Display(Name = "Pie de página despues de imagen")]
        public string SignatureAfter { get; set; }
        #endregion
    }
}

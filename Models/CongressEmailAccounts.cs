using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class CongressEmailAccounts : BaseModel
    {
        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Congreso")]
        public Guid CongressId { get; set; }
        [Display(Name = "Congreso")]
        public Congress Congress { get; set; }


        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Usuario")]
        public Guid AccountId { get; set; }
        [Display(Name = "Usuario")]
        public Account Account { get; set; }

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
    }
}

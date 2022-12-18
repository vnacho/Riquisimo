using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class ClientLocation : Location
    {
        [Display(Name = "Línea Sage")]
        public int? SageLine { get; set; }
        [Display(Name = "Código Sage")]
        #nullable enable
        public string? SageClient { get; set; }
        #nullable restore
        [Display(Name = "Cliente")]
        public Guid? ClientId { get; set; }
        public Client Client { get; set; }

    }
}

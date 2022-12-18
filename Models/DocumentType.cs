using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class DocumentType : BaseModel
    {
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        public bool IsInvoice()
        {
            return Name.Equals("Factura");
        }
    }
}

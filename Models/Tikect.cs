using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Tikect : BaseModel
    {

        
        public Tienda tienda { get; set; }

        [Display(Name = "Tienda")]
        public Guid tiendaID { get; set; } = new Guid("03E14C49-40F9-4702-84E8-366543D03D90");

        [Display(Name = "Fecha Compra")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [UIHint("Date")]
        public DateTime FechaTikect { get; set; } = DateTime.Now;

        [Display(Name = "Importe")]
        public decimal importe { get; set; }    

    }
}

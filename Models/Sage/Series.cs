using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public class Series
    {
        public decimal? Contador { get; set; }
        public string Empresa { get; set; }

        public string Serie { get; set; }
        public int Tipodoc { get; set; }
        public bool? Vista { get; set; }
        [Key]
        public string Guid_Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}

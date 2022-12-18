using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class BaseModel
    {
        [Key]
        [Display(Name = "Identificador interno")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Display(Name = "Fecha de creación")]
        [HiddenInput(DisplayValue = false)]
        public DateTime Created { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de modificación")]
        [HiddenInput(DisplayValue = false)]
        public DateTime Modified { get; set; } = DateTime.Now;
        [HiddenInput(DisplayValue = false)]
        public DateTime? Deleted { get; set; }
    }
}

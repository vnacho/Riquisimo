using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class PartePersonalFilter
    {
        [Display(Name = "Personal")]
        public int? PersonalId { get; set; }

        [Display(Name = "Centro de coste")]
        public int? CentroCosteId { get; set; }

        [Display(Name = "Fecha desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaDesde { get; set; }

        [Display(Name = "Fecha hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaHasta { get; set; }

        public Expression<Func<PartePersonal, bool>> ExpressionFilter()
        {
            return f => 
                (PersonalId.HasValue ? f.PersonalId == PersonalId.Value : true) &&
                (CentroCosteId.HasValue ? f.CentroCosteId == CentroCosteId.Value : true) &&
                (FechaDesde.HasValue ? f.Fecha >= FechaDesde : true) &&
                (FechaHasta.HasValue ? f.Fecha <= FechaHasta : true)
                ;
        }
    }
}

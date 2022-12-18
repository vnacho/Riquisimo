using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class CentroCosteFilter
    {
        [Display(Name = "Nivel analítico 1")]
        [MaxLength(3)]
        public string NivelAnalitico1 { get; set; }

        [Display(Name = "Nivel analítico 2")]
        [MaxLength(5)]
        public string NivelAnalitico2 { get; set; }

        [MaxLength(80)]
        public string Nombre { get; set; }

        [Display(Name = "Tipo de centro de coste")]
        public int? TipoCentroCosteId { get; set; }

        public Expression<Func<CentroCoste, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(NivelAnalitico1) ? true : f.NivelAnalitico1.Contains(NivelAnalitico1.Trim())) &&
                (string.IsNullOrWhiteSpace(NivelAnalitico2) ? true : f.NivelAnalitico2.Contains(NivelAnalitico2.Trim())) &&
                (string.IsNullOrWhiteSpace(Nombre) ? true : f.Nombre.Contains(Nombre.Trim())) &&
                (TipoCentroCosteId.HasValue ? f.TipoCentroCosteId == TipoCentroCosteId.Value : true);
        }
    }
}

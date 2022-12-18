using Ferpuser.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Ferpuser.BLL.Filters
{
    public class RestauracionFilter
    {
        [Display(Name = "Buscar por nombre, apellidos o NIF")]
        public string Term { get; set; }

        [Display(Name = "Encuentro")]
        public int? EncuentroId { get; set; }

        [Display(Name = "Nº de mesa")]
        public int? NumeroMesa { get; set; }

        public Expression<Func<Restauracion, bool>> ExpressionFilter()
        {
            return f =>
                (EncuentroId.HasValue ? f.EncuentroId == EncuentroId : true) &&
                (NumeroMesa.HasValue ? f.NumeroMesa == NumeroMesa : true) &&
                (string.IsNullOrWhiteSpace(Term) ? true : (
                    f.Registration.Registrant.Name.Contains(Term) ||
                    f.Registration.Registrant.Surnames.Contains(Term) ||
                    f.NIF.Contains(Term)
                ));
        }
    }
}

using Ferpuser.Data;
using Ferpuser.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.BLL.BusinessValidators
{
    public class TiendaValidator
    {
        public ApplicationDbContext _db { get; set; }

        public TiendaValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(Tienda model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }

        public IEnumerable<ValidationResult> Edit(Tienda model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

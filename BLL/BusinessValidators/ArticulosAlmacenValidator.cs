using Ferpuser.Data;
using Ferpuser.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.BLL.BusinessValidators
{
    public class ArticulosAlmacenValidator
    {
        public ApplicationDbContext _db { get; set; }

        public ArticulosAlmacenValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(ArticulosAlmacen model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }

        public IEnumerable<ValidationResult> Edit(ArticulosAlmacen model)
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

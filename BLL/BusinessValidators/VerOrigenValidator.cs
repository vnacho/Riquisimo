using Ferpuser.Data;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class VerOrigenValidator
    {
        public ApplicationDbContext _db { get; set; }

        public VerOrigenValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(VerOrigen model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.VerOrigen.Any(f => f.NivelAnalitico1.Trim() == model.NivelAnalitico1.Trim()))
                list.Add(new ValidationResult("Ya existe un elemento con ese código.", new string[] { "NivelAnalitico1" }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(VerOrigen model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.VerOrigen.Any(f => f.Id != model.Id && f.NivelAnalitico1.Trim() == model.NivelAnalitico1.Trim()))
                list.Add(new ValidationResult("Ya existe un elemento con ese código.", new string[] { "NivelAnalitico1" }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

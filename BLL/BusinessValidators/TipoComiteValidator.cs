using Ferpuser.Data;
using Ferpuser.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ferpuser.BLL.BusinessValidators
{
    public class TipoComiteValidator
    {
        public ApplicationDbContext _db { get; set; }

        public TipoComiteValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(TipoComite model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.TiposComite.Any(f => f.Nombre.Trim() == model.Nombre.Trim()))
                list.Add(new ValidationResult("Ya existe un tipo de comité con ese nombre.", new string[] { nameof(TipoComite.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(TipoComite model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.TiposComite.Any(f => f.Id != model.Id && f.Nombre.Trim() == model.Nombre.Trim()))
                list.Add(new ValidationResult("Ya existe un tipo de comité con ese nombre.", new string[] { nameof(TipoComite.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

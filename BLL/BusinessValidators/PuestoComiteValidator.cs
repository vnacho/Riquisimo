using Ferpuser.Data;
using Ferpuser.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ferpuser.BLL.BusinessValidators
{
    public class PuestoComiteValidator
    {
        public ApplicationDbContext _db { get; set; }

        public PuestoComiteValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(PuestoComite model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.PuestosComite.Any(f => f.Nombre.Trim() == model.Nombre.Trim()))
                list.Add(new ValidationResult("Ya existe un puesto de comité con ese nombre.", new string[] { nameof(PuestoComite.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(PuestoComite model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.PuestosComite.Any(f => f.Id != model.Id && f.Nombre.Trim() == model.Nombre.Trim()))
                list.Add(new ValidationResult("Ya existe un puesto de comité con ese nombre.", new string[] { nameof(PuestoComite.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}


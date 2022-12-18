using Ferpuser.Data;
using Ferpuser.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ferpuser.BLL.BusinessValidators
{
    public class PonenteValidator
    {
        public ApplicationDbContext _db { get; set; }

        public PonenteValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(Ponente model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            //if (!string.IsNullOrWhiteSpace(model.IdExterno)  _db.Ponentes.Any(f => f.IdExterno.Trim() == model.IdExterno.Trim()))
            //    list.Add(new ValidationResult("Ya existe una categoría con ese Id Externo.", new string[] { nameof(Ponente.IdExterno) }));
            //if (_db.Ponentes.Any(f => f.Nombre.Trim() == model.Nombre.Trim()))
            //    list.Add(new ValidationResult("Ya existe una categoría con ese nombre.", new string[] { nameof(Ponente.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(Ponente model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            //if (_db.Ponentes.Any(f => f.Id != model.Id && f.Nombre.Trim() == model.Nombre.Trim()))
            //    list.Add(new ValidationResult("Ya existe una categoría con ese nombre.", new string[] { nameof(Ponente.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

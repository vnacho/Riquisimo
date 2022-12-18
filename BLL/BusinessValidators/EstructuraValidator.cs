using Ferpuser.Data;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class EstructuraValidator
    {
        public ApplicationDbContext _db { get; set; }

        public EstructuraValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(Estructura model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.Estructura.Any(f => f.CentroCosteId == model.CentroCosteId))
                list.Add(new ValidationResult("El centro de coste ya existe en otro registro.", new string[] { "CentroCosteId" }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(Estructura model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.Estructura.Any(f => f.CentroCosteId == model.CentroCosteId && f.Id != model.Id))
                list.Add(new ValidationResult("El centro de coste ya existe en otro registro.", new string[] { "CentroCosteId" }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

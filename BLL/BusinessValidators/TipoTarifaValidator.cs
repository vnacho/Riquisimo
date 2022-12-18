using Ferpuser.Data;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class TipoTarifaValidator
    {
        public ApplicationDbContext _db { get; set; }

        public TipoTarifaValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(TipoTarifa model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.TiposTarifa.Any(f => f.Codigo == model.Codigo))
                list.Add(new ValidationResult("El código insertado ya existe en otro registro.",new string[] { "Codigo" } ));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(TipoTarifa model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.TiposTarifa.Any(f => f.Codigo == model.Codigo && f.Id != model.Id))
                list.Add(new ValidationResult("El código insertado ya existe en otro registro.", new string[] { "Codigo" }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

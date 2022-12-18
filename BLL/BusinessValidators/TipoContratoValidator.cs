using Ferpuser.Data;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class TipoContratoValidator
    {
        public ApplicationDbContext _db { get; set; }

        public TipoContratoValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(TipoContrato model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.TipoContrato.Any(f => f.Nombre.Trim() == model.Nombre.Trim()))
                list.Add(new ValidationResult("Ya existe un tipo de contrato con ese nombre.", new string[] { "Nombre" }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(TipoContrato model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.TipoContrato.Any(f => f.Id != model.Id && f.Nombre.Trim() == model.Nombre.Trim()))
                list.Add(new ValidationResult("Ya existe un tipo de contrato  con ese nombre.", new string[] { "Nombre" }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

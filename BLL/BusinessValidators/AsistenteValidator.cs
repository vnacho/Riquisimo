using Ferpuser.Data;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class AsistenteValidator
    {
        public ApplicationDbContext _db { get; set; }
        private readonly SageContext _sage;

        public AsistenteValidator(ApplicationDbContext dbContext, SageContext sage)
        {
            _db = dbContext;
            _sage = sage;
        }

        public IEnumerable<ValidationResult> Create(Asistente model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            list.AddRange(ValidacionesComunes(model));
            if (_db.Asistente.Any(f => f.NIF == model.NIF))
                list.Add(new ValidationResult("El asistente con el NIF indicado ya está introducido.", new string[] { "NIF" }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(Asistente model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            list.AddRange(ValidacionesComunes(model));
            if (_db.Asistente.Any(f => f.NIF == model.NIF && f.Id != model.Id))
                list.Add(new ValidationResult("El asistente con el NIF indicado ya está introducido.", new string[] { "NIF" }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }

        private IList<ValidationResult> ValidacionesComunes(Asistente model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (!string.IsNullOrWhiteSpace(model.CodigoPostal) &&
                !_sage.codpos.Any(f => f.CODIGO.Trim().ToUpper() == model.CodigoPostal.ToUpper()))
            {
                list.Add(new ValidationResult("El código postal introducido no está en los registros de SAGE", new string[] { "CodigoPostal" }));
            }
            
            return list;
        }
    }
}

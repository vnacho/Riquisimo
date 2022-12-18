using Ferpuser.Data;
using Ferpuser.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ferpuser.BLL.BusinessValidators
{
    public class SocioSociedadCientificaValidator
    {
        public ApplicationDbContext _db { get; set; }

        public SocioSociedadCientificaValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(SocioSociedadCientifica model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            //if (!string.IsNullOrWhiteSpace(model.Nombre) && _db.SociosSociedadCientifica.Any(f => f.Nombre.Trim() == model.Nombre.Trim()))
            //    list.Add(new ValidationResult("Ya existe un registro con ese nombre.", new string[] { nameof(model.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(SocioSociedadCientifica model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            //if (!string.IsNullOrWhiteSpace(model.Nombre) && _db.SociosSociedadCientifica.Any(f => f.Nombre.Trim() == model.Nombre.Trim() && f.Id != model.Id))
            //    list.Add(new ValidationResult("Ya existe un registro con ese nombre.", new string[] { nameof(model.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

using Ferpuser.Data;
using Ferpuser.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ferpuser.BLL.BusinessValidators
{
    public class SociedadCientificaValidator
    {
        public ApplicationDbContext db { get; set; }

        public SociedadCientificaValidator(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<ValidationResult> Create(SociedadCientifica model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (!string.IsNullOrWhiteSpace(model.Nombre) && db.SociedadesCientificas.Any(f => f.Nombre.Trim() == model.Nombre.Trim()))
                list.Add(new ValidationResult("Ya existe un registro con ese nombre.", new string[] { nameof(model.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(SociedadCientifica model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (!string.IsNullOrWhiteSpace(model.Nombre) && db.SociedadesCientificas.Any(f => f.Nombre.Trim() == model.Nombre.Trim() && f.Id != model.Id))
                list.Add(new ValidationResult("Ya existe un registro con ese nombre.", new string[] { nameof(model.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(int id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (db.Congresses.Any(f => f.SociedadCientificaId == id))
                list.Add(new ValidationResult("No es posible la eliminación del registro, existe algún evento que está relacionado."));
            if (db.SociosSociedadCientifica.Any(f => f.SociedadCientificaId == id))
                list.Add(new ValidationResult("No es posible la eliminación del registro, existe algún socio que está relacionado."));
            return list;
        }
    }
}

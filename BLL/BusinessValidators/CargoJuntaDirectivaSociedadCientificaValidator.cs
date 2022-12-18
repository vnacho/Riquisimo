using Ferpuser.Data;
using Ferpuser.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ferpuser.BLL.BusinessValidators
{
    public class CargoJuntaDirectivaSociedadCientificaValidator
    {
        public ApplicationDbContext db { get; set; }

        public CargoJuntaDirectivaSociedadCientificaValidator(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<ValidationResult> Create(CargoJuntaDirectivaSociedadCientifica model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (!string.IsNullOrWhiteSpace(model.Nombre) && db.CargosJuntaDirectivaSociedadCientifica.Any(f => f.Nombre.Trim() == model.Nombre.Trim()))
                list.Add(new ValidationResult("Ya existe un registro con ese nombre.", new string[] { nameof(model.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(CargoJuntaDirectivaSociedadCientifica model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (!string.IsNullOrWhiteSpace(model.Nombre) && db.CargosJuntaDirectivaSociedadCientifica.Any(f => f.Nombre.Trim() == model.Nombre.Trim() && f.Id != model.Id))
                list.Add(new ValidationResult("Ya existe un registro con ese nombre.", new string[] { nameof(model.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(int id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (db.SociosSociedadCientifica.Any(f => f.CargoJuntaDirectivaSociedadCientificaId == id))
                list.Add(new ValidationResult("No es posible la eliminación del registro, existe algún socio que está relacionado."));
            return list;
        }
    }
}

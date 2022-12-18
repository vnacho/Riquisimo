using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ferpuser.BLL.BusinessValidators
{
    public class EncuentroValidator
    {
        public ApplicationDbContext _db { get; set; }

        public EncuentroValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(Encuentro model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (!string.IsNullOrWhiteSpace(model.Nombre) && _db.Encuentros.Any(f => f.Nombre.Trim() == model.Nombre.Trim() && f.CongressId == model.CongressId))
                list.Add(new ValidationResult("Ya existe un encuentro con ese nombre.", new string[] { nameof(model.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(Encuentro model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (!string.IsNullOrWhiteSpace(model.Nombre) && _db.Encuentros.Any(f => f.Nombre.Trim() == model.Nombre.Trim() && f.CongressId == model.CongressId && f.Id != model.Id))
                list.Add(new ValidationResult("Ya existe un encuentro con ese nombre.", new string[] { nameof(model.Nombre) }));

            //Comprobar que la nueva cantidad de comensales es mayor a la de reservas efectuadas.
            var original = _db.Encuentros.AsNoTracking().FirstOrDefault(f => f.Id == model.Id);
            if (original.Reservados > (model.NumeroMesas * model.ComensalesMesa))
                list.Add(new ValidationResult("Las reservas efectuadas superan el número de plazas.", new string[] { nameof(model.NumeroMesas), nameof(model.ComensalesMesa) }));

            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

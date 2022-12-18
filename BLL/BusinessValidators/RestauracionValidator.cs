using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ferpuser.BLL.BusinessValidators
{
    public class RestauracionValidator
    {
        public ApplicationDbContext db { get; set; }

        public RestauracionValidator(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public IEnumerable<ValidationResult> ValidateNifs(string[] NIFs, Guid CongressId, int EncuentroId)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            var encuentro = db.Encuentros.AsNoTracking().SingleOrDefault(f => f.Id == EncuentroId);
            if (encuentro.Libres < NIFs.Count())
            {
                list.Add(new ValidationResult($"No existen plazas libres para {NIFs.Count()} nuevas reservas."));
                return list;
            }
            
            foreach (string NIF in NIFs)
            {
                list.AddRange(ValidateNif(NIF, CongressId, EncuentroId));
            }

            return list;
        }

        public IEnumerable<ValidationResult> ValidateNif(string NIF, Guid CongressId, int EncuentroId)
        {
            List<ValidationResult> list = new List<ValidationResult>();

            var encuentro = db.Encuentros.AsNoTracking().SingleOrDefault(f => f.Id == EncuentroId);
            if (encuentro.Libres < 1)
            {
                list.Add(new ValidationResult($"No existen plazas libres para {NIF.Count()} nueva reserva."));
                return list;
            }

            RestauracionManager manager = new RestauracionManager(db);
            var inscripcion = manager.GetInscripcion(NIF, CongressId);
            if (inscripcion == null)
                list.Add(new ValidationResult($"La persona con el NIF '{NIF}' no está inscrito en el congreso."));
            else
            {
                if (db.Restauraciones.Any(f => f.EncuentroId == EncuentroId && f.RegistrationId == inscripcion.Id))
                    list.Add(new ValidationResult($"La persona con el NIF '{NIF}' ya tiene reserva."));
            }
            return list;
        }

        public IEnumerable<ValidationResult> Create(Restauracion model)
        {
            List<ValidationResult> list = new List<ValidationResult>();            
            return list;
        }

        public IEnumerable<ValidationResult> Edit(Restauracion model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

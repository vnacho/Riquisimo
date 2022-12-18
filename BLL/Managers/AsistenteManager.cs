using Ferpuser.BLL.Helpers;
using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class AsistenteManager
    {
        public ApplicationDbContext _db { get; set; }

        public AsistenteManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task Create(Asistente model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.Asistente.Add(model);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(Asistente model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.Entry(model).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Delete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.Entry(_db.Asistente.Find(id)).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task<bool> ExportToAsistente(Guid idregistration)
        {
            var registration = _db.Registrations
                .Include(f => f.Registrant)
                .ThenInclude(f => f.Location)
                .SingleOrDefault(f => f.Id == idregistration);
            if (registration == null || registration.Registrant == null || string.IsNullOrWhiteSpace(registration.Registrant.NIF))
                return false;

            string nifFormatedo = NIFHelper.Format(registration.Registrant.NIF);
            if (string.IsNullOrWhiteSpace(nifFormatedo))
                return false;

            Asistente asistente = _db.Asistente.FirstOrDefault(f => f.NIF == nifFormatedo);
            if (asistente != null) //Ya está dado de alta, salir
                return false;

            using (var transaction = _db.Database.BeginTransaction())
            {
                asistente = new Asistente()
                {
                    NIF = nifFormatedo,
                    TreatmentId = registration.Registrant.TreatmentId,
                    Nombre = registration.Registrant.Name,
                    Apellidos = registration.Registrant.Surnames,
                    CentroTrabajo = registration.Registrant.Workplace,
                    CategoriaProfesional = registration.Registrant.ProfessionalCategory,
                    Cargo = registration.Registrant.Position,
                    Direccion = registration.Registrant.Location?.Address,
                    Poblacion = registration.Registrant.Location?.City,
                    CodigoPostal = registration.Registrant.Location?.ZipCode,
                    Ciudad = registration.Registrant.Location?.Province,                   
                    Telefono1 = registration.Registrant.Location?.Phone,
                    Telefono2 = registration.Registrant.Location?.Phone2,
                    Email1 = registration.Registrant.Email,
                    Email2 = registration.Registrant.Email2
                };
                
                _db.Asistente.Add(asistente);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
    }
}


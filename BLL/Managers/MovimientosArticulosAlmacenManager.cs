using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class MovimientosArticulosAlmacenManager
    {
        public ApplicationDbContext _db { get; set; }

        public MovimientosArticulosAlmacenManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task Create(MovimientosArticulosAlmacen model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.MovimientosArticulosAlmacens.Add(model);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(MovimientosArticulosAlmacen model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.Entry(model).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Delete(Guid id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.Entry(_db.MovimientosArticulosAlmacens.Find(id)).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}


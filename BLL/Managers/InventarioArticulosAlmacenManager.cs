using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class InventarioArticulosAlmacenManager
    {
        public ApplicationDbContext _db { get; set; }

        public InventarioArticulosAlmacenManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task Create(InventarioArticulosAlmacen model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.InventarioArticulosAlmacen.Add(model);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(InventarioArticulosAlmacen model)
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
                _db.Entry(_db.InventarioArticulosAlmacen.Find(id)).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}

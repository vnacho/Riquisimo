using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class TiendaManager
    {
        public ApplicationDbContext _db { get; set; }

        public TiendaManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task Create(Tienda model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.Tiendas.Add(model);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(Tienda model)
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
                _db.Entry(_db.Tiendas.Find(id)).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}

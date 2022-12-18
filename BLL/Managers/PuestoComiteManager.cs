using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class PuestoComiteManager
    {
        public ApplicationDbContext _db { get; set; }

        public PuestoComiteManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task Create(PuestoComite model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.PuestosComite.Add(model);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(PuestoComite model)
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
                _db.Entry(_db.PuestosComite.Find(id)).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}


using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class CategoriaInscritoManager
    {
        public ApplicationDbContext _db { get; set; }

        public CategoriaInscritoManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task Create(CategoriaInscrito model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.CategoriasInscritos.Add(model);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(CategoriaInscrito model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.Entry(model).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Delete(string id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.Entry(_db.CategoriasInscritos.Find(id)).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}
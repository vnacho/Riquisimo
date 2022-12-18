using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class CongressesManager
    {
        public ApplicationDbContext _db { get; set; }

        public CongressesManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public bool PermiteImpresionFormatoA(string codigoEvento)
        {
            var congress = _db.Congresses.AsNoTracking().FirstOrDefault(f => f.Number.ToString() == codigoEvento && !f.Deleted.HasValue);
            if (congress != null && !string.IsNullOrWhiteSpace(congress.LogoBase64) && !string.IsNullOrWhiteSpace(congress.TailBase64))
                return true;
            return false;
        }
        public async Task Create(Congress model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.Congresses.Add(model);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(Congress model)
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
                try
                {
                    _db.Entry(_db.Congresses.Find(id)).State = EntityState.Deleted;
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}

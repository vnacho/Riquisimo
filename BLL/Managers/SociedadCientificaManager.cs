using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class SociedadCientificaManager
    {
        public ApplicationDbContext db { get; set; }

        public SociedadCientificaManager(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task Create(SociedadCientifica model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                db.SociedadesCientificas.Add(model);
                await db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(SociedadCientifica model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Delete(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                db.Entry(db.SociedadesCientificas.Find(id)).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}


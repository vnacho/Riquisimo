using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class SocioSociedadCientificaManager
    {
        public ApplicationDbContext db { get; set; }

        public SocioSociedadCientificaManager(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task Create(SocioSociedadCientifica model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                db.SociosSociedadCientifica.Add(model);
                await db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(SocioSociedadCientifica model)
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
                db.Entry(db.SociosSociedadCientifica.Find(id)).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}

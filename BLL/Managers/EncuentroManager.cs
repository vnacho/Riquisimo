using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class EncuentroManager
    {
        public ApplicationDbContext db { get; set; }        

        public EncuentroManager(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task Create(Encuentro model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                model.TotalComensales = model.NumeroMesas * model.ComensalesMesa;
                model.Libres = model.TotalComensales;

                db.Encuentros.Add(model);
                await db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(Encuentro model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                model.TotalComensales = model.NumeroMesas * model.ComensalesMesa;
                model.Libres = model.TotalComensales - model.Reservados;

                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Delete(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                db.Entry(db.Encuentros.Find(id)).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}

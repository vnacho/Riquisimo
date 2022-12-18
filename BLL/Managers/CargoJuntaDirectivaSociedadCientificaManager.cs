using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class CargoJuntaDirectivaSociedadCientificaManager
    {
        public ApplicationDbContext db { get; set; }

        public CargoJuntaDirectivaSociedadCientificaManager(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task Create(CargoJuntaDirectivaSociedadCientifica model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                db.CargosJuntaDirectivaSociedadCientifica.Add(model);
                await db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(CargoJuntaDirectivaSociedadCientifica model)
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
                db.Entry(db.CargosJuntaDirectivaSociedadCientifica.Find(id)).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}

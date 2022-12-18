using Ferpuser.BLL.Filters;
using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class PartePersonalManager
    {
        public ApplicationDbContext _db { get; set; }

        public PartePersonalManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task Create(PartePersonal model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.PartePersonal.Add(model);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(PartePersonal model)
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
                _db.Entry(_db.PartePersonal.Find(id)).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task<int> Recalcular(PartePersonalFilter filter)
        {   
            try
            {
                IEnumerable<PartePersonal> list = await _db.PartePersonal
                    .Where(filter.ExpressionFilter())
                    .Include(f => f.Personal)
                    .ToListAsync();

                foreach (var item in list)
                {
                    item.Precio = item.Personal.CosteEstandar ?? 0;
                    item.Calcular();
                }
                await _db.SaveChangesAsync();
                return list.Count();
            }
            catch
            {
                return 0;
            }
        }
    }
}

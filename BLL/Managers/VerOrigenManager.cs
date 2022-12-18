using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class VerOrigenManager
    {
        public ApplicationDbContext _db { get; set; }

        public VerOrigenManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task Create(VerOrigen model)
        {
            model.NivelAnalitico1 = model.NivelAnalitico1.Trim();
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.VerOrigen.Add(model);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(VerOrigen model)
        {
            model.NivelAnalitico1 = model.NivelAnalitico1.Trim();
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
                _db.Entry(_db.VerOrigen.Find(id)).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}

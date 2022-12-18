using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class GrabacionE9Manager
    {
        public ApplicationDbContext _db { get; set; }

        public GrabacionE9Manager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task Create(GrabacionE9 model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.GrabacionE9.Add(model);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(GrabacionE9 model)
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
                _db.Entry(_db.GrabacionE9.Find(id)).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}

using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class TipoCentroCosteManager
    {
        public ApplicationDbContext _db { get; set; }

        public TipoCentroCosteManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task Create(TipoCentroCoste model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.TiposCentroCoste.Add(model);                
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(TipoCentroCoste model)
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
                _db.Entry(_db.TiposCentroCoste.Find(id)).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}

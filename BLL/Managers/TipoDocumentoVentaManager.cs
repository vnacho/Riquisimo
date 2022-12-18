using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class TipoDocumentoVentaManager
    {
        public ApplicationDbContext _db { get; set; }

        public TipoDocumentoVentaManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task Create(TipoDocumentoVenta model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.TiposDocumentoVenta.Add(model);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(TipoDocumentoVenta model)
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
                _db.Entry(_db.TiposDocumentoVenta.Find(id)).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}

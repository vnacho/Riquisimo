using Ferpuser.Data;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Dtos;
using Ferpuser.Models.Sage;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class ProveedorSageManager
    {
        private readonly SageContext _sageContext;

        public ProveedorSageManager(SageContext sageContext)
        {
            _sageContext = sageContext;
        }

        public async Task Create(ProveedorSage model)
        {
            using (var transaction = _sageContext.Database.BeginTransaction())
            {
                _sageContext.Proveedores.Add(model);
                await _sageContext.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(ProveedorSage model)
        {
            using (var transaction = _sageContext.Database.BeginTransaction())
            {
                _sageContext.Entry(model).State = EntityState.Modified;
                await _sageContext.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Delete(string CODIGO)
        {
            using (var transaction = _sageContext.Database.BeginTransaction())
            {
                _sageContext.Entry(_sageContext.Proveedores.Find(CODIGO)).State = EntityState.Deleted;
                await _sageContext.SaveChangesAsync();
                transaction.Commit();
            }
        }

    }
}

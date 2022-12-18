using Ferpuser.Data;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class RegistrationManager
    {
        public ApplicationDbContext _db { get; set; }
        private readonly SageComuContext _sageComuContext;

        public RegistrationManager(ApplicationDbContext dbContext, SageComuContext sageComuContext)
        {
            _db = dbContext;
            _sageComuContext = sageComuContext;
        }

        public void ComprobarPagada(Guid id)
        {
            var item = _db.Registrations.FirstOrDefault(r => r.Id.Equals(id));
            if (item == null)
                return;

            string nfactura = item.InvoiceNumber;
            if (string.IsNullOrWhiteSpace(nfactura))
                return;

            var previ_cl = _sageComuContext.Previ_Cl.FirstOrDefault(f => f.Factura == nfactura);
            if (previ_cl == null)
                return;

            item.Paid = previ_cl.Cobro.HasValue;
            if (previ_cl.Cobro.HasValue)
                item.PaidDate = previ_cl.Cobro.Value;

            _db.SaveChanges();
        }

        /// <summary>
        /// Método que comprueba si todas las registrations están pagadas o no
        /// </summary>
        /// <returns></returns>
        public async Task ActualizarPagadas()
        {
            var items = _db.Registrations.Where(f => !f.Paid).ToList(); //Solo comprobamos las que no estén pagadas

            foreach (Registration item in items)
            {
                string nfactura = item.InvoiceNumber;
                if (string.IsNullOrWhiteSpace(nfactura))
                    continue;

                var previ_cl = _sageComuContext.Previ_Cl.FirstOrDefault(f => f.Factura == nfactura);
                if (previ_cl == null)                
                    continue;
                
                item.Paid = previ_cl.Cobro.HasValue;
                if (previ_cl.Cobro.HasValue)
                    item.PaidDate = previ_cl.Cobro.Value;
            }

            await _db.SaveChangesAsync();
        }

    }
}

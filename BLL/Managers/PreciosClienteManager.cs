using Ferpuser.Data;
using Ferpuser.Models.Sage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Ferpuser.BLL.Managers
{
    public class PreciosClienteManager
    {
        private readonly SageContext _sageContext;
        public PreciosClienteManager(SageContext sageContext)
        {
            _sageContext = sageContext;
        }

        public pvp GetPrecio(string codigoArticulo, string codigoCliente, DateTime? fecha = null)
        {
            if (fecha == null)
                fecha = DateTime.Now.Date;            

            var cliente = _sageContext.Clientes.AsNoTracking().FirstOrDefault(f => f.Codigo == codigoCliente);

            if (cliente == null)
                return null;
            
            var tarifa = cliente.TARIFA;
            var pvp = _sageContext.pvp.AsNoTracking()
                .OrderByDescending(f => f.FECHAINI)
                .FirstOrDefault(f => f.ARTICULO == codigoArticulo && f.TARIFA == tarifa && f.FECHAINI <= fecha);

            if (pvp == null)
                return null;

            return pvp;
        }
    }
}

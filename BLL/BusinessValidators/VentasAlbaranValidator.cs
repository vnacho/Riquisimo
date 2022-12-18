using Ferpuser.Data;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class VentasAlbaranValidator
    {
        public ApplicationDbContext _db { get; set; }

        public VentasAlbaranValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IList<ValidationResult> Create(VentaAlbaran model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            list.AddRange(ValidarClientes(model));
            return list;
        }

        public IList<ValidationResult> Edit(VentaAlbaran model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            list.AddRange(ValidarClientes(model));
            return list;
        }

        /// <summary>
        /// Valida que un albarán no tenga líneas de distinto cliente al seleccionado en la cabecera
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<ValidationResult> ValidarClientes(VentaAlbaran model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (model.AlbaranLineas.Any(f => f.VentaPedidoLineaId.HasValue))
            {
                var lineasPedido = model.AlbaranLineas.Where(f => f.VentaPedidoLineaId.HasValue).Select(f => f.VentaPedidoLineaId);
                var clientes = _db.VentaPedidoLineas.Where(f => lineasPedido.Contains(f.IdPedidoLinea)).Select(f => f.Pedido.CodigoCliente).Distinct();
                if (clientes.Any(f => f != model.CodigoCliente))
                    list.Add(new ValidationResult("Existen líneas de albarán que pertenecen a pedidos distintos al cliente seleccionado. Por favor revise la información."));                
            }
            return list;
        }

        
    }
}

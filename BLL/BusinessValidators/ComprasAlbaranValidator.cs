using Ferpuser.Data;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class ComprasAlbaranValidator
    {
        public ApplicationDbContext _db { get; set; }

        public ComprasAlbaranValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IList<ValidationResult> Create(CompraAlbaran model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.CompraAlbaranes.Any(f => f.CodigoAlbaran == model.CodigoAlbaran && f.CodigoProveedor == model.CodigoProveedor))
                list.Add(new ValidationResult("El número de albarán ya está siendo utilizado por otro albarán para este mismo proveedor.", new string[] { "CodigoAlbaran" }));

            list.AddRange(ValidarProveedores(model));

            return list;
        }

        public IList<ValidationResult> Edit(CompraAlbaran model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.CompraAlbaranes.Any(f => f.Id != model.Id && f.CodigoAlbaran == model.CodigoAlbaran && f.CodigoProveedor == model.CodigoProveedor))
                list.Add(new ValidationResult("El número de albarán ya está siendo utilizado por otro albarán para este mismo proveedor.", new string[] { "CodigoAlbaran" }));

            list.AddRange(ValidarProveedores(model));

            return list;
        }

        /// <summary>
        /// Valida que un albarán no tenga líneas de distinto proveedor al seleccionado en la cabecera
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<ValidationResult> ValidarProveedores(CompraAlbaran model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (model.AlbaranLineas.Any(f => f.CompraPedidoLineaId.HasValue))
            {
                var lineasPedido = model.AlbaranLineas.Where(f => f.CompraPedidoLineaId.HasValue).Select(f => f.CompraPedidoLineaId);
                var proveedores = _db.CompraPedidoLineas.Where(f => lineasPedido.Contains(f.IdPedidoLinea)).Select(f => f.Pedido.CodigoProveedor).Distinct();
                if (proveedores.Any(f => f != model.CodigoProveedor))
                    list.Add(new ValidationResult("Existen líneas de albarán que pertenecen a pedidos distintos al proveedor seleccionado. Por favor revise la información."));
                
            }
            return list;
        }

        
    }
}

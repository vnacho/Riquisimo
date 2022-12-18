using Ferpuser.Data;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class ComprasPedidoValidator
    {
        public ApplicationDbContext _db { get; set; }

        public ComprasPedidoValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(CompraPedido model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.CompraPedidos.Any(f => f.CodigoPedido == model.CodigoPedido && f.CodigoProveedor == model.CodigoProveedor))
                list.Add(new ValidationResult("El número de pedido ya está siendo utilizado por otro pedido para este mismo proveedor.", new string[] { "CodigoPedido" }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(CompraPedido model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.CompraPedidos.Any(f => f.Id != model.Id && f.CodigoPedido == model.CodigoPedido && f.CodigoProveedor == model.CodigoProveedor))
                list.Add(new ValidationResult("El número de pedido ya está siendo utilizado por otro pedido para este mismo proveedor.", new string[] { "CodigoPedido" }));
            return list;
        }
    }
}

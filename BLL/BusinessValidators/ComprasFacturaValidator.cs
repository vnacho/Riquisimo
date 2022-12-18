using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class ComprasFacturaValidator
    {
        public ApplicationDbContext _db { get; set; }

        public ComprasFacturaValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IList<ValidationResult> Create(CompraFactura model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.CompraFacturas.Any(f => f.NumeroFactura == model.NumeroFactura && f.CodigoProveedor == model.CodigoProveedor))
                list.Add(new ValidationResult("El número de factura ya está siendo utilizado por otra factura para este mismo proveedor.", new string[] { "NumeroFactura" }));
            if (!string.IsNullOrWhiteSpace(model.Registro) && _db.CompraFacturas.Any(f => f.Registro == model.Registro))
                list.Add(new ValidationResult("El registro ya está siendo utilizado por otra factura.", new string[] { "Registro" }));

            list.AddRange(ValidarLineasMismoProveedor(model));
            list.AddRange(ValidarParametroFechaLimite(model));

            return list;
        }

        public IList<ValidationResult> Edit(CompraFactura model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.CompraFacturas.Any(f => f.Id != model.Id && f.NumeroFactura == model.NumeroFactura && f.CodigoProveedor == model.CodigoProveedor))
                list.Add(new ValidationResult("El número de factura ya está siendo utilizado por otra factura para este mismo proveedor.", new string[] { "NumeroFactura" }));
            if (!string.IsNullOrWhiteSpace(model.Registro) && _db.CompraFacturas.Any(f => f.Id != model.Id && f.Registro == model.Registro))
                list.Add(new ValidationResult("El registro ya está siendo utilizado por otra factura.", new string[] { "Registro" }));

            list.AddRange(ValidarLineasMismoProveedor(model));
            list.AddRange(ValidarParametroFechaLimite(model));

            return list;
        }

        public IList<ValidationResult> Traspasar(CompraFactura model, SageContext sageContext)
        {
            List<ValidationResult> list = new List<ValidationResult>();            
            if (sageContext.c_albcom.Any(f => f.FACTURA.Trim() == model.NumeroFactura.Trim() && f.PROVEEDOR.Trim() == model.CodigoProveedor.Trim()))
                list.Add(new ValidationResult("El número de factura ya está siendo utilizado en SAGE por otra factura para este mismo proveedor."));
            return list;
        }

        private IList<ValidationResult> ValidarLineasMismoProveedor(CompraFactura model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            var idAlbaranLineas = model.FacturaLineas.Where(f => f.CompraAlbaranLineaId.HasValue).Select(f => f.CompraAlbaranLineaId);
            if (idAlbaranLineas != null && idAlbaranLineas.Any())
            {
                var albaranes = _db.CompraAlbaranLineas.Where(f => idAlbaranLineas.Contains(f.IdAlbaranLinea)).Select(f => f.Albaran);
                var proveedores = albaranes.Select(f => f.CodigoProveedor).Distinct();
                if (proveedores.Any(f => f != model.CodigoProveedor))
                    list.Add(new ValidationResult("Existen líneas de factura que pertenecen a albaranes distintos al proveedor seleccionado. Por favor revise la información."));
            }

            var idPedidoLineas = model.FacturaLineas.Where(f => f.CompraPedidoLineaId.HasValue).Select(f => f.CompraPedidoLineaId);
            if (idPedidoLineas != null && idPedidoLineas.Any())
            {
                var pedidos = _db.CompraPedidoLineas.Where(f => idPedidoLineas.Contains(f.IdPedidoLinea)).Select(f => f.Pedido);
                var proveedores = pedidos.Select(f => f.CodigoProveedor).Distinct();
                if (proveedores.Any(f => f != model.CodigoProveedor))
                    list.Add(new ValidationResult("Existen líneas de factura que pertenecen a pedidos distintos al proveedor seleccionado. Por favor revise la información."));

            }
            return list;
        }

        private IList<ValidationResult> ValidarParametroFechaLimite(CompraFactura model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            ParametroManager manager = new ParametroManager(_db);
            DateTime? dt = manager.GetFechaLimite();
            if (dt.HasValue)
            {
                if (model.Fecha < dt.Value)
                {
                    list.Add(new ValidationResult(
                        $"La fecha de factura no puede ser inferior a '{dt.Value.ToShortDateString()}' que es la indicada en el parámetro '{Consts.PARAMETRO_CODIGO_FECHA_LIMITE_INFERIOR_FACTURAS_COMPRA_VENTA}'.",
                        new string[] { nameof(model.Fecha) })
                    );
                }
            }
            return list;
        }
    }
}

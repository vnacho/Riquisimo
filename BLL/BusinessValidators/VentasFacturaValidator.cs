using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class VentasFacturaValidator
    {
        public ApplicationDbContext db { get; set; }

        public VentasFacturaValidator(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public IList<ValidationResult> Create(VentaFactura model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            list.AddRange(ValidarLineasMismoCliente(model));
            list.AddRange(ValidarParametroFechaLimite(model));
            return list;
        }

        public IList<ValidationResult> Edit(VentaFactura model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            list.AddRange(ValidarLineasMismoCliente(model));
            list.AddRange(ValidarParametroFechaLimite(model));
            return list;
        }

        public IList<ValidationResult> Traspasar(VentaFactura model, SageContext sageContext)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (model.EstadoFactura == EstadoFactura.TaspasadoSAGE)
            {
                list.Add(new ValidationResult("Esta factura ya ha sido traspasada a SAGE."));
                return list;
            }
            return list;
        }

        private IList<ValidationResult> ValidarLineasMismoCliente(VentaFactura model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            var idAlbaranLineas = model.FacturaLineas.Where(f => f.VentaAlbaranLineaId.HasValue).Select(f => f.VentaAlbaranLineaId);
            if (idAlbaranLineas != null && idAlbaranLineas.Any())
            {
                var albaranes = db.VentaAlbaranLineas.Where(f => idAlbaranLineas.Contains(f.IdAlbaranLinea)).Select(f => f.Albaran);
                var clientes = albaranes.Select(f => f.CodigoCliente).Distinct();
                if (clientes.Any(f => f != model.CodigoCliente))
                    list.Add(new ValidationResult("Existen líneas de factura que pertenecen a albaranes distintos al cliente seleccionado. Por favor revise la información."));
            }

            var idPedidoLineas = model.FacturaLineas.Where(f => f.VentaPedidoLineaId.HasValue).Select(f => f.VentaPedidoLineaId);
            if (idPedidoLineas != null && idPedidoLineas.Any())
            {
                var pedidos = db.VentaPedidoLineas.Where(f => idPedidoLineas.Contains(f.IdPedidoLinea)).Select(f => f.Pedido);
                var clientes = pedidos.Select(f => f.CodigoCliente).Distinct();
                if (clientes.Any(f => f != model.CodigoCliente))
                    list.Add(new ValidationResult("Existen líneas de factura que pertenecen a pedidos distintos al cliente seleccionado. Por favor revise la información."));

            }
            return list;
        }

        private IList<ValidationResult> ValidarParametroFechaLimite(VentaFactura model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            ParametroManager manager = new ParametroManager(db);
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

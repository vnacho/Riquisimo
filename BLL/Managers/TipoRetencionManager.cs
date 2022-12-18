using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Enums;
using Ferpuser.Models.Sage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class TipoRetencionManager
    {
        private SageContext _sageContext;

        public TipoRetencionManager(SageContext sageContext)
        {
            _sageContext = sageContext;
        }

        public tipo_ret GetByProveedor(string codigoProveedor)
        {
            tipo_ret item = null;           
            
            var proveedor = _sageContext.Proveedores.FirstOrDefault(f => f.CODIGO == codigoProveedor);
            if (proveedor != null)
            {
                var codigo_ret = proveedor.TIPO_RET;
                item = _sageContext.tipo_ret.SingleOrDefault(f => f.CODIGO == codigo_ret);
            }            

            return item;
        }

        public tipo_ret GetByCliente(string codigoCliente)
        {
            tipo_ret item = null;

            var cliente = _sageContext.Clientes.FirstOrDefault(f => f.Codigo == codigoCliente);
            if (cliente != null)
            {
                var codigo_ret = cliente.TIPO_RET;
                item = _sageContext.tipo_ret.SingleOrDefault(f => f.CODIGO == codigo_ret);
            }

            return item;
        }

        public void RellenarDatosRetencion(VentaFactura factura)
        {
            if (!factura.TieneRetencion || string.IsNullOrWhiteSpace(factura.CodigoCliente))
            {
                VaciarDatosRetencion(factura);
                return;
            }

            Clientes cliente = _sageContext.Clientes.FirstOrDefault(f => f.Codigo == factura.CodigoCliente);
            if (cliente == null)
            {
                VaciarDatosRetencion(factura);
                return;
            }                

            factura.EsRetencionFiscal = cliente.RETENCION;            
            if (factura.EsRetencionFiscal.Value) //Retención fiscal
            {
                factura.ModoRetencion = cliente.MODO_RET ? ModoRetencion.SobreBase : ModoRetencion.SobreTotal;
                factura.Retencion_Porcentaje = _sageContext.tipo_ret.SingleOrDefault(f => f.CODIGO == cliente.TIPO_RET)?.RETENCION;
                return;
            }
            
            factura.EsRetencionNoFiscal = cliente.RETNOFISC;
            if (factura.EsRetencionNoFiscal.Value) //Retención no fiscal
            {
                if (cliente.MODRETNOFI != 0 && cliente.MODRETNOFI != 1)
                    return;

                factura.ModoRetencion = cliente.MODRETNOFI == 0 ? ModoRetencion.SobreTotal : ModoRetencion.SobreBase;
                factura.Retencion_Porcentaje = cliente.TPCRETNOFI;
                return;
            }

            VaciarDatosRetencion(factura);
        }

        private void VaciarDatosRetencion(VentaFactura factura)
        {
            factura.EsRetencionFiscal = false;
            factura.EsRetencionNoFiscal = false;
            factura.ModoRetencion = null;
            factura.Retencion_Porcentaje = 0;
        }
    }
}

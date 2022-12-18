using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Enums;
using Ferpuser.Transfer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ferpuser.Controllers
{
    public class ImportadorSageController : Controller
    {
        private readonly SageComuContext _sageComuContext;
        private readonly ApplicationDbContext _db;
        private SageContextFactoryHelper _sageContextFactory;

        public ImportadorSageController(SageComuContext sageComuContext, ApplicationDbContext dbContext, SageContextFactoryHelper sageContextFactory)
        {
            _sageComuContext = sageComuContext;
            _db = dbContext;
            _sageContextFactory = sageContextFactory;
        }

        public IActionResult FacturasVenta()
        {
            return View("FacturasVenta", "Inicio");
        }

        //Traer todas las facturas de venta de sage con sus líneas.
        [HttpPost, ActionName("FacturasVenta")]
        public IActionResult FacturasVentaConfirmed()
        {   
            //Recorrer los años (distintos ejercicios)
            int anio = DateTime.Now.Year;
            int correctos = 0;
            int erroneos = 0;
            int saltados = 0;

            var facturas_existentes = _db.VentaFacturas.ToList();

            for (int i = anio; i > 2015; i--)
            {
                var _sageContext = _sageContextFactory.CreateSageContext(i);

                if (_sageContext == null)
                    continue;

                var cabeceras_sage = _sageContext.C_Albven;
                foreach (var cab_sage in cabeceras_sage)
                {
                    try
                    {
                        if (!cab_sage.Fecha.HasValue)
                        {
                            saltados++;
                            continue;
                        }

                        var existente = facturas_existentes.FirstOrDefault(f => f.CodigoDisplay == cab_sage.Numero);
                        if (existente != null) //Existe, ya está importada
                        {
                            saltados++;
                            continue;
                        }

                        List<string> split_factura = cab_sage.Numero.Split(' ').ToList();
                        split_factura.RemoveAll(f => string.IsNullOrWhiteSpace(f));
                        if (split_factura != null && split_factura.Count != 2)
                        {
                            saltados++;
                            continue;
                        }

                        string letra = split_factura.First();
                        int codigo_factura = Convert.ToInt32(split_factura.Last());

                        var cliente = _sageContext.Clientes.FirstOrDefault(f => f.Codigo == cab_sage.Cliente);
                        var evento = _sageContext.Almacen.FirstOrDefault(f => f.Codigo == cab_sage.Almacen);
                        var operario = _sageComuContext.Operarios.FirstOrDefault(f => f.CODIGO == cab_sage.Operario);

                        //Si no existe se puede dar de alta en nuestra base de datos.
                        var factura = new VentaFactura()
                        {
                            CodigoCliente = cab_sage.Cliente,
                            CodigoEvento = cab_sage.Almacen,
                            CodigoFactura = codigo_factura,
                            CodigoVendedor = cab_sage.Operario,
                            CodigoPostal = cab_sage.Codpost,
                            EstadoFactura = EstadoFactura.TaspasadoSAGE,
                            Fecha = cab_sage.Fecha.Value,
                            NombreCliente = cliente == null ? string.Empty : cliente.Nombre,
                            NombreEvento = evento == null ? string.Empty : evento.Nombre,
                            NombreVendedor = operario == null ? string.Empty : operario.NOMBRE,
                            Observaciones = cab_sage.Observacio,
                            Serie = letra,
                            ImportadaSage = true,
                            FacturaLineas = new List<VentaFacturaLinea>()
                        };

                        var lineas_sage = _sageContext.d_albven.Where(f => f.NUMERO == cab_sage.Numero);
                        foreach (var lin_sage in lineas_sage.OrderBy(f => f.LINIA))
                        {
                            if (lin_sage.UNIDADES <= 0) //Es una línea de comentario, no tiene unidades
                                continue;

                            var tipo_iva = _sageContext.Tipo_IVA.FirstOrDefault(f => f.Codigo == lin_sage.TIPO_IVA);
                            var articulo = _sageContext.Articulo.FirstOrDefault(f => f.Codigo == lin_sage.ARTICULO);

                            if (articulo == null)
                                continue;                            

                            evento = _sageContext.Almacen.FirstOrDefault(f => f.Codigo == lin_sage.ALMACEN);

                            var linea = new VentaFacturaLinea()
                            {
                                BaseImponiblePrecioUnitario = lin_sage.PRECIO,
                                CodigoArticulo = lin_sage.ARTICULO,
                                CodigoEvento = lin_sage.ALMACEN,
                                CodigoTipoIVA = lin_sage.TIPO_IVA,
                                Descuento = lin_sage.DTO1,
                                IVA_Porcentaje = tipo_iva == null ? 0 : (int)tipo_iva.IVA,
                                NombreArticulo = articulo.Nombre,
                                NombreEvento = evento == null ? string.Empty : evento.Nombre,
                                Orden = factura.FacturaLineas.Count(),
                                Unidades = lin_sage.UNIDADES
                            };

                            linea.Calcular(); //ImporteDescuento y BaseImponibleTotal
                            factura.FacturaLineas.Add(linea);
                        }

                        //if (factura.FacturaLineas.Count() != lineas_sage.Count())
                        //{
                        //    saltados++;
                        //    continue;
                        //}

                        if (!factura.FacturaLineas.Any())
                        {
                            saltados++;
                            continue;
                        }

                        //Calcular los campos BaseImponible, Total, TotalIVA
                        factura.Calcular();

                        _db.VentaFacturas.Add(factura);
                        _db.SaveChanges();
                        correctos++;
                    }
                    catch (Exception ex)
                    {
                        erroneos++;
                    }
                }
            }

            string message = $"Correctos: {correctos}. Erróneos: {erroneos}. Saltados: {saltados}";
            return View("FacturasVenta", message);
        }
    }
}

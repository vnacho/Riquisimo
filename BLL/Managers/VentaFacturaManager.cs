using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Exceptions;
using Ferpuser.BLL.Filters;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class VentaFacturaManager
    {        
        public ApplicationDbContext _db { get; set; }
        private readonly SageComuContext _sageComuContext;
        private readonly SageContext _sageContext;

        public VentaFacturaManager(ApplicationDbContext dbContext, SageComuContext sageComuContext, SageContext sageContext)
        {
            _db = dbContext;
            _sageComuContext = sageComuContext;
            _sageContext = sageContext;
        }

        /// <summary>
        /// Creación de un albarán de venta mediante una transacción
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Create(VentaFactura model)
        {
            if (!model.Origen)
            {
                model.OrigenCodigoArticulo = string.Empty;
                model.OrigenNombreArticulo = string.Empty;
                model.OrigenImporte = null;
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                //Hallamos el número de factura
                var serie = _sageContext.Serie.SingleOrDefault(f =>
                    f.Tipodoc == Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_FACTURA &&
                    f.Empresa == Consts.CODIGO_EMPRESA &&
                    f.Serie == model.Serie
                );
                if (serie == null)
                    throw new Exception($"{nameof(VentaFacturaManager)} : No se encuentra la serie {model.Serie} para el Tipodoc {Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_FACTURA}");

                model.CodigoFactura = (int)serie.Contador + 1;

                _db.VentaFacturas.Add(model);
                await _db.SaveChangesAsync();

                if (model.FacturaLineas.Any(f => f.VentaAlbaranLineaId.HasValue || f.VentaPedidoLineaId.HasValue))
                {
                    //2 - Ajustamos estados de albaranes relacionados
                    var idAlbaranLineas = model.FacturaLineas.Select(f => f.VentaAlbaranLineaId).Distinct();
                    var listAlbaranes = _db.VentaAlbaranes.Where(f => f.AlbaranLineas.Any(g => idAlbaranLineas.Contains(g.IdAlbaranLinea)));
                    foreach (var albaran in listAlbaranes)
                    {
                        albaran.EstadoAlbaran = EstadoAlbaran.Facturado;
                        albaran.VentaFacturaId = model.Id;
                    }

                    await _db.SaveChangesAsync();

                    //3 - Ajustamos cantidades de pedidos relacionados
                    List<VentaPedidoLinea> listLineasPedidoAfectadas = new List<VentaPedidoLinea>();
                    foreach (var item in model.FacturaLineas.Where(f => f.VentaPedidoLineaId.HasValue))
                    {
                        VentaPedidoLinea pedidoLinea = _db.VentaPedidoLineas.Find(item.VentaPedidoLineaId.Value);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes - item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.Id.ToString(), pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }
                    await _db.SaveChangesAsync();

                    //3 - Ajustamos los estados de los pedidos relacionados
                    var idPedidos = listLineasPedidoAfectadas.Select(f => f.PedidoId).Distinct();
                    foreach (var idPedido in idPedidos)
                    {
                        var Pedido = _db.VentaPedidos.Include(f => f.PedidoLineas).Single(f => f.Id == idPedido);

                        if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes <= 0))
                            Pedido.EstadoPedido = EstadoPedido.Servido;
                        else if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes >= f.Unidades))
                            Pedido.EstadoPedido = EstadoPedido.Pendiente;
                        else
                            Pedido.EstadoPedido = EstadoPedido.PendienteParcial;
                    }
                    await _db.SaveChangesAsync();
                }

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                transaction.Commit();

                //Actualizar el contador en series
                serie.Contador = model.CodigoFactura;
                _sageContext.Entry(serie).State = EntityState.Modified;
                _sageContext.SaveChanges();
            }
        }

        /// <summary>
        /// Edición de un albarán de venta mediante una transacción
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Edit(VentaFactura model)
        {
            if (!model.Origen)
            {
                model.OrigenCodigoArticulo = string.Empty;
                model.OrigenNombreArticulo = string.Empty;
                model.OrigenImporte = null;
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                List<VentaPedidoLinea> listLineasPedidoAfectadas = new List<VentaPedidoLinea>();
                List<VentaAlbaranLinea> listLineasAlbaranAfectadas = new List<VentaAlbaranLinea>();               

                //1 - Actualización del modelo
                _db.Entry(model).State = EntityState.Modified;

                //2 - Creación de líneas
                foreach (var item in model.FacturaLineas.Where(f => f.IdFacturaLinea <= 0))
                {
                    if (item.VentaPedidoLineaId.HasValue)
                    {
                        VentaPedidoLinea pedidoLinea = _db.VentaPedidoLineas.Find(item.VentaPedidoLineaId);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes - item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0 || pedidoLinea.UnidadesPendientes > pedidoLinea.Unidades) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.Id.ToString(), pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }

                    if (item.VentaAlbaranLineaId.HasValue)
                    {
                        VentaAlbaranLinea albaranLinea = _db.VentaAlbaranLineas.Single(f => f.IdAlbaranLinea == item.VentaAlbaranLineaId);
                        listLineasAlbaranAfectadas.Add(albaranLinea);
                    }

                    _db.Entry(item).State = EntityState.Added;
                }

                //3 - Modificación de líneas
                var lineasModificadas = model.FacturaLineas.Where(f => f.IdFacturaLinea > 0);
                foreach (var item in lineasModificadas)
                {
                    if (item.VentaPedidoLineaId.HasValue)
                    {
                        VentaFacturaLinea facturaLineaOriginal = _db.VentaFacturaLineas.AsNoTracking().Single(f => f.IdFacturaLinea == item.IdFacturaLinea);
                        VentaPedidoLinea pedidoLinea = _db.VentaPedidoLineas.Single(f => f.IdPedidoLinea == item.VentaPedidoLineaId);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes + facturaLineaOriginal.Unidades - item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0 || pedidoLinea.UnidadesPendientes > pedidoLinea.Unidades) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.Id.ToString(), pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }
                    _db.Entry(item).State = EntityState.Modified;
                }

                //4 - Borrado de líneas
                var lineasBorradas = _db.VentaFacturaLineas.Where(f => f.VentaFacturaId == model.Id && !lineasModificadas.Select(f => f.IdFacturaLinea).Contains(f.IdFacturaLinea));
                foreach (var item in lineasBorradas)
                {
                    if (item.VentaPedidoLineaId.HasValue)
                    {
                        VentaPedidoLinea pedidoLinea = _db.VentaPedidoLineas.Find(item.VentaPedidoLineaId);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes + item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0 || pedidoLinea.UnidadesPendientes > pedidoLinea.Unidades) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.Id.ToString(), pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }

                    if (item.VentaAlbaranLineaId.HasValue)
                    {
                        VentaAlbaranLinea albaranLinea = _db.VentaAlbaranLineas.Single(f => f.IdAlbaranLinea == item.VentaAlbaranLineaId);
                        listLineasAlbaranAfectadas.Add(albaranLinea);
                    }
                    _db.Entry(item).State = EntityState.Deleted;
                }

                await _db.SaveChangesAsync();

                //5 - Ajustamos los estados de los pedidos relacionados
                var idPedidos = listLineasPedidoAfectadas.Select(f => f.PedidoId).Distinct();
                foreach (var idPedido in idPedidos)
                {
                    var Pedido = _db.VentaPedidos.Include(f => f.PedidoLineas).Single(f => f.Id == idPedido);

                    if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes <= 0))
                        Pedido.EstadoPedido = EstadoPedido.Servido;
                    else if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes >= f.Unidades))
                        Pedido.EstadoPedido = EstadoPedido.Pendiente;
                    else
                        Pedido.EstadoPedido = EstadoPedido.PendienteParcial;
                }

                //6 - Ajustamos los estados de los albaranes relacionados
                var idAlbaranes = listLineasAlbaranAfectadas.Select(f => f.AlbaranId).Distinct();
                foreach (var idAlbaran in idAlbaranes)
                {
                    var albaran = _db.VentaAlbaranes.Include(f => f.AlbaranLineas).Single(f => f.Id == idAlbaran);
                    IEnumerable<int> idLineasAlbaran = albaran.AlbaranLineas.Select(f => f.IdAlbaranLinea);
                    if (_db.VentaFacturaLineas.Where(f => f.VentaAlbaranLineaId.HasValue).Any(f => idLineasAlbaran.Contains(f.VentaAlbaranLineaId.Value)))
                        albaran.EstadoAlbaran = EstadoAlbaran.Facturado;
                    else
                        albaran.EstadoAlbaran = EstadoAlbaran.NoFacturado;
                }

                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }        

        public async Task Delete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var model = _db.VentaFacturas.Include(f => f.FacturaLineas).Single(f => f.Id == id);

                //1 - Comprobar estados de albarán
                if (model.FacturaLineas.Any(f => f.VentaAlbaranLineaId.HasValue))
                {
                    foreach (var linea in model.FacturaLineas.Where(f => f.VentaAlbaranLineaId.HasValue))
                    {
                        var albaran = _db.VentaAlbaranLineas.Single(f => f.IdAlbaranLinea == linea.VentaAlbaranLineaId)?.Albaran;
                        if (albaran != null && albaran.EstadoAlbaran == EstadoAlbaran.Facturado)
                            albaran.EstadoAlbaran = EstadoAlbaran.NoFacturado;
                    }
                }

                //2 - Comprobar cantidades y estados de pedido
                if (model.FacturaLineas.Any(f => f.VentaPedidoLineaId.HasValue))
                {
                    //2A - Ajustamos cantidades de pedidos relacionados
                    List<VentaPedidoLinea> listLineasPedidoAfectadas = new List<VentaPedidoLinea>();
                    foreach (var item in model.FacturaLineas.Where(f => f.VentaPedidoLineaId.HasValue))
                    {
                        VentaPedidoLinea pedidoLinea = _db.VentaPedidoLineas.Find(item.VentaPedidoLineaId.Value);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes + item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.Id.ToString(), pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }
                    await _db.SaveChangesAsync();

                    //2B - Ajustamos los estados de los pedidos relacionados
                    var idPedidos = listLineasPedidoAfectadas.Select(f => f.PedidoId).Distinct();
                    foreach (var idPedido in idPedidos)
                    {
                        var Pedido = _db.VentaPedidos.Include(f => f.PedidoLineas).Single(f => f.Id == idPedido);

                        if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes <= 0))
                            Pedido.EstadoPedido = EstadoPedido.Servido;
                        else if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes >= f.Unidades))
                            Pedido.EstadoPedido = EstadoPedido.Pendiente;
                        else
                            Pedido.EstadoPedido = EstadoPedido.PendienteParcial;
                    }
                }

                //3 - Comprobar si existe algún albarán relacionado
                var albaranes = _db.VentaAlbaranes.Where(f => f.VentaFacturaId == model.Id);
                if (albaranes != null)
                {
                    albaranes.ToList().ForEach(f => f.VentaFacturaId = null);
                    albaranes.ToList().ForEach(f => f.EstadoAlbaran = EstadoAlbaran.NoFacturado);
                }

                //4 - Borramos factura
                _db.VentaFacturas.Remove(model);
                await _db.SaveChangesAsync();

                transaction.Commit();
            }
        }

        /// <summary>
        /// Traspasar a SAGE una factura
        /// </summary>
        /// <param name="idFactura"></param>
        /// <returns></returns>
        public async Task<IList<ValidationResult>> Traspasar(int idFactura, AppSettings appSettings, IWebHostEnvironment HostingEnvironment)
        {
            string query;
            using (var transaction = _db.Database.BeginTransaction())
            {
                VentaFactura model = _db.VentaFacturas
                    .Include(f => f.FacturaLineas)
                    .ThenInclude(f => f.VentaPedidoLinea)
                    .ThenInclude(f => f.Pedido)
                    .Single(f => f.Id == idFactura);
                
                var list = new VentasFacturaValidator(_db).Traspasar(model, _sageContext); //Validaciones de negocio
                if (list.Any())
                    return list;

                model.FacturaLineas.ToList().ForEach(f => f.Calcular());
                model.Calcular();

                model.EstadoFactura = EstadoFactura.TaspasadoSAGE;
                await _db.SaveChangesAsync();

                using (var transaction_sage = await _sageContext.Database.BeginTransactionAsync())
                {
                    decimal importe = model.FacturaLineas.Sum(f => f.BaseImponibleTotal * (1 + ((decimal)(f.IVA_Porcentaje ?? 0) / 100)));
                    decimal retencion = model.Retencion_Porcentaje ?? 0;

                    //Hallamos el número de albarán para el campo NUMERO. Según requisitos indicados por el cliente
                    var serie_albaran = _sageContext.Serie.SingleOrDefault(f =>
                        f.Tipodoc == Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_ALBARAN &&
                        f.Empresa == Consts.CODIGO_EMPRESA &&
                        f.Serie == model.Serie
                    );
                    if (serie_albaran == null)
                        throw new Exception($"{nameof(VentaFacturaManager)} : No se encuentra la serie {model.Serie} para el Tipodoc {Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_ALBARAN}");

                    int numero_albaran = (int)serie_albaran.Contador + 1;
                    
                    //string sNumero = numero_albaran.ToString().PadLeft(10, '0'); //Como es de 10 caracteres se cogen los 10 últimos.

                    //Hay que coger los datos de la dirección del cliente de la tabla “env_cli” el registro con los criterios CLIENTE=’XXXXXXX’ AND LINEA=1
                    var envcli = _sageContext.Env_Cli.FirstOrDefault(f => f.Linea == 1 && f.Cliente == model.CodigoCliente);
                    int nEnvCli = 0; //Línea
                    string sCodPostal = string.Empty;
                    if (envcli != null)
                    {
                        nEnvCli = 1;
                        sCodPostal = envcli.CodPos;
                    }

                    var cliente = _sageContext.Clientes.Find(model.CodigoCliente);
                    //string sVendedor = cliente.VENDEDOR; //Código del vendedor de la tabla “clientes”
                    string sVendedor = model.CodigoVendedor; 
                    string sFPag = cliente.FPAG; //Forma de pago de la ficha del cliente                    
                    int nRecEquiv = cliente.RECARGO ? 1 : 0; //Si el cliente tiene Recargo de equivalencia, lo tiene el documento.
                    string cabecera_pedido = string.Empty;
                    bool multi_pedido = false;
                    string nombre_articulo;
                    //Añadir las líneas de factura en sage
                    foreach (VentaFacturaLinea linea in model.FacturaLineas)
                    {
                        string definicion = linea.NombreArticulo;
                        if (definicion.Length > 100)
                            definicion = definicion.Substring(0, 100);

                        int doc = 0;
                        string doc_num = string.Empty;
                        if (linea.VentaPedidoLinea != null && linea.VentaPedidoLinea.Pedido != null)
                        {
                            doc = 1; //si viene de un pedido 1, si no 0
                            doc_num = linea.VentaPedidoLinea.Pedido.CodigoDisplay?.PadLeft(10, ' ');  //el número de pedido del que procede con el formato ejem: “    220000SF”
                        }
                        
                        if (string.IsNullOrWhiteSpace(cabecera_pedido) && !string.IsNullOrWhiteSpace(doc_num)) //Si viene de varios pedidos, aquí nada en las líneas. 
                            cabecera_pedido = doc_num;

                        if (!string.IsNullOrWhiteSpace(doc_num) && !string.IsNullOrWhiteSpace(cabecera_pedido) && cabecera_pedido != doc_num)
                            multi_pedido = true;

                        nombre_articulo = linea.NombreArticulo.Length > 100 ? linea.NombreArticulo.Substring(0, 100) : linea.NombreArticulo;

                        query = $"INSERT INTO [dbo].[d_albven](" +
                            $"[USUARIO],[EMPRESA],[NUMERO],[ARTICULO]," +
                            $"[DEFINICION],[UNIDADES]," +
                            $"[DTO1]," +
                            $"[TIPO_IVA],[FECHA]," +
                            $"[LINIA],[CLIENTE]," +
                            $"[PRECIO]," +
                            $"[IMPORTE]," +
                            $"[PRECIOIVA]," +
                            $"[IMPORTEIVA]," +
                            $"[PRECIODIV]," +
                            $"[IMPORTEDIV],[TIPO]," +
                            $"[DOC],[DOC_NUM]," +
                            $"[LETRA]," +
                            $"[IMPDIVIVA]," +
                            $"[PREDIVIVA]," +
                            $"[VENDEDOR],[FACTURABLE]," +
                            $"[ALMACEN])" +
                            $" VALUES(" +
                            $"'{Consts.USUARIO_WEB_NET}','{Consts.CODIGO_EMPRESA}','{model.CodigoDisplay}', '{linea.CodigoArticulo}'," +
                            $"'{nombre_articulo}',{linea.Unidades.ToString(CultureInfo.InvariantCulture)}," +
                            $"{linea.Descuento.ToString(CultureInfo.InvariantCulture)}," +
                            $"'{linea.CodigoTipoIVA}',CAST('{model.Fecha}' AS smalldatetime)," +
                            $"{linea.Orden},'{model.CodigoCliente}'," +
                            $"{linea.BaseImponiblePrecioUnitario.ToString(CultureInfo.InvariantCulture)}," +
                            $"{linea.BaseImponibleTotal.ToString(CultureInfo.InvariantCulture)}," +
                            $"{linea.Total.ToString(CultureInfo.InvariantCulture)}," +
                            $"{linea.Total.ToString(CultureInfo.InvariantCulture)}," +
                            $"{linea.BaseImponibleTotal.ToString(CultureInfo.InvariantCulture)}," +
                            $"{linea.BaseImponibleTotal.ToString(CultureInfo.InvariantCulture)},0," +
                            $"{doc},'{doc_num}'," +
                            $"'{model.Serie}'," +
                            $"{linea.Total.ToString(CultureInfo.InvariantCulture)}," +
                            $"{linea.Total.ToString(CultureInfo.InvariantCulture)}," +
                            $"'{sVendedor}',1," +
                            $"'{linea.CodigoEvento}')";                            
                            
                        _sageContext.Database.ExecuteSqlRaw(query);
                    }

                    if (model.Origen)
                    {
                        nombre_articulo = model.OrigenNombreArticulo.Length > 100 ? model.OrigenNombreArticulo.Substring(0, 100) : model.OrigenNombreArticulo;
                        int linia = model.FacturaLineas.Max(f => f.Orden) + 1;
                        query = $"INSERT INTO [dbo].[d_albven](" +
                            $"[USUARIO],[EMPRESA],[NUMERO],[ARTICULO]," +
                            $"[DEFINICION],[UNIDADES]," +
                            $"[DTO1]," +
                            $"[TIPO_IVA],[FECHA]," +
                            $"[LINIA],[CLIENTE]," +
                            $"[PRECIO]," +
                            $"[IMPORTE]," +
                            $"[PRECIOIVA]," +
                            $"[IMPORTEIVA]," +
                            $"[PRECIODIV]," +
                            $"[IMPORTEDIV],[TIPO]," +
                            $"[DOC],[DOC_NUM]," +
                            $"[LETRA]," +
                            $"[IMPDIVIVA]," +
                            $"[PREDIVIVA]," +
                            $"[VENDEDOR],[FACTURABLE]," +
                            $"[ALMACEN])" +
                            $" VALUES(" +
                            $"'{Consts.USUARIO_WEB_NET}','{Consts.CODIGO_EMPRESA}','{model.CodigoDisplay}', '{model.OrigenCodigoArticulo}'," +
                            $"'{nombre_articulo}',{1.ToString(CultureInfo.InvariantCulture)}," +
                            $"{0.ToString(CultureInfo.InvariantCulture)}," +
                            $"'00',CAST('{model.Fecha}' AS smalldatetime)," +
                            $"{linia},'{model.CodigoCliente}'," +
                            $"-{model.OrigenImporte.Value.ToString(CultureInfo.InvariantCulture)}," +
                            $"-{model.OrigenImporte.Value.ToString(CultureInfo.InvariantCulture)}," +
                            $"-{model.OrigenImporte.Value.ToString(CultureInfo.InvariantCulture)}," +
                            $"-{model.OrigenImporte.Value.ToString(CultureInfo.InvariantCulture)}," +
                            $"-{model.OrigenImporte.Value.ToString(CultureInfo.InvariantCulture)}," +
                            $"-{model.OrigenImporte.Value.ToString(CultureInfo.InvariantCulture)},0," +
                            $"0,''," +
                            $"'{model.Serie}'," +
                            $"-{model.OrigenImporte.Value.ToString(CultureInfo.InvariantCulture)}," +
                            $"-{model.OrigenImporte.Value.ToString(CultureInfo.InvariantCulture)}," +
                            $"'{sVendedor}',1," +                            
                            $"'{model.CodigoEvento}')";

                        _sageContext.Database.ExecuteSqlRaw(query);
                    }

                    if (multi_pedido) //Si viene de varios pedidos, aquí nada en las líneas
                        cabecera_pedido = string.Empty;

                    int modo_ret = 0;                    
                    if (model.ModoRetencion.HasValue && model.ModoRetencion.Value == ModoRetencion.SobreTotal)
                        modo_ret = 1;

                    decimal porcen_ret_fiscal = 0;
                    decimal porcen_ret_no_fiscal = 0;
                    if (model.TieneRetencion && model.Retencion_Porcentaje.HasValue)
                    {                        
                        if (model.EsRetencionNoFiscal.HasValue && model.EsRetencionNoFiscal.Value)
                            porcen_ret_no_fiscal = model.Retencion_Porcentaje.Value;
                        else
                            porcen_ret_fiscal = model.Retencion_Porcentaje.Value;
                    }

                    //Añadir la cabecera de factura en sage
                    query = $"INSERT INTO [dbo].[c_albven](" +
                        $"[USUARIO],[EMPRESA],[NUMERO],[FECHA]," +
                        $"[CLIENTE],[ENV_CLI],[VENDEDOR],[IVA_INC]," +
                        $"[FACTURA],[FECHA_FAC],[FPAG]," +
                        $"[IMPORTE],[DIVISA],[CAMBIO]," +
                        $"[IMPDIVISA],[RECEQUIV],[LETRA],[FACTURABLE]," +
                        $"[PEDIDO]," +
                        $"[TOTALDOC],[CODPOST]," +
                        $"[TOTALDIV],[ALMACEN]," +
                        $"[MODO_RET],[PORCEN_RET],[TPCRETNOFI]," +
                        $"[OPERARIO],[OBSERVACIO]) " +
                        $"VALUES(" +
                        $"'{Consts.USUARIO_WEB_NET}','{Consts.CODIGO_EMPRESA}','{model.CodigoDisplay}',CAST('{model.Fecha}' AS smalldatetime)," +
                        $"'{model.CodigoCliente}',{nEnvCli},'{sVendedor}',0," +
                        $"'{model.CodigoDisplay}',CAST('{model.Fecha}' AS smalldatetime),'{sFPag}'," +
                        $"{model.BaseImponible.ToString(CultureInfo.InvariantCulture)},'000',1," +
                        $"{model.BaseImponible.ToString(CultureInfo.InvariantCulture)},{nRecEquiv},'{model.Serie}',1," +
                        $"'{cabecera_pedido}'," +
                        $"{importe.ToString(CultureInfo.InvariantCulture)},'{sCodPostal}'," +
                        $"{importe.ToString(CultureInfo.InvariantCulture)},'{model.CodigoEvento}'," +
                        $"{modo_ret},{porcen_ret_fiscal.ToString(CultureInfo.InvariantCulture)},{porcen_ret_no_fiscal.ToString(CultureInfo.InvariantCulture)}," +
                        $"'{model.CodigoVendedor}','{model.Observaciones}')";
                    _sageContext.Database.ExecuteSqlRaw(query);

                    //Actualizar el contador en series
                    serie_albaran.Contador = numero_albaran;
                    _sageContext.Entry(serie_albaran).State = EntityState.Modified;
                    _sageContext.SaveChanges();

                    transaction_sage.Commit();
                    transaction.Commit();
                }
            }

            return new List<ValidationResult>();
        }

        /// <summary>
        /// Método que comprueba si todas las facturas de venta 
        /// </summary>
        /// <returns></returns>
        public async Task ActualizarPagadas(string codigoOperario)
        {
            //TODO
            //Se pasa el código de operario para no tener que actualizar todas si no es necesario
            //Motivo: Intentar evitar posibles problemas de rendimiento
            //VentaFacturaFilter filter = new VentaFacturaFilter()
            //{
            //    CodigoOperario = codigoOperario
            //};
            //var facturas = _db.VentaFacturas.Where(filter.ExpressionFilter());

            //foreach (VentaFactura item in facturas)
            //{
            //    var previs = _sageComuContext.previs.FirstOrDefault(f =>
            //        f.PROVEEDOR.Trim() == item.CodigoProveedor.Trim() &&
            //        f.FACTURA.Trim() == item.NumeroFactura.Trim());

            //    if (previs == null)
            //        continue;

            //    item.Pagada = previs.PAGADA.Trim().ToUpper() == "S";
            //}

            //await _db.SaveChangesAsync();
        }

        ///// <summary>
        ///// El sistema generará tantas facturas como clientes distintos haya.
        ///// </summary>
        ///// <param name="IdInscripciones"></param>
        ///// <returns></returns>
        //public async Task<IList<ValidationResult>> CreateFacturaDesdeInscripciones(string[] IdInscripciones, string codigo_operario, string serie)
        //{
        //    IList<ValidationResult> errores = new List<ValidationResult>();
        //    if (IdInscripciones == null || !IdInscripciones.Any())
        //        errores.Add(new ValidationResult("No hay inscripciones que facturar."));
        //    if (string.IsNullOrWhiteSpace(codigo_operario))
        //        errores.Add(new ValidationResult("Es obligatorio introducir el código de operario."));
        //    if (string.IsNullOrWhiteSpace(serie))
        //        errores.Add(new ValidationResult("Es obligatorio introducir la serie."));

        //    if (errores.Any())
        //        return errores;

        //    using (var transaction = _db.Database.BeginTransaction())
        //    {
        //        var registrations = await _db.Registrations.Include(f => f.Congress).Where(f => IdInscripciones.Contains(f.Id.ToString())).ToListAsync();
        //        var id_clientes = registrations.Select(f => f.ClientId).Distinct();

        //        //Hallamos el número de factura
        //        var obj_serie = _sageContext.Serie.SingleOrDefault(f =>
        //            f.Tipodoc == Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_FACTURA &&
        //            f.Empresa == Consts.CODIGO_EMPRESA &&
        //            f.Serie == serie
        //        );
        //        if (serie == null)
        //            throw new Exception($"{nameof(VentaFacturaManager)} : No se encuentra la serie {serie} para el Tipodoc {Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_FACTURA}");

        //        int contador_factura = (int)obj_serie.Contador + 1;
        //        var articulo = _sageContext.Articulo.Single(f => f.Codigo == "70100"); //8/2/2022 => el código de artículo que hay que asignar es el 70100                 
        //        var operario = _sageComuContext.Operarios.Find(codigo_operario);

        //        foreach (var id_cliente in id_clientes)
        //        {
        //            Client cliente = _db.Clients.Find(id_cliente);
        //            if (cliente == null)
        //                continue;

        //            var registrations_lineas = registrations.Where(f => f.ClientId == id_cliente);

        //            List<VentaFacturaLinea> lineas = new List<VentaFacturaLinea>();
        //            int i = 1;
        //            foreach (var reg in registrations_lineas)
        //            {
        //                var tipo_iva = _sageContext.Tipo_IVA.SingleOrDefault(f => f.Codigo == reg.VATId);
        //                VentaFacturaLinea linea = new VentaFacturaLinea()
        //                {
        //                    BaseImponiblePrecioUnitario = reg.Fee,
        //                    CodigoArticulo = articulo.Codigo,
        //                    CodigoEvento = reg.Congress.Code,
        //                    CodigoTipoIVA = reg.VATId,
        //                    IVA_Porcentaje = (int)tipo_iva?.IVA,
        //                    NombreArticulo = articulo.Nombre,
        //                    NombreEvento = reg.Congress.DisplayName,
        //                    Orden = i,
        //                    Unidades = 1,
        //                    UnidadesPendientes = 0
        //                };
        //                linea.Calcular();
        //                lineas.Add(linea);
        //                i++;
        //            }

        //            VentaFactura factura = new VentaFactura();
        //            factura.FacturaLineas = lineas;
        //            factura.CodigoCliente = cliente.SageCode;
        //            factura.CodigoEvento = registrations.First().Congress.Code;
        //            factura.CodigoOperario = operario.CODIGO;
        //            factura.Serie = serie;
        //            factura.CodigoFactura = contador_factura;
        //            factura.Fecha = DateTime.Now.Date;
        //            factura.NombreCliente = cliente.BusinessName;
        //            factura.NombreEvento = registrations.First().Congress.DisplayName;
        //            factura.NombreOperario = operario.NOMBRE;

        //            factura.Calcular();

        //            _db.VentaFacturas.Add(factura);
        //            _db.SaveChanges();

        //            contador_factura++;
        //        }

        //        //Actualizar el contador en series
        //        obj_serie.Contador = contador_factura - 1;
        //        _sageContext.Entry(obj_serie).State = EntityState.Modified;
        //        _sageContext.SaveChanges();

        //        transaction.Commit();
        //    }

        //    return errores;

        //}

        /// <summary>
        /// El sistema generará tantas facturas como clientes distintos haya.
        /// </summary>
        /// <param name="IdInscripciones"></param>
        /// <returns></returns>
        public Tuple<int,IList<ValidationResult>> CreateFacturaDesdeInscripciones(string[] IdInscripciones, string codigo_operario, string serie)
        {
            int n_facturas = 0;
            IList<ValidationResult> errores = new List<ValidationResult>();
            
            if (IdInscripciones == null || !IdInscripciones.Any())
                errores.Add(new ValidationResult("No hay inscripciones que facturar."));
            if (string.IsNullOrWhiteSpace(codigo_operario))
                errores.Add(new ValidationResult("Es obligatorio introducir el código de operario."));
            if (string.IsNullOrWhiteSpace(serie))
                errores.Add(new ValidationResult("Es obligatorio introducir la serie."));
            if (errores.Any())
                return new Tuple<int, IList<ValidationResult>>(n_facturas, errores);

            using (var transaction = _db.Database.BeginTransaction())
            {
                var registrations = _db.Registrations.Include(f => f.Congress).Where(f => IdInscripciones.Contains(f.Id.ToString())).ToList();
                var id_clientes = registrations.Select(f => f.ClientId).Distinct();

                //Hallamos el número de factura
                var obj_serie = _sageContext.Serie.SingleOrDefault(f =>
                    f.Tipodoc == Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_FACTURA &&
                    f.Empresa == Consts.CODIGO_EMPRESA &&
                    f.Serie == serie
                );
                if (serie == null)
                    throw new Exception($"{nameof(VentaFacturaManager)} : No se encuentra la serie {serie} para el Tipodoc {Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_FACTURA}");

                int contador_factura = (int)obj_serie.Contador + 1;
                var articulo = _sageContext.Articulo.Single(f => f.Codigo == "70100"); //8/2/2022 => el código de artículo que hay que asignar es el 70100                 
                var operario = _sageComuContext.Operarios.Find(codigo_operario);

                foreach (var id_cliente in id_clientes)
                {
                    Client cliente = _db.Clients.Find(id_cliente);
                    if (cliente == null)
                        continue;

                    VentaFactura factura = new VentaFactura();
                    factura.Serie = serie;
                    factura.CodigoFactura = contador_factura;

                    var registrations_lineas = registrations.Where(f => f.ClientId == id_cliente);

                    List<VentaFacturaLinea> lineas = new List<VentaFacturaLinea>();
                    int i = 1;
                    foreach (var reg in registrations_lineas)
                    {
                        var tipo_iva = _sageContext.Tipo_IVA.SingleOrDefault(f => f.Codigo == reg.VATId);
                        VentaFacturaLinea linea = new VentaFacturaLinea()
                        {
                            BaseImponiblePrecioUnitario = reg.Fee,
                            CodigoArticulo = articulo.Codigo,
                            CodigoEvento = reg.Congress.Code,
                            CodigoTipoIVA = reg.VATId,
                            IVA_Porcentaje = (int)tipo_iva?.IVA,
                            NombreArticulo = articulo.Nombre,
                            NombreEvento = reg.Congress.DisplayName,
                            Orden = i,
                            Unidades = 1,
                            UnidadesPendientes = 0
                        };
                        linea.Calcular();
                        lineas.Add(linea);

                        reg.Exported = true; //Marcamos la inscripción como facturada
                        reg.InvoiceNumber = factura.CodigoDisplay;

                        i++;
                    }
                    
                    factura.FacturaLineas = lineas;
                    factura.CodigoCliente = cliente.SageCode;
                    factura.CodigoEvento = registrations.First().Congress.Code;
                    factura.CodigoVendedor = operario.CODIGO;                    
                    factura.Fecha = DateTime.Now.Date;
                    factura.NombreCliente = cliente.BusinessName;
                    factura.NombreEvento = registrations.First().Congress.DisplayName;
                    factura.NombreVendedor = operario.NOMBRE;

                    factura.Calcular();

                    _db.VentaFacturas.Add(factura);
                    _db.SaveChanges();
                    n_facturas++;

                    contador_factura++;
                }

                //Actualizar el contador en series
                obj_serie.Contador = contador_factura - 1;
                _sageContext.Entry(obj_serie).State = EntityState.Modified;
                _sageContext.SaveChanges();

                transaction.Commit();
            }

            return new Tuple<int, IList<ValidationResult>>(n_facturas, errores);
        }

    }
}

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
    public class CompraFacturaManager
    {        
        public ApplicationDbContext _db { get; set; }
        private readonly SageComuContext _sageComuContext;
        private readonly SageContext _sageContext;

        public CompraFacturaManager(ApplicationDbContext dbContext, SageComuContext sageComuContext, SageContext sageContext)
        {
            _db = dbContext;
            _sageComuContext = sageComuContext;
            _sageContext = sageContext;
        }

        /// <summary>
        /// Creación de un albarán de compra mediante una transacción
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Create(CompraFactura model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                //Calcular porcentaje de retención
                model.Retencion_Porcentaje = 0;
                TipoRetencionManager retencionManager = new TipoRetencionManager(_sageContext);
                if (model.TieneRetencion)
                {
                    var retencion = retencionManager.GetByProveedor(model.CodigoProveedor);
                    if (retencion != null)
                        model.Retencion_Porcentaje = retencion.RETENCION;
                }


                //1 - Añadimos la factura
                
                //2022/09/22 Se quita porque ahora se quiere meter el registro manualmente

                //string anio2dig = model.Fecha.ToString("yy");
                //string ultimoRegistro = _db.CompraFacturas
                //    .AsNoTracking()
                //    .Where(f => f.Registro.StartsWith(anio2dig))
                //    .OrderByDescending(f => f.Registro)
                //    .FirstOrDefault()?
                //    .Registro;

                //if (string.IsNullOrWhiteSpace(ultimoRegistro))
                //    model.Registro = $"{anio2dig}0001";
                //else
                //    model.Registro = (Convert.ToInt32(ultimoRegistro) + 1).ToString();

                _db.CompraFacturas.Add(model);
                await _db.SaveChangesAsync();

                if (!model.FacturaLineas.Any(f => f.CompraAlbaranLineaId.HasValue || f.CompraPedidoLineaId.HasValue))
                {
                    transaction.Commit();
                    return;
                }

                //2 - Ajustamos estados de albaranes relacionados
                var idAlbaranLineas = model.FacturaLineas.Select(f => f.CompraAlbaranLineaId).Distinct();
                var listAlbaranes = _db.CompraAlbaranes.Where(f => f.AlbaranLineas.Any(g => idAlbaranLineas.Contains(g.IdAlbaranLinea)));
                foreach (var albaran in listAlbaranes)
                {
                    albaran.EstadoAlbaran = EstadoAlbaran.Facturado;
                    albaran.CompraFacturaId = model.Id;
                }

                await _db.SaveChangesAsync();

                //3 - Ajustamos cantidades de pedidos relacionados
                List<CompraPedidoLinea> listLineasPedidoAfectadas = new List<CompraPedidoLinea>();
                foreach (var item in model.FacturaLineas.Where(f => f.CompraPedidoLineaId.HasValue))
                {
                    CompraPedidoLinea pedidoLinea = _db.CompraPedidoLineas.Find(item.CompraPedidoLineaId.Value);
                    pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes - item.Unidades;

                    if (pedidoLinea.UnidadesPendientes < 0) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                        throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.CodigoPedido, pedidoLinea.Orden);

                    listLineasPedidoAfectadas.Add(pedidoLinea);                    
                }
                await _db.SaveChangesAsync();

                //3 - Ajustamos los estados de los pedidos relacionados
                var idPedidos = listLineasPedidoAfectadas.Select(f => f.PedidoId).Distinct();
                foreach (var idPedido in idPedidos)
                {
                    var Pedido = _db.CompraPedidos.Include(f => f.PedidoLineas).Single(f => f.Id == idPedido);

                    if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes <= 0))
                        Pedido.EstadoPedido = EstadoPedido.Servido;
                    else if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes >= f.Unidades))
                        Pedido.EstadoPedido = EstadoPedido.Pendiente;
                    else
                        Pedido.EstadoPedido = EstadoPedido.PendienteParcial;
                }
                await _db.SaveChangesAsync();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                transaction.Commit();
            }
        }

        /// <summary>
        /// Edición de un albarán de compra mediante una transacción
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Edit(CompraFactura model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                List<CompraPedidoLinea> listLineasPedidoAfectadas = new List<CompraPedidoLinea>();
                List<CompraAlbaranLinea> listLineasAlbaranAfectadas = new List<CompraAlbaranLinea>();

                //Calcular porcentaje de retención
                model.Retencion_Porcentaje = 0;
                TipoRetencionManager retencionManager = new TipoRetencionManager(_sageContext);
                if (model.TieneRetencion)
                {
                    var retencion = retencionManager.GetByProveedor(model.CodigoProveedor);
                    if (retencion != null)
                        model.Retencion_Porcentaje = retencion.RETENCION;
                }

                //1 - Actualización del modelo
                _db.Entry(model).State = EntityState.Modified;

                //2 - Creación de líneas
                foreach (var item in model.FacturaLineas.Where(f => f.IdFacturaLinea <= 0))
                {                   
                    if (item.CompraPedidoLineaId.HasValue)
                    {
                        CompraPedidoLinea pedidoLinea = _db.CompraPedidoLineas.Find(item.CompraPedidoLineaId);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes - item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0 || pedidoLinea.UnidadesPendientes > pedidoLinea.Unidades) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.CodigoPedido, pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }

                    if (item.CompraAlbaranLineaId.HasValue)
                    {
                        CompraAlbaranLinea albaranLinea = _db.CompraAlbaranLineas.Single(f => f.IdAlbaranLinea == item.CompraAlbaranLineaId);
                        listLineasAlbaranAfectadas.Add(albaranLinea);
                    }

                    _db.Entry(item).State = EntityState.Added;
                }

                //3 - Modificación de líneas
                var lineasModificadas = model.FacturaLineas.Where(f => f.IdFacturaLinea > 0);
                foreach (var item in lineasModificadas)
                {
                    if (item.CompraPedidoLineaId.HasValue)
                    {
                        CompraFacturaLinea facturaLineaOriginal = _db.CompraFacturaLineas.AsNoTracking().Single(f => f.IdFacturaLinea == item.IdFacturaLinea);
                        CompraPedidoLinea pedidoLinea = _db.CompraPedidoLineas.Single(f => f.IdPedidoLinea == item.CompraPedidoLineaId);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes + facturaLineaOriginal.Unidades - item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0 || pedidoLinea.UnidadesPendientes > pedidoLinea.Unidades) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.CodigoPedido, pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }
                    _db.Entry(item).State = EntityState.Modified;
                }

                //4 - Borrado de líneas
                var lineasBorradas = _db.CompraFacturaLineas.Where(f => f.CompraFacturaId == model.Id && !lineasModificadas.Select(f => f.IdFacturaLinea).Contains(f.IdFacturaLinea));
                foreach (var item in lineasBorradas)
                {
                    if (item.CompraPedidoLineaId.HasValue)
                    {
                        CompraPedidoLinea pedidoLinea = _db.CompraPedidoLineas.Find(item.CompraPedidoLineaId);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes + item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0 || pedidoLinea.UnidadesPendientes > pedidoLinea.Unidades) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.CodigoPedido, pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }

                    if (item.CompraAlbaranLineaId.HasValue)
                    {
                        CompraAlbaranLinea albaranLinea = _db.CompraAlbaranLineas.Single(f => f.IdAlbaranLinea == item.CompraAlbaranLineaId);
                        listLineasAlbaranAfectadas.Add(albaranLinea);
                    }
                    _db.Entry(item).State = EntityState.Deleted;                    
                }

                await _db.SaveChangesAsync();

                //5 - Ajustamos los estados de los pedidos relacionados
                var idPedidos = listLineasPedidoAfectadas.Select(f => f.PedidoId).Distinct();
                foreach (var idPedido in idPedidos)
                {
                    var Pedido = _db.CompraPedidos.Include(f => f.PedidoLineas).Single(f => f.Id == idPedido);

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
                    var albaran = _db.CompraAlbaranes.Include(f => f.AlbaranLineas).Single(f => f.Id == idAlbaran);
                    IEnumerable<int> idLineasAlbaran = albaran.AlbaranLineas.Select(f => f.IdAlbaranLinea);
                    if (_db.CompraFacturaLineas.Where(f => f.CompraAlbaranLineaId.HasValue).Any(f => idLineasAlbaran.Contains(f.CompraAlbaranLineaId.Value)))
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
                var model = _db.CompraFacturas.Include(f => f.FacturaLineas).Single(f => f.Id == id);

                //1 - Comprobar estados de albarán
                if (model.FacturaLineas.Any(f => f.CompraAlbaranLineaId.HasValue))
                {
                    foreach(var linea in model.FacturaLineas.Where(f => f.CompraAlbaranLineaId.HasValue))
                    {
                        var albaran = _db.CompraAlbaranLineas.Single(f => f.IdAlbaranLinea == linea.CompraAlbaranLineaId)?.Albaran;
                        if (albaran != null && albaran.EstadoAlbaran == EstadoAlbaran.Facturado)
                            albaran.EstadoAlbaran = EstadoAlbaran.NoFacturado;
                    }
                }

                //2 - Comprobar cantidades y estados de pedido
                if (model.FacturaLineas.Any(f => f.CompraPedidoLineaId.HasValue))
                {
                    //2A - Ajustamos cantidades de pedidos relacionados
                    List<CompraPedidoLinea> listLineasPedidoAfectadas = new List<CompraPedidoLinea>();
                    foreach (var item in model.FacturaLineas.Where(f => f.CompraPedidoLineaId.HasValue))
                    {
                        CompraPedidoLinea pedidoLinea = _db.CompraPedidoLineas.Find(item.CompraPedidoLineaId.Value);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes + item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.CodigoPedido, pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }
                    await _db.SaveChangesAsync();

                    //2B - Ajustamos los estados de los pedidos relacionados
                    var idPedidos = listLineasPedidoAfectadas.Select(f => f.PedidoId).Distinct();
                    foreach (var idPedido in idPedidos)
                    {
                        var Pedido = _db.CompraPedidos.Include(f => f.PedidoLineas).Single(f => f.Id == idPedido);

                        if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes <= 0))
                            Pedido.EstadoPedido = EstadoPedido.Servido;
                        else if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes >= f.Unidades))
                            Pedido.EstadoPedido = EstadoPedido.Pendiente;
                        else
                            Pedido.EstadoPedido = EstadoPedido.PendienteParcial;
                    }
                }

                //3 - Comprobar si existe algún albarán relacionado
                var albaranes = _db.CompraAlbaranes.Where(f => f.CompraFacturaId == model.Id);
                if (albaranes != null)
                {
                    albaranes.ToList().ForEach(f => f.CompraFacturaId = null);
                    albaranes.ToList().ForEach(f => f.EstadoAlbaran = EstadoAlbaran.NoFacturado);
                }

                //4 - Borramos factura
                _db.CompraFacturas.Remove(model);
                await _db.SaveChangesAsync();                

                transaction.Commit();
            }
        }

        /// <summary>
        /// Traspasar a SAGE una factura
        /// </summary>
        /// <param name="idFactura"></param>
        /// <returns></returns>
        public async Task<IList<ValidationResult>> Traspasar(int idFactura, SageContext sageContext, AppSettings appSettings, IWebHostEnvironment HostingEnvironment)
        {
            string query;
            IList<ValidationResult> list = new List<ValidationResult>();
            CompraFactura model;
            using (var transaction = _db.Database.BeginTransaction())
            {
                model = _db.CompraFacturas.Include(f => f.FacturaLineas).Single(f => f.Id == idFactura);
                if (model.EstadoFactura == EstadoFactura.TaspasadoSAGE)
                {
                    list.Add(new ValidationResult("Esta factura ya ha sido traspasada a SAGE."));
                    return list; //Ya está traspasado, salir
                }

                //Validaciones de negocio
                list = new ComprasFacturaValidator(_db).Traspasar(model, sageContext);
                if (list.Any())
                    return list; //Errores de validación de negocio

                //Obtener el código sage que corresponda
                //int codigoFacturaSage = 1;
                //var empresa = sageContext.empresa.FirstOrDefault();
                //if (empresa != null)
                //    codigoFacturaSage = empresa.FACTUCOM;
                //model.CodigoSage = codigoFacturaSage;
                model.EstadoFactura = EstadoFactura.TaspasadoSAGE;
                await _db.SaveChangesAsync();

                //Ya está cambiado el estado, aunque no se haya hecho el commit.
                //Vamos a realizar las demás operativas en la otra base de datos y si falla no se llevará a cabo el commit.
                using (var transaction_sage = await sageContext.Database.BeginTransactionAsync())
                {
                    decimal importe = model.FacturaLineas.Sum(f => f.BaseImponibleTotal * (1 + ((decimal)(f.IVA_Porcentaje ?? 0) / 100)));
                    decimal retencion = model.Retencion_Porcentaje ?? 0;

                    string sFactura = model.NumeroFactura.Trim().PadLeft(24, ' ');
                    string sNumero = sFactura.Substring(14, 10); //Como es de 10 caracteres se cogen los 10 últimos.

                    //Añadir la cabecera de factura en sage
                    query = $"INSERT INTO [dbo].[c_albcom](" +
                        $"[USUARIO],[EMPRESA],[NUMERO],[FACTURA],[CAMBIO],"+
                        $"[PROVEEDOR],[IMPORTE],[IMPDIVISA]," +
                        $"[TOTALDOC],[TOTALDIV],[FECHA],[FECHA_FAC],[DIVISA],[ALMACEN]," +
                        $"[FPAG],[OPERARIO],[OBSERVACIO],[RET_DIV],[PORCEN_RET],[MODO_RET]) " +
					    $"VALUES(" +
                        $"'{Consts.USUARIO_WEB_NET}','{Consts.CODIGO_EMPRESA}','{sNumero}','{sFactura}',1," +
                        $"'{model.CodigoProveedor}',{model.BaseImponible.ToString(CultureInfo.InvariantCulture)},{model.BaseImponible.ToString(CultureInfo.InvariantCulture)}," +
                        $"{importe.ToString(CultureInfo.InvariantCulture)}, {importe.ToString(CultureInfo.InvariantCulture)}, CAST('{model.Fecha}' AS smalldatetime), CAST('{model.Fecha}' AS smalldatetime), '000', '{model.CodigoEvento}'," +
                        $"'00', '{model.CodigoOperario}','{model.Observaciones}','000',{retencion.ToString(CultureInfo.InvariantCulture)},1)";
                    sageContext.Database.ExecuteSqlRaw(query);

                    //Añadir las líneas de factura en sage
                    foreach (CompraFacturaLinea linea in model.FacturaLineas)
                    {
                        string definicion = linea.NombreArticulo;
                        if (definicion.Length > 100)
                            definicion = definicion.Substring(0, 100);

                        query = $"INSERT INTO [dbo].[d_albcom](" +
                            $"[USUARIO],[EMPRESA],[NUMERO],[ARTICULO],[TIPO_IVA]," +
                            $"[PROVEEDOR],[FECHA],[DEFINICION],[ALMACEN]," +
                            $"[UNIDADES],[LINIA],[PRECIO],[IMPORTE]," +
                            $"[PRECIODIV],[IMPORTEDIV]) " +
                            $"VALUES(" +
                            $"'{Consts.USUARIO_WEB_NET}','{Consts.CODIGO_EMPRESA}','{sNumero}', '{linea.CodigoArticulo}', '{linea.CodigoTipoIVA}'," +
                            $"'{model.CodigoProveedor}', CAST('{model.Fecha}' AS smalldatetime), '{definicion}','{linea.CodigoEvento}',"+
                            $"{linea.Unidades.ToString(CultureInfo.InvariantCulture)}, {linea.Orden}, {linea.BaseImponibleTotal.ToString(CultureInfo.InvariantCulture)}, {linea.BaseImponibleTotal.ToString(CultureInfo.InvariantCulture)},"+
                            $"{linea.BaseImponibleTotal.ToString(CultureInfo.InvariantCulture)}, {linea.BaseImponibleTotal.ToString(CultureInfo.InvariantCulture)})";
                        sageContext.Database.ExecuteSqlRaw(query);
                    }

                    //Añadir la cabecera de factura en sage (tabla c_factucom)
                    //La columna de IVA es la más complicada.
                    //Se trata de un formato JSON o parecido y se ha de hacer grupos por cada código de tipo de iva                    
                    IEnumerable<string> sCodigosIva = model.FacturaLineas.Select(f => f.CodigoTipoIVA).Distinct();
                    List<string> lstFormatoIva = new List<string>();
                    foreach(string sCodigoIVA in sCodigosIva)
                    {
                        var lineas = model.FacturaLineas.Where(f => f.CodigoTipoIVA == sCodigoIVA);

                        decimal dBI = lineas.Sum(f => f.BaseImponibleTotal);
                        string sBI = dBI.ToString("N2", CultureInfo.InvariantCulture);

                        int nPorcentajeIVA = lineas.First().IVA_Porcentaje ?? 0;
                        string sPorcentajeIVA = nPorcentajeIVA.ToString("N2", CultureInfo.InvariantCulture);

                        decimal dImporteIVA = lineas.Sum(f => f.ImporteIVA);
                        string sImporteIVA = dImporteIVA.ToString("N2", CultureInfo.InvariantCulture);

                        //Formato de ejemplo de formato de iva (¡uno solo! luego se hace un join con comas)
                        //{"_Codigo":"20","_Base":75.14,"_PrcIva":10.00,"_ImpIva":7.51,"_PrcRec":0.0,"_ImpRec":0.0,"_BaseDivisa":75.14,"_ImpIvaDivisa":7.51,"_ImpRecDivisa":0.0}
                        string sIva = "{\"_Codigo\":\"" + sCodigoIVA + " \", \"_Base\":" + sBI +
                            ",\"_PrcIva\":" + sPorcentajeIVA + ",\"_ImpIva\":" + sImporteIVA +
                            ",\"_PrcRec\":0.0,\"_ImpRec\":0.0,\"_BaseDivisa\":" + sBI + ",\"_ImpIvaDivisa\":" + sImporteIVA + ",\"_ImpRecDivisa\":0.0}";

                        lstFormatoIva.Add(sIva);
                    }

                    //Formato de ejemplo de varios tipos de iva
                    //[{"_Codigo":"20","_Base":75.14,"_PrcIva":10.00,"_ImpIva":7.51,"_PrcRec":0.0,"_ImpRec":0.0,"_BaseDivisa":75.14,"_ImpIvaDivisa":7.51,"_ImpRecDivisa":0.0},{"_Codigo":"03","_Base":8.26,"_PrcIva":21.00,"_ImpIva":1.73,"_PrcRec":0.0,"_ImpRec":0.0,"_BaseDivisa":8.26,"_ImpIvaDivisa":1.73,"_ImpRecDivisa":0.0}]
                    string sFormatoFinalIVA = $"[{string.Join(",",lstFormatoIva)}]";

                    query = "INSERT INTO [dbo].[c_factucom](" +
                        "[EMPRESA],[NUMERO],[PROVEEDOR],[IMPORTE],[IMPDIVISA]," +
                        "[TOTALDOC],[TOTALDIV],[IVA],[RETPORCEN]) " +
                        "VALUES(" +
                        "@EMPRESA,@NUMERO,@PROVEEDOR,@IMPORTE,@IMPDIVISA," +
                        "@TOTALDOC,@TOTALDIV,@IVA,@RETPORCEN)";
                    
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@EMPRESA", Consts.CODIGO_EMPRESA));
                    parameters.Add(new SqlParameter("@NUMERO", sFactura));
                    parameters.Add(new SqlParameter("@PROVEEDOR", model.CodigoProveedor));
                    parameters.Add(new SqlParameter("@IMPORTE", model.BaseImponible.ToString(CultureInfo.InvariantCulture)));
                    parameters.Add(new SqlParameter("@IMPDIVISA", model.BaseImponible.ToString(CultureInfo.InvariantCulture)));
                    parameters.Add(new SqlParameter("@TOTALDOC", importe.ToString(CultureInfo.InvariantCulture)));
                    parameters.Add(new SqlParameter("@TOTALDIV", importe.ToString(CultureInfo.InvariantCulture)));
                    parameters.Add(new SqlParameter("@IVA", sFormatoFinalIVA));
                    parameters.Add(new SqlParameter("@RETPORCEN", retencion.ToString(CultureInfo.InvariantCulture)));

                    sageContext.Database.ExecuteSqlRaw(query, parameters);

                    //Actualizar el código de factura siguiente en tabla empresa
                    //List<SqlParameter> parameters = new List<SqlParameter>();
                    //parameters.Add(new SqlParameter("@FACTUCOM", codigoFacturaSage + 1));
                    //parameters.Add(new SqlParameter("@CODIGO", empresa.CODIGO));
                    //sageContext.Database.ExecuteSqlRaw(
                    //    $"UPDATE empresa SET FACTUCOM=@FACTUCOM WHERE CODIGO=@CODIGO",
                    //    parameters);

                    await transaction_sage.CommitAsync();
                }
                transaction.Commit();
            }

            //Pasar el documento físico a la ruta de documentación de sage
            DirectoryInfo info = new DirectoryInfo(appSettings.PathDocSAGE);
            if (info.Exists && model != null && !string.IsNullOrWhiteSpace(model.FicheroUrl))
            {
                string pathFile = Path.Combine(HostingEnvironment.WebRootPath, model.FicheroUrl.Replace("~/", "").Replace("/", "\\"));
                FileInfo fileInfo = new FileInfo(pathFile);
                string destFileName = $"{appSettings.PathDocSAGE}/{DateTime.Now.ToString("yyyyMMddHHmmss")}_{model.FicheroNombre}";
                fileInfo.CopyTo(destFileName);
            }

            return list;
        }

        /// <summary>
        /// Método que comprueba si todas las facturas de compra 
        /// </summary>
        /// <returns></returns>
        public async Task ActualizarPagadas(string codigoOperario)
        {
            //Se pasa el código de operario para no tener que actualizar todas si no es necesario
            //Motivo: Intentar evitar posibles problemas de rendimiento
            CompraFacturaFilter filter = new CompraFacturaFilter()
            {
                CodigoOperario = codigoOperario
            };
            var facturas = _db.CompraFacturas.Where(filter.ExpressionFilter());

            foreach (CompraFactura item in facturas)
            {
                var previs = _sageComuContext.previs.FirstOrDefault(f => 
                    f.PROVEEDOR.Trim() == item.CodigoProveedor.Trim() &&
                    f.FACTURA.Trim() == item.NumeroFactura.Trim());

                if (previs == null)
                    continue;
                
                item.Pagada = previs.PAGADA.Trim().ToUpper() == "S";
            }

            await _db.SaveChangesAsync();
        }

    }
}

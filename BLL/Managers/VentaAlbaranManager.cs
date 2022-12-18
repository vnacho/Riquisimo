using Ferpuser.BLL.Exceptions;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Enums;
using Ferpuser.Transfer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class VentaAlbaranManager
    {
        public ApplicationDbContext _db { get; set; }
        public SageContextFactoryHelper _sageContextFactory { get; set; }

        public VentaAlbaranManager(ApplicationDbContext dbContext, SageContextFactoryHelper sageContextFactory)
        {
            _db = dbContext;
            _sageContextFactory = sageContextFactory;
        }

        /// <summary>
        /// Creación de un albarán de venta mediante una transacción
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Create(VentaAlbaran model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {

                //1 - Añadimos el albaran
                //Buscar el número de albaran que corresponde
                SageContext sageContext = _sageContextFactory.CreateSageContext(model.Fecha.Year);

                if (sageContext == null)
                    throw new Exception($"{nameof(VentaAlbaranManager)} : No se encuentra una base de datos para el año {model.Fecha.Year}.");

                var serie = sageContext.Serie.SingleOrDefault(f =>
                    f.Tipodoc == Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_ALBARAN &&
                    f.Empresa == Consts.CODIGO_EMPRESA &&
                    f.Serie == model.Serie
                );
                if (serie == null)
                    throw new Exception($"{nameof(VentaAlbaranManager)} : No se encuentra la serie {model.Serie} para el Tipodoc {Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_ALBARAN}");

                model.CodigoAlbaran = (int)serie.Contador + 1;
                _db.VentaAlbaranes.Add(model);

                if (model.AlbaranLineas.Any(f => f.VentaPedidoLineaId.HasValue))
                {
                    //2 - Ajustamos cantidades de líneas de pedido relacionadas
                    List<VentaPedidoLinea> listLineasPedidoAfectadas = new List<VentaPedidoLinea>();
                    foreach (var item in model.AlbaranLineas.Where(f => f.VentaPedidoLineaId.HasValue))
                    {
                        VentaPedidoLinea pedidoLinea = _db.VentaPedidoLineas.Find(item.VentaPedidoLineaId.Value);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes - item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Orden);

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
                }

                await _db.SaveChangesAsync();
                transaction.Commit();

                //Actualizar el contador en series
                serie.Contador = model.CodigoAlbaran;
                sageContext.Entry(serie).State = EntityState.Modified;
                sageContext.SaveChanges();
            }
        }

        /// <summary>
        /// Edición de un albarán de venta mediante una transacción
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Edit(VentaAlbaran model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                List<VentaPedidoLinea> listLineasPedidoAfectadas = new List<VentaPedidoLinea>();

                //1 - Actualización del modelo
                _db.Entry(model).State = EntityState.Modified;

                //2 - Creación de líneas
                foreach (var item in model.AlbaranLineas.Where(f => f.IdAlbaranLinea <= 0))
                {
                    _db.Entry(item).State = EntityState.Added;

                    if (item.VentaPedidoLineaId.HasValue)
                    {
                        VentaPedidoLinea pedidoLinea = _db.VentaPedidoLineas.Find(item.VentaPedidoLineaId);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes - item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0 || pedidoLinea.UnidadesPendientes > pedidoLinea.Unidades) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }
                }

                //3 - Modificación de líneas
                var lineasModificadas = model.AlbaranLineas.Where(f => f.IdAlbaranLinea > 0);
                foreach (var item in lineasModificadas)
                {
                    _db.Entry(item).State = EntityState.Modified;

                    if (item.VentaPedidoLineaId.HasValue)
                    {
                        VentaAlbaranLinea albaranLineaOriginal = _db.VentaAlbaranLineas.AsNoTracking().Single(f => f.IdAlbaranLinea == item.IdAlbaranLinea);
                        VentaPedidoLinea pedidoLinea = _db.VentaPedidoLineas.Single(f => f.IdPedidoLinea == item.VentaPedidoLineaId);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes + albaranLineaOriginal.Unidades - item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0 || pedidoLinea.UnidadesPendientes > pedidoLinea.Unidades) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }
                }

                //4 - Borrado de líneas
                var lineasBorradas = _db.VentaAlbaranLineas.Where(f => f.AlbaranId == model.Id && !lineasModificadas.Select(f => f.IdAlbaranLinea).Contains(f.IdAlbaranLinea));
                foreach (var item in lineasBorradas)
                {
                    _db.Entry(item).State = EntityState.Deleted;

                    if (item.VentaPedidoLineaId.HasValue)
                    {
                        VentaPedidoLinea pedidoLinea = _db.VentaPedidoLineas.Find(item.VentaPedidoLineaId);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes + item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0 || pedidoLinea.UnidadesPendientes > pedidoLinea.Unidades) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }
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

                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        /// <summary>
        /// Borrado de un albarán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                List<VentaPedidoLinea> listLineasPedidoAfectadas = new List<VentaPedidoLinea>();

                var model = _db.VentaAlbaranes.Include(f => f.AlbaranLineas).SingleOrDefault(f => f.Id == id);
                
                //1 - Restablecer líneas que están relacionadas con pedidos
                foreach (var item in model.AlbaranLineas.Where(f => f.VentaPedidoLineaId.HasValue))
                {
                    var pedidoLinea = _db.VentaPedidoLineas.SingleOrDefault(f => f.IdPedidoLinea == item.VentaPedidoLineaId);
                    pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes + item.Unidades;
                    listLineasPedidoAfectadas.Add(pedidoLinea);
                }
                await _db.SaveChangesAsync();

                //2 - Ajustar el estado de todos los pedidos relacionados                
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

                //3 - Borrar el albarán
                _db.Entry(model).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        
    }
}

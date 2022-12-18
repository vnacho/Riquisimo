using Ferpuser.BLL.Exceptions;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class CompraAlbaranManager
    {
        public ApplicationDbContext _db { get; set; }

        public CompraAlbaranManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Creación de un albarán de compra mediante una transacción
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Create(CompraAlbaran model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {

                //1 - Añadimos el albaran
                _db.CompraAlbaranes.Add(model);

                if (!model.AlbaranLineas.Any(f => f.CompraPedidoLineaId.HasValue))
                {
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    return;
                }

                //2 - Ajustamos cantidades de líneas de pedido relacionadas
                List<CompraPedidoLinea> listLineasPedidoAfectadas = new List<CompraPedidoLinea>();
                foreach (var item in model.AlbaranLineas.Where(f => f.CompraPedidoLineaId.HasValue))
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
        public async Task Edit(CompraAlbaran model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                List<CompraPedidoLinea> listLineasPedidoAfectadas = new List<CompraPedidoLinea>();

                //1 - Actualización del modelo
                _db.Entry(model).State = EntityState.Modified;

                //2 - Creación de líneas
                foreach (var item in model.AlbaranLineas.Where(f => f.IdAlbaranLinea <= 0))
                {
                    _db.Entry(item).State = EntityState.Added;

                    if (item.CompraPedidoLineaId.HasValue)
                    {
                        CompraPedidoLinea pedidoLinea = _db.CompraPedidoLineas.Find(item.CompraPedidoLineaId);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes - item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0 || pedidoLinea.UnidadesPendientes > pedidoLinea.Unidades) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.CodigoPedido, pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }
                }

                //3 - Modificación de líneas
                var lineasModificadas = model.AlbaranLineas.Where(f => f.IdAlbaranLinea > 0);
                foreach (var item in lineasModificadas)
                {
                    _db.Entry(item).State = EntityState.Modified;

                    if (item.CompraPedidoLineaId.HasValue)
                    {
                        CompraAlbaranLinea albaranLineaOriginal = _db.CompraAlbaranLineas.AsNoTracking().Single(f => f.IdAlbaranLinea == item.IdAlbaranLinea);
                        CompraPedidoLinea pedidoLinea = _db.CompraPedidoLineas.Single(f => f.IdPedidoLinea == item.CompraPedidoLineaId);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes + albaranLineaOriginal.Unidades - item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0 || pedidoLinea.UnidadesPendientes > pedidoLinea.Unidades) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.CodigoPedido, pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }
                }

                //4 - Borrado de líneas
                var lineasBorradas = _db.CompraAlbaranLineas.Where(f => f.AlbaranId == model.Id && !lineasModificadas.Select(f => f.IdAlbaranLinea).Contains(f.IdAlbaranLinea));
                foreach (var item in lineasBorradas)
                {
                    _db.Entry(item).State = EntityState.Deleted;

                    if (item.CompraPedidoLineaId.HasValue)
                    {
                        CompraPedidoLinea pedidoLinea = _db.CompraPedidoLineas.Find(item.CompraPedidoLineaId);
                        pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes + item.Unidades;

                        if (pedidoLinea.UnidadesPendientes < 0 || pedidoLinea.UnidadesPendientes > pedidoLinea.Unidades) //Algo está mal, no se pueden añadir más unidades que las pendientes.
                            throw new UnidadesPendientesNegativasException(item.Unidades, pedidoLinea.Pedido.CodigoPedido, pedidoLinea.Orden);

                        listLineasPedidoAfectadas.Add(pedidoLinea);
                    }
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
                List<CompraPedidoLinea> listLineasPedidoAfectadas = new List<CompraPedidoLinea>();

                var model = _db.CompraAlbaranes.Include(f => f.AlbaranLineas).SingleOrDefault(f => f.Id == id);
                
                //1 - Restablecer líneas que están relacionadas con pedidos
                foreach (var item in model.AlbaranLineas.Where(f => f.CompraPedidoLineaId.HasValue))
                {
                    var pedidoLinea = _db.CompraPedidoLineas.SingleOrDefault(f => f.IdPedidoLinea == item.CompraPedidoLineaId);
                    pedidoLinea.UnidadesPendientes = pedidoLinea.UnidadesPendientes + item.Unidades;
                    listLineasPedidoAfectadas.Add(pedidoLinea);
                }
                await _db.SaveChangesAsync();

                //2 - Ajustar el estado de todos los pedidos relacionados                
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

                //3 - Borrar el albarán
                _db.Entry(model).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        
    }
}

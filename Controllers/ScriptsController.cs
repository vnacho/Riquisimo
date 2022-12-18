using Ferpuser.Data;
using Ferpuser.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Ferpuser.Controllers
{
    public class ScriptsController : Controller
    {
        public ApplicationDbContext db { get; set; }
        public ScriptsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {            
            return View();
        }

        [HttpPost, ActionName("ActualizarEstadosPedidosCompra")]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarEstadosPedidosCompra()
        {
            try
            {
                var pedidos = db.CompraPedidos.Include(f => f.PedidoLineas);
                foreach (var Pedido in pedidos)
                {
                    if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes <= 0))
                        Pedido.EstadoPedido = EstadoPedido.Servido;
                    else if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes >= f.Unidades))
                        Pedido.EstadoPedido = EstadoPedido.Pendiente;
                    else
                        Pedido.EstadoPedido = EstadoPedido.PendienteParcial;
                }
                db.SaveChanges();
                TempData["Message"] = $"Actualizados {pedidos.Count()} pedidos.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex} {ex.Message} {ex.StackTrace}.";                
            }
            
            return View("Index");
        }

        [HttpPost, ActionName("ActualizarEstadosPedidosVenta")]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarEstadosPedidosVenta()
        {
            try
            {
                var pedidos = db.VentaPedidos.Include(f => f.PedidoLineas);
                foreach (var Pedido in pedidos)
                {
                    if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes <= 0))
                        Pedido.EstadoPedido = EstadoPedido.Servido;
                    else if (Pedido.PedidoLineas.All(f => f.UnidadesPendientes >= f.Unidades))
                        Pedido.EstadoPedido = EstadoPedido.Pendiente;
                    else
                        Pedido.EstadoPedido = EstadoPedido.PendienteParcial;
                }
                db.SaveChanges();
                TempData["Message"] = $"Actualizados {pedidos.Count()} pedidos.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex} {ex.Message} {ex.StackTrace}.";
            }

            return View("Index");
        }
    }
}

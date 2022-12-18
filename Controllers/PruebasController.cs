using Ferpuser.BLL.Helpers;
using Ferpuser.Data;
using Ferpuser.Models.Enums;
using Ferpuser.Models.Sage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    public class PruebasController : Controller
    {
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComuContext;
        private readonly ApplicationDbContext _db;

        private VentaFacturaPrint printHelper;

        public PruebasController(SageContext sageContext, SageComuContext sageComuContext, ApplicationDbContext dbContext, VentaFacturaPrint printHelper)
        {
            _sageContext = sageContext;
            _sageComuContext = sageComuContext;
            _db = dbContext;
            this.printHelper = printHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FromWeb()
        {
            Guid Id = Guid.Parse("fdd18f11-1877-4349-bdad-2516ae037344");

            return RedirectToAction("FromWeb", "FerpuserTransfer", new { id = Id });
        }
        

        [HttpPost, ActionName("Index")]
        public IActionResult IndexConfirmed()
        {
            int pedidos_modificados = 0;
            int albaranes_modificados = 0;
            var compras_pedidos = _db.CompraPedidos.Include(f => f.PedidoLineas).ToList();
            foreach (var compra in compras_pedidos)
            {
                foreach (var linea in compra.PedidoLineas)
                {
                    if (linea.IVA_Porcentaje.HasValue)
                        continue;

                    Articulo articulo = _sageContext.Articulo.FirstOrDefault(f => f.Codigo == linea.CodigoArticulo);
                    if (articulo != null)
                    {
                        var tipoIVA = _sageContext.Tipo_IVA.SingleOrDefault(f => f.Codigo == articulo.TIPO_IVA);

                        if (tipoIVA == null)
                            continue;

                        linea.CodigoTipoIVA = tipoIVA.Codigo;
                        linea.IVA_Porcentaje = (int)tipoIVA.IVA;
                        linea.Calcular();
                        _db.SaveChanges();
                    }
                }

                compra.Calcular();
                _db.SaveChanges();
                pedidos_modificados++;
            }

            var compras_albaranes = _db.CompraAlbaranes.Include(f => f.AlbaranLineas).ToList();
            foreach (var albaran in compras_albaranes)
            {
                foreach (var linea in albaran.AlbaranLineas)
                {
                    if (linea.IVA_Porcentaje.HasValue)
                        continue;

                    Articulo articulo = _sageContext.Articulo.FirstOrDefault(f => f.Codigo == linea.CodigoArticulo);
                    if (articulo != null)
                    {
                        var tipoIVA = _sageContext.Tipo_IVA.SingleOrDefault(f => f.Codigo == articulo.TIPO_IVA);
                        linea.CodigoTipoIVA = tipoIVA.Codigo;
                        linea.IVA_Porcentaje = (int)tipoIVA.IVA;
                        linea.Calcular();
                        _db.SaveChanges();
                    }
                }

                albaran.Calcular();
                _db.SaveChanges();
                albaranes_modificados++;
            }
            string model = $"{pedidos_modificados} pedidos modificados. {albaranes_modificados} albaranes modificados.";
            return View("Index", model);
        }

        public IActionResult ImprimirFactura()
        {
            int Id = 8454;
            var pdf = printHelper.GetFactura(Id, FormatoImpresion.A);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + $"_FacturaVenta_{Id}.pdf";
            return File(pdf, "application/pdf", fileName);
        }


        //public IActionResult Index()
        //{
        //    try
        //    {
        //        string sUser = "sanicongress@15enfermeriaquirurgica.com";
        //        SmtpClient smtp = new SmtpClient("mail.15enfermeriaquirurgica.com")
        //        {
        //            EnableSsl = true,
        //            Port = 587,
        //            UseDefaultCredentials = false,
        //            Credentials = new NetworkCredential(sUser, "kdx1mAVBM"),
        //        };
        //        MailMessage message = new MailMessage
        //        {
        //            Sender = new MailAddress(sUser, sUser),
        //            From = new MailAddress(sUser, sUser),
        //            Subject = "Asunto prueba",
        //            Body = "<div>Hola</div>",
        //            IsBodyHtml = true,
        //        };
        //        message.To.Add(new MailAddress("info@programadorburgos.es"));
        //        smtp.Send(message);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok();            
        //}

        //public IActionResult Index()
        //{
        //    try
        //    {
        //        using (var transaction = _db.Database.BeginTransaction())
        //        {
        //            var facturascompra = _db.CompraFacturas.OrderBy(f => f.Fecha);
        //            foreach (var model in facturascompra)
        //            {
        //                string anio2dig = model.Fecha.ToString("yy");
        //                string ultimoRegistro = _db.CompraFacturas
        //                    .AsNoTracking()
        //                    .Where(f => f.Registro.StartsWith(anio2dig))
        //                    .OrderByDescending(f => f.Registro)
        //                    .FirstOrDefault()?
        //                    .Registro;

        //                if (string.IsNullOrWhiteSpace(ultimoRegistro))
        //                    model.Registro = $"{anio2dig}0001";
        //                else
        //                    model.Registro = (Convert.ToInt32(ultimoRegistro) + 1).ToString();

        //                _db.SaveChanges();
        //            }
        //            transaction.Commit();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok();
        //    return View();
        //}
    }
}

    

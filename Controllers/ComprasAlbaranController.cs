using CsvHelper;
using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Exceptions;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Helpers;
using Ferpuser.BLL.Managers;
using Ferpuser.BLL.Services;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Core;
using Ferpuser.Models.Enums;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Compras")]
    public class ComprasAlbaranController : ComprasBaseController
    {
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComuContext;
        private readonly ApplicationDbContext _dbContext;

        public ComprasAlbaranController(SageContext sageContext, SageComuContext sageComuContext, ApplicationDbContext dbContext)
        {
            _sageContext = sageContext;
            _sageComuContext = sageComuContext;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int? pag, string sort = "", string currentsort = "", bool reset = false, bool export = false)
        {
            if (reset)
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(sort))
            {
                if (string.IsNullOrWhiteSpace(currentsort))
                    sort = "Fecha desc";
                else
                    sort = currentsort;
            }

            CompraAlbaranFilter filter = new CompraAlbaranFilter();
            await TryUpdateModelAsync<CompraAlbaranFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoProveedor,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoAlbaran);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoOperario = UserHelper.CodigoOperario(User);

            Pager pager = new Pager(await _dbContext.CompraAlbaranes.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<CompraAlbaran> list = await _dbContext.CompraAlbaranes.Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.NOMBRE);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);

            return View(list);
        }

        public async Task<FileResult> ExportCsv(string currentsort = "")
        {
            CompraAlbaranFilter filter = new CompraAlbaranFilter();
            await TryUpdateModelAsync<CompraAlbaranFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoProveedor,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoAlbaran);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoOperario = UserHelper.CodigoOperario(User);

            IEnumerable<CompraAlbaran> list = await _dbContext.CompraAlbaranes
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoAlbaranesCompra.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<ComprasAlbaranViewModelMap>();
                await csv.WriteRecordsAsync<CompraAlbaran>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

        public async Task<FileResult> ExportPdf(string currentsort = "")
        {
            CompraAlbaranFilter filter = new CompraAlbaranFilter();
            await TryUpdateModelAsync<CompraAlbaranFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoProveedor,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoAlbaran);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoOperario = UserHelper.CodigoOperario(User);

            IEnumerable<CompraAlbaran> list = await _dbContext.CompraAlbaranes
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string table = "<table class='w-100'><tr class='font-weight-bold'><td>Albarán</td><td>Fecha</td><td>Proveedor</td><td>Operario</td><td class='text-right'>Total</td></tr>{0}</table>";
            string rows = string.Empty;
            foreach (var item in list)
            {
                rows += $"<tr><td>{item.CodigoAlbaran}</td><td>{item.Fecha.ToShortDateString()}</td><td>{item.NombreProveedor}</td><td>{item.NombreOperario}</td><td class='text-right'>{item.Total.ToString("C")}</td></tr>";
            }

            var html = PrintService.GetHtmlTheme("Listado de albaranes de compra", string.Format(table, rows));
            var pdf = PrintService.GetBytes(html);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoAlbaranesCompra.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        public IActionResult Create(int? idPedido)
        {
            CompraAlbaran model;
            ViewBag.Operarios = _sageComuContext.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.CODIGO);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Codigo);
            ViewBag.IdPedido = idPedido;
            
            if (idPedido.HasValue)
            {   
                var pedido = _dbContext.CompraPedidos.Include(f => f.PedidoLineas).SingleOrDefault(f => f.Id == idPedido);
                if (pedido != null)
                {
                    TipoIVAManager manager = new TipoIVAManager(_sageContext);
                    model = new CompraAlbaran()
                    {
                        Fecha = DateTime.Now.Date,
                        CodigoProveedor = pedido.CodigoProveedor,
                        NombreProveedor = pedido.NombreProveedor,
                        CodigoOperario = pedido.CodigoOperario,
                        CodigoEvento = pedido.CodigoEvento,
                        NombreOperario = pedido.NombreOperario,
                        Observaciones = pedido.Observaciones,
                        AlbaranLineas = new List<CompraAlbaranLinea>()
                    };
                    foreach (var linea in pedido.PedidoLineas)
                    {
                        var tipoiva = manager.GetIVA(linea.CodigoArticulo);
                        CompraAlbaranLinea albaranLinea = new CompraAlbaranLinea()
                        {
                            CodigoArticulo = linea.CodigoArticulo,
                            NombreArticulo = linea.NombreArticulo,
                            CodigoEvento = linea.CodigoEvento,
                            NombreEvento = linea.NombreEvento,
                            ObservacionesAlbaranLinea = linea.ObservacionesPedidoLinea,
                            Orden = linea.Orden,
                            PrecioUnitario = linea.PrecioUnitario,
                            TotalAlbaranLinea = linea.TotalPedidoLinea,
                            Unidades = linea.UnidadesPendientes,
                            CompraPedidoLineaId = linea.IdPedidoLinea,
                            UnidadesPendientes = linea.UnidadesPendientes,
                            CodigoPedido = pedido.CodigoPedido,
                            IVA_Porcentaje = (int)tipoiva?.IVA,
                            CodigoTipoIVA = tipoiva?.Codigo
                        };
                        albaranLinea.Calcular();
                        model.AlbaranLineas.Add(albaranLinea);
                    }
                    model.Calcular();

                    return View(model);
                }
            }



            string codOperario = User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_CODIGO_OPERARIO))?.Value;
            var operario = _sageContext.Proveedores.SingleOrDefault(f => f.CODIGO == codOperario);
            model = new CompraAlbaran() { CodigoOperario = codOperario, NombreOperario = operario?.NOMBRE };
            return View(model);            
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed(int? idPedido)
        {
            CompraAlbaran model = new CompraAlbaran();

            var proveedor = _sageContext.Proveedores.Find(Request.Form["CodigoProveedor"]);
            model.NombreProveedor = proveedor == null ? string.Empty : proveedor.NOMBRE;
            var operario = _sageComuContext.Operarios.Find(Request.Form["CodigoOperario"]);
            model.NombreOperario = operario == null ? string.Empty : operario.NOMBRE;
            var evento = _sageContext.Almacen.Find(Request.Form["CodigoEvento"]);
            model.NombreEvento = evento == null ? string.Empty : evento.Nombre;

            model.EstadoAlbaran = EstadoAlbaran.NoFacturado;

            await TryUpdateModelAsync<CompraAlbaran>(model, "",
                f => f.CodigoAlbaran,
                f => f.Fecha,
                f => f.CodigoProveedor,
                f => f.CodigoOperario,
                f => f.CodigoEvento,
                f => f.Observaciones,
                f => f.AlbaranLineas);

            if (model.AlbaranLineas == null)
                model.AlbaranLineas = new List<CompraAlbaranLinea>();

            foreach (var item in model.AlbaranLineas)
            {
                item.Calcular();
            }
            model.Calcular();

            //Validación de negocio            
            ComprasAlbaranValidator validator = new ComprasAlbaranValidator(_dbContext);
            IList<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                CompraAlbaranManager manager = new CompraAlbaranManager(_dbContext);                
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");                
            }

            ViewBag.Operarios = _sageComuContext.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.CODIGO);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Codigo);
            ViewBag.IdPedido = idPedido;

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Operarios = _sageComuContext.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.CODIGO);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Codigo);

            var model = _dbContext.CompraAlbaranes.Include(f => f.AlbaranLineas).Single(f => f.Id == id);

            if (!string.IsNullOrWhiteSpace(model.CodigoEvento))
                ViewBag.EventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo.Trim() == model.CodigoEvento.Trim())?.DisplayName;

            if (!string.IsNullOrWhiteSpace(model.CodigoProveedor))
                ViewBag.ProveedorNombre = _sageContext.Proveedores.SingleOrDefault(f => f.CODIGO.Trim() == model.CodigoProveedor.Trim())?.NOMBRE;

            foreach (var item in model.AlbaranLineas)
            {
                if (item.CompraPedidoLineaId.HasValue)
                {
                    CompraPedidoLinea linea = _dbContext.CompraPedidoLineas.Include(f => f.Pedido).Single(f => f.IdPedidoLinea == item.CompraPedidoLineaId);
                    item.UnidadesPendientes = linea.UnidadesPendientes + item.Unidades;
                    item.CodigoPedido = linea.Pedido.CodigoPedido;
                }
            }

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            CompraAlbaran model = _dbContext.CompraAlbaranes.Find(id);

            var proveedor = _sageContext.Proveedores.Find(Request.Form["CodigoProveedor"]);
            model.NombreProveedor = proveedor == null ? string.Empty : proveedor.NOMBRE;
            var operario = _sageComuContext.Operarios.Find(Request.Form["CodigoOperario"]);
            model.NombreOperario = operario == null ? string.Empty : operario.NOMBRE;
            var evento = _sageContext.Almacen.Find(Request.Form["CodigoEvento"]);
            model.NombreEvento = evento == null ? string.Empty : evento.Nombre;

            await TryUpdateModelAsync<CompraAlbaran>(model, "",
                f => f.Id,
                f => f.CodigoAlbaran,
                f => f.Fecha,
                f => f.CodigoProveedor,
                f => f.CodigoOperario,
                f => f.CodigoEvento,
                f => f.Observaciones,
                f => f.AlbaranLineas);

            if (model.AlbaranLineas == null)
                model.AlbaranLineas = new List<CompraAlbaranLinea>();

            foreach (var item in model.AlbaranLineas)
            {
                item.Calcular();
            }
            model.Calcular();

            //Validación de negocio            
            ComprasAlbaranValidator validator = new ComprasAlbaranValidator(_dbContext);
            IList<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                CompraAlbaranManager manager = new CompraAlbaranManager(_dbContext);                
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");                
            }

            ViewBag.Operarios = _sageComuContext.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.CODIGO);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Codigo);

            return View(model);
        }

        public PartialViewResult AddLinea([FromQuery]string CodigoEvento)
        {
            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
            if (!string.IsNullOrWhiteSpace(CodigoEvento))
                ViewBag.LineaEventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo == CodigoEvento).DisplayName;
            return PartialView("~/Views/Shared/EditorTemplates/CompraAlbaranLinea.cshtml", new CompraAlbaranLinea() { Unidades = 1, CodigoArticulo = string.Empty, Orden = -1, CodigoEvento = CodigoEvento });
        }

        [HttpPost]
        public async Task<PartialViewResult> EditLinea([FromQuery]int orden)
        {
            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);

            CompraAlbaran model = new CompraAlbaran();
            await TryUpdateModelAsync<CompraAlbaran>(model, "",
                f => f.AlbaranLineas);
            if (model.AlbaranLineas == null)
                model.AlbaranLineas = new List<CompraAlbaranLinea>();

            CompraAlbaranLinea linea = model.AlbaranLineas.Single(f => f.Orden == orden);

            if (!string.IsNullOrWhiteSpace(linea.CodigoArticulo))
                ViewBag.ArticuloNombre = _sageContext.Articulo.SingleOrDefault(f => f.Codigo == linea.CodigoArticulo)?.Display;
            if (!string.IsNullOrWhiteSpace(linea.CodigoEvento))
                ViewBag.LineaEventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo == linea.CodigoEvento)?.DisplayName;

            return PartialView("~/Views/Shared/EditorTemplates/CompraAlbaranLinea.cshtml", linea);
        }

        public async Task<IActionResult> Delete(int id)
        {
            CompraAlbaranManager manager = new CompraAlbaranManager(_dbContext);
            await manager.Delete(id);
            TempData["Message"] = "El registro se ha eliminado correctamente.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> SaveLinea()
        {
            CompraAlbaranLinea linea = new CompraAlbaranLinea();

            await TryUpdateModelAsync<CompraAlbaranLinea>(linea, "linea",
                f => f.CodigoArticulo,
                f => f.Unidades,
                f => f.CodigoEvento,
                f => f.ObservacionesAlbaranLinea,
                f => f.PrecioUnitario,
                f => f.UnidadesPendientes,
                f => f.Orden,
                f => f.CompraPedidoLineaId,
                f => f.IdAlbaranLinea,
                f => f.CodigoPedido,
                f => f.CodigoTipoIVA
            );

            if (ModelState.IsValid)
            {
                CompraAlbaran model = new CompraAlbaran();
                await TryUpdateModelAsync<CompraAlbaran>(model, "",
                    f => f.AlbaranLineas);
                if (model.AlbaranLineas == null)
                    model.AlbaranLineas = new List<CompraAlbaranLinea>();

                linea.NombreArticulo = _sageContext.Articulo.Find(linea.CodigoArticulo).Nombre;
                linea.NombreEvento = _sageContext.Almacen.Find(linea.CodigoEvento).Nombre;
                linea.IVA_Porcentaje = (int)_sageContext.Tipo_IVA.Find(linea.CodigoTipoIVA).IVA;

                linea.Calcular();
                if (linea.Orden < 0) //Nuevo
                {
                    linea.Orden = model.AlbaranLineas.Count();
                    model.AlbaranLineas.Add(linea);
                }
                else //Editar
                {
                    var list = model.AlbaranLineas.ToList();
                    list[list.FindIndex(f => f.Orden == linea.Orden)] = linea;
                    model.AlbaranLineas = list;
                }

                model.Calcular();
                var responseValid = new ResponseObject()
                {
                    Status = 1,
                    Data = await this.RenderPartialViewToString("_AlbaranLineas", model)
                };
                return Json(responseValid);
            }

            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
            if (!string.IsNullOrWhiteSpace(linea.CodigoArticulo))
                ViewBag.ArticuloNombre = _sageContext.Articulo.SingleOrDefault(f => f.Codigo == linea.CodigoArticulo).Display;
            if (!string.IsNullOrWhiteSpace(linea.CodigoEvento))
                ViewBag.LineaEventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo == linea.CodigoEvento).DisplayName;

            var responseInvalid = new ResponseObject()
            {
                Status = 0,
                Data = await this.RenderPartialViewToString("EditorTemplates/CompraAlbaranLinea", linea)
            };
            return Json(responseInvalid);
        }       

        [HttpPost]
        public async Task<JsonResult> SaveLineasDesdePedido([FromQuery]int IdPedido)
        {
            var lineas = _dbContext.CompraPedidoLineas.Include(f => f.Pedido).Where(f => f.PedidoId == IdPedido && f.UnidadesPendientes > 0);

            CompraAlbaran model = new CompraAlbaran();
            await TryUpdateModelAsync<CompraAlbaran>(model, "",
                f => f.AlbaranLineas);
            if (model.AlbaranLineas == null)
                model.AlbaranLineas = new List<CompraAlbaranLinea>();

            foreach (var item in lineas)
            {
                CompraAlbaranLinea linea = new CompraAlbaranLinea()
                {
                    CodigoArticulo = item.CodigoArticulo,
                    CodigoEvento = item.CodigoEvento,
                    NombreArticulo = item.NombreArticulo,
                    NombreEvento = item.NombreEvento,
                    ObservacionesAlbaranLinea = item.ObservacionesPedidoLinea,
                    Orden = model.AlbaranLineas.Count(),
                    PrecioUnitario = item.PrecioUnitario,
                    Unidades = item.UnidadesPendientes,
                    CompraPedidoLineaId = item.IdPedidoLinea,
                    UnidadesPendientes = item.UnidadesPendientes,
                    CodigoPedido = item.Pedido.CodigoPedido
                };
                //Calcular el IVA por defecto para este artículo
                TipoIVAManager manager = new TipoIVAManager(_sageContext);
                var tipoiva = manager.GetIVA(item.CodigoArticulo);
                linea.CodigoTipoIVA = tipoiva?.Codigo;
                linea.IVA_Porcentaje = tipoiva == null ? (int?)null : (int)tipoiva.IVA;

                linea.Calcular();

                model.AlbaranLineas.Add(linea);
            }
            model.Calcular();

            var responseValid = new ResponseObject()
            {
                Status = 1,
                Data = await this.RenderPartialViewToString("_AlbaranLineas", model)
            };
            return Json(responseValid);            
        }

        [HttpPost]
        public async Task<PartialViewResult> DeleteLinea([FromQuery]int orden)
        {
            CompraAlbaran model = new CompraAlbaran();
            await TryUpdateModelAsync<CompraAlbaran>(model, "", f => f.AlbaranLineas);
            if (model.AlbaranLineas == null)
                model.AlbaranLineas = new List<CompraAlbaranLinea>();

            var linea = model.AlbaranLineas.SingleOrDefault(f => f.Orden == orden);
            model.AlbaranLineas.Remove(linea);

            foreach (var item in model.AlbaranLineas.Where(f => f.Orden > orden))
            {
                item.Orden--;
            }

            return PartialView("_AlbaranLineas", model);
        }

        public IActionResult ImprimirAlbaran(int Id)
        {
            var albaran = _dbContext.CompraAlbaranes.Include(f => f.AlbaranLineas).SingleOrDefault(f => f.Id == Id);

            if (albaran == null)
                return NotFound();

            //Obtener
            var pdf = PrintService.GetBytes(HtmlParaImprimir(albaran));
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + $"_AlbaranCompra_{Id}.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        public async Task<IActionResult> EnviarAlbaran(int id)
        {
            //Obtenemos los datos de smtp del usuario activo
            SmtpConfig smtpConfig = UserMailHelper.GetStmpConfig(User);           

            if (smtpConfig == null)
            {
                TempData["ErrorMessage"] = "No se ha podido enviar el mail. Su usuario no tiene establecida la configuración de envío de mails.";
                return RedirectToAction("Edit", new { id = id });
            }

            smtpConfig.EnableSsl = true;

            string sSmtpSendCopy = User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_SEND_COPY))?.Value;

            //Obtenemos los datos del proveedor
            var albaran = _dbContext.CompraAlbaranes.Include(f => f.AlbaranLineas).Single(f => f.Id == id);
            var proveedor = _sageContext.Proveedores.FirstOrDefault(f => f.CODIGO == albaran.CodigoProveedor);
            string emailTo = proveedor?.EMAIL;
            if (string.IsNullOrWhiteSpace(emailTo))
            {
                TempData["ErrorMessage"] = "No se ha podido enviar el mail. El proveedor seleccionado no tiene dirección de mail configurado.";
                return RedirectToAction("Edit", new { id = id });
            }

            string html = HtmlParaImprimir(albaran);
            List<string> to = new List<string>();
            to.Add(emailTo);
            if (!string.IsNullOrWhiteSpace(sSmtpSendCopy) && Convert.ToBoolean(sSmtpSendCopy))
            {
                string sMail = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;
                if (!string.IsNullOrWhiteSpace(sMail))
                    to.Add(sMail);
            }

            var pdf = PrintService.GetBytes(html);
            MemoryStream stream = new MemoryStream(pdf);
            Attachment attachment = new Attachment(stream, $"Albaran_{id}.pdf");
            List<Attachment> attachments = new List<Attachment>();
            attachments.Add(attachment);

            EmailService service = new EmailService(smtpConfig);
            service.Send($"Albarán Nº {id}", html, true, to.ToArray(), User.Identity.Name, attachments);

            TempData["Message"] = "El mensaje se ha enviado correctamente.";
            return RedirectToAction("Edit", new { id = id });
        }

        private string HtmlParaImprimir(CompraAlbaran albaran)
        {
            AppSettings appSettings = ((IOptions<AppSettings>)HttpContext.RequestServices.GetService(typeof(IOptions<AppSettings>))).Value;
            string html = string.Empty;

            using (StreamReader reader = new StreamReader(Path.Combine("HtmlTemplates", "CompraAlbaran.html")))
            {
                html = reader.ReadToEnd();
            }

            //Logo
            html = html.Replace("#URL_LOGO#", $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/img/logo.png");

            var proveedor = _sageContext.Proveedores.Find(albaran.CodigoProveedor);
            html = html.Replace("#PROVEEDOR_NOMBRE#", proveedor.NOMBRE);
            html = html.Replace("#PROVEEDOR_CIF#", proveedor.CIF);
            html = html.Replace("#PROVEEDOR_DIRECCION#", proveedor.DIRECCION);
            html = html.Replace("#PROVEEDOR_POBLACION#", $"{proveedor.CODPOST + " "}{proveedor.POBLACION}");
            html = html.Replace("#PROVEEDOR_PROVINCIA#", proveedor.PROVINCIA);
            html = html.Replace("#NUMERO_EVENTO#", albaran.CodigoEvento);
            html = html.Replace("#NOMBRE_EVENTO#", albaran.NombreEvento);
            html = html.Replace("#ID#", albaran.Id.ToString());
            html = html.Replace("#FECHA#", $"{albaran.Fecha.ToShortDateString()}");
            

            var datos_registro_empresa = _sageContext.modconfi.Find(Consts.CODIGO_EMPRESA);
            var empresa = _sageContext.empresa.Find(Consts.CODIGO_EMPRESA);
            html = html.Replace("#REG_PUB#", datos_registro_empresa.REG_PUB);
            html = html.Replace("#REG_MER#", datos_registro_empresa.REG_MER);
            html = html.Replace("#TOMO#", datos_registro_empresa.TOMO);
            html = html.Replace("#LIBRO#", datos_registro_empresa.LIBRO);
            html = html.Replace("#FOLIO#", datos_registro_empresa.FOLIO);
            html = html.Replace("#HOJA#", datos_registro_empresa.HOJA);
            html = html.Replace("#SECCION#", datos_registro_empresa.SECCION);
            html = html.Replace("#EMPRESA_NOMBRE#", empresa.NOMBRE);
            html = html.Replace("#EMPRESA_DIRECCION#", empresa.DIRECCION);
            html = html.Replace("#EMPRESA_CP#", empresa.CODPOS);
            html = html.Replace("#EMPRESA_PROVINCIA#", empresa.PROVINCIA);
            html = html.Replace("#EMPRESA_CIF#", empresa.CIF);

            string rows = string.Empty;
            foreach (var item in albaran.AlbaranLineas)
            {
                rows += $"<tr>" +
                    $"<td>{item.NombreArticulo}</td>" +
                    $"<td class='text-right'>{item.Unidades}</td>" +
                    $"<td class='text-right'>{item.PrecioUnitario}</td>" +
                    $"<td class='text-right'>{item.IVA_Porcentaje} %</td>" +
                    $"<td class='text-right'>{item.Total.ToString("C")}</td>" +
                    $"</tr>";
            }
            html = html.Replace("#ROWS#", rows);

            html = html.Replace("#TOTAL#", albaran.Total.ToString("C"));
            return html;

        }

        [HttpGet]
        public async Task<PartialViewResult> Buscador()
        {
            CompraAlbaranFilter filter = new CompraAlbaranFilter();
            await TryUpdateModelAsync<CompraAlbaranFilter>(filter, "",
                f => f.CodigoEvento,
                f => f.CodigoProveedor,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoAlbaran);

            IEnumerable<CompraAlbaran> list = await _dbContext.CompraAlbaranes
                .Where(filter.ExpressionFilter())
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();

            return PartialView("_BuscadorComprasAlbaran", list);
        }
    }
}

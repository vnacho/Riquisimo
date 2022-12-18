using CsvHelper;
using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Helpers;
using Ferpuser.BLL.Interfaces;
using Ferpuser.BLL.Managers;
using Ferpuser.BLL.Services;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Core;
using Ferpuser.Models.Enums;
using Ferpuser.Models.ViewModels;
using Ferpuser.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class ComprasPedidoController : ComprasBaseController
    {
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComuContext;
        private readonly ApplicationDbContext _dbContext;

        public ComprasPedidoController(SageContext sageContext, SageComuContext sageComuContext, ApplicationDbContext dbContext)
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

            CompraPedidoFilter filter = new CompraPedidoFilter();
            await TryUpdateModelAsync<CompraPedidoFilter>(filter, "filter", 
                f => f.CodigoEvento, 
                f => f.CodigoProveedor,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoPedido);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoOperario = UserHelper.CodigoOperario(User);

            Pager pager = new Pager(await _dbContext.CompraPedidos.CountAsync(filter.ExpressionFilter()), pag, 100, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            if (filter.EstadoPedido == EstadoPedido.Pendiente)
            {
                filter.EstadoPedido = null;
                filter.EstadosPedido = new EstadoPedido[] { EstadoPedido.Pendiente, EstadoPedido.PendienteParcial };
            }

            IEnumerable<CompraPedido> list = await _dbContext.CompraPedidos.Where(filter.ExpressionFilter())
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
            CompraPedidoFilter filter = new CompraPedidoFilter();
            await TryUpdateModelAsync<CompraPedidoFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoProveedor,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoPedido);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoOperario = UserHelper.CodigoOperario(User);

            IEnumerable<CompraPedido> list = await _dbContext.CompraPedidos
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoPedidosCompra.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<ComprasPedidoViewModelMap>();
                await csv.WriteRecordsAsync<CompraPedido>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

        public async Task<FileResult> ExportPdf(string currentsort = "")
        {
            CompraPedidoFilter filter = new CompraPedidoFilter();
            await TryUpdateModelAsync<CompraPedidoFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoProveedor,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoPedido);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoOperario = UserHelper.CodigoOperario(User);

            IEnumerable<CompraPedido> list = await _dbContext.CompraPedidos
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string table = "<table class='w-100'><tr class='font-weight-bold'><td>Pedido</td><td>Fecha</td><td>Proveedor</td><td>Operario</td><td class='text-right'>Total</td></tr>{0}</table>";
            string rows = string.Empty;
            foreach (var item in list)
            {
                rows += $"<tr><td>{item.CodigoPedido}</td><td>{item.Fecha.ToShortDateString()}</td><td>{item.NombreProveedor}</td><td>{item.NombreOperario}</td><td class='text-right'>{item.Total.ToString("C")}</td></tr>";
            }

            var html = PrintService.GetHtmlTheme("Listado de pedidos de compra", string.Format(table, rows));
            var pdf = PrintService.GetBytes(html);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoPedidosCompra.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        public IActionResult Create()
        {
            ViewBag.Operarios = _sageComuContext.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.CODIGO);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Codigo);
            string codOperario = User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_CODIGO_OPERARIO))?.Value;            
            CompraPedido model = new CompraPedido() { CodigoOperario = codOperario };
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed(IFormFile _FicheroUrl, string FicheroUrl, string FicheroNombre, [FromServices]SageContextFactoryHelper sageContextFactory, string guardar)
        {
            CompraPedido model = new CompraPedido();

            model.CodigoPedido = string.Empty;
            var proveedor = _sageContext.Proveedores.Find(Request.Form["CodigoProveedor"]);
            model.NombreProveedor = proveedor == null ? string.Empty : proveedor.NOMBRE;
            var operario = _sageComuContext.Operarios.Find(Request.Form["CodigoOperario"]);
            model.NombreOperario = operario == null ? string.Empty : operario.NOMBRE;
            var evento = _sageContext.Almacen.Find(Request.Form["CodigoEvento"]);
            model.NombreEvento = evento == null ? string.Empty : evento.DisplayName;

            model.EstadoPedido = EstadoPedido.Pendiente;

            model.FicheroUrl = FicheroUrl;
            model.FicheroNombre = FicheroNombre;

            if (string.IsNullOrWhiteSpace(FicheroUrl))
            {
                IFileUploader _fileUploader = (IFileUploader)HttpContext.RequestServices.GetService(typeof(IFileUploader));
                string url = await _fileUploader.UploadFile(_FicheroUrl);
                if (!string.IsNullOrWhiteSpace(url))
                {
                    model.FicheroNombre = _FicheroUrl.FileName;
                    model.FicheroUrl = url;
                }
            }

            await TryUpdateModelAsync<CompraPedido>(model, "", 
                f => f.Fecha,
                f => f.CodigoProveedor,
                f => f.CodigoOperario,
                f => f.CodigoEvento,
                f => f.Observaciones,
                f => f.PedidoLineas);
            
            if (model.PedidoLineas == null)
                model.PedidoLineas = new List<CompraPedidoLinea>();

            foreach (var item in model.PedidoLineas)
            {
                item.Calcular();
            }
            model.Calcular();

            //Validación de negocio            
            ComprasPedidoValidator validator = new ComprasPedidoValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                foreach(var linea in model.PedidoLineas)
                {
                    linea.UnidadesPendientes = linea.Unidades;
                }

                CompraPedidoManager manager = new CompraPedidoManager(_dbContext, sageContextFactory);
                manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                if (guardar == "nuevo")
                    return RedirectToAction("Create");
                else if (guardar == "continuar")
                    return RedirectToAction("Edit", "ComprasPedido", new { id = model.Id });

                return RedirectToAction("Index");                
            }

            ViewBag.Operarios = _sageComuContext.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.CODIGO);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Codigo);

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Operarios = _sageComuContext.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.CODIGO);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Codigo);
            var model = _dbContext.CompraPedidos.Include(f => f.PedidoLineas).Single(f => f.Id == id);

            model.documentos = _dbContext.DocumentoCompraVenta.Where(f => f.IdTabla == model.Id && f.ClaveDoc == "CP").ToList();

            if (model.documentos == null)
                model.documentos = new List<DocumentoCompraVenta>();

            ViewBag.EmailTo = _sageContext.Proveedores.FirstOrDefault(f => f.CODIGO.Trim() == model.CodigoProveedor.Trim())?.EMAIL;

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id, IFormFile _FicheroUrl, string FicheroUrl, string FicheroNombre)
        {
            CompraPedido model = _dbContext.CompraPedidos.Find(id);

            var proveedor = _sageContext.Proveedores.Find(Request.Form["CodigoProveedor"]);
            model.NombreProveedor = proveedor == null ? string.Empty : proveedor.NOMBRE;
            var operario = _sageComuContext.Operarios.Find(Request.Form["CodigoOperario"]);
            model.NombreOperario = operario == null ? string.Empty : operario.NOMBRE;
            var evento = _sageContext.Almacen.Find(Request.Form["CodigoEvento"]);
            model.NombreEvento = evento == null ? string.Empty : evento.Nombre;

            model.FicheroUrl = FicheroUrl;
            model.FicheroNombre = FicheroNombre;

            if (string.IsNullOrWhiteSpace(FicheroUrl))
            {
                IFileUploader _fileUploader = (IFileUploader)HttpContext.RequestServices.GetService(typeof(IFileUploader));
                string url = await _fileUploader.UploadFile(_FicheroUrl);
                if (!string.IsNullOrWhiteSpace(url))
                {
                    model.FicheroNombre = _FicheroUrl.FileName;
                    model.FicheroUrl = url;
                }
            }

            await TryUpdateModelAsync<CompraPedido>(model, "",
                f => f.Id,
                f => f.Fecha,
                f => f.CodigoProveedor,
                f => f.CodigoOperario,
                f => f.CodigoEvento,
                f => f.Observaciones,
                f => f.PedidoLineas);

            if (model.PedidoLineas == null)
                model.PedidoLineas = new List<CompraPedidoLinea>();

            foreach (var item in model.PedidoLineas)
            {
                item.Calcular();
            }
            model.Calcular();

            //Validación de negocio            
            ComprasPedidoValidator validator = new ComprasPedidoValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                _dbContext.Entry(model).State = EntityState.Modified;

                //Creación de líneas
                foreach (var linea in model.PedidoLineas.Where(f => f.IdPedidoLinea <= 0))
                {
                    linea.UnidadesPendientes = linea.Unidades;
                    _dbContext.Entry(linea).State = EntityState.Added;
                }

                //Borrado de líneas
                var idLineasModificadas = model.PedidoLineas.Where(f => f.IdPedidoLinea > 0).Select(f => f.IdPedidoLinea);
                foreach (var linea in idLineasModificadas)
                {
                    _dbContext.Entry(linea).State = EntityState.Modified;
                }

                var lineasBorradas = _dbContext.CompraPedidoLineas.Where(f => f.PedidoId == id && !idLineasModificadas.Contains(f.IdPedidoLinea));
                foreach (var linea in lineasBorradas)
                {
                    _dbContext.Entry(linea).State = EntityState.Deleted;
                }

                await _dbContext.SaveChangesAsync();
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.Operarios = _sageComuContext.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.CODIGO);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Codigo);

            ViewBag.EmailTo = _sageContext.Proveedores.FirstOrDefault(f => f.CODIGO.Trim() == model.CodigoProveedor.Trim())?.EMAIL;
            
            return View(model);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var pedido = _dbContext.CompraPedidos.Find(id);
            
            pedido.documentos = _dbContext.DocumentoCompraVenta.Where(f => f.IdTabla == pedido.Id && f.ClaveDoc == "CP").ToList();

            foreach (var f in pedido.documentos)
            {
                _dbContext.DocumentoCompraVenta.Remove(f);
            }

            _dbContext.CompraPedidos.Remove(pedido);
            await _dbContext.SaveChangesAsync();
            TempData["Message"] = "El registro se ha eliminado correctamente.";
            return RedirectToAction("Index");
        }

        public PartialViewResult AddLinea([FromQuery]string CodigoEvento)
        {
            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
            if (!string.IsNullOrWhiteSpace(CodigoEvento))
                ViewBag.LineaEventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo == CodigoEvento).DisplayName;
            return PartialView("~/Views/Shared/EditorTemplates/CompraPedidoLinea.cshtml", new CompraPedidoLinea() { Unidades = 1, CodigoArticulo = string.Empty, Orden = -1, CodigoEvento = CodigoEvento });
        }

        [HttpPost]
        public async Task<PartialViewResult> EditLinea([FromQuery]int orden)
        {
            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);

            CompraPedido model = new CompraPedido();
            await TryUpdateModelAsync<CompraPedido>(model, "",
                f => f.PedidoLineas);
            if (model.PedidoLineas == null)
                model.PedidoLineas = new List<CompraPedidoLinea>();

            CompraPedidoLinea linea = model.PedidoLineas.Single(f => f.Orden == orden);     
            
            if (!string.IsNullOrWhiteSpace(linea.CodigoArticulo))
                ViewBag.ArticuloNombre = _sageContext.Articulo.SingleOrDefault(f => f.Codigo == linea.CodigoArticulo)?.Display;
            if (!string.IsNullOrWhiteSpace(linea.CodigoEvento))
                ViewBag.LineaEventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo == linea.CodigoEvento)?.DisplayName;

            return PartialView("~/Views/Shared/EditorTemplates/CompraPedidoLinea.cshtml", linea);
        }

        [HttpPost]
        public async Task<JsonResult> SaveLinea()
        {
            CompraPedidoLinea linea = new CompraPedidoLinea();
            //linea.CodigoEvento = HttpContext.Request.Form["linea.CodigoEvento"]; //Ayuda para obtener el código de evento de la línea

            await TryUpdateModelAsync<CompraPedidoLinea>(linea, "linea",
                f => f.CodigoArticulo,
                f => f.Unidades,
                f => f.ObservacionesPedidoLinea,
                f => f.PrecioUnitario,
                f => f.CodigoEvento,
                f => f.Orden,
                f => f.UnidadesPendientes,
                f => f.CodigoTipoIVA);

            if (ModelState.IsValid)
            {
                CompraPedido model = new CompraPedido();
                await TryUpdateModelAsync<CompraPedido>(model, "",
                    f => f.PedidoLineas);
                if (model.PedidoLineas == null)
                    model.PedidoLineas = new List<CompraPedidoLinea>();

                linea.NombreArticulo = _sageContext.Articulo.Find(linea.CodigoArticulo).Nombre;
                linea.NombreEvento = _sageContext.Almacen.Find(linea.CodigoEvento).Nombre;
                linea.IVA_Porcentaje = (int)_sageContext.Tipo_IVA.Find(linea.CodigoTipoIVA).IVA;

                linea.Calcular();

                if (linea.Orden < 0) //Nuevo
                {
                    linea.UnidadesPendientes = linea.Unidades;
                    linea.Orden = model.PedidoLineas.Count();
                    model.PedidoLineas.Add(linea);
                }
                else //Editar
                {
                    var list = model.PedidoLineas.ToList();
                    list[list.FindIndex(f => f.Orden == linea.Orden)] = linea;
                    model.PedidoLineas = list;
                }

                model.Calcular();

                var responseValid = new ResponseObject()
                {
                    Status = 1,
                    Data = await this.RenderPartialViewToString("_PedidoLineas", model)
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
                Data = await this.RenderPartialViewToString("EditorTemplates/CompraPedidoLinea", linea)
            };

           
            return Json(responseInvalid);
        }        

        [HttpPost]
        public async Task<PartialViewResult> DeleteLinea([FromQuery]int orden)
        {            
            CompraPedido model = new CompraPedido();
            await TryUpdateModelAsync<CompraPedido>(model, "", f => f.PedidoLineas);
            if (model.PedidoLineas == null)
                model.PedidoLineas = new List<CompraPedidoLinea>();

            var linea = model.PedidoLineas.SingleOrDefault(f => f.Orden == orden);
            model.PedidoLineas.Remove(linea);

            foreach (var item in model.PedidoLineas.Where(f => f.Orden > orden))
            {
                item.Orden--;
            }
            
            return PartialView("_PedidoLineas", model);
        }

        public IActionResult ImprimirPedido(int Id)
        {
            var pedido = _dbContext.CompraPedidos.Include(f => f.PedidoLineas).SingleOrDefault(f => f.Id == Id);

            if (pedido == null)
                return NotFound();

            //Obtener
            var pdf = PrintService.GetBytes(HtmlParaImprimir(pedido));
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + $"_PedidoCompra_{pedido.CodigoPedido}.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        [HttpPost]
        public async Task<IActionResult> EnviarPedido(int? id, [FromBody] object request)
        {            
            if (id == null)
                return StatusCode(400, "El Id proporcionado es null");

            try
            {
                //Obtenemos los datos del pedido
                var pedido = _dbContext.CompraPedidos.Include(f => f.PedidoLineas).Single(f => f.Id == id);

                SmtpConfig smtpConfig = null;

                //Requisito funcional: 
                //26/02/2021 
                //Para el email ha de coger las credenciales que están en el congreso y si no están, las que hay en el fichero de usuarios.
                //Obtener datos de smtp del congreso y usuario activo
                int nCodigoEvento;
                if (Int32.TryParse(pedido.CodigoEvento, out nCodigoEvento))
                {
                    var congreso = await _dbContext
                        .Congresses
                        .Include(f => f.CongressEmailAccounts)
                        .FirstOrDefaultAsync(f => f.Number == nCodigoEvento);

                    CongressEmailAccounts congressEmail = null;
                    if (congreso != null)
                    {
                        string sAccountId = User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_ACCOUNT_ID))?.Value;
                        if (!string.IsNullOrWhiteSpace(sAccountId))
                        {
                            Guid guidAccountId = Guid.Parse(sAccountId);
                            congressEmail = congreso.CongressEmailAccounts.FirstOrDefault(f => f.AccountId == guidAccountId);
                        }
                    }

                    if (congressEmail != null && !string.IsNullOrWhiteSpace(congressEmail.OutgoingMailServer))
                    {
                        smtpConfig = new SmtpConfig()
                        {
                            SmtpPassword = congressEmail.MailPassword,
                            SmtpPort = congressEmail.OutgoingMailPort,
                            SmtpServer = congressEmail.OutgoingMailServer,
                            SmtpUser = congressEmail.MailUser
                        };
                    }
                }

                if (smtpConfig == null) //Obtenemos los datos de smtp del usuario activo
                    smtpConfig = UserMailHelper.GetStmpConfig(User);

                if (smtpConfig == null)
                {
                    //TempData["ErrorMessage"] = "No se ha podido enviar el mail. Su usuario no tiene establecida la configuración de envío de mails.";
                    //return RedirectToAction("Edit", new { id = id });
                    return StatusCode(400, "No existe ninguna configuración SMTP para poder enviar mails");
                }

                smtpConfig.EnableSsl = true;

                string sSmtpSendCopy = User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_SEND_COPY))?.Value;

                dynamic data = JToken.Parse(request.ToString());

                string htmlPdf = HtmlParaImprimir(pedido);
                string html = ((dynamic)request).body;
                string mails = ((dynamic)request).mails;
                string hfAttachments = ((dynamic)request).attachments;

                if (string.IsNullOrWhiteSpace(mails))
                    return StatusCode(400, "Debe indicar algún mail válido.");

                List<string> to = new List<string>();
                to.AddRange(mails.Split(";"));
                if (!string.IsNullOrWhiteSpace(sSmtpSendCopy) && Convert.ToBoolean(sSmtpSendCopy))
                {
                    string sMail = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;
                    if (!string.IsNullOrWhiteSpace(sMail))
                        to.Add(sMail);
                }

                var pdf = PrintService.GetBytes(htmlPdf);
                MemoryStream stream = new MemoryStream(pdf);
                Attachment attachment = new Attachment(stream, $"Pedido_{id}.pdf");
                List<Attachment> attachments = new List<Attachment>();
                attachments.Add(attachment);
                
                if (!string.IsNullOrWhiteSpace(hfAttachments))
                {
                    IWebHostEnvironment environment = (IWebHostEnvironment)HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
                    string[] arrAttachments = hfAttachments.Split(";");                    

                    foreach (string path in arrAttachments)
                    {
                        if (string.IsNullOrWhiteSpace(path))
                            continue;
                        attachments.Add(new Attachment(Path.Combine(environment.WebRootPath, path.Replace("~/","").Replace("/", "\\"))));
                    }
                }

                EmailService service = new EmailService(smtpConfig);
                service.Send($"Pedido Nº {id}", html, true, to.ToArray(), User.Identity.Name, attachments);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, "Error Message: " + ex.Message + Environment.NewLine + "Inner Exception: " + ex.InnerException);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EnviarPedidoAdjuntarFichero(IFormFile Fichero)
        {
            IFileUploader _fileUploader = (IFileUploader)HttpContext.RequestServices.GetService(typeof(IFileUploader));
            dynamic data = new { Path = await _fileUploader.UploadFile(Fichero), FileName = Fichero.FileName };
            return Ok(JsonConvert.SerializeObject(data));
        }

        private string HtmlParaImprimir(CompraPedido pedido)
        {
            string html = string.Empty;
            
            using (StreamReader reader = new StreamReader(Path.Combine("HtmlTemplates", "CompraPedido.html")))
            {
                html = reader.ReadToEnd();
            }

            //Logo
            html = html.Replace("#URL_LOGO#", $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/img/logo.png");

            var proveedor = _sageContext.Proveedores.Find(pedido.CodigoProveedor);
            html = html.Replace("#PROVEEDOR_NOMBRE#", proveedor.NOMBRE);
            html = html.Replace("#PROVEEDOR_CIF#", proveedor.CIF);
            html = html.Replace("#PROVEEDOR_DIRECCION#", proveedor.DIRECCION);
            html = html.Replace("#PROVEEDOR_POBLACION#", $"{proveedor.CODPOST + " "}{proveedor.POBLACION}");
            html = html.Replace("#PROVEEDOR_PROVINCIA#", proveedor.PROVINCIA);
            html = html.Replace("#CODIGO_EVENTO#", pedido.CodigoEvento);
            html = html.Replace("#NOMBRE_EVENTO#", pedido.NombreEvento);
            html = html.Replace("#ID#", pedido.CodigoPedido);
            html = html.Replace("#FECHA#", $"{pedido.Fecha.ToShortDateString()}");
            html = html.Replace("#OBSERVACIONES#", pedido.Observaciones);

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
            foreach (var item in pedido.PedidoLineas)
            {
                rows += $"<tr>" +
                    $"<td>{item.NombreArticulo}</td>" +
                    $"<td class='text-right'>{item.Unidades}</td>" +
                    $"<td class='text-right'>{item.PrecioUnitario}</td>" +
                    $"<td class='text-right'>{item.IVA_Porcentaje} %</td>" +
                    $"<td class='text-right'>{item.Total.ToString("C")}</td>" +
                    $"</tr>";
                if (!string.IsNullOrWhiteSpace(item.ObservacionesPedidoLinea))
                {
                    var sObservacionesPedidoLinea = item.ObservacionesPedidoLinea.Replace("\r\n", "<br/>");
                    rows += $"<tr><td colspan='5'>{sObservacionesPedidoLinea}</td></tr>";
                }
            }
            html = html.Replace("#ROWS#", rows);

            html = html.Replace("#TOTAL#", pedido.Total.ToString("C"));
            return html;

        }

        [HttpGet]
        public async Task<PartialViewResult> Buscador()
        {
            CompraPedidoFilter filter = new CompraPedidoFilter();
            await TryUpdateModelAsync<CompraPedidoFilter>(filter, "",
                f => f.CodigoEvento,
                f => f.CodigoProveedor,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoPedido,
                f => f.EstadosPedido);            
            
            IEnumerable<CompraPedido> list = await _dbContext.CompraPedidos
                .Where(filter.ExpressionFilter())
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();

            return PartialView("_BuscadorComprasPedido", list);
        }

        public IActionResult BorrarDocumento(int IdArchivo, int IdTabla)
        {
            _dbContext.DocumentoCompraVenta.Remove(_dbContext.DocumentoCompraVenta.Find(IdArchivo));
            _dbContext.SaveChanges();
            TempData["Message"] = "El fichero se ha borrado correctamente.";

            return RedirectToAction("Edit", new { Id = IdTabla });
        }

        [HttpPost]
        public async Task<IActionResult> EnviarDocumento(int tablaID, IFormFile fileDocumentacion)
        {
            IFileUploader _fileUploader = (IFileUploader)HttpContext.RequestServices.GetService(typeof(IFileUploader));

            DocumentoCompraVenta model = new DocumentoCompraVenta() { IdTabla = tablaID };

            model.FicheroNombre = fileDocumentacion.FileName;
            model.FicheroUrl = await _fileUploader.UploadFile(fileDocumentacion);
            model.ClaveDoc = "CP";
            try
            {
                _dbContext.DocumentoCompraVenta.Add(model);
                _dbContext.SaveChanges();
                TempData["Message"] = "El fichero se ha añadido correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }

            dynamic data = new { Path = await _fileUploader.UploadFile(fileDocumentacion), FileName = fileDocumentacion.FileName, idDoc = model.Id };

            return Ok(JsonConvert.SerializeObject(data));
        }
    }
}

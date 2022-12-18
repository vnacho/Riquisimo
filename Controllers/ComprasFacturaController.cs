using CsvHelper;
using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Exceptions;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Compras")]
    public class ComprasFacturaController : ComprasBaseController
    {
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComuContext;
        private readonly ApplicationDbContext _dbContext;
        private IWebHostEnvironment _hostEnvironment;

        public ComprasFacturaController(SageContext sageContext, SageComuContext sageComuContext, ApplicationDbContext dbContext)
        {
            _sageContext = sageContext;
            _sageComuContext = sageComuContext;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int? pag, string sort = "", string currentsort = "", bool reset = false, bool export = false)
        {
            //Guardar estado de listado con paginación, ordenación y filtros
            string url = HttpContext.Session.GetString(Consts.PRESERVE_COMPRAS_FACTURAS_URL);
            if (!string.IsNullOrWhiteSpace(url))
            {
                HttpContext.Session.Remove(Consts.PRESERVE_PERSONAL_URL);
                if (url.Contains("ComprasFactura")) //Puede venir de otra página por lo que realizamos esa comprobación
                    return Redirect(url);
            }

            if (reset)
                return RedirectToAction("Index");

            string codigoOperario = string.Empty;
            if (!UserHelper.AccesoAdmin(User))
                codigoOperario = UserHelper.CodigoOperario(User);

            //Actualizar campo "Pagada"
            //¡Atención! Se avisó al cliente que está actualización puede provocar problemas de rendimiento
            CompraFacturaManager manager = new CompraFacturaManager(_dbContext, _sageComuContext, _sageContext);
            await manager.ActualizarPagadas(codigoOperario);

            if (string.IsNullOrWhiteSpace(sort))
            {
                if (string.IsNullOrWhiteSpace(currentsort))
                    sort = "Fecha desc";
                else
                    sort = currentsort;
            }

            CompraFacturaFilter filter = new CompraFacturaFilter();
            await TryUpdateModelAsync<CompraFacturaFilter>(filter, "filter");

            filter.CodigoOperario = codigoOperario;

            Pager pager = new Pager(await _dbContext.CompraFacturas.CountAsync(filter.ExpressionFilter()), pag, 100, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<CompraFactura> list = await _dbContext.CompraFacturas.Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            CargarCombos();

            return View(list);
        }

        public async Task<FileResult> ExportCsv(string currentsort = "")
        {
            CompraFacturaFilter filter = new CompraFacturaFilter();
            await TryUpdateModelAsync<CompraFacturaFilter>(filter, "filter");

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoOperario = UserHelper.CodigoOperario(User);

            if (!UserHelper.AccesoAdmin(User))            
                filter.CodigoOperario = UserHelper.CodigoOperario(User);            

            IEnumerable<CompraFactura> list = await _dbContext.CompraFacturas
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoFacturasCompra.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<ComprasFacturaViewModelMap>();
                await csv.WriteRecordsAsync<CompraFactura>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

        public async Task<FileResult> ExportPdf(string currentsort = "")
        {
            CompraFacturaFilter filter = new CompraFacturaFilter();
            await TryUpdateModelAsync<CompraFacturaFilter>(filter, "filter");

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoOperario = UserHelper.CodigoOperario(User);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoOperario = UserHelper.CodigoOperario(User);

            IEnumerable<CompraFactura> list = await _dbContext.CompraFacturas
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string table = "<table class='w-100'><tr class='font-weight-bold'><td>Factura</td><td>Fecha</td><td>Proveedor</td><td>Operario</td><td>Evento</td><td class='text-right'>Total</td><td>Estado</td><td>Pagada</td></tr>{0}</table>";
            string rows = string.Empty;
            foreach (var item in list)
            {
                rows += $"<tr><td>{item.NumeroFactura}</td>" +
                    $"<td>{item.Fecha.ToShortDateString()}</td>" +
                    $"<td>{item.NombreProveedor}</td>" +
                    $"<td>{item.NombreOperario}</td>" +
                    $"<td>{item.NombreEvento}</td>" +
                    $"<td class='text-right text-nowrap'>{item.BaseImponible.ToString("C")}</td>" +
                    $"<td>{item.EstadoFactura}</td>" +
                    $"<td>{item.Pagada}</td>" +
                    $"</tr>";
            }

            var html = PrintService.GetHtmlTheme("Listado de facturas de compra", string.Format(table, rows));
            var pdf = PrintService.GetBytes(html);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoFacturasCompra.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        public IActionResult Create(int? idAlbaran, int? idPedido)
        {
            var url = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            if (url.EndsWith("ComprasFactura") || url.EndsWith($"ComprasFactura/") || url.Contains("Index") || url.Contains("ComprasFactura?")) //Guardar filtros y paginación de index
                HttpContext.Session.SetString(Consts.PRESERVE_COMPRAS_FACTURAS_URL, url);

            CompraFactura model;

            ViewBag.Operarios = _sageComuContext.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.CODIGO);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Codigo);

            //Calcular el IVA por defecto para este artículo
            TipoIVAManager manager = new TipoIVAManager(_sageContext);
            if (idAlbaran.HasValue)
            {
                var albaran = _dbContext.CompraAlbaranes.Include(f => f.AlbaranLineas).SingleOrDefault(f => f.Id == idAlbaran);
                if (albaran != null)
                {
                    model = new CompraFactura()
                    {
                        Fecha = DateTime.Now.Date,
                        CodigoProveedor = albaran.CodigoProveedor,
                        NombreProveedor = albaran.NombreProveedor,
                        CodigoOperario = albaran.CodigoOperario,
                        NombreOperario = albaran.NombreOperario,
                        CodigoEvento = albaran.CodigoEvento,
                        NombreEvento = albaran.NombreEvento,
                        Observaciones = albaran.Observaciones,
                        FacturaLineas = new List<CompraFacturaLinea>()
                    };
                    foreach (var linea in albaran.AlbaranLineas)
                    {   
                        var tipoiva = manager.GetIVA(linea.CodigoArticulo);                        
                        model.FacturaLineas.Add(new CompraFacturaLinea()
                        {
                            CodigoArticulo = linea.CodigoArticulo,
                            NombreArticulo = linea.NombreArticulo,
                            CodigoEvento = linea.CodigoEvento,
                            NombreEvento = linea.NombreEvento,
                            ObservacionesFacturaLinea = linea.ObservacionesAlbaranLinea,
                            Orden = linea.Orden,
                            BaseImponiblePrecioUnitario = linea.PrecioUnitario,
                            BaseImponibleTotal = linea.TotalAlbaranLinea,
                            Unidades = linea.Unidades,
                            CompraAlbaranLineaId = linea.IdAlbaranLinea,
                            CodigoAlbaran = albaran.CodigoAlbaran,
                            IVA_Porcentaje = (int)tipoiva?.IVA,
                            CodigoTipoIVA = tipoiva?.Codigo
                        });
                    }

                    return View(model);
                }
            }

            if (idPedido.HasValue)
            {
                var pedido = _dbContext.CompraPedidos.Include(f => f.PedidoLineas).SingleOrDefault(f => f.Id == idPedido);
                if (pedido != null)
                {
                    model = new CompraFactura()
                    {
                        Fecha = DateTime.Now.Date,
                        CodigoProveedor = pedido.CodigoProveedor,
                        NombreProveedor = pedido.NombreProveedor,
                        CodigoOperario = pedido.CodigoOperario,
                        NombreOperario = pedido.NombreOperario,
                        CodigoEvento = pedido.CodigoEvento,
                        NombreEvento = pedido.NombreEvento,
                        Observaciones = pedido.Observaciones,
                        FacturaLineas = new List<CompraFacturaLinea>()
                    };
                    foreach (var linea in pedido.PedidoLineas)
                    {
                        var tipoiva = manager.GetIVA(linea.CodigoArticulo);
                        model.FacturaLineas.Add(new CompraFacturaLinea()
                        {
                            CodigoArticulo = linea.CodigoArticulo,
                            NombreArticulo = linea.NombreArticulo,
                            CodigoEvento = linea.CodigoEvento,
                            NombreEvento = linea.NombreEvento,
                            ObservacionesFacturaLinea = linea.ObservacionesPedidoLinea,
                            Orden = linea.Orden,
                            BaseImponiblePrecioUnitario = linea.PrecioUnitario,
                            BaseImponibleTotal = linea.TotalPedidoLinea,
                            Unidades = linea.UnidadesPendientes,
                            CompraPedidoLineaId = linea.IdPedidoLinea,
                            CodigoPedido = pedido.CodigoPedido,
                            IVA_Porcentaje = (int)tipoiva?.IVA,
                            CodigoTipoIVA = tipoiva?.Codigo
                        });
                    }

                    if (!string.IsNullOrWhiteSpace(model.CodigoEvento))
                        ViewBag.EventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo == model.CodigoEvento)?.DisplayName;

                    if (!string.IsNullOrWhiteSpace(model.CodigoProveedor))
                        ViewBag.ProveedorNombre = _sageContext.Proveedores.SingleOrDefault(f => f.CODIGO == model.CodigoProveedor)?.NOMBRE;

                    return View(model);
                }
            }

            string codOperario = User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_CODIGO_OPERARIO))?.Value;
            model = new CompraFactura() { CodigoOperario = codOperario };
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed(IFormFile _FicheroUrl, string FicheroUrl, string FicheroNombre, string guardar)
        {
            CompraFactura model = new CompraFactura();

            var proveedor = _sageContext.Proveedores.Find(Request.Form["CodigoProveedor"]);
            model.NombreProveedor = proveedor == null ? string.Empty : proveedor.NOMBRE;
            var operario = _sageComuContext.Operarios.Find(Request.Form["CodigoOperario"]);
            model.NombreOperario = operario == null ? string.Empty : operario.NOMBRE;
            var evento = _sageContext.Almacen.Find(Request.Form["CodigoEvento"]);
            model.NombreEvento = evento == null ? string.Empty : evento.Nombre;

            model.EstadoFactura = EstadoFactura.NoTaspasadoSAGE;

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

            await TryUpdateModelAsync<CompraFactura>(model, "",
                f => f.NumeroFactura,
                f => f.Fecha,
                f => f.CodigoProveedor,
                f => f.CodigoOperario,
                f => f.CodigoEvento,
                f => f.Observaciones,
                f => f.FacturaLineas,
                f => f.TieneRetencion,
                f => f.Registro);

            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<CompraFacturaLinea>();

            foreach (var item in model.FacturaLineas)
            {
                //item.CodigoTipoIVA = 
                item.Calcular();
            }
            model.Calcular();

            //Validación de negocio            
            ComprasFacturaValidator validator = new ComprasFacturaValidator(_dbContext);
            IList<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                CompraFacturaManager manager = new CompraFacturaManager(_dbContext, _sageComuContext, _sageContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";

                //if (guardar == "continuar")
                //    return RedirectToAction("Edit", new { id = model.Id });

                
                switch  (guardar)
                {
                    case "nueva":
                        AppSettings appSettings = ((IOptions<AppSettings>)HttpContext.RequestServices.GetService(typeof(IOptions<AppSettings>))).Value;
                        IWebHostEnvironment hostingEnvironment = (IWebHostEnvironment)HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
                        var list = await manager.Traspasar(model.Id, _sageContext, appSettings, hostingEnvironment) ?? new List<ValidationResult>();
                        if (list.Any())
                            TempData["ErrorMessage"] = list.First().ErrorMessage;
                        else
                            TempData["Message"] = "El registro se ha creado y exportado correctamente.";
                        return RedirectToAction("Create");
                    case "continuar":
                        return RedirectToAction("Edit", "ComprasFactura", new { id = model.Id });
                }

                return RedirectToAction("Index");
            }

            ViewBag.Operarios = _sageComuContext.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.CODIGO);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Codigo);

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var url = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            if (url.EndsWith("ComprasFactura") || url.EndsWith($"ComprasFactura/") || url.Contains("Index") || url.Contains("ComprasFactura?")) //Guardar filtros y paginación de index
                HttpContext.Session.SetString(Consts.PRESERVE_COMPRAS_FACTURAS_URL, url);

            ViewBag.Operarios = _sageComuContext.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.CODIGO);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Codigo);
            var model = _dbContext.CompraFacturas.Include(f => f.FacturaLineas).Single(f => f.Id == id);

            model.documentos = _dbContext.DocumentoCompraVenta.Where(f => f.IdTabla == model.Id && f.ClaveDoc == "CF").ToList();

            if(model.documentos == null)
                model.documentos = new List<DocumentoCompraVenta>();

            foreach(var linea in model.FacturaLineas)
            {
                if (linea.CompraAlbaranLineaId.HasValue)
                    linea.CodigoAlbaran = _dbContext.CompraAlbaranLineas.Include(f => f.Albaran).Single(f => f.IdAlbaranLinea == linea.CompraAlbaranLineaId).Albaran.CodigoAlbaran;

                if (linea.CompraPedidoLineaId.HasValue)
                {
                    var lineaPedido = _dbContext.CompraPedidoLineas.Include(f => f.Pedido).Single(f => f.IdPedidoLinea == linea.CompraPedidoLineaId);
                    linea.CodigoPedido = lineaPedido.Pedido.CodigoPedido;
                    linea.UnidadesPendientes = lineaPedido.UnidadesPendientes + linea.Unidades;
                }
            }

            ViewBag.EjercicioSeleccionado = HttpContext.Session.GetInt32(Consts.SESSION_EJERCICIO);

            CompletarTotalesFactura(model);

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id, IFormFile _FicheroUrl, string FicheroUrl, string FicheroNombre, string guardar)
        {
            CompraFactura model = _dbContext.CompraFacturas.Find(id);

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

            await TryUpdateModelAsync<CompraFactura>(model, "",
                f => f.Id,
                f => f.NumeroFactura,
                f => f.Fecha,
                f => f.CodigoProveedor,
                f => f.CodigoOperario,
                f => f.CodigoEvento,
                f => f.Observaciones,
                f => f.FacturaLineas,
                f => f.TieneRetencion,
                f => f.Registro);

            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<CompraFacturaLinea>();

            foreach (var item in model.FacturaLineas)
            {
                item.Calcular();
            }
            model.Calcular();

            //Validación de negocio            
            ComprasFacturaValidator validator = new ComprasFacturaValidator(_dbContext);
            IList<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                CompraFacturaManager manager = new CompraFacturaManager(_dbContext, _sageComuContext, _sageContext);               
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                if (guardar == "continuar")
                    return RedirectToAction("Edit", new { id = model.Id });


                return RedirectToAction("Index");                
            }

            ViewBag.Operarios = _sageComuContext.Operarios.OrderBy(f => f.NOMBRE);
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.CODIGO);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Codigo);

            ViewBag.EjercicioSeleccionado = HttpContext.Session.GetInt32(Consts.SESSION_EJERCICIO);

            CompletarTotalesFactura(model);

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {            
            CompraFacturaManager manager = new CompraFacturaManager(_dbContext, _sageComuContext, _sageContext);

            var factura = _dbContext.CompraFacturas.Find(id);

            factura.documentos = _dbContext.DocumentoCompraVenta.Where(f => f.IdTabla == factura.Id && f.ClaveDoc == "CF").ToList();

            foreach (var f in factura.documentos)
            {
                _dbContext.DocumentoCompraVenta.Remove(f);
            }

            await manager.Delete(id);
            TempData["Message"] = "El registro se ha borrado correctamente.";
            return RedirectToAction("Index");            
        }

        public PartialViewResult AddLinea([FromQuery]string CodigoEvento)
        {
            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
            if (!string.IsNullOrWhiteSpace(CodigoEvento))
                ViewBag.LineaEventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo == CodigoEvento).DisplayName;
            return PartialView("~/Views/Shared/EditorTemplates/CompraFacturaLinea.cshtml", new CompraFacturaLinea() { Unidades = 1, CodigoArticulo = string.Empty, Orden = -1, CodigoEvento = CodigoEvento });
        }

        [HttpPost]
        public async Task<PartialViewResult> EditLinea([FromQuery]int orden)
        {
            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);

            CompraFactura model = new CompraFactura();
            await TryUpdateModelAsync<CompraFactura>(model, "",
                f => f.FacturaLineas);
            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<CompraFacturaLinea>();

            CompraFacturaLinea linea = model.FacturaLineas.Single(f => f.Orden == orden);
            
            if (!string.IsNullOrWhiteSpace(linea.CodigoArticulo))
                ViewBag.ArticuloNombre = _sageContext.Articulo.SingleOrDefault(f => f.Codigo == linea.CodigoArticulo)?.Display;
            if (!string.IsNullOrWhiteSpace(linea.CodigoEvento))
                ViewBag.LineaEventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo == linea.CodigoEvento)?.DisplayName;

            return PartialView("~/Views/Shared/EditorTemplates/CompraFacturaLinea.cshtml", linea);
        }

        [HttpPost]
        public async Task<JsonResult> SaveLinea()
        {
            CompraFacturaLinea linea = new CompraFacturaLinea();

            await TryUpdateModelAsync<CompraFacturaLinea>(linea, "linea",                
                f => f.IdFacturaLinea,
                f => f.CompraFacturaId,                
                f => f.CodigoArticulo,
                f => f.Unidades,
                f => f.CodigoEvento,
                f => f.ObservacionesFacturaLinea,
                f => f.BaseImponiblePrecioUnitario,
                f => f.BaseImponibleTotal,
                f => f.Orden,
                f => f.IdFacturaLinea,                
                f => f.CompraAlbaranLineaId,
                f => f.CodigoAlbaran,
                f => f.CompraPedidoLineaId,
                f => f.CodigoPedido,
                f => f.UnidadesPendientes,
                f => f.CodigoTipoIVA);

            if (ModelState.IsValid)
            {
                CompraFactura model = new CompraFactura();
                await TryUpdateModelAsync<CompraFactura>(model, "",
                    f => f.CodigoProveedor,
                    f => f.TieneRetencion,
                    f => f.FacturaLineas);
                if (model.FacturaLineas == null)
                    model.FacturaLineas = new List<CompraFacturaLinea>();

                linea.NombreArticulo = _sageContext.Articulo.Find(linea.CodigoArticulo).Nombre;
                linea.NombreEvento = _sageContext.Almacen.Find(linea.CodigoEvento).Nombre;
                linea.IVA_Porcentaje = (int)_sageContext.Tipo_IVA.Find(linea.CodigoTipoIVA).IVA;

                linea.Calcular();
                if (linea.Orden < 0) //Nuevo
                {                    
                    linea.Orden = model.FacturaLineas.Count();
                    model.FacturaLineas.Add(linea);
                }
                else //Editar
                {
                    var list = model.FacturaLineas.ToList();
                    list[list.FindIndex(f => f.Orden == linea.Orden)] = linea;
                    model.FacturaLineas = list;
                }

                CompletarTotalesFactura(model);

                var responseValid = new ResponseObject()
                {
                    Status = 1,
                    Data = await this.RenderPartialViewToString("_FacturaLineas", model)
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
                Data = await this.RenderPartialViewToString("EditorTemplates/CompraFacturaLinea", linea)
            };
            return Json(responseInvalid);
        }        

        [HttpPost]
        public async Task<JsonResult> SaveLineasDesdeAlbaran([FromQuery]int IdAlbaran)
        {
            var lineas = _dbContext.CompraAlbaranLineas.Include(f => f.Albaran).Where(f => f.AlbaranId == IdAlbaran);

            CompraFactura model = new CompraFactura();
            await TryUpdateModelAsync<CompraFactura>(model, "",
                f => f.CodigoProveedor,
                f => f.TieneRetencion,
                f => f.FacturaLineas);
            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<CompraFacturaLinea>();

            foreach (var item in lineas)
            {
                CompraFacturaLinea linea = new CompraFacturaLinea()
                {
                    CodigoArticulo = item.CodigoArticulo,
                    CodigoEvento = item.CodigoEvento,
                    NombreArticulo = item.NombreArticulo,
                    NombreEvento = item.NombreEvento,
                    ObservacionesFacturaLinea = item.ObservacionesAlbaranLinea,
                    Orden = model.FacturaLineas.Count(),
                    BaseImponiblePrecioUnitario = item.PrecioUnitario,
                    Unidades = item.Unidades,
                    CompraAlbaranLineaId = item.IdAlbaranLinea
                };

                //Calcular el IVA por defecto para este artículo
                TipoIVAManager manager = new TipoIVAManager(_sageContext);
                var tipoiva = manager.GetIVA(item.CodigoArticulo);
                linea.CodigoTipoIVA = tipoiva?.Codigo;
                linea.IVA_Porcentaje = tipoiva == null ? (int?)null : (int)tipoiva.IVA;

                linea.CodigoAlbaran = item.Albaran.CodigoAlbaran;

                linea.Calcular();

                model.FacturaLineas.Add(linea);
            }

            CompletarTotalesFactura(model);

            var responseValid = new ResponseObject()
            {
                Status = 1,
                Data = await this.RenderPartialViewToString("_FacturaLineas", model)
            };
            return Json(responseValid);
        }

        [HttpPost]
        public async Task<JsonResult> SaveLineasDesdePedido([FromQuery]int IdPedido)
        {
            var lineas = _dbContext.CompraPedidoLineas.Include(f => f.Pedido).Where(f => f.PedidoId == IdPedido);

            CompraFactura model = new CompraFactura();
            await TryUpdateModelAsync<CompraFactura>(model, "",
                f => f.CodigoProveedor,
                f => f.TieneRetencion,
                f => f.FacturaLineas);
            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<CompraFacturaLinea>();

            foreach (var item in lineas)
            {
                CompraFacturaLinea linea = new CompraFacturaLinea()
                {
                    CodigoArticulo = item.CodigoArticulo,
                    CodigoEvento = item.CodigoEvento,
                    NombreArticulo = item.NombreArticulo,
                    NombreEvento = item.NombreEvento,
                    ObservacionesFacturaLinea = item.ObservacionesPedidoLinea,
                    Orden = model.FacturaLineas.Count(),
                    BaseImponiblePrecioUnitario = item.PrecioUnitario,
                    Unidades = item.UnidadesPendientes,
                    UnidadesPendientes = item.UnidadesPendientes,
                    CompraPedidoLineaId = item.IdPedidoLinea,
                    CodigoPedido = item.Pedido.CodigoPedido
                };

                //Calcular el IVA por defecto para este artículo
                TipoIVAManager manager = new TipoIVAManager(_sageContext);
                var tipoiva = manager.GetIVA(item.CodigoArticulo);
                linea.CodigoTipoIVA = tipoiva?.Codigo;
                linea.IVA_Porcentaje = tipoiva == null ? (int?)null : (int)tipoiva.IVA;

                linea.Calcular();

                model.FacturaLineas.Add(linea);
            }

            CompletarTotalesFactura(model);

            var responseValid = new ResponseObject()
            {
                Status = 1,
                Data = await this.RenderPartialViewToString("_FacturaLineas", model)
            };
            return Json(responseValid);
        }

        [HttpPost]
        public async Task<PartialViewResult> DeleteLinea([FromQuery]int orden)
        {
            CompraFactura model = new CompraFactura();
            await TryUpdateModelAsync<CompraFactura>(model, "",
                f => f.CodigoProveedor,
                f => f.TieneRetencion,
                f => f.FacturaLineas);
            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<CompraFacturaLinea>();

            var linea = model.FacturaLineas.SingleOrDefault(f => f.Orden == orden);
            model.FacturaLineas.Remove(linea);

            foreach (var item in model.FacturaLineas.Where(f => f.Orden > orden))
            {
                item.Orden--;
            }

            CompletarTotalesFactura(model);

            return PartialView("_FacturaLineas", model);
        }

        [HttpPost]
        public async Task<PartialViewResult> DeleteAlbaran([FromQuery]string CodigoAlbaran)
        {
            CompraFactura model = new CompraFactura();
            await TryUpdateModelAsync<CompraFactura>(model, "",
                f => f.CodigoProveedor,
                f => f.TieneRetencion,
                f => f.FacturaLineas);
            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<CompraFacturaLinea>();

            var lineasBorrar = model.FacturaLineas.Where(f => f.CodigoAlbaran == CodigoAlbaran).ToDynamicArray();
            foreach (var linea in lineasBorrar)
            {
                int orden = linea.Orden;
                model.FacturaLineas.Remove(linea);
                foreach (var item in model.FacturaLineas.Where(f => f.Orden > orden))
                {
                    item.Orden--;
                }
            }

            CompletarTotalesFactura(model);
            return PartialView("_FacturaLineas", model);
        }

        private void CompletarTotalesFactura(CompraFactura item)
        {
            decimal totalRetencion = 0;
            item.BaseImponible = item.FacturaLineas.Sum(f => f.BaseImponibleTotal);

            if (item.TieneRetencion && !string.IsNullOrWhiteSpace(item.CodigoProveedor))
            {
                TipoRetencionManager retencionManager = new TipoRetencionManager(_sageContext);
                var retencion = retencionManager.GetByProveedor(item.CodigoProveedor);
                if (retencion != null)
                {
                    item.Retencion_Porcentaje = retencion.RETENCION;
                    totalRetencion = item.BaseImponible * item.Retencion_Porcentaje.Value / 100;
                }
            }

            item.TotalRetencion = totalRetencion;
            item.Total = item.FacturaLineas.Sum(f => f.Total) - totalRetencion;
        }

        [HttpPost]
        public async Task<PartialViewResult> RecargarPartialTotalFactura()
        {
            CompraFactura model = new CompraFactura();
            await TryUpdateModelAsync<CompraFactura>(model, "",
                f => f.CodigoProveedor,
                f => f.TieneRetencion,
                f => f.FacturaLineas);
            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<CompraFacturaLinea>();

            CompletarTotalesFactura(model);
            return PartialView("_TotalFactura", model);
        }

        public IActionResult ImprimirFactura(int Id)
        {
            var factura = _dbContext.CompraFacturas.Include(f => f.FacturaLineas).SingleOrDefault(f => f.Id == Id);

            if (factura == null)
                return NotFound();

            //Obtener
            var pdf = PrintService.GetBytes(HtmlParaImprimir(factura));
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + $"_FacturaCompra_{Id}.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        public IActionResult EnviarFactura(int id)
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
            var factura = _dbContext.CompraFacturas.Include(f => f.FacturaLineas).Single(f => f.Id == id);
            var proveedor = _sageContext.Proveedores.FirstOrDefault(f => f.CODIGO == factura.CodigoProveedor);
            string emailTo = proveedor?.EMAIL;
            if (string.IsNullOrWhiteSpace(emailTo))
            {
                TempData["ErrorMessage"] = "No se ha podido enviar el mail. El proveedor seleccionado no tiene dirección de mail configurado.";
                return RedirectToAction("Edit", new { id = id });
            }

            string html = HtmlParaImprimir(factura);
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
            Attachment attachment = new Attachment(stream, $"Factura_{id}.pdf");
            List<Attachment> attachments = new List<Attachment>();
            attachments.Add(attachment);

            EmailService service = new EmailService(smtpConfig);
            service.Send($"Factura Nº {id}", html, true, to.ToArray(), User.Identity.Name, attachments);

            TempData["Message"] = "El mensaje se ha enviado correctamente.";
            return RedirectToAction("Edit", new { id = id });
        }

        private string HtmlParaImprimir(CompraFactura factura)
        {
            string html = string.Empty;

            using (StreamReader reader = new StreamReader(Path.Combine("HtmlTemplates", "CompraFactura.html")))
            {
                html = reader.ReadToEnd();
            }

            //Logo
            html = html.Replace("#URL_LOGO", $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/img/logo.png");

            var proveedor = _sageContext.Proveedores.Find(factura.CodigoProveedor);
            html = html.Replace("#PROVEEDOR_NOMBRE", proveedor.NOMBRE);
            html = html.Replace("#PROVEEDOR_CIF", proveedor.CIF);
            html = html.Replace("#PROVEEDOR_DIRECCION", proveedor.DIRECCION);
            html = html.Replace("#PROVEEDOR_POBLACION", $"{proveedor.CODPOST + " "}{proveedor.POBLACION}");
            html = html.Replace("#PROVEEDOR_PROVINCIA", proveedor.PROVINCIA);
            html = html.Replace("#NUMERO_EVENTO", factura.CodigoEvento);
            html = html.Replace("#NOMBRE_EVENTO", factura.NombreEvento);
            html = html.Replace("#ID", factura.Id.ToString());
            html = html.Replace("#FECHA", $"{factura.Fecha.ToShortDateString()}");

            string rows = string.Empty;
            foreach (var item in factura.FacturaLineas)
            {
                rows += $"<tr><td>{item.NombreArticulo}</td><td class='text-right'>{item.Unidades}</td><td class='text-right'>{item.BaseImponiblePrecioUnitario}</td><td class='text-right'>{item.BaseImponibleTotal.ToString("C")}</td></tr>";
            }
            html = html.Replace("#ROWS", rows);

            html = html.Replace("#TOTAL", factura.BaseImponible.ToString("C"));
            return html;

        }

        public async Task<IActionResult> Traspasar(int id, string guardar)
        {
            AppSettings appSettings = ((IOptions<AppSettings>)HttpContext.RequestServices.GetService(typeof(IOptions<AppSettings>))).Value;
            IWebHostEnvironment hostingEnvironment = (IWebHostEnvironment)HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));

            CompraFacturaManager manager = new CompraFacturaManager(_dbContext, _sageComuContext, _sageContext);
            var list = await manager.Traspasar(id, _sageContext, appSettings, hostingEnvironment) ?? new List<ValidationResult>();
            if (list.Any())
            {
                TempData["ErrorMessage"] = list.First().ErrorMessage;
                return RedirectToAction("Edit", new { id = id });
            }
            else
            {
                TempData["Message"] = "El registro se ha exportado correctamente.";
                if (guardar == "nueva")
                    return RedirectToAction("Create");

                return RedirectToAction("Edit", new { id = id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DescargarDocumentacion(int id)
        {
            var errores = 0;

            
            CompraFacturaFilter filter = new CompraFacturaFilter();
            await TryUpdateModelAsync<CompraFacturaFilter>(filter, "filter");

            IEnumerable<CompraFactura> list = await _dbContext.CompraFacturas.Where(filter.ExpressionFilter())
                                                        .ToListAsync();

            using (var compressedFileStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, true))
                {

                    foreach (CompraFactura factura in list)
                    {
                        factura.documentos = _dbContext.DocumentoCompraVenta.Where(f => f.IdTabla == factura.Id && f.ClaveDoc == "CF").ToList();
                        foreach (DocumentoCompraVenta doc in factura.documentos)
                        {
                            string anomesdia = NormalizarNombreArchivo(factura.Fecha.ToString("yyyyMMdd")).Trim();
                            string numero = NormalizarNombreArchivo(factura.NumeroFactura.ToString()).Trim();
                            string nombre = NormalizarNombreArchivo(factura.NombreProveedor.ToString()).Trim();
                            string nombre_archivo  = NormalizarNombreArchivo(doc.FicheroNombre.ToString()).Trim();
                            string nombrePdf = $"{anomesdia}-{numero}-{nombre}-{nombre_archivo}";
                            string extension = ".pdf";
                            string PathFichero = Path.Combine(doc.FicheroUrl.ToString().Replace("~", "wwwroot"));

                            if (string.IsNullOrEmpty(PathFichero))
                            {
                                Console.WriteLine("No contiene ficheros");
                            }

                            MemoryStream fichero = null;
                        
                            try
                            {
                                fichero = new MemoryStream(System.IO.File.ReadAllBytes(PathFichero));
                            }
                            catch (Exception ex)
                            {
                                errores++;

                                fichero = new MemoryStream(Encoding.ASCII.GetBytes("FALTA EL DOCUMETO DE LA FACTURA ORIGINAL"));
                                extension = "-FALTA DOC.txt";

                                Console.WriteLine(ex.Message); 
                            }

                            var zipEntry = zipArchive.CreateEntry(nombrePdf+extension);
                            using (var originalFileStream = fichero)
                            using (var zipEntryStream = zipEntry.Open())
                            {
                                originalFileStream.CopyTo(zipEntryStream);
                            }
                        }
                    }
                }

                var resultado = "";
                if (errores > 0)
                    resultado = "ERROR: Faltan "+ errores +" ARCHIVO/S, se les ha puesto el sufijo -FALTA DOC, revise los ficheros en el archivo comprimido.";
                else
                    resultado = "El archivo comprimido se ha creado correctament.";

                return File(compressedFileStream.ToArray(), "application/zip", resultado);
            }
        }

        private void CargarCombos()
        {
            ViewBag.Proveedores = _sageContext.Proveedores.OrderBy(f => f.NOMBRE);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
        }

        private string NormalizarNombreArchivo(string Cadena)
        {   // Carácteres NO válidos \ / : * ? " < > |
            string resultado = "";
            resultado = Cadena;
            resultado = resultado.Replace("\\", "");
            resultado = resultado.Replace("/", "");
            resultado = resultado.Replace(":", "");
            resultado = resultado.Replace("*", "");
            resultado = resultado.Replace("?", "");
            resultado = resultado.Replace("\"", "");
            resultado = resultado.Replace("<", "");
            resultado = resultado.Replace(">", "");
            resultado = resultado.Replace("|", "");

            return resultado;
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

            DocumentoCompraVenta model = new DocumentoCompraVenta() { IdTabla = tablaID};

            model.FicheroNombre = fileDocumentacion.FileName;
            model.FicheroUrl = await _fileUploader.UploadFile(fileDocumentacion);
            model.ClaveDoc = "CF"; 
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

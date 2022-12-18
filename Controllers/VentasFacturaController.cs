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
using Ferpuser.Models.Dtos;
using Ferpuser.Models.Enums;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
    [Authorize(Policy = "Ventas")]
    public class VentasFacturaController : VentasBaseController
    {
        private readonly SageContext sageContext;
        private readonly SageComuContext sageComuContext;
        private readonly ApplicationDbContext dbContext;
        private IOptions<AppSettings> appSettings;
        //private VentasFacturaPrintHelper _printHelper;
        private VentaFacturaPrint printHelper;
        private SerieManager serieManager;

        public VentasFacturaController(
            SageContext sageContext, 
            SageComuContext sageComuContext, 
            ApplicationDbContext dbContext, 
            IOptions<AppSettings> appSettings,
            VentaFacturaPrint printHelper,
            SerieManager serieManager)
        {
            this.sageContext = sageContext;
            this.sageComuContext = sageComuContext;
            this.dbContext = dbContext;
            this.appSettings = appSettings;
            this.printHelper = printHelper;
            this.serieManager = serieManager;
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

            VentaFacturaFilter filter = new VentaFacturaFilter();
            await TryUpdateModelAsync<VentaFacturaFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoCliente,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoFactura,
                f => f.Pagada);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoVendedor = UserHelper.CodigoVendedor(User);

            Pager pager = new Pager(await dbContext.VentaFacturas.CountAsync(filter.ExpressionFilter()), pag, 100, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<VentaFactura> list = await dbContext.VentaFacturas.Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .ThenByDescending(f => f.Id)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            var listClientes = new List<Select2Response>();
            if (!string.IsNullOrWhiteSpace(filter.CodigoCliente))
            {
                var cliente = sageContext.Clientes.Find(filter.CodigoCliente);
                listClientes.Add(new Select2Response() { id = cliente.Codigo, text = cliente.DisplayName });
            }
            ViewBag.Clientes = listClientes;
            //ViewBag.Clientes = _sageContext.Clientes.OrderBy(f => f.Nombre);
            ViewBag.Eventos = sageContext.Almacen.OrderBy(f => f.Nombre);
            
            return View(list);
        }

        public async Task<FileResult> ExportCsv(string currentsort = "")
        {
            VentaFacturaFilter filter = new VentaFacturaFilter();
            await TryUpdateModelAsync<VentaFacturaFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoCliente,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoFactura,
                f => f.Pagada);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoVendedor = UserHelper.CodigoVendedor(User);

            IEnumerable<VentaFactura> list = await dbContext.VentaFacturas
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoFacturasVenta.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<VentasFacturaViewModelMap>();
                await csv.WriteRecordsAsync<VentaFactura>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

        public async Task<FileResult> ExportPdf(string currentsort = "")
        {
            VentaFacturaFilter filter = new VentaFacturaFilter();
            await TryUpdateModelAsync<VentaFacturaFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoCliente,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoFactura,
                f => f.Pagada);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoVendedor = UserHelper.CodigoVendedor(User);

            IEnumerable<VentaFactura> list = await dbContext.VentaFacturas
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string table = "<table class='w-100'><tr class='font-weight-bold'><td>Factura</td><td>Fecha</td><td>Cliente</td><td>Vendedor</td><td>Evento</td><td class='text-right'>Total</td><td>Estado</td><td>Pagada</td></tr>{0}</table>";
            string rows = string.Empty;
            foreach (var item in list)
            {
                rows += $"<tr><td>{item.CodigoFactura}</td>" +
                    $"<td>{item.Fecha.ToShortDateString()}</td>" +
                    $"<td>{item.NombreCliente}</td>" +
                    $"<td>{item.NombreVendedor}</td>" +
                    $"<td>{item.NombreEvento}</td>" +
                    $"<td class='text-right text-nowrap'>{item.BaseImponible.ToString("C")}</td>" +
                    $"<td>{item.EstadoFactura}</td>" +
                    $"<td>{item.Pagada}</td>" +
                    $"</tr>";
            }

            var html = PrintService.GetHtmlTheme("Listado de facturas de compra", string.Format(table, rows));
            var pdf = PrintService.GetBytes(html);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoFacturasVenta.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        public IActionResult Create(int? idAlbaran, int? idPedido, int? IdFacturaDuplicado)
        {
            VentaFactura model;

            ViewBag.Vendedores = sageContext.Vendedor.OrderBy(f => f.Nombre);           
            ViewBag.Clientes = new List<Select2Response>();
            //ViewBag.Clientes = sageContext.Clientes.OrderBy(f => f.Codigo);
            ViewBag.Eventos = sageContext.Almacen.OrderBy(f => f.Codigo);
            ViewBag.Series = serieManager.Get(Consts.CODIGO_EMPRESA, Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_FACTURA);
            ViewBag.Articulos = sageContext.Articulo.OrderBy(f => f.Codigo);

            if (idAlbaran.HasValue)
            {
                var albaran = dbContext.VentaAlbaranes.Include(f => f.AlbaranLineas).SingleOrDefault(f => f.Id == idAlbaran);
                if (albaran != null)
                {
                    model = new VentaFactura()
                    {
                        Fecha = DateTime.Now.Date,
                        CodigoCliente = albaran.CodigoCliente,
                        NombreCliente = albaran.NombreCliente,
                        CodigoVendedor = albaran.CodigoVendedor,
                        NombreVendedor = albaran.NombreVendedor,
                        CodigoEvento = albaran.CodigoEvento,
                        NombreEvento = albaran.NombreEvento,
                        Observaciones = albaran.Observaciones,
                        LineaEnvCli = albaran.LineaEnvCli,
                        Direccion = albaran.Direccion,
                        CodigoPostal = albaran.CodigoPostal,
                        Poblacion = albaran.Poblacion,
                        Provincia = albaran.Provincia,
                        FacturaLineas = new List<VentaFacturaLinea>()
                    };
                    foreach (var linea in albaran.AlbaranLineas)
                    {
                        VentaFacturaLinea addLinea = new VentaFacturaLinea()
                        {
                            CodigoArticulo = linea.CodigoArticulo,
                            NombreArticulo = linea.NombreArticulo,
                            CodigoEvento = linea.CodigoEvento,
                            NombreEvento = linea.NombreEvento,
                            ObservacionesFacturaLinea = linea.ObservacionesAlbaranLinea,
                            Orden = linea.Orden,
                            BaseImponiblePrecioUnitario = linea.PrecioUnitario,
                            Descuento = linea.Descuento,
                            ImporteDescuento = linea.ImporteDescuento,
                            DescripcionAmpliada = linea.DescripcionAmpliada,
                            TextoDescripcionAmpliada = linea.TextoDescripcionAmpliada,
                            BaseImponibleTotal = linea.TotalAlbaranLinea,
                            Unidades = linea.Unidades,
                            VentaAlbaranLineaId = linea.IdAlbaranLinea,
                            CodigoAlbaranDisplay = albaran.CodigoDisplay,
                            IVA_Porcentaje = linea.IVA_Porcentaje,
                            CodigoTipoIVA = linea.CodigoTipoIVA,
                            TieneTiempo = linea.TieneTiempo,
                            Tiempo = linea.Tiempo
                        };
                        addLinea.Calcular();
                        model.FacturaLineas.Add(addLinea);
                    }
                    model.Calcular();
                    return View(model);
                }
            }

            if (idPedido.HasValue)
            {
                var pedido = dbContext.VentaPedidos.Include(f => f.PedidoLineas).SingleOrDefault(f => f.Id == idPedido);
                if (pedido != null)
                {
                    model = new VentaFactura()
                    {
                        Fecha = DateTime.Now.Date,
                        CodigoCliente = pedido.CodigoCliente,
                        NombreCliente = pedido.NombreCliente,
                        CodigoVendedor = pedido.CodigoVendedor,
                        NombreVendedor = pedido.NombreVendedor,
                        CodigoEvento = pedido.CodigoEvento,
                        NombreEvento = pedido.NombreEvento,
                        Observaciones = pedido.Observaciones,
                        LineaEnvCli = pedido.LineaEnvCli,
                        Direccion = pedido.Direccion,
                        CodigoPostal = pedido.CodigoPostal,
                        Poblacion = pedido.Poblacion,
                        Provincia = pedido.Provincia,
                        FacturaLineas = new List<VentaFacturaLinea>()
                    };
                    foreach (var linea in pedido.PedidoLineas)
                    {
                        VentaFacturaLinea addLinea = new VentaFacturaLinea()
                        {
                            CodigoArticulo = linea.CodigoArticulo,
                            NombreArticulo = linea.NombreArticulo,
                            CodigoEvento = linea.CodigoEvento,
                            NombreEvento = linea.NombreEvento,
                            ObservacionesFacturaLinea = linea.ObservacionesPedidoLinea,
                            Orden = linea.Orden,
                            BaseImponiblePrecioUnitario = linea.PrecioUnitario,
                            Descuento = linea.Descuento,
                            ImporteDescuento = linea.ImporteDescuento,
                            DescripcionAmpliada = linea.DescripcionAmpliada,
                            TextoDescripcionAmpliada = linea.TextoDescripcionAmpliada,
                            BaseImponibleTotal = linea.TotalPedidoLinea,
                            Unidades = linea.UnidadesPendientes,
                            VentaPedidoLineaId = linea.IdPedidoLinea,
                            IVA_Porcentaje = linea.IVA_Porcentaje,
                            CodigoTipoIVA = linea.CodigoTipoIVA,
                            CodigoPedidoDisplay = pedido.CodigoDisplay,
                            TieneTiempo = linea.TieneTiempo,
                            Tiempo = linea.Tiempo
                        };
                        addLinea.Calcular();
                        model.FacturaLineas.Add(addLinea);
                    }
                    model.Calcular();
                    return View(model);
                }
            }

            string codVendedor = UserHelper.CodigoVendedor(User);
            if (IdFacturaDuplicado.HasValue)
            {
                model = dbContext.VentaFacturas.Include(f => f.FacturaLineas).First(f => f.Id == IdFacturaDuplicado);
                model.Id = 0;
                model.EstadoFactura = EstadoFactura.NoTaspasadoSAGE;
                model.Fecha = DateTime.Now.Date;
                model.CodigoVendedor = codVendedor;
                model.Serie = null;
                foreach (var linea in model.FacturaLineas)
                {
                    linea.IdFacturaLinea = 0;
                    linea.UnidadesPendientes = linea.Unidades;
                    linea.VentaPedidoLineaId = null;
                    linea.VentaAlbaranLineaId = null;
                }
            }
            else
            {
                model = new VentaFactura() { CodigoVendedor = codVendedor };
            }
            return View(model);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed(IFormFile _FicheroUrl, string FicheroUrl, string FicheroNombre, string guardar)
        {
            VentaFactura model = new VentaFactura();

            var cliente = sageContext.Clientes.Find(Request.Form["CodigoCliente"]);
            model.NombreCliente = cliente == null ? string.Empty : cliente.Nombre;
            var vendedor = sageContext.Vendedor.Find(Request.Form["CodigoVendedor"]);
            model.NombreVendedor = vendedor == null ? string.Empty : vendedor.Nombre;
            var evento = sageContext.Almacen.Find(Request.Form["CodigoEvento"]);
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

            await TryUpdateModelAsync<VentaFactura>(model, "",
                f => f.Serie,
                f => f.Fecha,
                f => f.CodigoCliente,
                f => f.CodigoVendedor,
                f => f.CodigoEvento,
                f => f.Observaciones,
                f => f.FacturaLineas,
                f => f.TieneRetencion,
                f => f.Origen,
                f => f.OrigenCodigoArticulo,
                f => f.OrigenNombreArticulo,
                f => f.OrigenImporte,
                f => f.LineaEnvCli,
                f => f.Direccion,
                f => f.CodigoPostal,
                f => f.Poblacion,
                f => f.Provincia,
                f => f.IncluirDatosEvento);

            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<VentaFacturaLinea>();

            foreach (var item in model.FacturaLineas)
            {
                item.Calcular();
            }

            //Aplicar la retención si la hubiera
            TipoRetencionManager retencionManager = new TipoRetencionManager(sageContext);
            retencionManager.RellenarDatosRetencion(model);
            model.Calcular();

            //Validación de negocio            
            VentasFacturaValidator validator = new VentasFacturaValidator(dbContext);
            IList<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                VentaFacturaManager manager = new VentaFacturaManager(dbContext, sageComuContext, sageContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";

                switch (guardar)
                {
                    case "nueva":
                        AppSettings appSettings = ((IOptions<AppSettings>)HttpContext.RequestServices.GetService(typeof(IOptions<AppSettings>))).Value;
                        IWebHostEnvironment hostingEnvironment = (IWebHostEnvironment)HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
                        var list = await manager.Traspasar(model.Id, appSettings, hostingEnvironment) ?? new List<ValidationResult>();
                        if (list.Any())
                            TempData["ErrorMessage"] = list.First().ErrorMessage;
                        else
                            TempData["Message"] = "El registro se ha creado y exportado correctamente.";
                        return RedirectToAction("Create");
                    case "continuar":
                        return RedirectToAction("Edit", "VentasFactura", new { id = model.Id });
                }


                return RedirectToAction("Index");
            }

            ViewBag.Vendedores = sageContext.Vendedor.OrderBy(f => f.Nombre);

            var listClientes = new List<Select2Response>();
            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                listClientes.Add(new Select2Response() { id = cliente.Codigo, text = cliente.DisplayName });
            ViewBag.Clientes = listClientes;

            //ViewBag.Clientes = sageContext.Clientes.OrderBy(f => f.Codigo);
            ViewBag.Eventos = sageContext.Almacen.OrderBy(f => f.Codigo);
            ViewBag.Series = serieManager.Get(Consts.CODIGO_EMPRESA, Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_FACTURA);
            ViewBag.Articulos = sageContext.Articulo.OrderBy(f => f.Codigo);

            if (!string.IsNullOrWhiteSpace(model.CodigoEvento))
                ViewBag.EventoNombre = sageContext.Almacen.SingleOrDefault(f => f.Codigo == model.CodigoEvento)?.DisplayName;

            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                ViewBag.ClienteNombre = sageContext.Clientes.SingleOrDefault(f => f.Codigo == model.CodigoCliente)?.Nombre;

            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
            {
                var direcciones = sageContext.Env_Cli.Where(f => f.Cliente.Trim() == model.CodigoCliente).ToList();
                ViewBag.Direcciones = direcciones;
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Vendedores = sageContext.Vendedor.OrderBy(f => f.Nombre);
            //ViewBag.Clientes = sageContext.Clientes.OrderBy(f => f.Codigo);
            ViewBag.Eventos = sageContext.Almacen.OrderBy(f => f.Codigo);
            ViewBag.Articulos = sageContext.Articulo.OrderBy(f => f.Codigo);

            var model = dbContext.VentaFacturas.Include(f => f.FacturaLineas).Single(f => f.Id == id);


            model.documentos = dbContext.DocumentoCompraVenta.Where(f => f.IdTabla == model.Id && f.ClaveDoc == "VF").ToList();

            if (model.documentos == null)
                model.documentos = new List<DocumentoCompraVenta>();

            var listClientes = new List<Select2Response>();
            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
            {
                var cliente = sageContext.Clientes.Find(model.CodigoCliente);
                listClientes.Add(new Select2Response() { id = cliente.Codigo, text = cliente.DisplayName });
            }
            ViewBag.Clientes = listClientes;

            model.Calcular();


            foreach (var linea in model.FacturaLineas)
            {
                if (linea.VentaAlbaranLineaId.HasValue)
                    linea.CodigoAlbaranDisplay = dbContext.VentaAlbaranLineas.Include(f => f.Albaran).Single(f => f.IdAlbaranLinea == linea.VentaAlbaranLineaId).Albaran.CodigoDisplay;

                if (linea.VentaPedidoLineaId.HasValue)
                {
                    var lineaPedido = dbContext.VentaPedidoLineas.Include(f => f.Pedido).Single(f => f.IdPedidoLinea == linea.VentaPedidoLineaId);
                    linea.UnidadesPendientes = lineaPedido.UnidadesPendientes + linea.Unidades;
                }
            }

            if (!string.IsNullOrWhiteSpace(model.CodigoEvento))
            {
                ViewBag.EventoNombre = sageContext.Almacen.SingleOrDefault(f => f.Codigo.Trim() == model.CodigoEvento.Trim())?.DisplayName;
                ViewBag.Clave = dbContext.Congresses.SingleOrDefault(f => !f.Deleted.HasValue && f.Number == Int32.Parse(model.CodigoEvento.Trim()))?.Code;
            }

            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                ViewBag.ClienteNombre = sageContext.Clientes.SingleOrDefault(f => f.Codigo.Trim() == model.CodigoCliente.Trim())?.Nombre;

            ViewBag.EjercicioSeleccionado = HttpContext.Session.GetInt32(Consts.SESSION_EJERCICIO);
            ViewBag.EmailTo = sageContext.Clientes.FirstOrDefault(f => f.Codigo.Trim() == model.CodigoCliente.Trim())?.Email;

            //Si una factura o pedido lleva aunque sea solamente una línea con el check de TIEMPO activado, su formato de impresión será obligatoriamente el FORMATO C
            if (model.Origen)
                ViewBag.FormatoImpresionForzado = FormatoImpresion.D;
            else if (model.FacturaLineas.Any(f => f.TieneTiempo))
                ViewBag.FormatoImpresionForzado = FormatoImpresion.C;
            else
            {
                CongressesManager congressManager = new CongressesManager(dbContext);
                if (!congressManager.PermiteImpresionFormatoA(model.CodigoEvento))
                    ViewBag.FormatoImpresionForzado = FormatoImpresion.B;
            }

            var direcciones = sageContext.Env_Cli.Where(f => f.Cliente.Trim() == model.CodigoCliente).ToList();
            ViewBag.Direcciones = direcciones;

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id, IFormFile _FicheroUrl, string FicheroUrl, string FicheroNombre, string guardar)
        {
            VentaFactura model = dbContext.VentaFacturas.Find(id);

            var cliente = sageContext.Clientes.Find(Request.Form["CodigoCliente"]);
            model.NombreCliente = cliente == null ? string.Empty : cliente.Nombre;
            var vendedor = sageContext.Vendedor.Find(Request.Form["CodigoVendedor"]);
            model.NombreVendedor = vendedor == null ? string.Empty : vendedor.Nombre;
            var evento = sageContext.Almacen.Find(Request.Form["CodigoEvento"]);
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

            await TryUpdateModelAsync<VentaFactura>(model, "",
                f => f.Id,
                f => f.CodigoFactura,
                f => f.Fecha,
                f => f.CodigoCliente,
                f => f.CodigoVendedor,
                f => f.CodigoEvento,
                f => f.Observaciones,
                f => f.FacturaLineas,
                f => f.TieneRetencion,
                f => f.Origen,
                f => f.OrigenCodigoArticulo,
                f => f.OrigenNombreArticulo,
                f => f.OrigenImporte,
                f => f.LineaEnvCli,
                f => f.Direccion,
                f => f.CodigoPostal,
                f => f.Poblacion,
                f => f.Provincia,
                f => f.IncluirDatosEvento);

            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<VentaFacturaLinea>();

            foreach (var item in model.FacturaLineas)
            {
                item.Calcular();
            }
            //Aplicar la retención si la hubiera
            TipoRetencionManager retencionManager = new TipoRetencionManager(sageContext);
            retencionManager.RellenarDatosRetencion(model);
            model.Calcular();

            //Validación de negocio            
            VentasFacturaValidator validator = new VentasFacturaValidator(dbContext);
            IList<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                VentaFacturaManager manager = new VentaFacturaManager(dbContext, sageComuContext, sageContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                if (guardar == "continuar")
                    return RedirectToAction("Edit", new { id = model.Id });
                return RedirectToAction("Index");
            }

            ViewBag.Vendedores = sageContext.Vendedor.OrderBy(f => f.Nombre);

            var listClientes = new List<Select2Response>();
            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))                
                listClientes.Add(new Select2Response() { id = cliente.Codigo, text = cliente.DisplayName });
            ViewBag.Clientes = listClientes;

            //ViewBag.Clientes = sageContext.Clientes.OrderBy(f => f.Codigo);
            ViewBag.Eventos = sageContext.Almacen.OrderBy(f => f.Codigo);
            ViewBag.Articulos = sageContext.Articulo.OrderBy(f => f.Codigo);

            if (!string.IsNullOrWhiteSpace(model.CodigoEvento))
                ViewBag.EventoNombre = sageContext.Almacen.SingleOrDefault(f => f.Codigo.Trim() == model.CodigoEvento.Trim())?.DisplayName;

            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                ViewBag.ClienteNombre = sageContext.Clientes.SingleOrDefault(f => f.Codigo.Trim() == model.CodigoCliente.Trim())?.Nombre;

            ViewBag.EjercicioSeleccionado = HttpContext.Session.GetInt32(Consts.SESSION_EJERCICIO);
            ViewBag.EmailTo = sageContext.Clientes.FirstOrDefault(f => f.Codigo.Trim() == model.CodigoCliente.Trim())?.Email;

            //Si una factura o pedido lleva aunque sea solamente una línea con el check de TIEMPO activado, su formato de impresión será obligatoriamente el FORMATO C
            if (model.Origen)
                ViewBag.FormatoImpresionForzado = FormatoImpresion.D;
            else if (model.FacturaLineas.Any(f => f.TieneTiempo))
                ViewBag.FormatoImpresionForzado = FormatoImpresion.C;
            else
            {
                CongressesManager congressManager = new CongressesManager(dbContext);
                if (!congressManager.PermiteImpresionFormatoA(model.CodigoEvento))
                    ViewBag.FormatoImpresionForzado = FormatoImpresion.B;
            }

            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
            {
                var direcciones = sageContext.Env_Cli.Where(f => f.Cliente.Trim() == model.CodigoCliente).ToList();
                ViewBag.Direcciones = direcciones;
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            VentaFacturaManager manager = new VentaFacturaManager(dbContext, sageComuContext, sageContext);

            var factura = dbContext.VentaFacturas.Find(id);

            factura.documentos = dbContext.DocumentoCompraVenta.Where(f => f.IdTabla == factura.Id && f.ClaveDoc == "VF").ToList();

            foreach (var f in factura.documentos)
            {
                dbContext.DocumentoCompraVenta.Remove(f);
            }

            await manager.Delete(id);
            TempData["Message"] = "El registro se ha borrado correctamente.";
            return RedirectToAction("Index");
        }

        public PartialViewResult AddLinea([FromQuery] string CodigoEvento)
        {
            ViewBag.TiposIVA = sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
            ViewBag.Eventos = sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Articulos = sageContext.Articulo.OrderBy(f => f.Codigo);
            return PartialView("~/Views/Shared/EditorTemplates/VentaFacturaLinea.cshtml", new VentaFacturaLinea() { Unidades = 1, CodigoArticulo = string.Empty, Orden = -1, CodigoEvento = CodigoEvento });
        }

        [HttpPost]
        public async Task<PartialViewResult> EditLinea([FromQuery] int orden)
        {
            ViewBag.TiposIVA = sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
            ViewBag.Eventos = sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Articulos = sageContext.Articulo.OrderBy(f => f.Codigo);

            VentaFactura model = new VentaFactura();
            await TryUpdateModelAsync<VentaFactura>(model, "",
                f => f.FacturaLineas);
            
            VentaFacturaLinea linea = model.FacturaLineas.Single(f => f.Orden == orden);
            return PartialView("~/Views/Shared/EditorTemplates/VentaFacturaLinea.cshtml", linea);
        }

        [HttpPost]
        public async Task<JsonResult> SaveLinea()
        {
            VentaFacturaLinea linea = new VentaFacturaLinea();
            try
            {
                await TryUpdateModelAsync<VentaFacturaLinea>(linea, "linea",
                    f => f.IdFacturaLinea,
                    f => f.VentaFacturaId,
                    f => f.CodigoArticulo,
                    f => f.NombreArticulo,
                    f => f.Unidades,
                    f => f.CodigoEvento,
                    f => f.ObservacionesFacturaLinea,
                    f => f.BaseImponiblePrecioUnitario,
                    f => f.Descuento,
                    f => f.ImporteDescuento,
                    f => f.DescripcionAmpliada,
                    f => f.TextoDescripcionAmpliada,
                    f => f.BaseImponibleTotal,
                    f => f.Orden,
                    f => f.IdFacturaLinea,
                    f => f.VentaAlbaranLineaId,
                    f => f.CodigoAlbaranDisplay,
                    f => f.VentaPedidoLineaId,
                    f => f.CodigoPedidoDisplay,
                    f => f.UnidadesPendientes,
                    f => f.CodigoTipoIVA,
                    f => f.TieneTiempo,
                    f => f.Tiempo
                );

                if (ModelState.IsValid)
                {
                    VentaFactura model = new VentaFactura();
                    await TryUpdateModelAsync<VentaFactura>(model, "",
                        f => f.Serie,
                        f => f.CodigoFactura,
                        f => f.CodigoCliente,
                        f => f.TieneRetencion,
                        f => f.Origen,
                        f => f.OrigenCodigoArticulo,
                        f => f.OrigenNombreArticulo,
                        f => f.OrigenImporte,
                        f => f.FacturaLineas);

                    if (model.FacturaLineas == null)
                        model.FacturaLineas = new List<VentaFacturaLinea>();

                    linea.NombreEvento = sageContext.Almacen.Find(linea.CodigoEvento).Nombre;
                    linea.IVA_Porcentaje = (int)sageContext.Tipo_IVA.Find(linea.CodigoTipoIVA).IVA;

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

                    //Aplicar la retención si la hubiera
                    TipoRetencionManager retencionManager = new TipoRetencionManager(sageContext);
                    retencionManager.RellenarDatosRetencion(model);
                    model.Calcular();

                    var responseValid = new ResponseObject()
                    {
                        Status = 1,
                        Data = await this.RenderPartialViewToString("_FacturaLineas", model)
                    };
                    return Json(responseValid);
                }

                ViewBag.TiposIVA = sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
                ViewBag.Eventos = sageContext.Almacen.OrderBy(f => f.Nombre);
                ViewBag.Articulos = sageContext.Articulo.OrderBy(f => f.Codigo);
                var responseInvalid = new ResponseObject()
                {
                    Status = 0,
                    Data = await this.RenderPartialViewToString("EditorTemplates/VentaFacturaLinea", linea)
                };
                return Json(responseInvalid);
            }
            catch (Exception ex)
            {
                var responseInvalid = new ResponseObject()
                {
                    Status = 0,
                    Data = await this.RenderPartialViewToString("EditorTemplates/VentaFacturaLinea", linea)
                };
                return Json(responseInvalid);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveLineasDesdeAlbaran([FromQuery] int IdAlbaran)
        {
            var lineas = dbContext.VentaAlbaranLineas.Include(f => f.Albaran).Where(f => f.AlbaranId == IdAlbaran);

            VentaFactura model = new VentaFactura();
            await TryUpdateModelAsync<VentaFactura>(model, "",
                f => f.CodigoCliente,
                f => f.TieneRetencion,
                f => f.Origen,
                f => f.OrigenCodigoArticulo,
                f => f.OrigenNombreArticulo,
                f => f.OrigenImporte,
                f => f.FacturaLineas);
            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<VentaFacturaLinea>();

            foreach (var item in lineas)
            {
                VentaFacturaLinea linea = new VentaFacturaLinea()
                {
                    CodigoArticulo = item.CodigoArticulo,
                    NombreArticulo = item.NombreArticulo,
                    CodigoEvento = item.CodigoEvento,                    
                    NombreEvento = item.NombreEvento,
                    ObservacionesFacturaLinea = item.ObservacionesAlbaranLinea,
                    Orden = model.FacturaLineas.Count(),
                    BaseImponiblePrecioUnitario = item.PrecioUnitario,
                    Descuento = item.Descuento,
                    DescripcionAmpliada = item.DescripcionAmpliada,
                    TextoDescripcionAmpliada = item.TextoDescripcionAmpliada,
                    Unidades = item.Unidades,
                    VentaAlbaranLineaId = item.IdAlbaranLinea,
                    CodigoAlbaranDisplay = item.Albaran.CodigoDisplay,
                    CodigoTipoIVA = item.CodigoTipoIVA,
                    IVA_Porcentaje = item.IVA_Porcentaje,
                    TieneTiempo = item.TieneTiempo,
                    Tiempo = item.Tiempo
                };
                linea.Calcular();
                model.FacturaLineas.Add(linea);
            }

            //Aplicar la retención si la hubiera
            TipoRetencionManager retencionManager = new TipoRetencionManager(sageContext);
            retencionManager.RellenarDatosRetencion(model);
            model.Calcular();

            var responseValid = new ResponseObject()
            {
                Status = 1,
                Data = await this.RenderPartialViewToString("_FacturaLineas", model)
            };
            return Json(responseValid);
        }

        [HttpPost]
        public async Task<JsonResult> SaveLineasDesdePedido([FromQuery] int IdPedido)
        {
            var lineas = dbContext.VentaPedidoLineas.Include(f => f.Pedido).Where(f => f.PedidoId == IdPedido);

            VentaFactura model = new VentaFactura();
            await TryUpdateModelAsync<VentaFactura>(model, "",
                f => f.CodigoCliente,
                f => f.TieneRetencion,
                f => f.Origen,
                f => f.OrigenCodigoArticulo,
                f => f.OrigenNombreArticulo,
                f => f.OrigenImporte,
                f => f.FacturaLineas);
            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<VentaFacturaLinea>();

            foreach (var item in lineas)
            {
                VentaFacturaLinea linea = new VentaFacturaLinea()
                {
                    CodigoArticulo = item.CodigoArticulo,
                    CodigoEvento = item.CodigoEvento,
                    NombreArticulo = item.NombreArticulo,
                    NombreEvento = item.NombreEvento,
                    ObservacionesFacturaLinea = item.ObservacionesPedidoLinea,
                    Orden = model.FacturaLineas.Count(),
                    BaseImponiblePrecioUnitario = item.PrecioUnitario,
                    Descuento = item.Descuento,
                    DescripcionAmpliada = item.DescripcionAmpliada,
                    TextoDescripcionAmpliada = item.TextoDescripcionAmpliada,
                    Unidades = item.UnidadesPendientes,
                    UnidadesPendientes = item.UnidadesPendientes,
                    VentaPedidoLineaId = item.IdPedidoLinea,
                    CodigoPedidoDisplay = item.Pedido.CodigoDisplay,
                    TieneTiempo = item.TieneTiempo,
                    Tiempo = item.Tiempo
                };

                //Calcular el IVA por defecto para este artículo
                TipoIVAManager manager = new TipoIVAManager(sageContext);
                var tipoiva = manager.GetIVA(item.CodigoArticulo);
                linea.CodigoTipoIVA = tipoiva?.Codigo;
                linea.IVA_Porcentaje = tipoiva == null ? (int?)null : (int)tipoiva.IVA;

                linea.Calcular();

                model.FacturaLineas.Add(linea);
            }

            //Aplicar la retención si la hubiera
            TipoRetencionManager retencionManager = new TipoRetencionManager(sageContext);
            retencionManager.RellenarDatosRetencion(model);
            model.Calcular();

            var responseValid = new ResponseObject()
            {
                Status = 1,
                Data = await this.RenderPartialViewToString("_FacturaLineas", model)
            };
            return Json(responseValid);
        }

        [HttpPost]
        public async Task<PartialViewResult> DeleteLinea([FromQuery] int orden)
        {
            VentaFactura model = new VentaFactura();
            await TryUpdateModelAsync<VentaFactura>(model, "",
                f => f.CodigoCliente,
                f => f.TieneRetencion,
                f => f.Origen,
                f => f.OrigenCodigoArticulo,
                f => f.OrigenNombreArticulo,
                f => f.OrigenImporte,
                f => f.FacturaLineas);
            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<VentaFacturaLinea>();

            var linea = model.FacturaLineas.SingleOrDefault(f => f.Orden == orden);
            model.FacturaLineas.Remove(linea);

            foreach (var item in model.FacturaLineas.Where(f => f.Orden > orden))
            {
                item.Orden--;
            }

            //Aplicar la retención si la hubiera
            TipoRetencionManager retencionManager = new TipoRetencionManager(sageContext);
            retencionManager.RellenarDatosRetencion(model);
            model.Calcular();

            return PartialView("_FacturaLineas", model);
        }

        [HttpPost]
        public async Task<PartialViewResult> DeleteAlbaran([FromQuery] string CodigoAlbaran)
        {
            VentaFactura model = new VentaFactura();
            await TryUpdateModelAsync<VentaFactura>(model, "",
                f => f.CodigoCliente,
                f => f.TieneRetencion,
                f => f.Origen,
                f => f.OrigenCodigoArticulo,
                f => f.OrigenNombreArticulo,
                f => f.OrigenImporte,
                f => f.FacturaLineas);
            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<VentaFacturaLinea>();

            var lineasBorrar = model.FacturaLineas.Where(f => f.CodigoAlbaranDisplay == CodigoAlbaran).ToDynamicArray();
            foreach (var linea in lineasBorrar)
            {
                int orden = linea.Orden;
                model.FacturaLineas.Remove(linea);
                foreach (var item in model.FacturaLineas.Where(f => f.Orden > orden))
                {
                    item.Orden--;
                }
            }

            //Aplicar la retención si la hubiera
            TipoRetencionManager retencionManager = new TipoRetencionManager(sageContext);
            retencionManager.RellenarDatosRetencion(model);
            model.Calcular();

            return PartialView("_FacturaLineas", model);
        }
        
        [HttpPost]
        public async Task<PartialViewResult> RecargarPartialTotalFactura()
        {
            VentaFactura model = new VentaFactura();
            await TryUpdateModelAsync<VentaFactura>(model, "",
                f => f.CodigoCliente,
                f => f.TieneRetencion,
                f => f.Origen,
                f => f.OrigenCodigoArticulo,
                f => f.OrigenNombreArticulo,
                f => f.OrigenImporte,
                f => f.FacturaLineas);
            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<VentaFacturaLinea>();

            //Aplicar la retención si la hubiera
            TipoRetencionManager retencionManager = new TipoRetencionManager(sageContext);
            retencionManager.RellenarDatosRetencion(model);
            model.Calcular();

            return PartialView("_TotalFactura", model);
        }

        public IActionResult ImprimirFactura(int Id, FormatoImpresion formato)
        {
            var factura = dbContext.VentaFacturas.Include(f => f.FacturaLineas).SingleOrDefault(f => f.Id == Id);
            if (factura == null)
                return NotFound();

            var pdf = printHelper.GetFactura(Id, formato);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + $"_FacturaVenta_{Id}.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        [HttpPost]
        public async Task<IActionResult> EnviarFactura(int id, FormatoImpresion formato, [FromBody] object request)
        {
            try
            {
                //Obtenemos los datos del registro
                var model = dbContext.VentaFacturas.Include(f => f.FacturaLineas).Single(f => f.Id == id);

                SmtpConfig smtpConfig = null;

                //Requisito funcional: 
                //26/02/2021 
                //Para el email ha de coger las credenciales que están en el congreso y si no están, las que hay en el fichero de usuarios.
                //Obtener datos de smtp del congreso y usuario activo
                int nCodigoEvento;
                if (Int32.TryParse(model.CodigoEvento, out nCodigoEvento))
                {
                    var congreso = await dbContext
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
                    return StatusCode(400, "No existe ninguna configuración SMTP para poder enviar mails");

                smtpConfig.EnableSsl = true;

                string sSmtpSendCopy = User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_SEND_COPY))?.Value;

                dynamic data = JToken.Parse(request.ToString());

                var pdf = printHelper.GetFactura(id, formato);

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

                MemoryStream stream = new MemoryStream(pdf);
                Attachment attachment = new Attachment(stream, $"Factura_{model.CodigoDisplay}.pdf");
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
                        attachments.Add(new Attachment(Path.Combine(environment.WebRootPath, path.Replace("~/", "").Replace("/", "\\"))));
                    }
                }

                EmailService service = new EmailService(smtpConfig);
                service.Send($"Factura Nº {model.CodigoDisplay}", html, true, to.ToArray(), User.Identity.Name, attachments);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, "Error Message: " + ex.Message + Environment.NewLine + "Inner Exception: " + ex.InnerException);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EnviarFacturaAdjuntarFichero(IFormFile Fichero)
        {
            IFileUploader _fileUploader = (IFileUploader)HttpContext.RequestServices.GetService(typeof(IFileUploader));
            dynamic data = new { Path = await _fileUploader.UploadFile(Fichero), FileName = Fichero.FileName };
            return Ok(JsonConvert.SerializeObject(data));
        }        

        public async Task<IActionResult> Traspasar(int id)
        {
            AppSettings appSettings = ((IOptions<AppSettings>)HttpContext.RequestServices.GetService(typeof(IOptions<AppSettings>))).Value;
            IWebHostEnvironment hostingEnvironment = (IWebHostEnvironment)HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));

            VentaFacturaManager manager = new VentaFacturaManager(dbContext, sageComuContext, sageContext);
            var list = await manager.Traspasar(id, appSettings, hostingEnvironment) ?? new List<ValidationResult>();
            if (list.Any())
            {
                TempData["ErrorMessage"] = list.First().ErrorMessage;
                return RedirectToAction("Edit", new { id = id });
            }
            else
            {
                TempData["Message"] = "El registro se ha exportado correctamente.";
                return RedirectToAction("Edit", new { id = id });
            }
        }
        public IActionResult BorrarDocumento(int IdArchivo, int IdTabla)
        {
            dbContext.DocumentoCompraVenta.Remove(dbContext.DocumentoCompraVenta.Find(IdArchivo));
            dbContext.SaveChanges();
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
            model.ClaveDoc = "VF";
            try
            {
                dbContext.DocumentoCompraVenta.Add(model);
                dbContext.SaveChanges();
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

using CsvHelper;
using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Helpers;
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
using System.Text;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Ventas")]
    public class VentasAlbaranController : VentasBaseController
    {
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComuContext;
        private readonly ApplicationDbContext _dbContext;
        private IOptions<AppSettings> _appSettings;        
        private VentaAlbaranManager _manager;
        private VentasAlbaranPrintHelper _printHelper;
        private SerieManager _serieManager;

        public VentasAlbaranController(
            SageContext sageContext, 
            SageComuContext sageComuContext, 
            ApplicationDbContext dbContext, 
            VentaAlbaranManager manager, 
            IOptions<AppSettings> appSettings,
            VentasAlbaranPrintHelper printHelper,
            SerieManager serieManager)
        {
            _sageContext = sageContext;
            _sageComuContext = sageComuContext;
            _dbContext = dbContext;
            _manager = manager;
            _appSettings = appSettings;
            _printHelper = printHelper;
            _serieManager = serieManager;
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

            VentaAlbaranFilter filter = new VentaAlbaranFilter();
            await TryUpdateModelAsync<VentaAlbaranFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoCliente,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoAlbaran);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoVendedor = UserHelper.CodigoVendedor(User);

            Pager pager = new Pager(await _dbContext.VentaAlbaranes.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<VentaAlbaran> list = await _dbContext.VentaAlbaranes.Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .ThenByDescending(f => f.Id)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            //ViewBag.Clientes = _sageContext.Clientes.OrderBy(f => f.Nombre);
            var listClientes = new List<Select2Response>();
            if (!string.IsNullOrWhiteSpace(filter.CodigoCliente))
            {
                var cliente = _sageContext.Clientes.Find(filter.CodigoCliente);
                listClientes.Add(new Select2Response() { id = cliente.Codigo, text = cliente.DisplayName });
            }
            ViewBag.Clientes = listClientes;
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);

            if (!string.IsNullOrWhiteSpace(filter.CodigoEvento))
                ViewBag.EventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo == filter.CodigoEvento)?.DisplayName;

            if (!string.IsNullOrWhiteSpace(filter.CodigoCliente))
                ViewBag.ClienteNombre = _sageContext.Clientes.SingleOrDefault(f => f.Codigo == filter.CodigoCliente).Nombre;


            return View(list);
        }

        public async Task<FileResult> ExportCsv(string currentsort = "")
        {
            VentaAlbaranFilter filter = new VentaAlbaranFilter();
            await TryUpdateModelAsync<VentaAlbaranFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoCliente,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoAlbaran);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoVendedor = UserHelper.CodigoVendedor(User);

            IEnumerable<VentaAlbaran> list = await _dbContext.VentaAlbaranes
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoAlbaranesVenta.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<VentasAlbaranViewModelMap>();
                await csv.WriteRecordsAsync<VentaAlbaran>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

        public async Task<FileResult> ExportPdf(string currentsort = "")
        {
            VentaAlbaranFilter filter = new VentaAlbaranFilter();
            await TryUpdateModelAsync<VentaAlbaranFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoCliente,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoAlbaran);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoVendedor = UserHelper.CodigoVendedor(User);

            IEnumerable<VentaAlbaran> list = await _dbContext.VentaAlbaranes
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string table = "<table class='w-100'><tr class='font-weight-bold'><td>Albarán</td><td>Fecha</td><td>Cliente</td><td>Vendedor</td><td class='text-right'>Total</td></tr>{0}</table>";
            string rows = string.Empty;
            foreach (var item in list)
            {
                rows += $"<tr><td>{item.CodigoAlbaran}</td><td>{item.Fecha.ToShortDateString()}</td><td>{item.NombreCliente}</td><td>{item.NombreVendedor}</td><td class='text-right'>{item.BaseImponible.ToString("C")}</td></tr>";
            }

            var html = PrintService.GetHtmlTheme("Listado de albaranes de venta", string.Format(table, rows));
            var pdf = PrintService.GetBytes(html);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoAlbaranesVenta.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        public IActionResult Create(int? idPedido, int? IdAlbaranDuplicado)
        {
            VentaAlbaran model;
            ViewBag.Clientes = new List<Select2Response>();
            //ViewBag.Clientes = _sageContext.Clientes.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Vendedores = _sageContext.Vendedor.OrderBy(f => f.Nombre);
            ViewBag.Series = _serieManager.Get(Consts.CODIGO_EMPRESA, Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_ALBARAN);
            ViewBag.IdPedido = idPedido;

            string codVendedor = UserHelper.CodigoVendedor(User);

            if (idPedido.HasValue)
            {
                var pedido = _dbContext.VentaPedidos.Include(f => f.PedidoLineas).SingleOrDefault(f => f.Id == idPedido);
                if (pedido != null)
                {
                    model = new VentaAlbaran()
                    {
                        Fecha = DateTime.Now.Date,
                        Serie = pedido.Serie,
                        CodigoCliente = pedido.CodigoCliente,
                        NombreCliente = pedido.NombreCliente,
                        CodigoVendedor = pedido.CodigoVendedor,
                        CodigoEvento = pedido.CodigoEvento,
                        NombreVendedor = pedido.NombreVendedor,
                        Observaciones = pedido.Observaciones,
                        LineaEnvCli = pedido.LineaEnvCli,
                        Direccion = pedido.Direccion,
                        CodigoPostal = pedido.CodigoPostal,
                        Poblacion = pedido.Poblacion,
                        Provincia = pedido.Provincia,
                        AlbaranLineas = new List<VentaAlbaranLinea>()
                    };
                    foreach (var linea in pedido.PedidoLineas)
                    {
                        VentaAlbaranLinea albaranLinea = new VentaAlbaranLinea()
                        {
                            CodigoArticulo = linea.CodigoArticulo,
                            NombreArticulo = linea.NombreArticulo,
                            CodigoEvento = linea.CodigoEvento,
                            NombreEvento = linea.NombreEvento,
                            ObservacionesAlbaranLinea = linea.ObservacionesPedidoLinea,
                            Orden = linea.Orden,
                            PrecioUnitario = linea.PrecioUnitario,
                            Descuento = linea.Descuento,
                            DescripcionAmpliada = linea.DescripcionAmpliada,
                            TextoDescripcionAmpliada = linea.TextoDescripcionAmpliada,
                            TotalAlbaranLinea = linea.TotalPedidoLinea,
                            Unidades = linea.UnidadesPendientes,
                            VentaPedidoLineaId = linea.IdPedidoLinea,
                            UnidadesPendientes = linea.UnidadesPendientes,
                            CodigoPedidoDisplay = pedido.CodigoDisplay,
                            IVA_Porcentaje = linea.IVA_Porcentaje,
                            CodigoTipoIVA = linea.CodigoTipoIVA,
                            TieneTiempo = linea.TieneTiempo,
                            Tiempo = linea.Tiempo
                        };
                        albaranLinea.Calcular();
                        model.AlbaranLineas.Add(albaranLinea);
                    }
                    model.Calcular();

                    if (!string.IsNullOrWhiteSpace(model.CodigoEvento))
                        ViewBag.EventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo == model.CodigoEvento)?.DisplayName;

                    if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                        ViewBag.ClienteNombre = _sageContext.Clientes.SingleOrDefault(f => f.Codigo == model.CodigoCliente)?.Nombre;

                    return View(model);
                }
            }
            if (IdAlbaranDuplicado.HasValue)
            {
                model = _dbContext.VentaAlbaranes.Include(f => f.AlbaranLineas).First(f => f.Id == IdAlbaranDuplicado);
                model.Id = 0;
                model.EstadoAlbaran = EstadoAlbaran.NoFacturado;
                model.Fecha = DateTime.Now.Date;
                model.CodigoVendedor = codVendedor;
                model.Serie = null;
                foreach (var linea in model.AlbaranLineas)
                {
                    linea.IdAlbaranLinea = 0;
                    linea.UnidadesPendientes = linea.Unidades;
                    linea.VentaPedidoLineaId = null;
                }
            }
            else
            {
                model = new VentaAlbaran() { CodigoVendedor = codVendedor };
            }

            return View(model);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed(int? idPedido)
        {
            VentaAlbaran model = new VentaAlbaran();

            var cliente = _sageContext.Clientes.Find(Request.Form["CodigoCliente"]);
            model.NombreCliente = cliente == null ? string.Empty : cliente.Nombre;
            var vendedor = _sageContext.Vendedor.Find(Request.Form["CodigoVendedor"]);
            model.NombreVendedor = vendedor == null ? string.Empty : vendedor.Nombre;
            var evento = _sageContext.Almacen.Find(Request.Form["CodigoEvento"]);
            model.NombreEvento = evento == null ? string.Empty : evento.Nombre;

            model.EstadoAlbaran = EstadoAlbaran.NoFacturado;

            await TryUpdateModelAsync<VentaAlbaran>(model, "",
                f => f.Serie,
                f => f.Fecha,
                f => f.CodigoCliente,
                f => f.CodigoVendedor,
                f => f.CodigoEvento,
                f => f.Observaciones,
                f => f.AlbaranLineas,
                f => f.LineaEnvCli,
                f => f.Direccion,
                f => f.CodigoPostal,
                f => f.Poblacion,
                f => f.Provincia);

            if (model.AlbaranLineas == null)
                model.AlbaranLineas = new List<VentaAlbaranLinea>();

            foreach (var item in model.AlbaranLineas)
            {
                item.Calcular();
            }
            model.Calcular();

            //Validación de negocio            
            VentasAlbaranValidator validator = new VentasAlbaranValidator(_dbContext);
            IList<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                await _manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }

            var listClientes = new List<Select2Response>();
            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                listClientes.Add(new Select2Response() { id = cliente.Codigo, text = cliente.DisplayName });
            ViewBag.Clientes = listClientes;
            //ViewBag.Clientes = _sageContext.Clientes.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Vendedores = _sageContext.Vendedor.OrderBy(f => f.Nombre);
            ViewBag.Series = _serieManager.Get(Consts.CODIGO_EMPRESA, Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_ALBARAN);
            ViewBag.IdPedido = idPedido;

            if (!string.IsNullOrWhiteSpace(model.CodigoEvento))
                ViewBag.EventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo == model.CodigoEvento)?.DisplayName;

            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                ViewBag.ClienteNombre = _sageContext.Clientes.SingleOrDefault(f => f.Codigo == model.CodigoCliente)?.Nombre;

            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
            {
                var direcciones = _sageContext.Env_Cli.Where(f => f.Cliente.Trim() == model.CodigoCliente).ToList();
                ViewBag.Direcciones = direcciones;
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            //ViewBag.Clientes = _sageContext.Clientes.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Vendedores = _sageContext.Vendedor.OrderBy(f => f.Nombre);

            var model = _dbContext.VentaAlbaranes.Include(f => f.AlbaranLineas).Single(f => f.Id == id);

            var listClientes = new List<Select2Response>();
            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
            {
                var cliente = _sageContext.Clientes.Find(model.CodigoCliente);
                listClientes.Add(new Select2Response() { id = cliente.Codigo, text = cliente.DisplayName });
            }
            ViewBag.Clientes = listClientes;

            if (!string.IsNullOrWhiteSpace(model.CodigoEvento))
                ViewBag.EventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo.Trim() == model.CodigoEvento.Trim())?.DisplayName;

            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                ViewBag.ClienteNombre = _sageContext.Clientes.SingleOrDefault(f => f.Codigo.Trim() == model.CodigoCliente.Trim())?.Nombre;

            foreach (var item in model.AlbaranLineas)
            {
                if (item.VentaPedidoLineaId.HasValue)
                {
                    VentaPedidoLinea linea = _dbContext.VentaPedidoLineas.Include(f => f.Pedido).Single(f => f.IdPedidoLinea == item.VentaPedidoLineaId);
                    item.UnidadesPendientes = linea.UnidadesPendientes + item.Unidades;
                    item.CodigoPedidoDisplay = linea.Pedido.CodigoDisplay;
                }
            }

            //Si una factura o pedido lleva aunque sea solamente una línea con el check de TIEMPO activado, su formato de impresión será obligatoriamente el FORMATO C
            if (model.AlbaranLineas.Any(f => f.TieneTiempo))
                ViewBag.FormatoImpresionForzado = FormatoImpresion.C;
            else
            {
                CongressesManager congressManager = new CongressesManager(_dbContext);
                if (!congressManager.PermiteImpresionFormatoA(model.CodigoEvento))
                    ViewBag.FormatoImpresionForzado = FormatoImpresion.B;
            }

            var direcciones = _sageContext.Env_Cli.Where(f => f.Cliente.Trim() == model.CodigoCliente).ToList();
            ViewBag.Direcciones = direcciones;

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            VentaAlbaran model = _dbContext.VentaAlbaranes.Find(id);

            var cliente = _sageContext.Clientes.Find(Request.Form["CodigoCliente"]);
            model.NombreCliente = cliente == null ? string.Empty : cliente.Nombre;
            var vendedor = _sageContext.Vendedor.Find(Request.Form["CodigoVendedor"]);
            model.NombreVendedor = vendedor == null ? string.Empty : vendedor.Nombre;
            var evento = _sageContext.Almacen.Find(Request.Form["CodigoEvento"]);
            model.NombreEvento = evento == null ? string.Empty : evento.Nombre;

            await TryUpdateModelAsync<VentaAlbaran>(model, "",
                f => f.Fecha,
                f => f.CodigoCliente,
                f => f.CodigoVendedor,
                f => f.CodigoEvento,
                f => f.Observaciones,
                f => f.AlbaranLineas,
                f => f.LineaEnvCli,
                f => f.Direccion,
                f => f.CodigoPostal,
                f => f.Poblacion,
                f => f.Provincia);

            if (model.AlbaranLineas == null)
                model.AlbaranLineas = new List<VentaAlbaranLinea>();

            foreach (var item in model.AlbaranLineas)
            {
                item.Calcular();
            }
            model.Calcular();

            //Validación de negocio            
            VentasAlbaranValidator validator = new VentasAlbaranValidator(_dbContext);
            IList<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                await _manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }

            //ViewBag.Clientes = _sageContext.Clientes.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Vendedores = _sageContext.Vendedor.OrderBy(f => f.Nombre);

            var listClientes = new List<Select2Response>();
            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                listClientes.Add(new Select2Response() { id = cliente.Codigo, text = cliente.DisplayName });
            ViewBag.Clientes = listClientes;

            if (!string.IsNullOrWhiteSpace(model.CodigoEvento))
                ViewBag.EventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo.Trim() == model.CodigoEvento.Trim())?.DisplayName;

            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                ViewBag.ClienteNombre = _sageContext.Clientes.SingleOrDefault(f => f.Codigo.Trim() == model.CodigoCliente.Trim())?.Nombre;

            //Si una factura o pedido lleva aunque sea solamente una línea con el check de TIEMPO activado, su formato de impresión será obligatoriamente el FORMATO C
            if (model.AlbaranLineas.Any(f => f.TieneTiempo))
                ViewBag.FormatoImpresionForzado = FormatoImpresion.C;
            else
            {
                CongressesManager congressManager = new CongressesManager(_dbContext);
                if (!congressManager.PermiteImpresionFormatoA(model.CodigoEvento))
                    ViewBag.FormatoImpresionForzado = FormatoImpresion.B;
            }

            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
            {
                var direcciones = _sageContext.Env_Cli.Where(f => f.Cliente.Trim() == model.CodigoCliente).ToList();
                ViewBag.Direcciones = direcciones;
            }

            return View(model);
        }

        public PartialViewResult AddLinea([FromQuery] string CodigoEvento)
        {
            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Articulos = _sageContext.Articulo.OrderBy(f => f.Codigo);
            return PartialView("~/Views/Shared/EditorTemplates/VentaAlbaranLinea.cshtml", new VentaAlbaranLinea() { Unidades = 1, CodigoArticulo = string.Empty, Orden = -1, CodigoEvento = CodigoEvento });
        
        }

        [HttpPost]
        public async Task<PartialViewResult> EditLinea([FromQuery] int orden)
        {
            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Articulos = _sageContext.Articulo.OrderBy(f => f.Codigo);

            VentaAlbaran model = new VentaAlbaran();
            await TryUpdateModelAsync<VentaAlbaran>(model, "", f => f.AlbaranLineas);
            
            VentaAlbaranLinea linea = model.AlbaranLineas.Single(f => f.Orden == orden);
            
            return PartialView("~/Views/Shared/EditorTemplates/VentaAlbaranLinea.cshtml", linea);
        }

        public async Task<IActionResult> Delete(int id)
        {           
            await _manager.Delete(id);
            TempData["Message"] = "El registro se ha eliminado correctamente.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> SaveLinea()
        {
            VentaAlbaranLinea linea = new VentaAlbaranLinea();

            //this.HttpContext.Request.Form
            await TryUpdateModelAsync<VentaAlbaranLinea>(linea, "linea",
                f => f.CodigoArticulo,
                f => f.NombreArticulo,
                f => f.Unidades,
                f => f.CodigoEvento,
                f => f.ObservacionesAlbaranLinea,
                f => f.PrecioUnitario,
                f => f.Descuento,
                f => f.ImporteDescuento,
                f => f.DescripcionAmpliada,
                f => f.TextoDescripcionAmpliada,
                f => f.UnidadesPendientes,
                f => f.Orden,
                f => f.VentaPedidoLineaId,
                f => f.IdAlbaranLinea,
                f => f.CodigoPedidoDisplay,
                f => f.CodigoTipoIVA,
                f => f.TieneTiempo,
                f => f.Tiempo
            );

            if (ModelState.IsValid)
            {
                VentaAlbaran model = new VentaAlbaran();
                await TryUpdateModelAsync<VentaAlbaran>(model, "",
                    f => f.AlbaranLineas);

                if (model.AlbaranLineas == null)
                    model.AlbaranLineas = new List<VentaAlbaranLinea>();

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

                var responseValid = new ResponseObject()
                {
                    Status = 1,
                    Data = await this.RenderPartialViewToString("_AlbaranLineas", model)
                };
                return Json(responseValid);
            }

            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Articulos = _sageContext.Articulo.OrderBy(f => f.Codigo);

            var responseInvalid = new ResponseObject()
            {
                Status = 0,
                Data = await this.RenderPartialViewToString("EditorTemplates/VentaAlbaranLinea", linea)
            };
            
            return Json(responseInvalid);
        }

        [HttpPost]
        public async Task<JsonResult> SaveLineasDesdePedido([FromQuery] int IdPedido)
        {
            var lineas = _dbContext.VentaPedidoLineas.Include(f => f.Pedido).Where(f => f.PedidoId == IdPedido && f.UnidadesPendientes > 0);

            VentaAlbaran model = new VentaAlbaran();
            await TryUpdateModelAsync<VentaAlbaran>(model, "",
                f => f.AlbaranLineas);
            if (model.AlbaranLineas == null)
                model.AlbaranLineas = new List<VentaAlbaranLinea>();

            foreach (var item in lineas)
            {
                VentaAlbaranLinea linea = new VentaAlbaranLinea()
                {
                    CodigoArticulo = item.CodigoArticulo,
                    CodigoEvento = item.CodigoEvento,
                    NombreArticulo = item.NombreArticulo,
                    NombreEvento = item.NombreEvento,
                    ObservacionesAlbaranLinea = item.ObservacionesPedidoLinea,
                    Orden = model.AlbaranLineas.Count(),
                    PrecioUnitario = item.PrecioUnitario,
                    Descuento = item.Descuento,
                    DescripcionAmpliada = item.DescripcionAmpliada,
                    TextoDescripcionAmpliada = item.TextoDescripcionAmpliada,
                    Unidades = item.UnidadesPendientes,
                    VentaPedidoLineaId = item.IdPedidoLinea,
                    UnidadesPendientes = item.UnidadesPendientes,
                    CodigoPedidoDisplay = item.Pedido.CodigoDisplay,
                    CodigoTipoIVA = item.CodigoTipoIVA,
                    IVA_Porcentaje = item.IVA_Porcentaje,
                    TieneTiempo = item.TieneTiempo,
                    Tiempo = item.Tiempo
                };
                linea.Calcular();
                model.AlbaranLineas.Add(linea);
            }

            var responseValid = new ResponseObject()
            {
                Status = 1,
                Data = await this.RenderPartialViewToString("_AlbaranLineas", model)
            };
            return Json(responseValid);
        }

        [HttpPost]
        public async Task<PartialViewResult> DeleteLinea([FromQuery] int orden)
        {
            VentaAlbaran model = new VentaAlbaran();
            await TryUpdateModelAsync<VentaAlbaran>(model, "", f => f.AlbaranLineas);
            if (model.AlbaranLineas == null)
                model.AlbaranLineas = new List<VentaAlbaranLinea>();

            var linea = model.AlbaranLineas.SingleOrDefault(f => f.Orden == orden);
            model.AlbaranLineas.Remove(linea);

            foreach (var item in model.AlbaranLineas.Where(f => f.Orden > orden))
            {
                item.Orden--;
            }

            return PartialView("_AlbaranLineas", model);
        }

        public IActionResult ImprimirAlbaran(int Id, FormatoImpresion formato)
        {
            var albaran = _dbContext.VentaAlbaranes.Include(f => f.AlbaranLineas).SingleOrDefault(f => f.Id == Id);
            if (albaran == null)
                return NotFound();
            
            var pdf = _printHelper.GetAlbaran(Id, formato);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + $"_AlbaranVenta_{Id}.pdf";
            return File(pdf, "application/pdf", fileName);
        }                    

        [HttpGet]
        public async Task<PartialViewResult> Buscador()
        {
            VentaAlbaranFilter filter = new VentaAlbaranFilter();
            await TryUpdateModelAsync<VentaAlbaranFilter>(filter, "",
                f => f.CodigoEvento,
                f => f.CodigoCliente,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoAlbaran);

            IEnumerable<VentaAlbaran> list = await _dbContext.VentaAlbaranes
                .Where(filter.ExpressionFilter())
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();

            return PartialView("_BuscadorVentasAlbaran", list);
        }
    }
}

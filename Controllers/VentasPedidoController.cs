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
using Ferpuser.Transfer;
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
    public class VentasPedidoController : VentasBaseController
    {
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComuContext;
        private readonly ApplicationDbContext _dbContext;
        private IOptions<AppSettings> _appSettings;
        private VentasPedidoPrintHelper _printHelper;
        private SerieManager _serieManager;

        public VentasPedidoController(
            SageContext sageContext, 
            SageComuContext sageComuContext, 
            ApplicationDbContext dbContext, 
            IOptions<AppSettings> appSettings,
            VentasPedidoPrintHelper printHelper,
            SerieManager serieManager)
        {
            _sageContext = sageContext;
            _sageComuContext = sageComuContext;
            _dbContext = dbContext;
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

            VentaPedidoFilter filter = new VentaPedidoFilter();
            await TryUpdateModelAsync<VentaPedidoFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoCliente,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoPedido);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoVendedor = UserHelper.CodigoVendedor(User);

            Pager pager = new Pager(await _dbContext.VentaPedidos.CountAsync(filter.ExpressionFilter()), pag, 100, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            if (filter.EstadoPedido == EstadoPedido.Pendiente)
            {
                filter.EstadoPedido = null;
                filter.EstadosPedido = new EstadoPedido[] { EstadoPedido.Pendiente, EstadoPedido.PendienteParcial };
            }

            IEnumerable<VentaPedido> list = await _dbContext.VentaPedidos.Where(filter.ExpressionFilter())
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
            VentaPedidoFilter filter = new VentaPedidoFilter();
            await TryUpdateModelAsync<VentaPedidoFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoCliente,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoPedido);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoVendedor = UserHelper.CodigoVendedor(User);

            IEnumerable<VentaPedido> list = await _dbContext.VentaPedidos
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoPedidosVenta.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<VentasPedidoViewModelMap>();
                await csv.WriteRecordsAsync<VentaPedido>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

        public async Task<FileResult> ExportPdf(string currentsort = "")
        {
            VentaPedidoFilter filter = new VentaPedidoFilter();
            await TryUpdateModelAsync<VentaPedidoFilter>(filter, "filter",
                f => f.CodigoEvento,
                f => f.CodigoCliente,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoPedido);

            if (!UserHelper.AccesoAdmin(User))
                filter.CodigoVendedor = UserHelper.CodigoVendedor(User);

            IEnumerable<VentaPedido> list = await _dbContext.VentaPedidos
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string table = "<table class='w-100'><tr class='font-weight-bold'><td>Fecha</td><td>Cliente</td><td>Vendedor</td><td class='text-right'>Total</td></tr>{0}</table>";
            string rows = string.Empty;
            foreach (var item in list)
            {
                rows += $"<tr><td>{item.Fecha.ToShortDateString()}</td><td>{item.NombreCliente}</td><td>{item.NombreVendedor}</td><td class='text-right'>{item.BaseImponible.ToString("C")}</td></tr>";
            }

            var html = PrintService.GetHtmlTheme("Listado de pedidos de compra", string.Format(table, rows));
            var pdf = PrintService.GetBytes(html);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoPedidosVenta.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        public IActionResult Create(int? IdPedidoDuplicado)
        {
            ViewBag.TiposDocumentoVenta = _dbContext.TiposDocumentoVenta.OrderBy(f => f.Descripcion);
            ViewBag.Clientes = new List<Select2Response>();
            //ViewBag.Clientes = _sageContext.Clientes.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Vendedores = _sageContext.Vendedor.OrderBy(f => f.Nombre);
            ViewBag.Series = _serieManager.Get(Consts.CODIGO_EMPRESA, Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_PEDIDO);

            string codVendedor = UserHelper.CodigoVendedor(User);
            VentaPedido model = new VentaPedido() { CodigoVendedor = codVendedor };
            if (IdPedidoDuplicado.HasValue)
            {
                model = _dbContext.VentaPedidos.Include(f => f.PedidoLineas).First(f => f.Id == IdPedidoDuplicado);
                model.Id = 0;
                model.EstadoPedido = EstadoPedido.Pendiente;
                model.Fecha = DateTime.Now.Date;
                model.CodigoVendedor = codVendedor;
                model.Serie = null;
                foreach (var linea in model.PedidoLineas)
                {
                    linea.IdPedidoLinea = 0;
                    linea.UnidadesPendientes = linea.Unidades;
                }
            }

            return View(model);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed([FromServices] SageContextFactoryHelper sageContextFactory, string guardar)
        {
            VentaPedido model = new VentaPedido();
            
            var cliente = _sageContext.Clientes.Find(Request.Form["CodigoCliente"]);
            model.NombreCliente = cliente == null ? string.Empty : cliente.Nombre;
            var vendedor = _sageContext.Vendedor.Find(Request.Form["CodigoVendedor"]);
            model.NombreVendedor = vendedor == null ? string.Empty : vendedor.Nombre;
            var evento = _sageContext.Almacen.Find(Request.Form["CodigoEvento"]);
            model.NombreEvento = evento == null ? string.Empty : evento.Nombre;

            model.EstadoPedido = EstadoPedido.Pendiente;

            await TryUpdateModelAsync<VentaPedido>(model, "",
                f => f.Serie,
                f => f.Fecha,
                f => f.TipoDocumentoVentaId,
                f => f.CodigoCliente,
                f => f.CodigoVendedor,
                f => f.CodigoEvento,
                f => f.Observaciones,
                f => f.PedidoLineas,
                f => f.LineaEnvCli,
                f => f.Direccion,
                f => f.CodigoPostal,
                f => f.Poblacion,
                f => f.Provincia);

            if (model.PedidoLineas == null)
                model.PedidoLineas = new List<VentaPedidoLinea>();

            foreach (var item in model.PedidoLineas)
            {
                item.Calcular();
            }
            model.Calcular();

            //Validación de negocio            
            VentasPedidoValidator validator = new VentasPedidoValidator(_dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                foreach (var linea in model.PedidoLineas)
                {
                    linea.UnidadesPendientes = linea.Unidades;
                }

                VentaPedidoManager manager = new VentaPedidoManager(_dbContext, sageContextFactory);
                manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                if (guardar == "nuevo")
                    return RedirectToAction("Create");
                else if (guardar == "continuar")
                    return RedirectToAction("Edit", "VentasPedido", new { id = model.Id });
                return RedirectToAction("Index");
            }

            ViewBag.TiposDocumentoVenta = _dbContext.TiposDocumentoVenta.OrderBy(f => f.Descripcion);

            var listClientes = new List<Select2Response>();
            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                listClientes.Add(new Select2Response() { id = cliente.Codigo, text = cliente.DisplayName });
            ViewBag.Clientes = listClientes;
            //ViewBag.Clientes = _sageContext.Clientes.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Vendedores = _sageContext.Vendedor.OrderBy(f => f.Nombre);
            ViewBag.Series = _serieManager.Get(Consts.CODIGO_EMPRESA, Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_PEDIDO);

            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
            {
                var direcciones = _sageContext.Env_Cli.Where(f => f.Cliente.Trim() == model.CodigoCliente).ToList();
                ViewBag.Direcciones = direcciones;
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.TiposDocumentoVenta = _dbContext.TiposDocumentoVenta.OrderBy(f => f.Descripcion);
            //ViewBag.Clientes = _sageContext.Clientes.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Vendedores = _sageContext.Vendedor.OrderBy(f => f.Nombre);

            var model = _dbContext.VentaPedidos.Include(f => f.PedidoLineas).Single(f => f.Id == id);

            model.documentos = _dbContext.DocumentoCompraVenta.Where(f => f.IdTabla == model.Id && f.ClaveDoc == "VP").ToList();

            if (model.documentos == null)
                model.documentos = new List<DocumentoCompraVenta>();

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
            
            ViewBag.EmailTo = _sageContext.Clientes.FirstOrDefault(f => f.Codigo.Trim() == model.CodigoCliente.Trim())?.Email;

            //Si una factura o pedido lleva aunque sea solamente una línea con el check de TIEMPO activado, su formato de impresión será obligatoriamente el FORMATO C
            if (model.PedidoLineas.Any(f => f.TieneTiempo))
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
            VentaPedido model = _dbContext.VentaPedidos.Find(id);

            var cliente = _sageContext.Clientes.Find(Request.Form["CodigoCliente"]);
            model.NombreCliente = cliente == null ? string.Empty : cliente.Nombre;
            var vendedor = _sageContext.Vendedor.Find(Request.Form["CodigoVendedor"]);
            model.NombreVendedor = vendedor == null ? string.Empty : vendedor.Nombre;
            var evento = _sageContext.Almacen.Find(Request.Form["CodigoEvento"]);
            model.NombreEvento = evento == null ? string.Empty : evento.Nombre;

            await TryUpdateModelAsync<VentaPedido>(model, "",
                f => f.Fecha,
                f => f.TipoDocumentoVentaId,
                f => f.CodigoCliente,
                f => f.CodigoVendedor,
                f => f.CodigoEvento,
                f => f.Observaciones,
                f => f.PedidoLineas,
                f => f.LineaEnvCli,
                f => f.Direccion,
                f => f.CodigoPostal,
                f => f.Poblacion,
                f => f.Provincia);

            if (model.PedidoLineas == null)
                model.PedidoLineas = new List<VentaPedidoLinea>();

            foreach (var item in model.PedidoLineas)
            {
                item.Calcular();
            }
            model.Calcular();

            //Validación de negocio            
            VentasPedidoValidator validator = new VentasPedidoValidator(_dbContext);
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

                var lineasBorradas = _dbContext.VentaPedidoLineas.Where(f => f.PedidoId == id && !idLineasModificadas.Contains(f.IdPedidoLinea));
                foreach (var linea in lineasBorradas)
                {
                    _dbContext.Entry(linea).State = EntityState.Deleted;
                }

                await _dbContext.SaveChangesAsync();
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.TiposDocumentoVenta = _dbContext.TiposDocumentoVenta.OrderBy(f => f.Descripcion);
            var listClientes = new List<Select2Response>();
            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                listClientes.Add(new Select2Response() { id = cliente.Codigo, text = cliente.DisplayName });
            ViewBag.Clientes = listClientes;
            //ViewBag.Clientes = _sageContext.Clientes.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Vendedores = _sageContext.Vendedor.OrderBy(f => f.Nombre);

            if (!string.IsNullOrWhiteSpace(model.CodigoEvento))
                ViewBag.EventoNombre = _sageContext.Almacen.SingleOrDefault(f => f.Codigo.Trim() == model.CodigoEvento.Trim())?.DisplayName;

            if (!string.IsNullOrWhiteSpace(model.CodigoCliente))
                ViewBag.ClienteNombre = _sageContext.Clientes.SingleOrDefault(f => f.Codigo.Trim() == model.CodigoCliente.Trim())?.Nombre;

            ViewBag.EmailTo = _sageContext.Clientes.FirstOrDefault(f => f.Codigo.Trim() == model.CodigoCliente.Trim())?.Email;

            //Si una factura o pedido lleva aunque sea solamente una línea con el check de TIEMPO activado, su formato de impresión será obligatoriamente el FORMATO C
            if (model.PedidoLineas.Any(f => f.TieneTiempo))
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

        public async Task<IActionResult> Delete(int id)
        {
            var pedido = _dbContext.VentaPedidos.Find(id);

            pedido.documentos = _dbContext.DocumentoCompraVenta.Where(f => f.IdTabla == pedido.Id && f.ClaveDoc == "VP").ToList();

            foreach (var f in pedido.documentos)
            {
                _dbContext.DocumentoCompraVenta.Remove(f);
            }

            _dbContext.VentaPedidos.Remove(pedido);
            await _dbContext.SaveChangesAsync();
            TempData["Message"] = "El registro se ha eliminado correctamente.";
            return RedirectToAction("Index");
        }

        public PartialViewResult AddLinea([FromQuery] string CodigoEvento)
        {
            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Articulos = _sageContext.Articulo.OrderBy(f => f.Codigo);
            return PartialView("~/Views/Shared/EditorTemplates/VentaPedidoLinea.cshtml", new VentaPedidoLinea() { Unidades = 1, CodigoArticulo = string.Empty, Orden = -1, CodigoEvento = CodigoEvento });
        }

        [HttpPost]
        public async Task<PartialViewResult> EditLinea([FromQuery] int orden)
        {
            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Articulos = _sageContext.Articulo.OrderBy(f => f.Codigo);

            VentaPedido model = new VentaPedido();
            await TryUpdateModelAsync<VentaPedido>(model, "",
                f => f.PedidoLineas);
            
            VentaPedidoLinea linea = model.PedidoLineas.Single(f => f.Orden == orden);            
            return PartialView("~/Views/Shared/EditorTemplates/VentaPedidoLinea.cshtml", linea);
        }

        [HttpPost]
        public async Task<JsonResult> SaveLinea()
        {
            VentaPedidoLinea linea = new VentaPedidoLinea();
            await TryUpdateModelAsync<VentaPedidoLinea>(linea, "linea",
                f => f.CodigoArticulo,
                f => f.NombreArticulo,
                f => f.Unidades,
                f => f.ObservacionesPedidoLinea,
                f => f.PrecioUnitario,
                f => f.Descuento,
                f => f.ImporteDescuento,
                f => f.DescripcionAmpliada,
                f => f.TextoDescripcionAmpliada,
                f => f.CodigoEvento,
                f => f.Orden,
                f => f.UnidadesPendientes,
                f => f.CodigoTipoIVA,
                f => f.TieneTiempo,
                f => f.Tiempo
                );

            if (ModelState.IsValid)
            {
                VentaPedido model = new VentaPedido();
                await TryUpdateModelAsync<VentaPedido>(model, "",
                    f => f.PedidoLineas);
                if (model.PedidoLineas == null)
                    model.PedidoLineas = new List<VentaPedidoLinea>();

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

                var responseValid = new ResponseObject()
                {
                    Status = 1,
                    Data = await this.RenderPartialViewToString("_PedidoLineas", model)
                };
                return Json(responseValid);
            }

            ViewBag.TiposIVA = _sageContext.Tipo_IVA.OrderBy(f => f.Nombre);
            ViewBag.Eventos = _sageContext.Almacen.OrderBy(f => f.Nombre);
            ViewBag.Articulos = _sageContext.Articulo.OrderBy(f => f.Codigo);
            var responseInvalid = new ResponseObject()
            {
                Status = 0,
                Data = await this.RenderPartialViewToString("EditorTemplates/VentaPedidoLinea", linea)
            };

            return Json(responseInvalid);
        }

        [HttpPost]
        public async Task<PartialViewResult> DeleteLinea([FromQuery] int orden)
        {
            VentaPedido model = new VentaPedido();
            await TryUpdateModelAsync<VentaPedido>(model, "", f => f.PedidoLineas);
            if (model.PedidoLineas == null)
                model.PedidoLineas = new List<VentaPedidoLinea>();

            var linea = model.PedidoLineas.SingleOrDefault(f => f.Orden == orden);
            model.PedidoLineas.Remove(linea);

            foreach (var item in model.PedidoLineas.Where(f => f.Orden > orden))
            {
                item.Orden--;
            }

            return PartialView("_PedidoLineas", model);
        }

        public IActionResult ImprimirPedido(int id, FormatoImpresion formato)
        {
            var pedido = _dbContext.VentaPedidos.Include(f => f.PedidoLineas).AsNoTracking().SingleOrDefault(f => f.Id == id);
            if (pedido == null)
                return NotFound();

            var pdf = _printHelper.GetAlbaran(id, formato);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + $"_PedidoVenta_{pedido.Id}.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        [HttpPost]
        public async Task<IActionResult> EnviarPedido(int id, FormatoImpresion formato, [FromBody] object request)
        {
            try
            {
                //Obtenemos los datos del registro
                var model = _dbContext.VentaPedidos.Include(f => f.PedidoLineas).Single(f => f.Id == id);

                SmtpConfig smtpConfig = null;

                //Requisito funcional: 
                //26/02/2021 
                //Para el email ha de coger las credenciales que están en el congreso y si no están, las que hay en el fichero de usuarios.
                //Obtener datos de smtp del congreso y usuario activo
                int nCodigoEvento;
                if (Int32.TryParse(model.CodigoEvento, out nCodigoEvento))
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
                    return StatusCode(400, "No existe ninguna configuración SMTP para poder enviar mails");

                smtpConfig.EnableSsl = true;

                string sSmtpSendCopy = User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_SEND_COPY))?.Value;

                dynamic data = JToken.Parse(request.ToString());

                var pdf = _printHelper.GetAlbaran(id, formato);

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
                Attachment attachment = new Attachment(stream, $"Pedido_{model.CodigoDisplay}.pdf");
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
                service.Send($"Pedido Nº {model.CodigoDisplay}", html, true, to.ToArray(), User.Identity.Name, attachments);

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

        [HttpGet]
        public async Task<PartialViewResult> Buscador()
        {
            VentaPedidoFilter filter = new VentaPedidoFilter();
            await TryUpdateModelAsync<VentaPedidoFilter>(filter, "",
                f => f.CodigoEvento,
                f => f.CodigoCliente,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.EstadoPedido,
                f => f.EstadosPedido);

            IEnumerable<VentaPedido> list = await _dbContext.VentaPedidos
                .Where(filter.ExpressionFilter())
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();

            return PartialView("_BuscadorVentasPedido", list);
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
            model.ClaveDoc = "VP";
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

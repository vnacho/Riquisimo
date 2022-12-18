using CsvHelper;
using CsvHelper.Configuration;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models.Consts;
using Ferpuser.Models.SageComu;
using Ferpuser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Ventas")]
    public class VentasInformesController : VentasBaseController
    {
        private readonly ApplicationDbContext _context; 
        private readonly SageComuContext _sageComu;
        private readonly SageContext _sage;

        public VentasInformesController(ApplicationDbContext context, SageComuContext comu, SageContext sage)
        {
            _context = context;
            _sageComu = comu;
            _sage = sage;
        }

        #region UnpaidExpenses

        public IActionResult UnpaidExpenses()
        {
            List<UnpaidExpensesViewModel> vm = new List<UnpaidExpensesViewModel>();
            ViewBag.Range = false;
            ViewData["Start"] = DateTime.MinValue;
            ViewData["End"] = DateTime.MaxValue;
            ViewData["Total"] = vm.Sum(e => e.Pending);
            ViewData["Cliente"] = new SelectList(_sage.Clientes.OrderBy(a => a.Codigo), "Codigo", "DisplayName", string.Empty);

            //Hay que coger los vendedores y eventos de todos los ejercicios
            EjercicioManager manager = new EjercicioManager(_sageComu);
            var ejercicios = manager.GetAll();
            List<Models.Sage.Almacen> listAlmacenes = new List<Models.Sage.Almacen>();
            List<Models.Sage.Vendedor> listVendedores = new List<Models.Sage.Vendedor>();
            foreach (var ejercicio in ejercicios)
            {
                HttpContext.Session.SetInt32(Consts.SESSION_EJERCICIO, Convert.ToInt32(ejercicio.ANY));
                SageContext _sageTmp = (SageContext)HttpContext.RequestServices.GetService(typeof(SageContext));

                var codigosAlmacenes = listAlmacenes.Select(f => f.Codigo);
                listAlmacenes.AddRange(_sageTmp.Almacen.Where(f => !codigosAlmacenes.Contains(f.Codigo)).ToList());

                var codigoVendedores = listVendedores.Select(f => f.Codigo);
                listVendedores.AddRange(_sageTmp.Vendedor.Where(f => !codigoVendedores.Contains(f.Codigo)).ToList());
            }

            ViewData["Almacen"] = new SelectList(listAlmacenes.OrderBy(f => f.Codigo), "Codigo", "DisplayName", string.Empty);
            ViewData["Vendedor"] = new SelectList(listVendedores.OrderBy(f => f.Codigo), "Codigo", "Display", string.Empty);

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem("Cobradas", "True"));
            list.Add(new SelectListItem("No cobradas", "False"));
            ViewBag.Title = "Todas las facturas";            
            ViewBag.Estados = list;
            
            return View();
        }

        [HttpPost, ActionName("UnpaidExpenses")]
        public IActionResult UnpaidExpensesConfirmed(DateTime? start, DateTime? end, string Cliente = "", string Vendedor = "", string Almacen = "", bool? FilterEstado = null, bool? FilterRetencion = null)
        {
            int? ejercicioSession = null;
            try
            {
                List<UnpaidExpensesViewModel> vm;
                GetUnpaidExpensesSage(start, end, out vm, Cliente, Vendedor, Almacen, FilterEstado, FilterRetencion);

                EjercicioManager manager = new EjercicioManager(_sageComu);
                //Guardamos el ejercicio seleccionado en sesión para recuperarla al finalizar el método
                ejercicioSession = HttpContext.Session.GetInt32(Consts.SESSION_EJERCICIO);

                ViewBag.Range = false;
                ViewData["Start"] = start.HasValue ? start : DateTime.MinValue;
                ViewData["End"] = end.HasValue ? end : DateTime.MaxValue;
                ViewData["Total"] = vm.Sum(e => e.Pending);

                //Hay que coger los clientes del ejercicio activo, se arrastran de ejercicio en ejercicio con lo que están todos en el ejercicio actual
                ejercici ejercicioActual = manager.GetEjercicioActivo();
                //Asignar la sesión temporalmente
                HttpContext.Session.SetInt32(Consts.SESSION_EJERCICIO, Convert.ToInt32(ejercicioActual.ANY));
                ViewData["Cliente"] = new SelectList(_sage.Clientes.OrderBy(a => a.Codigo), "Codigo", "DisplayName", Cliente);

                //Hay que coger los vendedores y eventos de todos los ejercicios
                var ejercicios = manager.GetAll();
                List<Models.Sage.Almacen> listAlmacenes = new List<Models.Sage.Almacen>();
                List<Models.Sage.Vendedor> listVendedores = new List<Models.Sage.Vendedor>();
                foreach (var ejercicio in ejercicios)
                {
                    HttpContext.Session.SetInt32(Consts.SESSION_EJERCICIO, Convert.ToInt32(ejercicio.ANY));
                    SageContext _sageTmp = (SageContext)HttpContext.RequestServices.GetService(typeof(SageContext));

                    var codigosAlmacenes = listAlmacenes.Select(f => f.Codigo);
                    listAlmacenes.AddRange(_sageTmp.Almacen.Where(f => !codigosAlmacenes.Contains(f.Codigo)).ToList());

                    var codigoVendedores = listVendedores.Select(f => f.Codigo);
                    listVendedores.AddRange(_sageTmp.Vendedor.Where(f => !codigoVendedores.Contains(f.Codigo)).ToList());
                }

                ViewData["Almacen"] = new SelectList(listAlmacenes.OrderBy(f => f.Codigo), "Codigo", "DisplayName", Almacen);
                ViewData["Vendedor"] = new SelectList(listVendedores.OrderBy(f => f.Codigo), "Codigo", "Display", Vendedor);


                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem("Cobradas", "True"));
                list.Add(new SelectListItem("No cobradas", "False"));

                switch (FilterEstado)
                {
                    case true:
                        ViewBag.Title = "Facturas cobradas";
                        break;
                    case false:
                        ViewBag.Title = "Facturas no cobradas";
                        break;
                    default:
                        ViewBag.Title = "Todas las facturas";
                        break;
                }

                ViewBag.Estados = list;
                ViewBag.Estado = FilterEstado;
                ViewBag.FilterRetencion = FilterRetencion;

                return View(vm);
            }
            catch
            {
                throw;
            }
            finally
            {
                //Recuperar el valor de la sesión guardada al iniciar el método
                if (ejercicioSession.HasValue)
                    HttpContext.Session.SetInt32(Consts.SESSION_EJERCICIO, Convert.ToInt32(ejercicioSession.Value));
            }
        }

        private void GetUnpaidExpensesSage(DateTime? FiltroFechaDesde, DateTime? FiltroFechaHasta, out List<UnpaidExpensesViewModel> vm, string FiltroCliente = "", string FiltroVendedor = "", string FiltroAlmacen = "", bool? FilterEstado = null, bool? FilterRetencion = null)
        {
            int? ejercicioSession = null;
            SageContext _sageTmp = null;
            try
            {
                EjercicioManager manager = new EjercicioManager(_sageComu);
                //Guardamos el ejercicio seleccionado en sesión para recuperarla
                ejercicioSession = HttpContext.Session.GetInt32(Consts.SESSION_EJERCICIO);

                //El filtro de vendedor y evento se hace posteriormente
                var query = _sageComu.Previ_Cl.Where(p =>
                    p.FEC_OPER.HasValue &&
                    (string.IsNullOrWhiteSpace(FiltroCliente) ? true : p.Cliente == FiltroCliente));

                if (FilterEstado.HasValue)
                {
                    if (FilterEstado.Value)
                        query = query.Where(f => f.Cobro.HasValue);
                    else
                        query = query.Where(f => !f.Cobro.HasValue);
                }
                var previ = query
                    .OrderByDescending(f => f.Periodo)
                    .ToList();

                //Obtener el ejercicio actual, el más reciente
                ejercici ejercicioActual = manager.GetEjercicioActivo();
                //Asignar la sesión temporalmente
                HttpContext.Session.SetInt32(Consts.SESSION_EJERCICIO, Convert.ToInt32(ejercicioActual.ANY));
                //Para obtener los clientes nos vale la conexión al ejercicio más reciente. Ya que los clientes se arrastran de año a año.
                var clientes = _sage.Clientes;

                vm = new List<UnpaidExpensesViewModel>();
                int nTmpAnio = 0;
                foreach (var pendiente in previ)
                {
                    decimal importeRetencion = 0;
                    int anioPendiente = pendiente.Periodo;
                    if (nTmpAnio != anioPendiente) //Para obtener los eventos y los vendedores hay que ir al ejercicio de lo que nos marca la factura.            
                    {
                        //Refrescamos la conexión con la base de datos del ejercicio correspondiente
                        HttpContext.Session.SetInt32(Consts.SESSION_EJERCICIO, Convert.ToInt32(anioPendiente));
                        _sageTmp = (SageContext)HttpContext.RequestServices.GetService(typeof(SageContext));
                        nTmpAnio = anioPendiente;
                    }

                    var albaran = _sageTmp.C_Albven.FirstOrDefault(f => f.Factura == pendiente.Factura);

                    DateTime? dtFechaFactura = null;

                    string sCodigoAlmacen = string.Empty;
                    string sNombreAlmacen = string.Empty;
                    string sCodigoVendedor = string.Empty;

                    if (albaran != null)
                    {
                        dtFechaFactura = albaran.Fecha_Fac;
                        sCodigoAlmacen = albaran.Almacen;
                        sNombreAlmacen = _sageTmp.Almacen.FirstOrDefault(f => f.Codigo == sCodigoAlmacen)?.Nombre;
                        sCodigoVendedor = albaran.Vendedor;

                        if (albaran.Recc.HasValue && albaran.Recc.Value && albaran.Porcen_Ret.HasValue)
                            importeRetencion = pendiente.Importe * albaran.Porcen_Ret.Value / 100;
                    }

                    //Aplicamos los filtros que faltaban
                    if (!string.IsNullOrWhiteSpace(FiltroAlmacen) && sCodigoAlmacen != FiltroAlmacen)
                        continue;
                    if (!string.IsNullOrWhiteSpace(FiltroVendedor) && sCodigoVendedor != FiltroVendedor)
                        continue;
                    if (FiltroFechaDesde.HasValue && dtFechaFactura.HasValue && dtFechaFactura.Value < FiltroFechaDesde.Value)
                        continue;
                    if (FiltroFechaHasta.HasValue && dtFechaFactura.HasValue && dtFechaFactura.Value > FiltroFechaHasta.Value)
                        continue;

                    var cliente = clientes.FirstOrDefault(f => f.Codigo == pendiente.Cliente);
                    if (FilterRetencion.HasValue)
                    {
                        if (FilterRetencion.Value && pendiente.Orden != 0)
                            continue;
                        else if (!FilterRetencion.Value && pendiente.Orden == 0)
                            continue;
                    }

                    vm.Add(new UnpaidExpensesViewModel()
                    {
                        Factura = pendiente.Factura,
                        Fecha = dtFechaFactura,
                        Cliente = pendiente.Cliente,
                        Nombre = cliente?.Nombre,
                        TieneRetencion = pendiente.Orden == 0 ? true : false,
                        Pending = pendiente.Importe - importeRetencion,
                        Almacen = sCodigoAlmacen,
                        Descripcion = sNombreAlmacen,
                        Cobrada = pendiente.Cobro.HasValue
                    });
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                //Recuperar el valor de la sesión guardada al iniciar el método
                if (ejercicioSession.HasValue)
                    HttpContext.Session.SetInt32(Consts.SESSION_EJERCICIO, Convert.ToInt32(ejercicioSession.Value));
            }
        }

        public async Task<FileResult> UnpaidExpensesCsv(DateTime? start, DateTime? end, string Cliente = "", string Vendedor = "", string Almacen = "", bool? FilterEstado = null, bool? FilterRetencion = null)
        {
            GetUnpaidExpensesSage(start, end, out List<UnpaidExpensesViewModel> records, Cliente, Vendedor, Almacen, FilterEstado, FilterRetencion);
            string fileName = "informe.csv";
            return await GetCsvAsync<UnpaidExpensesViewModel, UnpaidExpensesViewModelMap>(records, fileName);
        }

        #endregion

        private async Task<FileResult> GetCsvAsync<T, E>(IEnumerable<T> records, string fileName) where E : ClassMap<T>
        {
            if (!records.Any())
                return null;

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            //using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.GetCultureInfo("es-ES")))
            {
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<E>();
                await csv.WriteRecordsAsync<T>(records);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);
        }

    }
}

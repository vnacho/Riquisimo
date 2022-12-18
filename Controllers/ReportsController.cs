using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.SageComu;
using Ferpuser.Models.ViewModels;
using Ferpuser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Z.EntityFramework.Plus;

namespace Ferpuser.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SageContext _sage;
        private readonly SageComuContext _sageComu;
        
        public ReportsController(ApplicationDbContext context, SageContext sage, SageComuContext comu)
        {
            _context = context;
            _sage = sage;
            _sageComu = comu;
        }
        [Authorize(Policy = "Facturacion")]
        public IActionResult UnpaidExpenses(DateTime? start, DateTime? end, string Cliente = "", string Vendedor = "", string Almacen = "", bool? FilterEstado = null, bool? FilterRetencion = null)
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
        [Authorize(Policy = "Facturacion")]
        public async Task<FileResult> UnpaidExpensesCsv(DateTime? start, DateTime? end, string Cliente = "", string Vendedor = "", string Almacen = "", bool? FilterEstado = null, bool? FilterRetencion = null)
        {
            GetUnpaidExpensesSage(start, end, out List<UnpaidExpensesViewModel> records, Cliente, Vendedor, Almacen, FilterEstado, FilterRetencion);
            string fileName = "informe.csv";
            return await GetCsvAsync<UnpaidExpensesViewModel, UnpaidExpensesViewModelMap>(records, fileName);
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
                    if (FilterRetencion.HasValue && cliente.RETENCION != FilterRetencion)
                        continue;

                    vm.Add(new UnpaidExpensesViewModel()
                    {
                        Factura = pendiente.Factura,
                        Fecha = dtFechaFactura,
                        Cliente = pendiente.Cliente,
                        Nombre = cliente?.Nombre,
                        TieneRetencion = cliente == null ? false : cliente.RETENCION,
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

        [Authorize(Policy = "Congress")]
        public IActionResult Attendants(Guid? CongressId = null, int Filter = 0)
        {
            ViewData["Filter"] = Filter;
            return View(GetAttendants(CongressId, Filter));
        }

        [Authorize(Policy = "Congress")]
        public async Task<FileResult> AttendantsCsvAsync(Guid? CongressId = null, int Filter = 0)
        {
            List<ByClientModel> records = GetAttendants(CongressId, Filter).ToList();
            var first = records.First();
            string fileName = first.Number + "-" + first.CongressName + "-general.csv";
            return await GetCsvAsync<ByClientModel, ByClientModelMap>(records, fileName);
        }

        private IOrderedEnumerable<ByClientModel> GetAttendants(Guid? CongressId, int Filter)
        {
            ViewData["Filter"] = Filter;
            var accommodations = _context.Accommodations
                            .IncludeOptimized(r => r.BillingLocation)
                            .IncludeOptimized(r => r.Client)
                            .Include(r => r.Congress)
                            .Include(r => r.Registrant)
                                .ThenInclude(c => c.Treatment)
                            .Include(r => r.Registrant)
                                .ThenInclude(c => c.Location)
                            .Include(r => r.Registrant)
                                .ThenInclude(c => c.CategoriaInscrito)
                            .Where(r => r.Deleted == null && !r.Congress.HideRegistrations && (CongressId.HasValue && CongressId.Value.Equals(r.CongressId)));
            if (Filter == 1)
            {
                accommodations = accommodations.Where(a => a.OnlyBilling);
            }
            else if (Filter == 2)
            {
                accommodations = accommodations.Where(a => !a.OnlyBilling);
            }
            //var registrations = _context.Registrations
            //    .IncludeOptimized(r => r.BillingLocation)
            //    .IncludeOptimized(r => r.Client)
            //    .IncludeOptimized(r => r.Congress)
            //    .IncludeOptimized(r => r.RegistrationType)
            //    .Include(r => r.Registrant)
            //        .ThenInclude(c => c.Treatment)
            //    .Include(r => r.Registrant)
            //        .ThenInclude(c => c.Location)
            //    .Include(r => r.Registrant)
            //        .ThenInclude(c => c.CategoriaInscrito)
            //    .Where(r => r.Deleted == null && !r.Congress.HideRegistrations && (CongressId.HasValue && CongressId.Value.Equals(r.CongressId)));

            var registrations = _context.Registrations
                .Include(r => r.BillingLocation)
                .Include(r => r.Client)
                .Include(r => r.Congress)
                .Include(r => r.RegistrationType)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Treatment)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Location)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.CategoriaInscrito)
                .Where(r => r.Deleted == null && !r.Congress.HideRegistrations && (CongressId.HasValue && CongressId.Value.Equals(r.CongressId)));
            if (Filter == 1)
                registrations = registrations.Where(a => a.OnlyBilling);
            else if (Filter == 2)
                registrations = registrations.Where(a => !a.OnlyBilling);

            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).Where(c => !c.HideRegistrations && c.IsCongress).OrderBy(c => c.Number), "Id", "DisplayName", CongressId);

            List<ByClientModel> join = accommodations.Join(registrations,
                    a => a.Registrant.NIF,
                    r => r.Registrant.NIF,
                    (a, r) => a.CongressId.Equals(r.CongressId) ? new ByClientModel
                    {
                        CongressId = r.CongressId,
                        AccommodationId = a.Id,
                        RegistrationId = r.Id,
                        CongressName = a.Congress.Name,
                        Number = a.Congress.Number,
                        Code = a.Congress.Code,
                        LogoBase64 = a.Congress.LogoBase64,
                        RegistrationBusinessName = r.Client.BusinessName,
                        AccommodationBusinessName = a.Client.BusinessName,
                        Name = r.Registrant.Name,
                        Surnames = r.Registrant.Surnames,
                        RegistrationFee = r.Fee,
                        RegistrationType = r.RegistrationType.Description,
                        AccommodationFee = a.Fee,
                        Category = GetCategory(a, r),
                        RegistrationPaid = r.Paid,
                        AccommodationPaid = a.Paid,
                        AccommodationHotel = a.Hotel,
                        AccommodationStart = a.StartDate,
                        AccommodationEnd = a.EndDate
                    } : null
                ).ToList();

            foreach (var r in registrations.IncludeOptimized(r => r.Congress).IncludeOptimized(r => r.Client).IncludeOptimized(r => r.RegistrationType).IncludeOptimized(r => r.Registrant).ToList().Where(a => a.Deleted == null && a.CongressId.Equals(CongressId) && !join.Any(j => j.RegistrationId.Equals(a.Id))))
            {
                join.Add(new ByClientModel
                {
                    CongressId = r.CongressId,
                    RegistrationId = r.Id,
                    CongressName = r.Congress.Name,
                    Number = r.Congress.Number,
                    Code = r.Congress.Code,
                    LogoBase64 = r.Congress.LogoBase64,
                    RegistrationBusinessName = r.Client?.BusinessName,
                    Name = r.Registrant.Name,
                    Surnames = r.Registrant.Surnames,
                    Category = r.Registrant.CategoriaInscrito == null ? r.Registrant.Category : r.Registrant.CategoriaInscrito.Nombre,
                    RegistrationType = r.RegistrationType.Description,
                    RegistrationFee = r.Fee,
                    RegistrationPaid = r.Paid,
                });
            }
            var ordered = join.OrderBy(c => c.CongressNumber).ThenBy(c => c.Surnames);
            return ordered;
        }

        private static string GetCategory(Accommodation a, Registration r)
        {
            if (r.Registrant != null && r.Registrant.CategoriaInscrito != null)
                return r.Registrant.CategoriaInscrito.Nombre;
            if (r.Registrant != null && !string.IsNullOrWhiteSpace(r.Registrant.Category))
                return r.Registrant.Category;
            if (a.Registrant != null && a.Registrant.CategoriaInscrito != null)
                return a.Registrant.CategoriaInscrito.Nombre;
            if (a.Registrant != null && !string.IsNullOrWhiteSpace(a.Registrant.Category))
                return a.Registrant.Category;
            return string.Empty;
        }

        [Authorize(Policy = "Congress")]
        public IActionResult Registrants(Guid? CongressId = null, int Filter = 0)
        {
            return View(GetRegistrants(CongressId, Filter));
        }

        [Authorize(Policy = "Congress")]
        public async Task<FileResult> RegistrantsCsvAsync(Guid? CongressId = null, int Filter = 0)
        {
            List<ByClientModel> records = GetRegistrants(CongressId, Filter).ToList();
            var first = records.First();
            string fileName = first.Number + "-" + first.CongressName + "-general.csv";
            return await GetCsvAsync<ByClientModel, RegistrantsModelMap>(records, fileName);
        }
        private IOrderedEnumerable<ByClientModel> GetRegistrants(Guid? CongressId, int Filter = 0)
        {

            ViewData["Filter"] = Filter;
            var registrations = _context.Registrations
                .IncludeOptimized(r => r.BillingLocation)
                .IncludeOptimized(r => r.Client)
                .IncludeOptimized(r => r.Congress)
                .IncludeOptimized(r => r.RegistrationType)
                .IncludeOptimized(r => r.Registrant)
                .IncludeOptimizedByPath("Registrant.Location")
                .IncludeOptimizedByPath("Registrant.CategoriaInscrito")
                .Where(r => r.Deleted == null && !r.Congress.HideRegistrations && (CongressId.HasValue && CongressId.Value.Equals(r.CongressId)));
            if (Filter == 1)
                registrations = registrations.Where(a => a.OnlyBilling);
            else if (Filter == 2)
                registrations = registrations.Where(a => !a.OnlyBilling);

            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).Where(c => !c.HideRegistrations && c.IsCongress).OrderBy(c => c.Number), "Id", "DisplayName", CongressId);

            List<ByClientModel> join = new List<ByClientModel>();
            foreach (var r in registrations)
            {
                join.Add(new ByClientModel
                {
                    CongressId = r.CongressId,
                    RegistrationId = r.Id,
                    CongressName = r.Congress.Name,
                    Number = r.Congress.Number,
                    Code = r.Congress.Code,
                    LogoBase64 = r.Congress.LogoBase64,
                    RegistrationBusinessName = r.Client?.BusinessName,
                    Treatment = _context.Treatments.Find(r.Registrant.TreatmentId).Name,
                    Workplace = r.Registrant.Workplace,
                    Email = r.Registrant.Email,
                    Name = r.Registrant.Name,
                    Surnames = r.Registrant.Surnames,
                    Category = r.Registrant.CategoriaInscrito == null ? r.Registrant.Category : r.Registrant.CategoriaInscrito.Nombre,
                    RegistrationType = r.RegistrationType.Description,
                    RegistrationFee = r.Fee,
                    RegistrationPaid = r.Paid,
                    Province = r.Registrant.Location.Province
                });
            }
            var ordered = join.OrderBy(c => c.Surnames);
            return ordered;
        }

        //[Authorize(Policy = "Congress")]
        public IActionResult PaidPending(Guid? CongressId = null, string Paid = "", int Filter = 0)
        {
            List<Registration> list = GetPaidPending(CongressId, Paid, Filter);
            if (User.Claims.Any(c => c.Type.Equals(Consts.CLAIM_PERFIL_USUARIO_CLIENTE)))
            {
                ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null && c.Number == Consts.NUMBER_EVENTO_HARDCODEADO).Where(c => !c.HideRegistrations && c.IsCongress).OrderBy(c => c.Number), "Id", "DisplayName", CongressId);
            }
            else
            {
                ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).Where(c => !c.HideRegistrations && c.IsCongress).OrderBy(c => c.Number), "Id", "DisplayName", CongressId);
            }
             
            ViewData["Paid"] = Paid;
            ViewData["Filter"] = Filter;
            return View(list.ToList());
        }

        public async Task<IActionResult> PaidPendingCsv(Guid? CongressId = null, string Paid = "", int Filter = 0)
        {
            List<Registration> records = GetPaidPending(CongressId, Paid, Filter);

            var first = records.First();
            string fileName = first.Congress.Number + "-" + first.Congress.Name + "-inscripciones-pagadas-pendientes.csv";
            return await GetCsvAsync<Registration, PaidPendingMap>(records, fileName);
        }

        private List<Registration> GetPaidPending(Guid? CongressId, string Paid, int Filter = 0)
        {
            ViewData["Filter"] = Filter;

            bool? filter_pagada = null;

            if (Paid.Trim().ToLower().Equals("paid"))
                filter_pagada = true;
            else if (Paid.Trim().ToLower().Equals("unpaid"))
                filter_pagada = false;

            //var all = Paid.Trim().ToLower().Equals("all");
            //var paid = Paid.Trim().ToLower().Equals("paid");
            //var unpaid = Paid.Trim().ToLower().Equals("unpaid");

            //var registrations = _context.Registrations
            //                .Include(r => r.BillingLocation)
            //                .Include(r => r.Client)
            //                .Include(r => r.Congress)
            //                .Include(r => r.RegistrationType)
            //                .Include(r => r.Registrant)
            //                    .ThenInclude(c => c.Treatment)
            //                .Where(r => 
            //                    r.Deleted == null && 
            //                    !r.Congress.HideRegistrations && 
            //                    (all || r.Paid == paid) && 
            //                    (CongressId.HasValue && CongressId.Value.Equals(r.CongressId)))
            //                .OrderBy(r => r.Registrant.Surnames).ToList();

            var registrations = _context.Registrations
                            .Include(r => r.BillingLocation)
                            .Include(r => r.Client)
                            .Include(r => r.Congress)
                            .Include(r => r.RegistrationType)
                            .Include(r => r.Registrant)
                                .ThenInclude(c => c.Treatment)
                            .Where(r =>
                                r.Deleted == null &&
                                !r.Congress.HideRegistrations &&
                                (filter_pagada.HasValue ? r.Paid == filter_pagada : true) &&
                                (CongressId.HasValue && CongressId.Value.Equals(r.CongressId)))
                            .OrderBy(r => r.Registrant.Surnames).ToList();

            if (Filter == 1)
            {
                registrations = registrations.Where(a => a.OnlyBilling).ToList();
            }
            else if (Filter == 2)
            {
                registrations = registrations.Where(a => !a.OnlyBilling).ToList();
            }

            if (User.Claims.Any(c => c.Type.Equals(Consts.CLAIM_PERFIL_USUARIO_CLIENTE)))
            {
                registrations = registrations.Where(f => f.Congress.Number == Consts.NUMBER_EVENTO_HARDCODEADO).ToList();
            }
            return registrations;
        }

        [Authorize(Policy = "Congress")]
        public IActionResult ByWorkplace(string Workplace, string City, Guid? CongressId = null)
        {
            var applicationDbContext = _context.Registrations
                .Include(r => r.BillingLocation)
                .Include(r => r.Client)
                .Include(r => r.Congress)
                .Include(r => r.RegistrationType)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Treatment)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Location)
                .Where(r => !r.Congress.HideRegistrations &&
                    (Workplace == null || r.Registrant.Workplace.Trim().ToLower().Equals(Workplace.Trim().ToLower())) &&
                    (City == null || r.Registrant.Location.City.Trim().ToLower().Equals(City.Trim().ToLower())) &&
                    (r.Deleted == null && CongressId.HasValue && CongressId.Value.Equals(r.CongressId))).OrderBy(r => r.Registrant.Workplace);

            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).Where(c => !c.HideRegistrations && c.IsCongress).OrderBy(c => c.Number), "Id", "DisplayName", CongressId);
            if (CongressId.HasValue)
            {
                ViewData["Workplaces"] = new SelectList(_context.Registrations.Include(r => r.Registrant).Where(r => r.Deleted == null && r.CongressId.Equals(CongressId)).Select(r => r.Registrant.Workplace.Trim()).Distinct(), Workplace);
            }
            else
            {
                ViewData["Workplaces"] = new SelectList(_context.Registrant.Select(r => r.Workplace.Trim()).Distinct(), Workplace);
            }
            if (CongressId.HasValue)
            {
                ViewData["Cities"] = new SelectList(_context.Registrations.Include(r => r.Registrant).ThenInclude(rr => rr.Location).Where(r => r.Deleted == null && r.CongressId.Equals(CongressId)).Select(r => r.Registrant.Location.City).Distinct(), City);
            }
            else
            {
                ViewData["Cities"] = new SelectList(_context.Registrant.Include(r => r.Location).Select(r => r.Location.City).Distinct(), City);
            }

            return View(applicationDbContext.ToList());
        }

        [Authorize(Policy = "Congress")]
        public IActionResult ByInscriptionType(Guid? InscriptionTypeId, Guid? CongressId = null)
        {
            var applicationDbContext = _context.Registrations
                .Include(r => r.BillingLocation)
                .Include(r => r.Client)
                .Include(r => r.Congress)
                .Include(r => r.RegistrationType)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Treatment)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Location)
                .Where(r => r.Deleted == null && !r.Congress.HideRegistrations &&
                    CongressId.HasValue &&
                    (InscriptionTypeId == null || r.RegistrationTypeId.Equals(InscriptionTypeId)) &&
                    CongressId.Value.Equals(r.CongressId)
                    ).OrderBy(r => r.RegistrationType.Name.ToLower()).ThenBy(r => r.Registrant.Surnames);
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).Where(c => !c.HideRegistrations && c.IsCongress).OrderBy(c => c.Number), "Id", "DisplayName", CongressId);
            var regTypes = _context.RegistrationTypes.ToList().Where(r => applicationDbContext.Any(a => a.RegistrationTypeId.Equals(r.Id)));
            if (InscriptionTypeId.HasValue)
            {
                ViewData["InscriptionTypeId"] = new SelectList(regTypes, "Id", "Name", InscriptionTypeId);
            }
            else
            {
                ViewData["InscriptionTypeId"] = new SelectList(regTypes, "Id", "Name");
            }
            return View(applicationDbContext.ToList());
        }

        [Authorize(Policy = "Congress")]
        public IActionResult ByHotel(string Hotel, Guid CongressId)
        {
            List<Accommodation> model = GetByHotel(Hotel, CongressId);
            return base.View(model);
        }

        [Authorize(Policy = "Congress")]
        public async Task<FileResult> ByHotelCsvAsync(string Hotel, Guid? CongressId = null)
        {
            List<Accommodation> records = GetByHotel(Hotel, CongressId).OrderBy(f => f.Hotel).ThenBy(f => f.Registrant.Surnames).ToList();
            var first = records.First();
            string fileName = first.Congress.Number + "-" + first.Congress.Name + "-hotel.csv";
            return await GetCsvAsync<Accommodation, AccommodationPaidPendingMap>(records, fileName);
        }

        private List<Accommodation> GetByHotel(string Hotel, Guid? CongressId)
        {
            var result = _context.Accommodations
                .IncludeOptimized(r => r.BillingLocation)
                .IncludeOptimized(r => r.Client)
                .IncludeOptimized(r => r.Congress)
                .IncludeOptimized(r => r.RoomType)
                .IncludeOptimized(r => r.Registrant)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Treatment)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Location)
                .Where(r => r.Deleted == null && !r.Congress.HideRegistrations &&
                    (Hotel == null || r.Hotel.Equals(Hotel)) &&
                    (CongressId.Equals(r.CongressId))
                    ).OrderBy(r => r.Registrant.Surnames);

            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).Where(c => !c.HideRegistrations && c.IsCongress).OrderBy(c => c.Number), "Id", "DisplayName", CongressId);
            if (Hotel != null)
            {
                ViewData["Hotels"] = new SelectList(_context.Accommodations.Where(a => a.CongressId.Equals(CongressId)).OrderBy(a => a.Hotel).Select(a => a.Hotel).Distinct(), Hotel);
            }
            else
            {
                ViewData["Hotels"] = new SelectList(_context.Accommodations.Where(a => a.CongressId.Equals(CongressId)).OrderBy(a => a.Hotel).Select(a => a.Hotel).Distinct());
            }
            List<Accommodation> model = result.ToList();
            return model;
        }

        [Authorize(Policy = "Congress")]
        public IActionResult Accommodations(Guid? CongressId = null, int Filter = 0)
        {
            var applicationDbContext = GetAccommodations(CongressId, Filter);
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).Where(c => !c.HideRegistrations && c.IsCongress).OrderBy(c => c.Number), "Id", "DisplayName", CongressId);
            return View(applicationDbContext);
        }
        public async Task<FileResult> AccommodationsCsvAsync(Guid? CongressId = null, int Filter = 0)
        {
            List<Accommodation> records = GetAccommodations(CongressId,Filter);
            var first = records.First();
            string fileName = first.Number + "-" + first.Congress.Name + "-general.csv";
            return await GetCsvAsync<Accommodation, AccommodationMap>(records, fileName);
        }

        private List<Accommodation> GetAccommodations(Guid? CongressId, int Filter = 0)
        {
            ViewData["Filter"] = Filter;
            var accomodations = _context.Accommodations
                  .IncludeOptimized(r => r.BillingLocation)
                  .IncludeOptimized(r => r.Client)
                  .IncludeOptimized(r => r.Congress)
                  .IncludeOptimized(r => r.RoomType)
                  .IncludeOptimized(r => r.Registrant)
                  .IncludeOptimized(r => r.Registrant)
                  .IncludeOptimizedByPath("Registrant.Treatment")
                  .Where(r => r.Deleted == null && !r.Congress.HideRegistrations && (CongressId.HasValue && CongressId.Value.Equals(r.CongressId)));
            if (Filter == 1)
                accomodations = accomodations.Where(a => a.OnlyBilling);
            else if (Filter == 2)
                accomodations = accomodations.Where(a => !a.OnlyBilling);
            
            return accomodations.OrderBy(r => r.Registrant.Surnames).ToList();
        }

        //[Authorize(Policy = "Congress")]
        public IActionResult AccommodationPaidPending(Guid? CongressId = null, string Paid = "")
        {
            var all = Paid.Trim().ToLower().Equals("all");
            var paid = Paid.Trim().ToLower().Equals("paid");
            ViewData["Paid"] = Paid;

            List<Accommodation> list;
            if (User.Claims.Any(c => c.Type.Equals(Consts.CLAIM_PERFIL_USUARIO_CLIENTE)))
            {
                list = _context.Accommodations
                    .IncludeOptimized(r => r.BillingLocation)
                    .IncludeOptimized(r => r.Client)
                    .IncludeOptimized(r => r.Congress)
                    .IncludeOptimized(r => r.RoomType)
                    .IncludeOptimized(r => r.Registrant)
                    .Include(r => r.Registrant)
                        .ThenInclude(c => c.Treatment)
                    .Include(r => r.Registrant)
                        .ThenInclude(c => c.Location)
                    .Where(r => 
                        r.Congress.Number == Consts.NUMBER_EVENTO_HARDCODEADO &&
                        r.Deleted == null && 
                        !r.Congress.HideRegistrations && (all || r.Paid == paid) && 
                        (CongressId.HasValue && CongressId.Value.Equals(r.CongressId)))
                    .OrderBy(r => r.Registrant.Surnames)
                    .ToList();
            
                ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null && c.Number == Consts.NUMBER_EVENTO_HARDCODEADO).Where(c => !c.HideRegistrations && c.IsCongress).OrderBy(c => c.Number), "Id", "DisplayName", CongressId);
            }
            else
            {
                list = _context.Accommodations
                    .IncludeOptimized(r => r.BillingLocation)
                    .IncludeOptimized(r => r.Client)
                    .IncludeOptimized(r => r.Congress)
                    .IncludeOptimized(r => r.RoomType)
                    .IncludeOptimized(r => r.Registrant)
                    .Include(r => r.Registrant)
                        .ThenInclude(c => c.Treatment)
                    .Include(r => r.Registrant)
                        .ThenInclude(c => c.Location)
                    .Where(r =>
                        //r.Congress.Number == Consts.NUMBER_EVENTO_HARDCODEADO &&
                        r.Deleted == null &&
                        !r.Congress.HideRegistrations && (all || r.Paid == paid) &&
                        (CongressId.HasValue && CongressId.Value.Equals(r.CongressId)))
                    .OrderBy(r => r.Registrant.Surnames)
                    .ToList();
                ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).Where(c => !c.HideRegistrations && c.IsCongress).OrderBy(c => c.Number), "Id", "DisplayName", CongressId);
            }
            
            return View(list);
        }

        public async Task<FileResult> AccommodationPaidPendingCsv(Guid? CongressId = null, string Paid = "")
        {
            var all = Paid.Trim().ToLower().Equals("all");
            var paid = Paid.Trim().ToLower().Equals("paid");
            var list = _context.Accommodations
                .IncludeOptimized(r => r.BillingLocation)
                .IncludeOptimized(r => r.Client)
                .IncludeOptimized(r => r.Congress)
                .IncludeOptimized(r => r.RoomType)
                .IncludeOptimized(r => r.Registrant)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Treatment)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Location)
                .Where(r => r.Deleted == null && !r.Congress.HideRegistrations && (all || r.Paid == paid) && (CongressId.HasValue && CongressId.Value.Equals(r.CongressId))).OrderBy(r => r.Registrant.Surnames);

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoFacturasCompra.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<AccommodationPaidPendingViewModelMap>();
                await csv.WriteRecordsAsync<Accommodation>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

        [Authorize(Policy = "Congress")]
        public IActionResult ByClient(Guid? CongressId = null)
        {
            var data = ByClientData(CongressId);
            return View(data);
        }

        [Authorize(Policy = "Congress")]
        public async Task<FileResult> ByClientCsvAsync(Guid CongressId)
        {
            var data = ByClientData(CongressId);
            //string fileName = data.First().CongressName + "-Relación de facturadores.csv";
            string fileName = "Relación de facturadores.csv";
            return await GetCsvAsync<ByClientModel, ByClientModelFacturadoresMap>(data, fileName);
        }

        /// <summary>
        /// Obtiene los datos del informe de ByClient
        /// </summary>
        /// <param name="CongressId"></param>
        /// <returns></returns>
        private IEnumerable<ByClientModel> ByClientData(Guid? CongressId)
        {
            var accommodations = _context.Accommodations
                .IncludeOptimized(r => r.BillingLocation)
                .IncludeOptimized(r => r.Client)
                .IncludeOptimized(r => r.Congress)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Treatment)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Location)
                .AsNoTracking()
                .Where(r => r.Deleted == null && !r.Congress.HideRegistrations && (CongressId.HasValue && CongressId.Value.Equals(r.CongressId)));

            var registrations = _context.Registrations
                .IncludeOptimized(r => r.BillingLocation)
                .IncludeOptimized(r => r.Client)
                .IncludeOptimized(r => r.Congress)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Treatment)
                .Include(r => r.Registrant)
                    .ThenInclude(c => c.Location)
                .AsNoTracking()
                .Where(r => r.Deleted == null && !r.Congress.HideRegistrations && (CongressId.HasValue && CongressId.Value.Equals(r.CongressId)));

            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).AsNoTracking().Where(c => !c.HideRegistrations && c.IsCongress).OrderBy(c => c.Number), "Id", "DisplayName", CongressId);

            List<ByClientModel> join = accommodations.Join(registrations,
                    a => a.Registrant.NIF,
                    r => r.Registrant.NIF,
                    (a, r) => a.CongressId.Equals(r.CongressId) ? new ByClientModel
                    {
                        CongressId = r.CongressId,
                        CongressNumber = a.Congress.Number,
                        AccommodationId = a.Id,
                        RegistrationId = r.Id,
                        CongressName = a.Congress.Name,
                        Number = a.Congress.Number,
                        Code = a.Congress.Code,
                        LogoBase64 = a.Congress.LogoBase64,
                        RegistrationBusinessName = r.Client.BusinessName,
                        AccommodationBusinessName = a.Client.BusinessName,
                        Name = r.Registrant.Name,
                        Surnames = r.Registrant.Surnames,
                        RegistrationFee = r.Fee,
                        AccommodationFee = a.Fee,
                        RegistrationPaid = r.Paid,
                        AccommodationPaid = a.Paid
                    } : null
                ).ToList();
            foreach (var a in accommodations
                .IncludeOptimized(r => r.Congress)
                .IncludeOptimized(r => r.Client)
                .IncludeOptimized(r => r.Registrant)
                .AsNoTracking().ToList().Where(a => a.Deleted == null && a.CongressId.Equals(CongressId) && !join.Any(j => j.AccommodationId.Equals(a.Id))))
            {
                join.Add(new ByClientModel
                {
                    CongressId = a.CongressId,
                    CongressNumber = a.Congress.Number,
                    AccommodationId = a.Id,
                    CongressName = a.Congress.Name,
                    Number = a.Congress.Number,
                    Code = a.Congress.Code,
                    LogoBase64 = a.Congress.LogoBase64,
                    AccommodationBusinessName = a.Client?.BusinessName,
                    Name = a.Registrant.Name,
                    Surnames = a.Registrant.Surnames,
                    AccommodationFee = a.Fee,
                    AccommodationPaid = a.Paid,
                    AccommodationHotel = a.Hotel,
                    AccommodationStart = a.StartDate,
                    AccommodationEnd = a.EndDate
                });
            }

            foreach (var r in registrations
                .IncludeOptimized(r => r.Congress)
                .IncludeOptimized(r => r.Client)
                .IncludeOptimized(r => r.Registrant)
                .AsNoTracking().ToList().Where(a => a.Deleted == null && a.CongressId.Equals(CongressId) && !join.Any(j => j.RegistrationId.Equals(a.Id))))
            {
                join.Add(new ByClientModel
                {
                    CongressId = r.CongressId,
                    CongressNumber = r.Congress.Number,
                    RegistrationId = r.Id,
                    CongressName = r.Congress.Name,
                    Number = r.Congress.Number,
                    Code = r.Congress.Code,
                    LogoBase64 = r.Congress.LogoBase64,
                    RegistrationBusinessName = r.Client?.BusinessName,
                    Name = r.Registrant.Name,
                    Surnames = r.Registrant.Surnames,
                    RegistrationFee = r.Fee,
                    RegistrationPaid = r.Paid,
                });
            }

            return join.OrderBy(f => f.RegistrationBusinessName);
        }


        [Authorize(Policy = "Congress")]
        public IActionResult Asistentes()
        {
            ViewBag.Asistentes = _context.Asistente.OrderBy(f => f.Nombre).ThenBy(f => f.Apellidos);
            ViewBag.Congresses = _context.Congresses.OrderBy(f => f.Number).ThenBy(f => f.Name);
            return View();
        }

        [Authorize(Policy = "Congress")]
        [HttpPost, ActionName("Asistentes")]
        public async Task<IActionResult> AsistentesConfirmed()
        {
            ReportAsistentesViewModel model = new ReportAsistentesViewModel();
            await TryUpdateModelAsync<ReportAsistentesViewModel>(model, "",
                f => f.NIF,
                f => f.IdCongress);

            if (ModelState.IsValid)
            {
                var registrants = _context.Registrant.Where(f => f.NIF.Trim().ToUpper() == model.NIF.ToUpper());
                foreach(var reg in registrants)
                {
                    var congresses = _context.Congresses.Where(f => f.Registrations.Any(f => f.RegistrantId == reg.Id));
                    if (model.IdCongress.HasValue)
                        congresses = congresses.Where(f => f.Id == model.IdCongress);

                    foreach (var congress in congresses)
                    {
                        Registration registration = _context.Registrations.Include(f => f.RegistrationType).FirstOrDefault(f => 
                            f.CongressId == congress.Id && 
                            f.RegistrantId == reg.Id);

                        model.Items.Add(new ReportAsistentesItem()
                        {
                            CodigoEvento = congress.Number,
                            NombreEvento = congress.Name,
                            Lugar = congress.Place,
                            FechaInicio = congress.StartDate,
                            FechaFin = congress.EndDate,
                            TipoInscripcion = registration?.RegistrationType?.Name,
                            ImporteInscripcion = registration?.Fee
                        });
                    }
                }

                model.Asistente = _context.Asistente.Include(f => f.Treatment).FirstOrDefault(f => f.NIF == model.NIF);
            }   

            ViewBag.Asistentes = _context.Asistente.OrderBy(f => f.Nombre).ThenBy(f => f.Apellidos);
            ViewBag.Congresses = _context.Congresses.OrderBy(f => f.Number).ThenBy(f => f.Name);
            return View(model);
        }


        [Authorize(Policy = "Congress")]
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
    public class ByClientModelMap : ClassMap<ByClientModel>
    {
        public ByClientModelMap()
        {
            Map(m => m.Surnames).Name("Apellidos");
            Map(m => m.Name).Name("Nombre");
            Map(m => m.RegistrationType).Name("Tipo de inscripción");
            Map(m => m.Category).Name("Categoría");
            Map(m => m.RegistrationPaid).Name("Inscripción Pagada");
            Map(m => m.RegistrationFee).Name("Cuota Inscipción");
            Map(m => m.RegistrationBusinessName).Name("Razón Social Inscripción");
            Map(m => m.AccommodationHotel).Name("Hotel");
            Map(m => m.AccommodationStart).Name("Entrada");
            Map(m => m.AccommodationEnd).Name("Salida");
            Map(m => m.AccommodationPaid).Name("Alojamiento Pagado");
            Map(m => m.AccommodationFee).Name("Cuota Alojamiento");
            Map(m => m.AccommodationBusinessName).Name("Razón Social Alojamiento");
        }
    }

    public class ByClientModelFacturadoresMap : ClassMap<ByClientModel>
    {
        public ByClientModelFacturadoresMap()
        {
            Map(m => m.RegistrationBusinessName).Name("Razón Social Inscripción").Index(0);            
            Map(m => m.Name).Name("Nombre").Index(1);
            Map(m => m.Surnames).Name("Apellidos").Index(2);
            Map(m => m.RegistrationFee).Name("Cuota Inscipción").Index(3);
            Map(m => m.RegistrationPaid).Name("Inscripción Pagada").Index(4);
            Map(m => m.AccommodationPaid).Name("Alojamiento Pagado").Index(5);
            Map(m => m.AccommodationBusinessName).Name("Razón Social Alojamiento").Index(6);
            Map(m => m.AccommodationFee).Name("Cuota Alojamiento").Index(7);
        }
    }

    public class UnpaidExpensesViewModelMap : ClassMap<UnpaidExpensesViewModel>
    {
        public UnpaidExpensesViewModelMap()
        {
            Map(m => m.Factura).Name("Factura");
            Map(m => m.FechaString).Name("Fecha");
            Map(m => m.Nombre).Name("Cliente");
            Map(m => m.PendingDisplay).Name("Pendiente");
            Map(m => m.Almacen).Name("Cod. Evento");
            Map(m => m.Descripcion).Name("Descripcion evento");
            Map(m => m.Cobrada).Name("Cobrada");
        }
    }
    public class RegistrantsModelMap : ClassMap<ByClientModel>
    {
        public RegistrantsModelMap()
        {
            Map(m => m.Treatment).Name("Tratamiento");
            Map(m => m.Surnames).Name("Apellidos");
            Map(m => m.Name).Name("Nombre");
            Map(m => m.Province).Name("Provincia");
            Map(m => m.Category).Name("Categoría");
            Map(m => m.Workplace).Name("Centro de trabajo");
            Map(m => m.Email).Name("Email");
            Map(m => m.RegistrationBusinessName).Name("Razón social de facturación");
        }
    }
    public class AccommodationMap : ClassMap<Accommodation>
    {
        public AccommodationMap()
        {
            Map(m => m.Registrant.Treatment.Name).Name("Tratamiento");
            Map(m => m.Registrant.Surnames).Name("Apellidos");
            Map(m => m.Registrant.Name).Name("Nombre");
            Map(m => m.Hotel).Name("Hotel").Optional().Default("No indicado");
            Map(m => m.RoomType.Description).Name("Tipo de habitación");
            Map(m => m.StartDate).Name("Entrada");
            Map(m => m.EndDate).Name("Salida");
        }
    }
    public class AccommodationPaidPendingMap : ClassMap<Accommodation>
    {
        public AccommodationPaidPendingMap()
        {
            Map(m => m.Hotel).Name("Hotel").Optional().Default("No indicado");
            Map(m => m.Registrant.Surnames).Name("Apellidos");
            Map(m => m.Registrant.Name).Name("Nombre");
            Map(m => m.StartDate).Name("Entrada");
            Map(m => m.EndDate).Name("Salida");
            Map(m => m.RoomType.Name).Name("Tipo de habitación");
            Map(m => m.RoomType.Occupants).Name("Ocupantes");
        }
    }
    public class PaidPendingMap : ClassMap<Registration>
    {
        public PaidPendingMap()
        {
            Map(r => r.Registrant.Surnames).Name("Apellidos").Index(0);
            Map(r => r.Registrant.Treatment.Name).Name("Tratamiento").Index(1);
            Map(r => r.Registrant.Name).Name("Nombre").Index(2);
            Map(r => r.RegistrationType.Name).Name("Tipo de inscripción").Index(3);
            Map(r => r.InvoiceNumber).Name("Nº Factura").Index(4);
            Map(r => r.FeeDisplay).Name("Precio").Index(5);
            Map(r => r.Client.BusinessName).Name("Cliente").Index(6);
            Map(r => r.Paid).Name("Pagado").Index(7);
        }
    }    

    public class ComprasPedidoViewModelMap : ClassMap<CompraPedido>
    {
        public ComprasPedidoViewModelMap()
        {
            Map(m => m.CodigoPedido).Name("Código");
            Map(m => m.Fecha).Name("Fecha");
            Map(m => m.NombreProveedor).Name("Proveedor");
            Map(m => m.NombreOperario).Name("Operario");
            Map(m => m.Total).Name("Total");
        }
    }

    public class VentasPedidoViewModelMap : ClassMap<VentaPedido>
    {
        public VentasPedidoViewModelMap()
        {
            Map(m => m.Fecha).Name("Fecha");
            Map(m => m.NombreCliente).Name("Cliente");
            Map(m => m.NombreVendedor).Name("Operario");
            Map(m => m.BaseImponible).Name("Total");
        }
    }

    public class ComprasAlbaranViewModelMap : ClassMap<CompraAlbaran>
    {
        public ComprasAlbaranViewModelMap()
        {
            Map(m => m.CodigoAlbaran).Name("Código");
            Map(m => m.Fecha).Name("Fecha");
            Map(m => m.NombreProveedor).Name("Proveedor");
            Map(m => m.NombreOperario).Name("Operario");
            Map(m => m.Total).Name("Total");
        }
    }

    public class VentasAlbaranViewModelMap : ClassMap<VentaAlbaran>
    {
        public VentasAlbaranViewModelMap()
        {
            Map(m => m.CodigoAlbaran).Name("Código");
            Map(m => m.Fecha).Name("Fecha");
            Map(m => m.NombreCliente).Name("Cliente");
            Map(m => m.NombreVendedor).Name("Operario");
            Map(m => m.BaseImponible).Name("Total");
        }
    }

    public class PersonalMap : ClassMap<Personal>
    {
        public PersonalMap()
        {
            Map(r => r.Id).Name("Cód").Index(0);
            Map(r => r.NIF).Name("NIF").Index(1);
            Map(r => r.Nombre).Name("Nombre").Index(2);
            Map(r => r.FechaValidezNIF).Name("Fecha val. NIF").Index(3);
            Map(r => r.Categoria).Name("Categoría").Index(4);
            Map(r => r.RevisionMedica).Name("Rev. médica").Index(5);
            Map(r => r.FechaUltimaRevisionMedica).Name("Fecha rev. médica").Index(6);
            Map(r => r.SAP).Name("SAP").Index(7);
            Map(r => r.FechaApto).Name("Fecha apto").Index(8);
            Map(r => r.IBAN).Name("IBAN").Index(9);
            Map(r => r.CT).Name("CT").Index(10);
            Map(r => r.FechaAlta).Name("Fecha alta").Index(11);
            Map(r => r.FechaBaja).Name("Fecha baja").Index(12);
            Map(r => r.TipoUltimoContrato.Nombre).Name("Tipo último contrato").Index(13);
            Map(r => r.FechaUltimoContrato).Name("Fecha último contrato").Index(14);
            Map(r => r.Obra.NivelAnalitico2).Name("Cód Obra").Index(15);
            Map(r => r.Obra.Nombre).Name("Obra").Index(16);
            Map(r => r.PrecioHora).Name("Precio hora").Index(17);
            Map(r => r.CosteEstandar).Name("Coste estándar").Index(18);
            Map(r => r.Venta).Name("Venta").Index(19);
            Map(r => r.TipoTarifa.Nombre).Name("Tipo de tarifa").Index(20);
            Map(r => r.CentroCoste.Nombre).Name("Centro de coste").Index(21);
        }
    }

    public class PartePersonalMap : ClassMap<PartePersonal>
    {
        public PartePersonalMap()
        {
            Map(m => m.Id).Name("Id").Index(0);
            Map(m => m.Fecha).Name("Fecha").Index(1);
            Map(m => m.Unidades).Name("Unidades").Index(2);
            Map(m => m.Precio).Name("Precio").Index(3);
            Map(m => m.Importe).Name("Importe").Index(4);
            Map(m => m.Personal.Nombre).Name("Personal").Index(5);
            Map(m => m.CentroCoste.Display).Name("Centro de coste").Index(6);
        }
    }
    public class ArticulosAlmacenMap : ClassMap<ArticulosAlmacen>
    {
        public ArticulosAlmacenMap()
        {
            Map(m => m.ProductCode).Name("Código producto").Index(0);
            Map(m => m.ProductDescription).Name("Descripcción").Index(1);
            Map(m => m.ProductDescriptionExt).Name("Descripcción Ext.").Index(2);
            Map(m => m.Rate).Name("Tipo tarifa 1").Index(3);
            Map(m => m.Rate2).Name("Tipo tarifa 2").Index(4);
            Map(m => m.Price).Name("Precio").Index(5);
            Map(m => m.CentroCoste.Display).Name("Centro de coste").Index(6);
        }
    }

    public class PartesInternosAlmacenMap : ClassMap<PartesInternosAlmacen>
    {
        public PartesInternosAlmacenMap()
        {
            Map(m => m.fecha).Name("Fecha").Index(0);
            Map(m => m.ArticulosAlmacen.ProductCode).Name("Código producto").Index(1);
            Map(m => m.TariffTypeUnits).Name("TTF_1").Index(2);
            Map(m => m.TariffTypeUnits2).Name("TTF_2").Index(3);
            Map(m => m.Price).Name("Precio").Index(4);
            Map(m => m.Amount).Name("Importe").Index(5);
            Map(m => m.Destino.Display).Name("Centro de coste").Index(6);
        }
    }

    public class InventarioArticulosAlmacenMap : ClassMap<InventarioArticulosAlmacen>
    {
        public InventarioArticulosAlmacenMap()
        {
            Map(m => m.Modified).Name("Fecha").Index(0);
            Map(m => m.ArticulosAlmacen.ProductCode).Name("Código producto").Index(1);
            Map(m => m.ArticulosAlmacen.Rate).Name("TIPO UNIDAD").Index(2);
            Map(m => m.Unidades).Name("UNIDADES").Index(3);
            //Map(m => m.TariffTypeUnits2).Name("TTF_2").Index(3);
            //Map(m => m.Price).Name("Precio").Index(4);
            //Map(m => m.Amount).Name("Importe").Index(5);
            //Map(m => m.Destino.Display).Name("Centro de coste").Index(6);
        }
    }

    public class MovimientosArticulosAlmacenMap : ClassMap<MovimientosArticulosAlmacen>
    {
        public MovimientosArticulosAlmacenMap()
        {
            Map(m => m.FechaMovimiento).Name("Fecha").Index(0);
            Map(m => m.TipoMovimiento).Name("ENTRADA/SALIDA").Index(1);
            Map(m => m.ArticulosAlmacen.ProductCode).Name("Código producto").Index(1);
            Map(m => m.ArticulosAlmacen.Rate).Name("TIPO UNIDAD").Index(2);
            Map(m => m.Unidades).Name("UNIDADES").Index(3);
            Map(m => m.CentroCoste.Display).Name("Centro de coste").Index(4);
            //Map(m => m.TariffTypeUnits2).Name("TTF_2").Index(3);
            //Map(m => m.Price).Name("Precio").Index(4);
            //Map(m => m.Amount).Name("Importe").Index(5);
            //Map(m => m.Destino.Display).Name("Centro de coste").Index(6);
        }
    }

    public class ComprasFacturaViewModelMap : ClassMap<CompraFactura>
    {
        public ComprasFacturaViewModelMap()
        {
            Map(m => m.NumeroFactura).Name("NumeroFactura");
            Map(m => m.Fecha).Name("Fecha");
            Map(m => m.NombreProveedor).Name("Proveedor");
            Map(m => m.NombreOperario).Name("Operario");
            Map(m => m.NombreEvento).Name("Evento");
            Map(m => m.BaseImponible).Name("Total");
            Map(m => m.EstadoFactura).Name("Estado");
            Map(m => m.Pagada).Name("Pagada");
        }
    }

    public class VentasFacturaViewModelMap : ClassMap<VentaFactura>
    {
        public VentasFacturaViewModelMap()
        {
            Map(m => m.CodigoFactura).Name("NumeroFactura");
            Map(m => m.Fecha).Name("Fecha");
            Map(m => m.NombreCliente).Name("Cliente");
            Map(m => m.NombreVendedor).Name("Operario");
            Map(m => m.NombreEvento).Name("Evento");
            Map(m => m.BaseImponible).Name("Total");
            Map(m => m.EstadoFactura).Name("Estado");
            Map(m => m.Pagada).Name("Pagada");
        }
    }
 

    public class AccommodationPaidPendingViewModelMap : ClassMap<Accommodation>
    {
        public AccommodationPaidPendingViewModelMap()
        {
            Map(m => m.Registrant.Surnames).Name("Apellidos").Index(0);
            Map(m => m.Registrant.Name).Name("Nombre").Index(1);
            Map(m => m.RoomType.Name).Name("Nombre").Index(2);
            Map(m => m.Hotel).Name("Hotel").Index(3);
            Map(m => m.InvoiceNumber).Name("Num Factura").Index(4);
            Map(m => m.Fee).Name("Precio").Index(5);
            Map(m => m.Client.BusinessName).Name("Cliente").Index(6);
        }
    }

    public class PonenteViewModelMap : ClassMap<Ponente>
    {
        public PonenteViewModelMap()
        {
            Map(m => m.NIF).Name("NIF").Index(0);
            Map(m => m.Apellidos).Name("Apellidos").Index(1);
            Map(m => m.Nombre).Name("Nombre").Index(2);
            Map(m => m.CentroTrabajo).Name("Centro de trabajo").Index(3);
            Map(m => m.Mail).Name("Email").Index(4);
            Map(m => m.Telefono).Name("Telefono").Index(5);
        }
    }

    public class CompraFacturaMap : ClassMap<CompraFactura>
    {
        public CompraFacturaMap()
        {
            Map(m => m.NumeroFactura).Name("Factura").Index(0); ;
            Map(m => m.Fecha).Name("Fecha").Index(1); 
            Map(m => m.CodigoProveedor).Name("Proveedor").Index(2);
            Map(m => m.NombreProveedor).Name("Proveedor").Index(3);
            Map(m => m.Total).Name("Total").Index(4);
            Map(m => m.CodigoEvento).Name("Centro de coste").Index(5);
            Map(m => m.TieneRetencion).Name("Tiene retención").Index(6);
            Map(m => m.NombreEvento).Name("Evento").Index(7);
            Map(m => m.Pagada).Name("Pagada").Index(8);
        }
    }

    public class ProveedorViewModelMap : ClassMap<Proveedor>
    {
        public ProveedorViewModelMap()
        {
            Map(m => m.NIF).Name("NIF").Index(0);
            Map(m => m.NOMBRECOMERCIAL).Name("Nombre Comercial").Index(1);
            //Map(m => m.Nombre).Name("Nombre").Index(2);
            //Map(m => m.CentroTrabajo).Name("Centro de trabajo").Index(3);
            //Map(m => m.Mail).Name("Email").Index(4);
            //Map(m => m.Telefono).Name("Telefono").Index(5);
        }
    }

    public class InscritosEncuentroDtoMap : ClassMap<InscritosEncuentroDto>
    {
        public InscritosEncuentroDtoMap()
        {
            Map(m => m.registration.Registrant.Surnames).Name("Apellidos").Index(0);
            Map(m => m.registration.Registrant.Name).Name("Nombre").Index(1);
            Map(m => m.registration.Registrant.NIF).Name("NIF").Index(2);
            Map(m => m.NumeroMesa).Name("Numero mesa").Index(3); ;

        }
    }

    public class SocioSociedadCientificaMap : ClassMap<SocioSociedadCientifica>
    {
        public SocioSociedadCientificaMap()
        {
            Map(m => m.NIF).Name("NIF").Index(0);
            Map(m => m.Nombre).Name("Nombre").Index(1);
            Map(m => m.Apellidos).Name("Apellidos").Index(2);
            Map(m => m.FechaInicioCargo).Name("Fecha inicio de cargo").Index(3); ;
            Map(m => m.FechaFinCargo).Name("Fecha fin de cargo").Index(4); ;
            Map(m => m.JuntaDirectiva).Name("Junta directiva").Index(5); ;
            Map(m => m.CargoJuntaDirectivaSociedadCientifica.Nombre).Name("Cargo").Index(6);
        }
    }

    public class ByClientModel
    {
        public Guid CongressId { get; set; }
        [Display(Name = "Código")]
        public int Number { get; set; } = 1;
        [Display(Name = "Nombre")]
        public string CongressName { get; set; }
        [Display(Name = "Logo")]
        public string LogoBase64 { get; set; }
        [Display(Name = "Clave")]
        public string Code { get; set; }

        [Display(Name = "Razón Social Inscripción")]
        public string RegistrationBusinessName { get; set; }
        [Display(Name = "Razón Social Alojamiento")]
        public string AccommodationBusinessName { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Tratamiento")]
        public string Treatment { get; set; }

        [Display(Name = "Categoría")]
        public string Category { get; set; }

        [Display(Name = "Apellidos")]
        public string Surnames { get; set; }
        [Display(Name = "Cuota Inscipción")]
        public decimal RegistrationFee { get; set; }
        [Display(Name = "Cuota Alojamiento")]
        public decimal AccommodationFee { get; set; }

        [Display(Name = "Inscripción Pagada")]
        public bool? RegistrationPaid { get; set; }

        [Display(Name = "Alojamiento Pagado")]
        public bool? AccommodationPaid { get; set; }
        public Guid AccommodationId { get; internal set; }
        public Guid RegistrationId { get; internal set; }
        [Display(Name = "Tipo de inscripción")]
        public string RegistrationType { get; internal set; }
        [Display(Name = "Hotel")]
        public string AccommodationHotel { get; internal set; }
        [Display(Name = "Entrada")]
        public DateTime? AccommodationStart { get; internal set; }
        [Display(Name = "Salida")]
        public DateTime? AccommodationEnd { get; internal set; }
        public int CongressNumber { get; internal set; }
        [Display(Name = "Centro de trabajo")]
        public string Workplace { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Provincia")]
        public string Province { get; set; }
    }
}
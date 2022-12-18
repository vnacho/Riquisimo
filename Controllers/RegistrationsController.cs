using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Z.EntityFramework.Plus;
using Microsoft.AspNetCore.Authorization;
using Ferpuser.MailSender;
using Ferpuser.ViewModels;
using Ferpuser.Models.Consts;
using Ferpuser.Models.SessionObjects;
using Ferpuser.Models.Core;
using Ferpuser.BLL.Filters;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using Ferpuser.BLL.Managers;
using Serilog;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Congress")]
    public class RegistrationsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly SageContext _sage;
        private readonly SageComuContext _sagecomu;

        public RegistrationsController(ApplicationDbContext context, SageContext sage, SageComuContext sagecomu)
        {
            db = context;
            _sage = sage;
            _sagecomu = sagecomu;
        }

        public async Task<IActionResult> Index(int? pag, string sort = "", string currentsort = "", bool reset = false)
        {
            if (reset)
            {
                HttpContext.Session.Remove(Consts.SESSION_REGISTRATION_LIST_STATE);
                return RedirectToAction("Index");
            }

            //Actualizamos los estados de los registros (pagadas/no pagadas)
            RegistrationManager manager = new RegistrationManager(db, _sagecomu);
            await manager.ActualizarPagadas();

            RegistrationSession objSesion;
            Pager pager;
            RegistrationFilter filter = new RegistrationFilter();

            string sSesion = HttpContext.Session.GetString(Consts.SESSION_REGISTRATION_LIST_STATE);

            var previous = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            //Viene de una página que quiere restaurar el estado del listado cuando se fue de él
            if (!string.IsNullOrWhiteSpace(previous) && previous.Contains("/Registrations/Edit", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(sSesion))
            {
                objSesion = JsonConvert.DeserializeObject<RegistrationSession>(sSesion);
                sort = objSesion.sort;
                filter = objSesion.filter;
                pager = new Pager(await db.Registrations.Where(r => r.Deleted == null && !r.Congress.HideRegistrations).CountAsync(filter.ExpressionFilter()), objSesion.page, 50, 5);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(sort))
                {
                    if (string.IsNullOrWhiteSpace(currentsort))
                        sort = "Created desc";
                    else
                        sort = currentsort;
                }
                await TryUpdateModelAsync<RegistrationFilter>(filter, "filter",
                    f => f.Estado,
                    f => f.InvoiceNumber,
                    f => f.Registrant,
                    f => f.Congress,
                    f => f.CodigoEvento,
                    f => f.ClientId,
                    f => f.Cliente
                );

                pager = new Pager(await db.Registrations.Where(r => r.Deleted == null && !r.Congress.HideRegistrations).CountAsync(filter.ExpressionFilter()), pag, 50, 5);
            }

            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;
            ViewData["Filter"] = filter;

            //Guardado del estado de la búsqueda, ordenación y paginación por si se quiere recuperar en una posterior visita a esta página
            objSesion = new RegistrationSession()
            {
                filter = filter,
                sort = sort,
                page = pager.Page
            };
            HttpContext.Session.SetString(Consts.SESSION_REGISTRATION_LIST_STATE, JsonConvert.SerializeObject(objSesion));           

            var list = db.Registrations
                .Include(r => r.Client)
                .Include(r => r.Congress)
                .Include(r => r.Registrant)
                .Include(r => r.RegistrationType)
                .Where(r => r.Deleted == null && !r.Congress.HideRegistrations)
                .Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .Select(r => new Registration
                {
                    Imported = r.Imported,
                    Reviewed = r.Reviewed,
                    Exported = r.Exported,
                    Number = r.Number,
                    Paid = r.Paid,
                    Fee = r.Fee,
                    Id = r.Id,
                    InvoiceNumber = r.InvoiceNumber,
                    Registrant = new Registrant
                    {
                        Id = r.RegistrantId,
                        Name = r.Registrant.Name,
                        Surnames = r.Registrant.Surnames
                    },
                    Congress = new Congress
                    {
                        Id = r.CongressId,
                        Name = r.Congress.Name,
                        Number = r.Congress.Number,
                        Code = r.Congress.Code
                    },
                    Client = new Client
                    {
                        Id = r.ClientId.Value,
                        BusinessName = r.Client.BusinessName
                    }
                }).AsNoTracking();


            if (!string.IsNullOrWhiteSpace(filter.CodigoEvento))
                ViewBag.EventoNombre = _sage.Almacen.SingleOrDefault(f => f.Codigo == filter.CodigoEvento)?.DisplayName;

            if (filter.ClientId.HasValue)
                ViewBag.ClienteNombre = db.Clients.SingleOrDefault(f => f.Id == filter.ClientId)?.BusinessName;

            return View(list.ToList());
        }

        public IActionResult Unsent()
        {
            var unsent = db.Registrations
                .Include(r => r.BillingLocation)
                .Include(r => r.Client)
                .Include(r => r.Congress)
                .Include(r => r.Registrant)
                .Include(r => r.RegistrationType).Where(r => r.Deleted == null && !r.Congress.HideRegistrations && !r.Imported && r.Reviewed)
                .Select(r => Selector.SelectRegistration(r)).AsNoTracking();

            var unreviewed = db.Registrations
                .Include(r => r.BillingLocation)
                .Include(r => r.Client)
                .Include(r => r.Congress)
                .Include(r => r.Registrant)
                .Include(r => r.RegistrationType).Where(r => r.Deleted == null && !r.Congress.HideRegistrations && !r.Reviewed)
                .Select(r => Selector.SelectRegistration(r)).AsNoTracking();
            var vm = new HomeViewModel
            {
                Unsent = unsent.ToList(),
                Unreviewed = unreviewed.ToList(),
            };
            return View(vm);
        }

        public IActionResult Create(int? PonenteId)
        {
            var account = db.Accounts.AsNoTracking().FirstOrDefault(a => a.Name.Equals(User.Identity.Name));
            ViewData["AccountId"] = new SelectList(db.Accounts, "Id", "Name", account.Id);

            var registration = new Registration
            {
                Registrant = new Registrant
                {
                    Location = new RegistrantLocation()
                },
                BillingLocation = new ClientLocation(),
                Client = new Client(),
                AccountId = account.Id
            };
            registration.Registrant.Location.RegistrantId = registration.Registrant.Id;
            registration.BillingLocation.ClientId = registration.Client.Id;

            var treatments = db.Treatments.ToList();

            if (PonenteId.HasValue)
            {
                Ponente model = db.Ponentes.SingleOrDefault(f => f.Id == PonenteId.Value);
                if (model != null)
                {   
                    //Arreglo para conseguir el tratamiento.
                    //Esto de los tratamientos no debería haberse dejado administrar por tablas en bd, sino ser simplemente un enumerado
                    Guid tratamientoId = treatments.First().Id;
                    if (model.Tratamiento.HasValue)
                    {                        
                        switch (model.Tratamiento.Value)
                        {
                            case Models.Enums.Tratamiento.D:
                            case Models.Enums.Tratamiento.DrD:
                                tratamientoId = treatments.First().Id;
                                break;
                            case Models.Enums.Tratamiento.Da:
                            case Models.Enums.Tratamiento.DraDa:
                                tratamientoId = treatments.Skip(1).FirstOrDefault().Id;
                                break;
                        }
                    }

                    registration.CongressId = model.CongressId;
                    registration.Registrant.TreatmentId = tratamientoId;
                    registration.Registrant.Name = model.Nombre;
                    registration.Registrant.Surnames = model.Apellidos;
                    registration.Registrant.NIF = model.NIF;
                    registration.Registrant.Email = model.Mail;
                    registration.Registrant.Email2 = model.Mail2;
                    registration.Registrant.Position = model.Cargo;
                    registration.Registrant.Workplace = model.CentroTrabajo;
                    registration.Registrant.Location.Province = model.Provincia;
                    registration.Registrant.Location.Phone = model.Movil;
                    registration.Registrant.Location.Phone2 = model.Telefono;

                }
            }

            ViewData["ClientId"] = new SelectList(db.Clients, "Id", "FullName", registration.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA, "Codigo", "Nombre", registration.VATId);
            ViewData["CongressId"] = new SelectList(db.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).Where(c => !c.HideRegistrations && c.IsCongress), "Id", "DisplayName", registration.CongressId);
            ViewData["RegistrationTypeId"] = new SelectList(db.RegistrationTypes.Where(rt => rt.Deleted == null), "Id", "Name", registration.RegistrationTypeId);
            ViewData["TreatmentId"] = new SelectList(treatments.Where(t => t.Deleted == null), "Id", "Name", registration.Registrant.TreatmentId);
            ViewBag.CategoriasInscritos = db.CategoriasInscritos.OrderBy(f => f.Nombre);

            return View(registration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Registration registration)
        {
            //FixModelState(registration, out bool addClient, out bool addMainLocation, out bool addBillingLocation);
            if (ModelState.IsValid)
            {
                var max = 1;
                if (db.Registrations.Any(r => r.Deleted == null && r.CongressId.Equals(registration.CongressId)))
                {
                    max = db.Registrations.Where(r => r.Deleted == null && r.CongressId.Equals(registration.CongressId)).Max(r => r.Number) + 1;
                }
                registration.Number = max;
                db.Add(registration);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(db.Accounts, "Id", "Name", registration.AccountId);

            ViewData["BillingLocationId"] = new SelectList(db.ClientLocations.Where(l => l.ClientId.Equals(registration.ClientId)), "Id", "FullAddressWithPhone", registration.BillingLocationId);
            ViewData["ClientId"] = new SelectList(db.Clients, "Id", "FullName", registration.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA, "Codigo", "Nombre", registration.VATId);
            ViewData["CongressId"] = new SelectList(db.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).Where(c => !c.HideRegistrations && c.IsCongress), "Id", "DisplayName", registration.CongressId);
            ViewData["MainLocationId"] = new SelectList(db.RegistrantLocations.Where(l => l.RegistrantId.Equals(registration.ClientId)), "Id", "FullAddressWithPhone", registration.Registrant.LocationId);
            ViewData["RegistrationTypeId"] = new SelectList(db.RegistrationTypes.Where(rt => rt.Deleted == null), "Id", "Name", registration.RegistrationTypeId);
            ViewData["TreatmentId"] = new SelectList(db.Treatments.Where(t => t.Deleted == null), "Id", "Name", registration.Registrant.TreatmentId);

            ViewBag.CategoriasInscritos = db.CategoriasInscritos.OrderBy(f => f.Nombre);
            return View(registration);
        }

        public async Task<IActionResult> Edit(Guid? id, string returnUrl = "")
        {
            if (id == null)
                return NotFound();

            //Comprobar y actualizar que la factura ha sido cobrada
            //RegistrationManager manager = (RegistrationManager)HttpContext.RequestServices.GetService(typeof(RegistrationManager));
            RegistrationManager manager = new RegistrationManager(db, _sagecomu);
            manager.ComprobarPagada(id.Value);

            var registration = await db.Registrations
                .Include(r => r.Client)
                .ThenInclude(rg => rg.Locations)
                .Include(r => r.Registrant)
                .ThenInclude(rg => rg.Location)
                .Include(r => r.BillingLocation)
                .Include(r => r.Account)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            if (registration == null)
                return NotFound();

            if (registration.Account == null)
                registration.Account = db.Accounts.AsNoTracking().FirstOrDefault(a => a.Name.Equals(User.Identity.Name));

            ViewData["ExisteSAGE"] = false;
            if (registration.Client != null)
            {
                var clientSAGE = await _sage.Clientes.FirstOrDefaultAsync(c => c.CIF.Trim().ToLower().Equals(registration.Client.NIF.ToLower().Trim()));
                ViewData["ExisteSAGE"] = clientSAGE != null;
            }

            ViewData["AccountId"] = new SelectList(db.Accounts, "Id", "Name", registration.AccountId);
            ViewData["BillingLocationId"] = new SelectList(db.ClientLocations.Where(cl => cl.ClientId.Equals(registration.ClientId)), "Id", "FullAddressWithPhone", registration.BillingLocationId);
            ViewData["ClientId"] = new SelectList(db.Clients, "Id", "FullName", registration.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA, "Codigo", "Nombre", registration.VATId);
            ViewData["CongressId"] = new SelectList(db.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).Where(c => !c.HideRegistrations && c.IsCongress), "Id", "DisplayName", registration.CongressId);
            ViewData["RegistrationTypeId"] = new SelectList(db.RegistrationTypes.Where(rt => rt.Deleted == null), "Id", "Name", registration.RegistrationTypeId);
            ViewData["TreatmentId"] = new SelectList(db.Treatments.Where(t => t.Deleted == null), "Id", "Name", registration.Registrant.TreatmentId);
            ViewData["returnUrl"] = returnUrl;

            ViewBag.CategoriasInscritos = db.CategoriasInscritos.OrderBy(f => f.Nombre);

            //Comprobar si el nif de la persona inscrita está dado de alta en asistentes
            SetViewBag(registration);

            return View(registration);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Registration registration, string returnUrl = "")
        {
            bool bExportarAsistente = false;

            if (id != registration.Id)
                return NotFound();            

            if (ModelState.IsValid)
            {
                //04/10/2021
                //Si el registro se ha marcado como revisado, hay que comprobar que su estado anterior en desactivado.
                //Si se cumple todo entonces significa que ha cambiado su estado y hay que exportar los datos a la tabla asistentes.
                if (registration.Reviewed)
                {
                    var currentRegistration = db.Registrations.AsNoTracking().FirstOrDefault(f => f.Id == id);
                    if (currentRegistration != null && !currentRegistration.Reviewed)
                        bExportarAsistente = true;
                }

                try
                {
                    bool clienteNuevo = true;
                    
                    //hasClient = true => solo si es de tipo Invitado
                    var hasClient = !registration.RegistrationTypeId.Equals(Guid.Parse("0a2ac937-5ddd-458c-8155-b7cc648eb1e0"));
                    if (hasClient)
                    {
                        if (registration.Client != null && registration.Client.Id != Guid.Empty)
                            registration.ClientId = registration.Client.Id;

                        if (db.Clients.Any(f => f.Id == registration.ClientId))
                            clienteNuevo = false;

                        Client client = null;
                        //if (!registration.ClientId.HasValue || (registration.ClientId.HasValue && registration.ClientId == Guid.Empty))
                        //{
                        //    //No hay cliente relacionado, buscar un cliente con el mismo nif por si está dado de alta
                        //    client = _context.Clients.FirstOrDefault(c => c.NIF.ToLower().Trim().Equals(registration.Client.NIF.ToLower().Trim()));
                        //    if (client != null)
                        //    {
                        //        //Aplicar los cambios al cliente                                
                        //        client.Email = registration.Client.Email;
                        //        client.Email2 = registration.Client.Email2;
                        //        client.BusinessName = registration.Client.BusinessName;
                        //        client.NIF = registration.Client.NIF;
                        //        registration.Client = null;
                        //        registration.ClientId = client.Id;

                        //        clienteNuevo = false;
                        //    }
                        //}
                        if (registration.Client != null && !string.IsNullOrWhiteSpace(registration.Client.NIF))
                        {
                            //Buscar un cliente con el mismo nif por si está dado de alta
                            client = db.Clients.FirstOrDefault(c => c.NIF.ToLower().Trim().Equals(registration.Client.NIF.ToLower().Trim()));
                            if (client != null)
                            {
                                //Aplicar los cambios al cliente                                
                                client.Email = registration.Client.Email;
                                client.Email2 = registration.Client.Email2;
                                client.BusinessName = registration.Client.BusinessName;
                                client.NIF = registration.Client.NIF;
                                registration.Client = null;
                                registration.ClientId = client.Id;

                                clienteNuevo = false;
                            }
                        }

                        if (registration.BillingLocation != null)
                        {
                            if (!clienteNuevo)
                            {
                                if (client != null)
                                    registration.BillingLocation.ClientId = client.Id;
                                else
                                    registration.BillingLocation.ClientId = registration.ClientId;
                            }
                        }   
                        if (clienteNuevo)
                        {
                            if (registration.BillingLocation != null)
                                registration.BillingLocation.Client = registration.Client;
                            if (registration.Client != null)
                                db.Add(registration.Client);
                        }

                        if (registration.BillingLocationId == null && registration.BillingLocation != null && registration.BillingLocation.Id != Guid.Empty)
                            registration.BillingLocationId = registration.BillingLocation.Id;

                        //if (_context.ClientLocations.Find(registration.BillingLocationId) == null)
                        //    _context.Add(registration.BillingLocation);

                        if (!db.ClientLocations.Any(f => f.Id == registration.BillingLocationId) && registration.BillingLocation != null)
                            db.Add(registration.BillingLocation);

                        //if (_context.Clients.Find(registration.ClientId) == null)
                        //    _context.Add(client);
                    }
                    else
                    {
                        registration.BillingLocation = null;
                    }
                    registration.Modified = DateTime.Now;
                    registration.VAT = _sage.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(registration.VATId)).IVA;
                    db.Update(registration);
                    await db.SaveChangesAsync();
                    //if (!string.IsNullOrWhiteSpace(returnUrl))
                    //    return Redirect(returnUrl);

                    //04/10/2021
                    //Si ha cambiado su estado hay que exportar los datos a la tabla asistentes.
                    if (bExportarAsistente)
                    {
                        try
                        {
                            AsistenteManager manager = new AsistenteManager(db);
                            await manager.ExportToAsistente(id);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "Error al exportar la inscripción al asistente.");
                        }
                    }

                    return RedirectToAction("Edit", new { id = id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            ViewData["AccountId"] = new SelectList(db.Accounts, "Id", "Name", registration.AccountId);
            ViewData["BillingLocationId"] = new SelectList(db.ClientLocations.Where(cl => cl.ClientId.Equals(registration.ClientId)), "Id", "FullAddressWithPhone", registration.BillingLocationId);
            ViewData["ClientId"] = new SelectList(db.Clients, "Id", "FullName", registration.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA, "Codigo", "Nombre", registration.VATId);
            ViewData["CongressId"] = new SelectList(db.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).Where(c => !c.HideRegistrations && c.IsCongress), "Id", "DisplayName", registration.CongressId);
            ViewData["RegistrationTypeId"] = new SelectList(db.RegistrationTypes.Where(rt => rt.Deleted == null), "Id", "Name", registration.RegistrationTypeId);
            ViewData["TreatmentId"] = new SelectList(db.Treatments.Where(t => t.Deleted == null), "Id", "Name", registration.Registrant.TreatmentId);
            ViewData["ShowBillingLocation"] = true;
            ViewData["ExisteSAGE"] = false;

            ViewBag.CategoriasInscritos = db.CategoriasInscritos.OrderBy(f => f.Nombre);

            var clientSAGE = await _sage.Clientes.FirstOrDefaultAsync(c => c.CIF.Trim().ToLower().Equals(registration.Client.NIF.ToLower().Trim()));
            if (clientSAGE != null)
                ViewData["ExisteSAGE"] = true;

            SetViewBag(registration);
            return View(registration);
        }

        private void SetViewBag(Registration registration)
        {
            ViewBag.IdAsistente = null;
            string sNifFormateado = registration.Registrant?.NIF;
            if (!string.IsNullOrWhiteSpace(sNifFormateado))
            {
                var asistente = db.Asistente.FirstOrDefault(f => f.NIF == sNifFormateado);
                if (asistente != null)
                    ViewBag.IdAsistente = asistente.Id;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Notify(Guid? id, [FromBody]object request, EmailType emailType = EmailType.Notification)
        {
            if (id == null)
                return NotFound();

            var registration = await db.Registrations
                .Include(r => r.Congress)
                .Include(r => r.Client)
                .Include(r => r.DocumentType)
                .Include(r => r.BillingLocation)
                .Include(r => r.Account)
                .Include(r => r.Registrant)
                .ThenInclude(t => t.Treatment)
                .Include(r => r.Registrant)
                .ThenInclude(f => f.CategoriaInscrito)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));
            if (registration.IsInvited)
                emailType = EmailType.Notification;
            
            var user = registration.Account;
            
            if (user == null)
                user = db.Accounts.FirstOrDefault(u => u.Name.Equals(User.Identity.Name));
            
            var mail = db.CongressEmailAccounts
                .IncludeOptimized(c => c.Account)
                .IncludeOptimized(c => c.Congress)
                .FirstOrDefault(c => c.CongressId.Equals(registration.CongressId) && c.AccountId.Equals(user.Id));
            if (mail == null)
            {
                mail = new CongressEmailAccounts
                {
                    OutgoingMailPort = registration.Congress.OutgoingMailPort,
                    OutgoingMailServer = registration.Congress.OutgoingMailServer,
                    MailUser = registration.Congress.MailUser,
                    MailPassword = registration.Congress.MailPassword,
                    Congress = registration.Congress,
                    Account = registration.Account
                };
            }

            try
            {
                decimal vat = _sage.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(registration.VATId)).IVA;
                var docType = registration.DocumentType.Name;
                if (emailType == EmailType.Notification)
                    docType = "Confirmación";
                MailSender.MailSender.SendRegistration(docType, mail, registration, request, vat, emailType);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
            if (emailType == EmailType.All || emailType == EmailType.Notification)
            {
                registration.Notified = true;

                db.Update(registration);
                await db.SaveChangesAsync();
            }
            return Ok();
        }

        public async Task<IActionResult> Notification(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await db.Registrations
                .Include(r => r.Congress)
                .Include(r => r.DocumentType)
                .Include(r => r.Registrant)
                    .ThenInclude(t => t.Treatment)
                .Include(r => r.Registrant)
                    .ThenInclude(t => t.CategoriaInscrito)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            if (registration == null)
            {
                return NotFound();
            }

            try
            {
                var html = MailSender.MailSender.NotificationMailAttachment(registration.Congress, registration);
                var pdf = MailSender.MailSender.HtmlToPdf(html, registration.InvoiceDate, registration.InvoiceNumber);
                return File(pdf, "application/pdf");
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
        public async Task<IActionResult> InvoicePreview(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await db.Registrations
                .Include(r => r.Congress)
                .Include(r => r.Client)
                .Include(r => r.BillingLocation)
                .Include(r => r.DocumentType)
                .Include(r => r.Registrant)
                    .ThenInclude(t => t.Treatment)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            

            decimal vat = _sage.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(registration.VATId)).IVA;


            if (registration == null)
            {
                return NotFound();
            }

            try
            {
                var html = MailSender.MailSender.InvoiceMailAttachment(registration.Congress, registration, vat);
                var pdf = MailSender.MailSender.HtmlToPdf(html, registration.InvoiceDate, registration.InvoiceNumber);
                return File(pdf, "application/pdf");
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
        public async Task<IActionResult> NotificationMail(Guid? id, EmailType emailType = EmailType.Notification)
        {
            if (id == null)
                return NotFound();

            var registration = await db.Registrations
                .Include(r => r.Congress)
                .Include(r => r.DocumentType)
                .Include(r => r.Account)
                .Include(r => r.Registrant)
                    .ThenInclude(t => t.Treatment)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            if (registration == null)
                return NotFound();
            
            var signature = "";
            var user = registration.Account;            
            if (user == null)            
                user = db.Accounts.FirstOrDefault(u => u.Name.Equals(User.Identity.Name));
            
            var mail = db.CongressEmailAccounts.FirstOrDefault(c => c.CongressId.Equals(registration.CongressId) && c.AccountId.Equals(user.Id));
            if (mail == null)
                signature = registration.Congress.SignatureAfter;
            else
                signature = mail.SignatureAfter;
            
            try
            {
                var html = MailSender.MailSender.MailBody(signature, registration, emailType);
                return Content(html, "text/html");
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
        public async Task<IActionResult> CreditsCertificate(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await db.Registrations
                .Include(r => r.Congress)
                .Include(r => r.Registrant).ThenInclude(f => f.CategoriaInscrito)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));
            if (registration == null)
            {
                return NotFound();
            }
            return View(registration);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var registration = await db.Registrations.FindAsync(id);
            registration.Deleted = DateTime.Now;
            db.Registrations.Update(registration);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationExists(Guid id)
        {
            return db.Registrations.Any(e => e.Deleted == null && e.Id == id);
        }
    }
}

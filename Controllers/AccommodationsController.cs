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
using Ferpuser.BLL.Filters;
using Ferpuser.Models.Core;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using Ferpuser.Models.Consts;
using Newtonsoft.Json;
using Ferpuser.Models.SessionObjects;
using Ferpuser.BLL.Managers;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Congress")]
    public class AccommodationsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly SageContext _sage;
        private readonly SageComuContext _sagecomu;

        public AccommodationsController(ApplicationDbContext context, SageContext sage, SageComuContext sagecomu)
        {
            db = context;
            _sage = sage;
            _sagecomu = sagecomu;
        }

        public async Task<IActionResult> Index(int? pag, string sort = "", string currentsort = "", bool reset = false)
        {   
            if (reset)
            {   
                HttpContext.Session.Remove(Consts.SESSION_ACOMMODATION_LIST_STATE);
                return RedirectToAction("Index");
            }

            //Actualizamos los estados de los registros (pagadas/no pagadas)
            AcommodationManager manager = new AcommodationManager(db, _sagecomu);
            await manager.ActualizarPagadas();

            AcommodationSession objSesion;
            Pager pager;
            AcommodationFilter filter = new AcommodationFilter();
            
            string sSesion = HttpContext.Session.GetString(Consts.SESSION_ACOMMODATION_LIST_STATE);

            var previous = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            //Viene de una página que quiere restaurar el estado del listado cuando se fue de él
            if (!string.IsNullOrWhiteSpace(previous) && previous.Contains("/Accommodations/Edit", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(sSesion)) 
            {
                objSesion = JsonConvert.DeserializeObject<AcommodationSession>(sSesion);
                sort = objSesion.sort;
                filter = objSesion.filter;
                pager = new Pager(await db.Accommodations.Where(r => r.Deleted == null && !r.Congress.HideRegistrations).CountAsync(filter.ExpressionFilter()), objSesion.page, 50, 5);                
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
                await TryUpdateModelAsync<AcommodationFilter>(filter, "filter",
                    f => f.Estado,
                    f => f.InvoiceNumber,
                    f => f.Registrant,
                    f => f.Congress,
                    f => f.CodigoEvento,
                    f => f.ClientId,
                    f => f.Fecha,
                    f => f.RoomType,
                    f => f.NumeroOcupantes,
                    f => f.Cliente
                );

                pager = new Pager(await db.Accommodations.Where(r => r.Deleted == null && !r.Congress.HideRegistrations).CountAsync(filter.ExpressionFilter()), pag, 50, 5);
            }

            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;
            ViewData["Filter"] = filter;

            //Guardado del estado de la búsqueda, ordenación y paginación por si se quiere recuperar en una posterior visita a esta página
            objSesion = new AcommodationSession()
            {
                filter = filter,
                sort = sort,
                page = pager.Page
            };
            HttpContext.Session.SetString(Consts.SESSION_ACOMMODATION_LIST_STATE, JsonConvert.SerializeObject(objSesion));

            var list = db.Accommodations
                .Include(r => r.Registrant)
                .Include(r => r.Congress)
                .Include(r => r.RoomType)
                .Include(r => r.Client)
                .Where(r => r.Deleted == null && !r.Congress.HideRegistrations)
                .Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .Select(r => Selector.SelectAccommodation(r)).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.CodigoEvento))
                ViewBag.EventoNombre = _sage.Almacen.SingleOrDefault(f => f.Codigo == filter.CodigoEvento)?.DisplayName;

            if (filter.ClientId.HasValue)
                ViewBag.ClienteNombre = db.Clients.SingleOrDefault(f => f.Id == filter.ClientId)?.BusinessName;

            return View(list);
        }

        public IActionResult Unsent()
        {
            var unsentAcc = db.Accommodations
                .Include(r => r.BillingLocation)
                .Include(r => r.Client)
                .Include(r => r.Congress)
                .Include(r => r.Registrant).Where(r => r.Deleted == null && !r.Congress.HideRegistrations && !r.Imported && r.Reviewed)
                .Select(r => Selector.SelectAccommodation(r)).AsNoTracking();

            var unreviewedAcc = db.Accommodations
                .Include(r => r.BillingLocation)
                .Include(r => r.Client)
                .Include(r => r.Congress)
                .Include(r => r.Registrant).Where(r => r.Deleted == null && !r.Congress.HideRegistrations && !r.Reviewed)
                .Select(r => Selector.SelectAccommodation(r)).AsNoTracking();

            var vm = new HomeViewModel
            {
                UnsentAcc = unsentAcc.ToList(),
                UnreviewedAcc = unreviewedAcc.ToList(),
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Notify(Guid? id, [FromBody]object request, EmailType emailType = EmailType.Notification)
        {
            if (id == null)
                return NotFound();

            var accommodation = await db.Accommodations
                .Include(r => r.Congress)
                .Include(r => r.Client)
                .Include(r => r.BillingLocation)
                .Include(r => r.Account)
                .Include(r => r.DocumentType)
                .Include(r => r.RoomType)
                .Include(r => r.Registrant)
                    .ThenInclude(t => t.Treatment)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            if (accommodation == null)
                return NotFound();

            if (accommodation.IsInvited)
                emailType = EmailType.Notification;

            var user = accommodation.Account;
            if (user == null)
                user = db.Accounts.FirstOrDefault(u => u.Name.Equals(User.Identity.Name));
            
            var mail = db.CongressEmailAccounts
                .IncludeOptimized(c => c.Account)
                .IncludeOptimized(c => c.Congress)
                .FirstOrDefault(c => c.CongressId.Equals(accommodation.CongressId) && c.AccountId.Equals(user.Id));
            if (mail == null)
            {
                mail = new CongressEmailAccounts
                {
                    OutgoingMailPort = accommodation.Congress.OutgoingMailPort,
                    OutgoingMailServer = accommodation.Congress.OutgoingMailServer,
                    MailUser = accommodation.Congress.MailUser,
                    MailPassword = accommodation.Congress.MailPassword,
                    Congress = accommodation.Congress,
                    Account = accommodation.Account
                };
            }

            try
            {
                decimal vat = _sage.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(accommodation.VATId)).IVA; 
                var docType = accommodation.DocumentType.Name;
                if (emailType == EmailType.Notification)
                    docType = "Confirmación";
                
                MailSender.MailSender.Send(docType, mail, accommodation, request, vat, emailType);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }

            accommodation.Notified = true;

            db.Update(accommodation);
            await db.SaveChangesAsync();

            return Ok();

        }
        public async Task<IActionResult> Notification(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accommodation = await db.Accommodations
                .Include(r => r.Congress)
                .Include(r => r.DocumentType)
                .Include(r => r.Registrant)
                    .ThenInclude(t => t.Treatment)
                .Include(r => r.Registrant)
                    .ThenInclude(t => t.CategoriaInscrito)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            if (accommodation == null)
            {
                return NotFound();
            }

            try
            {
                var html = MailSender.MailSender.NotificationMailAttachment(accommodation.Congress, accommodation);

                var pdf = MailSender.MailSender.HtmlToPdf(html, accommodation.InvoiceDate, accommodation.InvoiceNumber);
                return File(pdf, "application/pdf");
                //return View((object) html);
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

            var registration = await db.Accommodations
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
            {
                return NotFound();
            }

            var accommodation = await db.Accommodations
                .Include(r => r.Congress)
                .Include(r => r.Account)
                .Include(r => r.DocumentType)
                .Include(r => r.Registrant)
                    .ThenInclude(t => t.Treatment)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            if (accommodation == null)
            {
                return NotFound();
            }
            var signature = "";
            var user = accommodation.Account;

            if (user == null)
            {
                user = db.Accounts.FirstOrDefault(u => u.Name.Equals(User.Identity.Name));
            }
            var mail = db.CongressEmailAccounts.FirstOrDefault(c => c.CongressId.Equals(accommodation.CongressId) && c.AccountId.Equals(user.Id));
            if (mail == null)
            {
                signature = accommodation.Congress.SignatureAfter;
            }
            else
            {
                signature = mail.SignatureAfter;
            }
            try
            {
                var html = MailSender.MailSender.MailBody(signature, accommodation, emailType);
                return Content(html, "text/html");
                //return View((object) html);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
        public IActionResult FromRegistration(Guid id)
        {
            var registration = db.Registrations
                .IncludeOptimized(r => r.Congress)
                .IncludeOptimized(r => r.Registrant)
                .IncludeOptimizedByPath("Registrant.Location")
                .IncludeOptimized(r => r.Client)
                .IncludeOptimized(r => r.BillingLocation)
                .FirstOrDefault(r => r.Id.Equals(id));
            var accommodation = new Accommodation
            {
                Congress = registration.Congress,
                Registrant = registration.Registrant,
                BillingLocation = registration.BillingLocation,
                Client = registration.Client,
                Hotel = "",
                RoomType = db.RoomTypes.FirstOrDefault(),
                Registration = registration
            };
            accommodation.Number = GetMaxNumber(registration.CongressId);

            db.Add(accommodation);
            db.SaveChanges();
            ////ViewData["AlternateLocationId"] = new SelectList(_context.Locations, "Id", "FullAddress", accommodation.AlternateLocationId);
            ////ViewData["BillingLocationId"] = new SelectList(_context.Locations, "Id", "FullAddress", accommodation.BillingLocationId);
            //ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "FullName", accommodation.ClientId);
            //ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", accommodation.VATId);
            //ViewData["CongressId"] = new SelectList(_context.Congresses.AsNoTracking().Where(c => !c.HideRegistrations), "Id", "DisplayName", accommodation.CongressId);
            ////ViewData["MainLocationId"] = new SelectList(_context.Locations, "Id", "FullAddress", accommodation.MainLocationId);
            //ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes.AsNoTracking().Where(rt => rt.Deleted == null), "Id", "Name", accommodation.RoomTypeId);
            //ViewData["TreatmentId"] = new SelectList(_context.Treatments.AsNoTracking().Where(t => t.Deleted == null), "Id", "Name", accommodation.Registrant.TreatmentId);

            return RedirectToAction("Edit", new { id = accommodation.Id });
        }
        public IActionResult Create(int? PonenteId)
        {
            var account = db.Accounts.AsNoTracking().FirstOrDefault(a => a.Name.Equals(User.Identity.Name));
            ViewData["AccountId"] = new SelectList(db.Accounts, "Id", "Name", account.Id);

            var accommodation = new Accommodation
            {
                Registrant = new Registrant
                {
                    Location = new RegistrantLocation()
                },
                BillingLocation = new ClientLocation(),
                Client = new Client(),
                Companion = new Registrant
                {
                    Location = new RegistrantLocation()
                },
                AccountId = account.Id
            };
            accommodation.Registrant.Location.RegistrantId = accommodation.Registrant.Id;
            accommodation.BillingLocation.ClientId = accommodation.Client.Id;

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

                    accommodation.CongressId = model.CongressId;
                    accommodation.Registrant.TreatmentId = tratamientoId;
                    accommodation.Registrant.Name = model.Nombre;
                    accommodation.Registrant.Surnames = model.Apellidos;
                    accommodation.Registrant.NIF = model.NIF;
                    accommodation.Registrant.Email = model.Mail;
                    accommodation.Registrant.Email2 = model.Mail2;
                    accommodation.Registrant.Position = model.Cargo;
                    accommodation.Registrant.Workplace = model.CentroTrabajo;
                    accommodation.Registrant.Location.Province = model.Provincia;
                    accommodation.Registrant.Location.Phone = model.Movil;
                    accommodation.Registrant.Location.Phone2 = model.Telefono;

                }
            }

            ViewData["ClientId"] = new SelectList(db.Clients.AsNoTracking(), "Id", "FullName", accommodation.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", accommodation.VATId);
            ViewData["CongressId"] = new SelectList(db.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).AsNoTracking().Where(c => !c.HideRegistrations && c.IsCongress), "Id", "DisplayName", accommodation.CongressId);
            ViewData["RoomTypeId"] = new SelectList(db.RoomTypes.AsNoTracking().Where(rt => rt.Deleted == null), "Id", "Name", accommodation.RoomTypeId);
            ViewData["TreatmentId"] = new SelectList(treatments.Where(t => t.Deleted == null), "Id", "Name", accommodation.Registrant.TreatmentId);

            ViewBag.CategoriasInscritos = db.CategoriasInscritos.OrderBy(f => f.Nombre);

            return View(accommodation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Accommodation accommodation)
        {

            //FixModelState(accommodation, out bool addClient, out bool addMainLocation, out bool addBillingLocation);
            if (accommodation.RoomType == null)
            {
                accommodation.RoomType = db.RoomTypes.AsNoTracking().FirstOrDefault(t => t.Id.Equals(accommodation.RoomTypeId));
                ModelState["RoomType"].Errors.Clear();
                ModelState["RoomType"].ValidationState = ModelValidationState.Valid;
            }
            if (accommodation.RegistrantId.Equals(Guid.Empty) && accommodation.Registrant != null)
                accommodation.RegistrantId = accommodation.Registrant.Id;
            
            if (accommodation.Companion != null && accommodation.Companion.Location != null)
                accommodation.Companion.LocationId = accommodation.Companion.Location.Id;
            
            if (ModelState.IsValid)
            {
                int max = GetMaxNumber(accommodation.CongressId);
                accommodation.Number = max;
                db.Update(accommodation.Registrant);
                accommodation.RegistrantId = accommodation.Registrant.Id;
                accommodation.Registrant = null;
                db.Add(accommodation);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(db.Accounts, "Id", "Name", accommodation.AccountId);
            ViewData["BillingLocationId"] = new SelectList(db.ClientLocations.AsNoTracking().Where(l => l.ClientId.Equals(accommodation.ClientId)), "Id", "FullAddressWithPhone", accommodation.BillingLocationId);
            ViewData["ClientId"] = new SelectList(db.Clients.AsNoTracking(), "Id", "FullName", accommodation.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", accommodation.VATId);
            ViewData["CongressId"] = new SelectList(db.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).AsNoTracking().Where(c => !c.HideRegistrations && c.IsCongress), "Id", "DisplayName", accommodation.CongressId);
            ViewData["MainLocationId"] = new SelectList(db.RegistrantLocations.AsNoTracking().Where(l => l.RegistrantId.Equals(accommodation.ClientId)), "Id", "FullAddressWithPhone", accommodation.Registrant.LocationId);
            ViewData["RoomTypeId"] = new SelectList(db.RoomTypes.AsNoTracking().Where(rt => rt.Deleted == null), "Id", "Name", accommodation.RoomTypeId);
            ViewData["TreatmentId"] = new SelectList(db.Treatments.AsNoTracking().Where(t => t.Deleted == null), "Id", "Name", accommodation.Registrant.TreatmentId);
            ViewBag.CategoriasInscritos = db.CategoriasInscritos.OrderBy(f => f.Nombre);
            return View(accommodation);
        }

        private int GetMaxNumber(Guid congressId)
        {
            var max = 1;
            if (db.Accommodations.AsNoTracking().Any(r => r.Deleted == null && r.CongressId.Equals(congressId)))
                max = db.Accommodations.AsNoTracking().Where(r => r.Deleted == null && r.CongressId.Equals(congressId)).Max(r => r.Number) + 1;
            return max;
        }

        public async Task<IActionResult> Edit(Guid? id, string returnUrl = "")
        {
            if (id == null)
                return NotFound();

            //Comprobar y actualizar que la factura ha sido cobrada
            //AcommodationManager manager = (AcommodationManager)HttpContext.RequestServices.GetService(typeof(AcommodationManager));
            AcommodationManager manager = new AcommodationManager(db, _sagecomu);
            manager.ComprobarPagada(id.Value);

            var accommodation = await db.Accommodations
                .Include(r => r.Client)
                    .ThenInclude(c => c.Locations)
                .Include(r => r.Registrant)
                    .ThenInclude(rg => rg.Location)
                .Include(r => r.BillingLocation)
                .Include(r => r.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id.Equals(id));

            if (accommodation == null)
                return NotFound();
            
            ViewData["BillingLocationId"] = new SelectList(db.ClientLocations.AsNoTracking().Where(cl => cl.ClientId.Equals(accommodation.ClientId)), "Id", "FullAddressWithPhone", accommodation.BillingLocationId);
            ViewData["ClientId"] = new SelectList(db.Clients.AsNoTracking(), "Id", "FullName", accommodation.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", accommodation.VATId);
            ViewData["CongressId"] = new SelectList(db.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).Where(c => !c.HideRegistrations && c.IsCongress).AsNoTracking(), "Id", "DisplayName", accommodation.CongressId);
            ViewData["RoomTypeId"] = new SelectList(db.RoomTypes.AsNoTracking().Where(rt => rt.Deleted == null), "Id", "Name", accommodation.RoomTypeId);
            ViewData["TreatmentId"] = new SelectList(db.Treatments.AsNoTracking().Where(t => t.Deleted == null), "Id", "Name", accommodation.Registrant.TreatmentId);
            ViewData["AccountId"] = new SelectList(db.Accounts, "Id", "Name", accommodation.AccountId);

            ViewBag.CategoriasInscritos = db.CategoriasInscritos.OrderBy(f => f.Nombre);

            SelectList selectList = SelectRegistration(accommodation);
            ViewData["RegistrationId"] = selectList;

            ViewData["returnUrl"] = returnUrl;

            return View(accommodation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Accommodation accommodation, string returnUrl = "")
        {
            if (id != accommodation.Id)
                return NotFound();
            
            if (accommodation.RoomType == null)
            {
                accommodation.RoomType = db.RoomTypes.AsNoTracking().FirstOrDefault(t => t.Id.Equals(accommodation.RoomTypeId));
                ModelState["RoomType"].Errors.Clear();
                ModelState["RoomType"].ValidationState = ModelValidationState.Valid;
            }
            if (accommodation.RegistrantId.Equals(Guid.Empty) && accommodation.Registrant != null)
                accommodation.RegistrantId = accommodation.Registrant.Id;

            //if (!accommodation.RegistrationId.HasValue) //Si la inscripción es Invitado no chequear los datos de facturación
            if (accommodation.EsInvitado) //Si es invitado no chequear los datos de facturación
            {
                accommodation.Client = null;
                accommodation.ClientId = null;
                accommodation.BillingLocation = null;
                accommodation.BillingLocationId = null;
                ModelState.Remove("Client.NIF");
                ModelState.Remove("Client.BusinessName");
                ModelState.Remove("Client.Email");
                ModelState.Remove("Client.Email2");
                ModelState.Remove("Client.Id");
                ModelState.Remove("Client.Created");
                ModelState.Remove("Client.Modified");
                ModelState.Remove("BillingLocation.Id");                
            }
            else
            {
                ModelState.Remove("Client.Id");
                ModelState.Remove("Client.Created");
                ModelState.Remove("Client.Modified");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        Client client = null;
                        if (accommodation.Client != null)
                        {
                            //Buscamos el nif entre todos los existentes
                            client = db.Clients.AsNoTracking().FirstOrDefault(c => c.NIF.ToLower().Trim().Equals(accommodation.Client.NIF.ToLower().Trim()));
                            if (client == null)
                            {
                                client = accommodation.Client;
                                client.Id = Guid.Empty;
                            }
                            else
                            {
                                client.NIF = accommodation.Client.NIF;
                                client.BusinessName = accommodation.Client.BusinessName;
                                client.Email = accommodation.Client.Email;
                                client.Email2 = accommodation.Client.Email2;
                            }

                            if (client.Id == Guid.Empty)
                                db.Add(client);
                            else
                                db.Entry<Client>(client).State = EntityState.Modified;

                            await db.SaveChangesAsync();
                            
                            accommodation.ClientId = client.Id;
                        }

                        accommodation.Client = null;

                        //Guardar accommodation
                        if (accommodation.Companion != null)
                            accommodation.CompanionId = accommodation.Companion.Id;

                        if (accommodation.BillingLocation != null && client != null)
                            accommodation.BillingLocation.ClientId = client.Id;

                        if (accommodation.BillingLocationId == null && accommodation.BillingLocation != null)
                            accommodation.BillingLocationId = accommodation.BillingLocation.Id;

                        if (accommodation.BillingLocationId != null && db.ClientLocations.Find(accommodation.BillingLocationId) == null)
                            db.Add(accommodation.BillingLocation);

                        accommodation.BillingLocation = null;

                        if (accommodation.Companion != null)
                        {
                            var companion = db.Registrant.AsNoTracking().FirstOrDefault(c => c.NIF.ToLower().Trim().Equals(accommodation.Registrant.NIF.ToLower().Trim()));
                            if (companion == null)
                                companion = accommodation.Registrant;
                            accommodation.RegistrantId = companion.Id;

                            if (db.Registrant.AsNoTracking().FirstOrDefault(t => t.Id.Equals(companion.Id)) == null)
                                db.Add(accommodation.Registrant);
                        }
                        else
                        {
                            accommodation.CompanionId = null;
                        }
                        accommodation.Companion = null;
                        accommodation.Modified = DateTime.Now;

                        accommodation.VAT = _sage.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(accommodation.VATId)).IVA;

                        if (accommodation.RegistrationId != null)
                        {
                            var r = db.Registrations.FirstOrDefault(r => r.Id.Equals(accommodation.RegistrationId));
                            r.AccommodationId = accommodation.Id;
                            db.Update(r);

                            //accommodation.RegistrationId = null; //JIR, 28/10/2021. Si no se introduce esta línea da un error en algún caso.
                        }

                        db.Update(accommodation);

                        await db.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccommodationExists(accommodation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(returnUrl);
                }
            }
            ViewData["AccountId"] = new SelectList(db.Accounts, "Id", "Name", accommodation.AccountId);

            ViewData["BillingLocationId"] = new SelectList(db.ClientLocations.AsNoTracking().Where(cl => cl.ClientId.Equals(accommodation.ClientId)), "Id", "FullAddressWithPhone", accommodation.BillingLocationId);
            ViewData["ClientId"] = new SelectList(db.Clients.AsNoTracking(), "Id", "FullName", accommodation.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", accommodation.VATId);
            ViewData["CongressId"] = new SelectList(db.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).Where(c => !c.HideRegistrations && c.IsCongress).AsNoTracking(), "Id", "DisplayName", accommodation.CongressId);
            ViewData["RoomTypeId"] = new SelectList(db.RoomTypes.AsNoTracking().Where(rt => rt.Deleted == null), "Id", "Name", accommodation.RoomTypeId);
            ViewData["TreatmentId"] = new SelectList(db.Treatments.AsNoTracking().Where(t => t.Deleted == null), "Id", "Name", accommodation.Registrant.TreatmentId);

            ViewBag.CategoriasInscritos = db.CategoriasInscritos.OrderBy(f => f.Nombre);

            ViewData["ShowBillingLocation"] = true;
            SelectList selectList = SelectRegistration(accommodation);
            ViewData["RegistrationId"] = selectList;
            return View(accommodation);
        }

        private SelectList SelectRegistration(Accommodation accommodation)
        {
            return new SelectList(db.Registrations.IncludeOptimized(r => r.Client).IncludeOptimized(r => r.Registrant).AsNoTracking()
                .Where(
                t => t.Deleted == null &&
                t.AccommodationId.Equals(accommodation.Id) ||
                (
                    t.AccommodationId == null &&
                    t.CongressId.Equals(accommodation.CongressId) &&
                    t.ClientId.Equals(accommodation.ClientId) &&
                    (
                        t.RegistrantId.Equals(accommodation.RegistrantId) || 
                        (!string.IsNullOrWhiteSpace(t.Registrant.NIF) && t.Registrant.NIF.ToLower().EndsWith((accommodation.Registrant.NIF ?? String.Empty).ToLower()))
                    )
                )
                ).OrderBy(r => r.Number), "Id", "DisplayName", accommodation.RegistrationId);
        }

        //private SelectList SelectRegistrationForEdit(Accommodation accommodation)
        //{
        //    var list = _context
        //        .Registrations
        //        .IncludeOptimized(r => r.Client)
        //        .IncludeOptimized(r => r.Registrant)
        //        .AsNoTracking()
        //        .Where(
        //            t => t.Deleted == null &&
        //            t.AccommodationId.Equals(accommodation.Id) ||
        //            (
        //                t.AccommodationId == null &&
        //                t.CongressId.Equals(accommodation.CongressId) &&
        //                t.ClientId.Equals(accommodation.ClientId) &&
        //                (t.RegistrantId.Equals(accommodation.RegistrantId) || t.Registrant.NIF.ToLower().EndsWith(accommodation.Registrant.NIF.ToLower()))
        //            )
        //        )
        //        .OrderBy(r => r.Number).ToList();

        //    return new SelectList(list, "Id", "DisplayName", accommodation.RegistrationId);
        //}

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accommodation = await db.Accommodations.FindAsync(id);
            accommodation.Deleted = DateTime.Now;
            db.Accommodations.Update(accommodation);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccommodationExists(Guid id)
        {
            return db.Accommodations.AsNoTracking().Any(e => e.Id == id);
        }
    }
}

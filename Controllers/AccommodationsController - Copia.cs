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

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Congress")]
    public class AccommodationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SageContext _sage;


        public AccommodationsController(ApplicationDbContext context, SageContext sage)
        {
            _context = context;
            _sage = sage;
        }

        public IActionResult Index()
        {
            var applicationDbContext = _context.Accommodations
                .Include(r => r.BillingLocation)
                .Include(r => r.Client)
                .Include(r => r.Congress)
                .Include(r => r.Registrant)
                .Include(r => r.RoomType)
                .Where(r => r.Deleted == null && !r.Congress.HideRegistrations)
                .Select(r => Selector.SelectAccommodation(r)).AsNoTracking();
            return View(applicationDbContext.ToList());
        }

        public IActionResult Unsent()
        {
            var unsentAcc = _context.Accommodations
                .Include(r => r.BillingLocation)
                .Include(r => r.Client)
                .Include(r => r.Congress)
                .Include(r => r.Registrant).Where(r => r.Deleted == null && !r.Congress.HideRegistrations && !r.Imported && r.Reviewed)
                .Select(r => Selector.SelectAccommodation(r)).AsNoTracking();

            var unreviewedAcc = _context.Accommodations
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
            {
                return NotFound();
            }

            var accommodation = await _context.Accommodations
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
            {
                return NotFound();
            }
            if (accommodation.IsInvited)
            {
                emailType = EmailType.Notification;
            }

            var user = accommodation.Account;
            if (user == null)
            {
                user = _context.Accounts.FirstOrDefault(u => u.Name.Equals(User.Identity.Name));
            }
            var mail = _context.CongressEmailAccounts
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
                {
                    docType = "Confirmación";
                }
                MailSender.MailSender.Send(docType, mail, accommodation, request, vat, emailType);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }

            accommodation.Notified = true;

            _context.Update(accommodation);
            await _context.SaveChangesAsync();

            return Ok();

        }
        public async Task<IActionResult> Notification(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accommodation = await _context.Accommodations
                .Include(r => r.Congress)
                .Include(r => r.DocumentType)
                .Include(r => r.Registrant)
                    .ThenInclude(t => t.Treatment)
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

            var registration = await _context.Accommodations
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

            var accommodation = await _context.Accommodations
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
                user = _context.Accounts.FirstOrDefault(u => u.Name.Equals(User.Identity.Name));
            }
            var mail = _context.CongressEmailAccounts.FirstOrDefault(c => c.CongressId.Equals(accommodation.CongressId) && c.AccountId.Equals(user.Id));
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
            var registration = _context.Registrations
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
                RoomType = _context.RoomTypes.FirstOrDefault(),
                Registration = registration
            };
            accommodation.Number = GetMaxNumber(registration.CongressId);

            _context.Add(accommodation);
            _context.SaveChanges();
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
        public IActionResult Create()
        {
            var account = _context.Accounts.AsNoTracking().FirstOrDefault(a => a.Name.Equals(User.Identity.Name));
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", account.Id);

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

            //ViewData["AlternateLocationId"] = new SelectList(_context.Locations, "Id", "FullAddress", accommodation.AlternateLocationId);
            //ViewData["BillingLocationId"] = new SelectList(_context.Locations, "Id", "FullAddress", accommodation.BillingLocationId);
            ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "FullName", accommodation.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", accommodation.VATId);
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).AsNoTracking().Where(c => !c.HideRegistrations && c.IsCongress), "Id", "DisplayName", accommodation.CongressId);
            //ViewData["MainLocationId"] = new SelectList(_context.Locations, "Id", "FullAddress", accommodation.MainLocationId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes.AsNoTracking().Where(rt => rt.Deleted == null), "Id", "Name", accommodation.RoomTypeId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.AsNoTracking().Where(t => t.Deleted == null), "Id", "Name", accommodation.Registrant.TreatmentId);

            return View(accommodation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Accommodation accommodation)
        {

            //FixModelState(accommodation, out bool addClient, out bool addMainLocation, out bool addBillingLocation);
            if (accommodation.RoomType == null)
            {
                accommodation.RoomType = _context.RoomTypes.AsNoTracking().FirstOrDefault(t => t.Id.Equals(accommodation.RoomTypeId));
                ModelState["RoomType"].Errors.Clear();
                ModelState["RoomType"].ValidationState = ModelValidationState.Valid;
            }
            if (accommodation.RegistrantId.Equals(Guid.Empty) && accommodation.Registrant != null)
            {
                accommodation.RegistrantId = accommodation.Registrant.Id;
            }
            if (accommodation.Companion != null && accommodation.Companion.Location != null)
            {
                accommodation.Companion.LocationId = accommodation.Companion.Location.Id;
            }
            if (ModelState.IsValid)
            {
                int max = GetMaxNumber(accommodation.CongressId);
                accommodation.Number = max;
                _context.Update(accommodation.Registrant);
                accommodation.RegistrantId = accommodation.Registrant.Id;
                accommodation.Registrant = null;
                _context.Add(accommodation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", accommodation.AccountId);
            ViewData["BillingLocationId"] = new SelectList(_context.ClientLocations.AsNoTracking().Where(l => l.ClientId.Equals(accommodation.ClientId)), "Id", "FullAddressWithPhone", accommodation.BillingLocationId);
            ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "FullName", accommodation.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", accommodation.VATId);
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).AsNoTracking().Where(c => !c.HideRegistrations && c.IsCongress), "Id", "DisplayName", accommodation.CongressId);
            ViewData["MainLocationId"] = new SelectList(_context.RegistrantLocations.AsNoTracking().Where(l => l.RegistrantId.Equals(accommodation.ClientId)), "Id", "FullAddressWithPhone", accommodation.Registrant.LocationId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes.AsNoTracking().Where(rt => rt.Deleted == null), "Id", "Name", accommodation.RoomTypeId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.AsNoTracking().Where(t => t.Deleted == null), "Id", "Name", accommodation.Registrant.TreatmentId);
            return View(accommodation);
        }

        private int GetMaxNumber(Guid congressId)
        {
            var max = 1;
            if (_context.Accommodations.AsNoTracking().Any(r => r.Deleted == null && r.CongressId.Equals(congressId)))
            {
                max = _context.Accommodations.AsNoTracking().Where(r => r.Deleted == null && r.CongressId.Equals(congressId)).Max(r => r.Number) + 1;
            }

            return max;
        }

        public async Task<IActionResult> Edit(Guid? id, string returnUrl = "")
        {
            if (id == null)
            {
                return NotFound();
            }

            var accommodation = await _context.Accommodations
                .Include(r => r.Client)
                    .ThenInclude(c => c.Locations)
                .Include(r => r.Registrant)
                    .ThenInclude(rg => rg.Location)
                .Include(r => r.BillingLocation)
                .Include(r => r.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id.Equals(id));
            if (accommodation == null)
            {
                return NotFound();
            }


            ViewData["BillingLocationId"] = new SelectList(_context.ClientLocations.AsNoTracking().Where(cl => cl.ClientId.Equals(accommodation.ClientId)), "Id", "FullAddressWithPhone", accommodation.BillingLocationId);
            ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "FullName", accommodation.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", accommodation.VATId);
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).Where(c => !c.HideRegistrations && c.IsCongress).AsNoTracking(), "Id", "DisplayName", accommodation.CongressId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes.AsNoTracking().Where(rt => rt.Deleted == null), "Id", "Name", accommodation.RoomTypeId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.AsNoTracking().Where(t => t.Deleted == null), "Id", "Name", accommodation.Registrant.TreatmentId);
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", accommodation.AccountId);

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
            {
                return NotFound();
            }
            if (accommodation.RoomType == null)
            {
                accommodation.RoomType = _context.RoomTypes.AsNoTracking().FirstOrDefault(t => t.Id.Equals(accommodation.RoomTypeId));
                ModelState["RoomType"].Errors.Clear();
                ModelState["RoomType"].ValidationState = ModelValidationState.Valid;
            }
            if (accommodation.RegistrantId.Equals(Guid.Empty) && accommodation.Registrant != null)
            {
                accommodation.RegistrantId = accommodation.Registrant.Id;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _context.Clients.AsNoTracking().FirstOrDefault(c => c.NIF.ToLower().Trim().Equals(accommodation.Client.NIF.ToLower().Trim()));
                    if (client == null)
                    {
                        client = accommodation.Client;
                    }

                    if (accommodation.Client != null)
                    {
                        client.Email = accommodation.Client.Email;
                        client.Email2 = accommodation.Client.Email2;
                    }
                    accommodation.ClientId = client.Id;
                    if (accommodation.Companion != null)
                    {
                        accommodation.CompanionId = accommodation.Companion.Id;
                    }
                    if (accommodation.BillingLocation != null)
                    {
                        accommodation.BillingLocation.ClientId = client.Id;
                    }
                    if (accommodation.BillingLocationId == null && accommodation.BillingLocation != null)
                    {
                        accommodation.BillingLocationId = accommodation.BillingLocation.Id;
                    }
                    if (_context.ClientLocations.Find(accommodation.BillingLocationId) == null)
                    {
                        _context.Add(accommodation.BillingLocation);
                    }
                    if (_context.Clients.Find(client.Id) == null)
                    {
                        _context.Add(accommodation.Client);
                    }

                    accommodation.BillingLocation = null;
                    accommodation.Client = null;
                    if (accommodation.Companion != null)
                    {
                        var companion = _context.Registrant.AsNoTracking().FirstOrDefault(c => c.NIF.ToLower().Trim().Equals(accommodation.Registrant.NIF.ToLower().Trim()));
                        if (companion == null)
                        {
                            companion = accommodation.Registrant;
                        }
                        accommodation.RegistrantId = companion.Id;

                        if (_context.Registrant.AsNoTracking().FirstOrDefault(t => t.Id.Equals(companion.Id)) == null)
                        {
                            _context.Add(accommodation.Registrant);
                        }
                    }
                    else
                    {
                        accommodation.CompanionId = null;
                    }
                    accommodation.Companion = null;
                    accommodation.Modified = DateTime.Now;
                    if (accommodation.RegistrationId != null)
                    {
                        var r = _context.Registrations.FirstOrDefault(r => r.Id.Equals(accommodation.RegistrationId));
                        r.Accommodation = accommodation;
                        r.AccommodationId = accommodation.Id;
                        _context.Update(r);
                    }

                    accommodation.VAT = _sage.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(accommodation.VATId)).IVA;
                    _context.Update(accommodation);
                    await _context.SaveChangesAsync();
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", accommodation.AccountId);

            ViewData["BillingLocationId"] = new SelectList(_context.ClientLocations.AsNoTracking().Where(cl => cl.ClientId.Equals(accommodation.ClientId)), "Id", "FullAddressWithPhone", accommodation.BillingLocationId);
            ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "FullName", accommodation.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", accommodation.VATId);
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).Where(c => !c.HideRegistrations && c.IsCongress).AsNoTracking(), "Id", "DisplayName", accommodation.CongressId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes.AsNoTracking().Where(rt => rt.Deleted == null), "Id", "Name", accommodation.RoomTypeId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.AsNoTracking().Where(t => t.Deleted == null), "Id", "Name", accommodation.Registrant.TreatmentId);
            ViewData["ShowBillingLocation"] = true;
            SelectList selectList = SelectRegistration(accommodation);
            ViewData["RegistrationId"] = selectList;
            return View(accommodation);
        }

        private SelectList SelectRegistration(Accommodation accommodation)
        {
            return new SelectList(_context.Registrations.IncludeOptimized(r => r.Client).IncludeOptimized(r => r.Registrant).AsNoTracking()
                .Where(
                t => t.Deleted == null &&
                t.AccommodationId.Equals(accommodation.Id) ||
                (
                    t.AccommodationId == null &&
                    t.CongressId.Equals(accommodation.CongressId) &&
                    t.ClientId.Equals(accommodation.ClientId) &&
                    (t.RegistrantId.Equals(accommodation.RegistrantId) || t.Registrant.NIF.ToLower().EndsWith(accommodation.Registrant.NIF.ToLower()))
                )
                ).OrderBy(r => r.Number), "Id", "DisplayName", accommodation.RegistrationId);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accommodation = await _context.Accommodations.FindAsync(id);
            accommodation.Deleted = DateTime.Now;
            _context.Accommodations.Update(accommodation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccommodationExists(Guid id)
        {
            return _context.Accommodations.AsNoTracking().Any(e => e.Id == id);
        }
    }
}

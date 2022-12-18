using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.MailSender;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.ViewFunctions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Ferpuser.Controllers
{

    //[Authorize(Policy = "Collaborations")]
    [Authorize(Policy = "Facturacion")]
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly SageContext _sage;
        private readonly SageComuContext _sageComu;


        public ExpensesController(ApplicationDbContext context, SageContext sage, SageComuContext comu)
        {
            _context = context;
            _sage = sage;
            _sageComu = comu;
        }

        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("MENU", "MENU_FACTURACION");

            var user = _context.Accounts.FirstOrDefault(u => u.Name.Equals(User.Identity.Name));

            var applicationDbContext = await _context.Expenses
                 .IncludeOptimized(e => e.Products)
                 .IncludeOptimized(e => e.Client)
                 .IncludeOptimized(e => e.Congress)
                 .IncludeOptimized(e => e.DocumentType)
                 .Where(r => r.Deleted == null && /*!r.Congress.HideRegistrations &&*/ (user.PermisoAdministracion || r.AccountId.Equals(user.Id))).ToListAsync();
            bool changed = false;
            foreach (var exp in applicationDbContext.Where(exp => IsCollectedInSage(exp) && !exp.Paid))
            {
                changed = true;
                exp.Paid = true;
                _context.Expenses.Update(exp);
            }

            if (changed)
                _context.SaveChanges();

            return View(applicationDbContext);
        }

        public async Task<IActionResult> Sync()
        {
            RegistrationManager registrationManager = new RegistrationManager(_context, _sageComu);
            registrationManager.ActualizarPagadas();

            AcommodationManager acommodationManager = new AcommodationManager(_context, _sageComu);
            await acommodationManager.ActualizarPagadas();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Notify(Guid? id, [FromBody] object request, EmailType emailType = EmailType.Notification)
        {
            if (id == null)
                return NotFound();

            var expense = await _context.Expenses
                .Include(r => r.Account)
                .IncludeOptimized(r => r.Congress)
                .IncludeOptimized(r => r.Client)
                .IncludeOptimized(r => r.BillingLocation)
                .IncludeOptimized(r => r.DocumentType)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            if (expense == null)
                return NotFound();

            var user = expense.Account;
            if (user == null)
                user = _context.Accounts.FirstOrDefault(u => u.Name.Equals(User.Identity.Name));

            if (user == null)
                return StatusCode(400, "No se ha encontrado ningún usuario ligado a la factura ni tampoco al usuario autenticado.");

            var congressMail = _context.Congresses.FirstOrDefault(f => f.Id == expense.CongressId && !string.IsNullOrWhiteSpace(f.OutgoingMailServer));

            CongressEmailAccounts mail = null;
            if (congressMail != null)
            {
                mail = new CongressEmailAccounts
                {
                    OutgoingMailPort = congressMail.OutgoingMailPort,
                    OutgoingMailServer = congressMail.OutgoingMailServer,
                    MailUser = congressMail.MailUser,
                    MailPassword = congressMail.MailPassword,
                    Congress = expense.Congress,
                    Account = expense.Account
                };
            }

            if (mail == null)
                mail = _context.CongressEmailAccounts
                    .IncludeOptimized(c => c.Account)
                    .IncludeOptimized(c => c.Congress)
                    .FirstOrDefault(c => c.CongressId.Equals(expense.CongressId) && c.AccountId.Equals(user.Id) && !string.IsNullOrWhiteSpace(c.OutgoingMailServer));

            if (mail == null)
                mail = _context.CongressEmailAccounts
                    .IncludeOptimized(c => c.Account)
                    .IncludeOptimized(c => c.Congress)
                    .FirstOrDefault(c => c.CongressId.Equals(expense.CongressId) && !string.IsNullOrWhiteSpace(c.OutgoingMailServer));
            
            if (mail == null && !string.IsNullOrWhiteSpace(user.OutgoingMailServer))
            {                
                mail = new CongressEmailAccounts
                {
                    OutgoingMailPort = user.OutgoingMailPort,
                    OutgoingMailServer = user.OutgoingMailServer,
                    MailUser = user.MailUser,
                    MailPassword = user.MailPassword,
                    Congress = expense.Congress,
                    Account = expense.Account
                };                               
            }

            if (mail == null)
            {
                var e = "No hay ninguna configuración de servidor de correo asociado a este registro.";
                //return BadRequest(e);
                return StatusCode(400, "No hay ninguna configuración de servidor de correo asociado a este registro.");
            }

            try
            {
                decimal vat = _sage.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(expense.VATId)).IVA;
                MailSender.MailSender.Send(expense.DocumentType.Name, mail, expense, request, vat, emailType);
            }
            catch (Exception e)
            {
                string response = $"MailServer: {mail.OutgoingMailServer}, MailUser: {mail.MailUser}, MailPort: {mail.OutgoingMailPort}, Exception: {e.ToString()}";
                return StatusCode(400, response);
                //return BadRequest(e.ToString());
            }

            expense.Notified = true;

            _context.Update(expense);
            await _context.SaveChangesAsync();

            return Ok();

        }
        public async Task<IActionResult> Notification(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(r => r.Congress)
                .Include(r => r.DocumentType)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            if (expense == null)
            {
                return NotFound();
            }

            try
            {
                var html = MailSender.MailSender.NotificationMailAttachment(expense.Congress, expense);
                string number = "";
                if (expense.DocumentType.IsInvoice())
                {
                    number = expense.InvoiceNumber;
                }
                else
                {
                    number = ViewHelpers.PadCongress(expense.Number);
                }
                var pdf = MailSender.MailSender.HtmlToPdf(html, expense.InvoiceDate, number);
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

            var expense = await _context.Expenses
                .Include(r => r.Congress)
                .Include(r => r.Client)
                .Include(r => r.BillingLocation)
                .Include(r => r.Products)
                .Include(r => r.DocumentType)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            decimal vat = _sage.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(expense.VATId)).IVA;

            if (expense == null)
            {
                return NotFound();
            }

            try
            {
                var html = MailSender.MailSender.InvoiceMailAttachment(expense.Congress, expense, vat);
                string number = "";
                if (expense.DocumentType.IsInvoice())
                {
                    number = expense.InvoiceNumber;
                }
                else
                {
                    number = ViewHelpers.PadCongress(expense.Number);
                }
                var pdf = MailSender.MailSender.HtmlToPdf(html, expense.InvoiceDate, number);

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

            var expense = await _context.Expenses
                .Include(r => r.Congress)
                .Include(r => r.DocumentType)
                .Include(r => r.Account)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            if (expense == null)
            {
                return NotFound();
            }
            var signature = "";
            var user = expense.Account;

            if (user == null)
            {
                user = _context.Accounts.FirstOrDefault(u => u.Name.Equals(User.Identity.Name));
            }
            var mail = _context.CongressEmailAccounts.FirstOrDefault(c => c.CongressId.Equals(expense.CongressId) && c.AccountId.Equals(user.Id));
            if (mail == null)
            {
                signature = expense.Congress.SignatureAfter;
            }
            else
            {
                signature = mail.SignatureAfter;
            }
            try
            {
                var html = MailSender.MailSender.MailBody(signature, expense, emailType);
                return Content(html, "text/html");
                //return View((object) html);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
        // GET: Expenses/ProductTemplate
        public IActionResult ProductTemplate(Guid? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            var product = new Product
            {
                ExpenseId = id.Value
            };
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", product.VATId);
            ViewData["Products"] = _sage.Articulo.AsNoTracking().Where(a => a.Tipo_Art < 3);
            return PartialView("/Views/Shared/EditorTemplates/ProductTemplate.cshtml", product);
        }
        // GET: Expenses/Create
        public IActionResult Create()
        {
            var account = _context.Accounts.AsNoTracking().FirstOrDefault(a => a.Name.Equals(User.Identity.Name));
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", account.Id);

            var expense = new Expense
            {
                BillingLocation = new ClientLocation(),
                Client = new Client(),
                Account = account,
                AccountId = account.Id
            };
            expense.BillingLocation.ClientId = expense.Client.Id;

            ViewData["BillingLocationId"] = new SelectList(_context.ClientLocations.Where(c => c.ClientId.Equals(expense.ClientId)), "Id", "FullAddressWithPhone", expense.BillingLocationId);

            ViewData["DocumentTypes"] = new SelectList(_context.DocumentTypes, "Id", "Name", expense.DocumentTypeId);
            ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "FullName", expense.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", expense.VATId);
            ViewData["FPag"] = new SelectList(_sage.FPag.AsNoTracking(), "Codigo", "Nombre", expense.FPag);
            ViewData["Products"] = _sage.Articulo.AsNoTracking().Where(a => a.Tipo_Art < 3);
            ViewData["Serie"] = new SelectList(_sageComu.Letras.AsNoTracking(), "Codigo", "Display", null);
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).AsNoTracking()/*.Where(c => !c.HideRegistrations)*/, "Id", "DisplayName", null);

            return View(expense);
        }
        private int GetMaxNumber(Guid congressId)
        {
            var max = 1;
            if (_context.Expenses.AsNoTracking().Any(r => r.Deleted == null && r.CongressId.Equals(congressId)))
            {
                max = _context.Expenses.AsNoTracking().Where(r => r.Deleted == null && r.CongressId.Equals(congressId)).Max(r => r.Number) + 1;
            }

            return max;
        }
        // POST: Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            foreach (var p in expense.Products)
            {
                p.VAT = _sage.Tipo_IVA.AsNoTracking().FirstOrDefault(i => i.Codigo.Equals(p.VATId)).IVA;
            }

            if (ModelState.IsValid)
            {
                int max = GetMaxNumber(expense.CongressId);
                expense.Number = max;
                expense.Products = expense.Products.Where(p => p.Deleted == null).ToList();

                var client = await _context.Clients.FirstOrDefaultAsync(c => c.NIF.ToLower().Trim().Equals(expense.Client.NIF.ToLower().Trim()));
                if (client == null)
                {
                    client = expense.Client;
                }
                if (expense.Client != null)
                {
                    client.Email = expense.Client.Email;
                    client.Email2 = expense.Client.Email2;
                }
                expense.ClientId = client.Id;

                if (expense.BillingLocation != null)
                {
                    expense.BillingLocation.ClientId = client.Id;
                }
                if (expense.BillingLocationId == null && expense.BillingLocation != null)
                {
                    expense.BillingLocationId = expense.BillingLocation.Id;
                }
                if (await _context.ClientLocations.FirstOrDefaultAsync(c => c.Id.Equals(expense.BillingLocationId)) == null)
                {
                    _context.Add(expense.BillingLocation);
                }
                if (await _context.Clients.FirstOrDefaultAsync(c => c.Id.Equals(client.Id)) == null)
                {
                    _context.Add(expense.Client);
                }
                foreach (var p in expense.Products)
                {
                    if (!_context.Products.AsNoTracking().Any(e => e.Id == p.Id))
                    {
                        _context.Entry(p).State = EntityState.Added;
                    }
                    else if (p.Deleted != null)
                    {
                        _context.Entry(p).State = EntityState.Deleted;
                    }
                    else
                    {
                        _context.Entry(p).State = EntityState.Modified;
                    }
                }
                expense.Products = null;
                expense.BillingLocation = null;
                expense.Client = null;
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { expense.Id });
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", expense.AccountId);
            ViewData["DocumentTypes"] = new SelectList(_context.DocumentTypes, "Id", "Name", expense.DocumentTypeId);
            ViewData["BillingLocationId"] = new SelectList(_context.ClientLocations.Where(c => c.ClientId.Equals(expense.ClientId)), "Id", "FullAddressWithPhone", expense.BillingLocationId);
            ViewData["FPag"] = new SelectList(_sage.FPag.AsNoTracking(), "Codigo", "Nombre", expense.FPag);

            ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "FullName", expense.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", expense.VATId);
            ViewData["Products"] = _sage.Articulo.AsNoTracking().Where(a => a.Tipo_Art < 3);
            ViewData["Serie"] = new SelectList(_sageComu.Letras.AsNoTracking(), "Codigo", "Display", expense.Serie);
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).AsNoTracking()/*.Where(c => !c.HideRegistrations)*/, "Id", "DisplayName", expense.CongressId);
            return View(expense);
        }

        public bool IsCollectedInSage(Expense expense)
        {
            var previ = _sageComu.Previ_Cl.FirstOrDefault(p => p.Factura.Equals(expense.InvoiceNumber));
            return previ != null && previ.Cobro.HasValue;
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var exp = await _context.Expenses.FirstOrDefaultAsync(r => r.Id.Equals(id));
            if (IsCollectedInSage(exp))
            {
                exp.Paid = true;
                _context.Expenses.Update(exp);
                _context.SaveChanges();
            }
            var expense = await _context.Expenses
                .Include(r => r.Client)
                    .ThenInclude(c => c.Locations)
                .Include(r => r.BillingLocation)
                .Include(r => r.Products)
                .Include(r => r.Account)
                .Include(r => r.DocumentType)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id.Equals(id));

            if (expense == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name");
            ViewData["BillingLocationId"] = new SelectList(_context.ClientLocations.Where(c => c.ClientId.Equals(expense.ClientId)), "Id", "FullAddressWithPhone");
            ViewData["FPag"] = new SelectList(_sage.FPag.AsNoTracking(), "Codigo", "Nombre", expense.FPag);

            ViewData["DocumentTypes"] = new SelectList(_context.DocumentTypes, "Id", "Name", expense.DocumentTypeId);
            ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "FullName", expense.ClientId);
            ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", expense.VATId);
            ViewData["Products"] = _sage.Articulo.AsNoTracking().Where(a => a.Tipo_Art < 3);
            ViewData["Serie"] = new SelectList(_sageComu.Letras.AsNoTracking(), "Codigo", "Display", expense.Serie);
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).OrderByDescending(a => a.Number).AsNoTracking()/*.Where(c => !c.HideRegistrations)*/, "Id", "DisplayName", expense.CongressId);




            return View(expense);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Expense expense, bool refresh = true)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }
            foreach (var p in expense.Products)
            {
                p.VAT = _sage.Tipo_IVA.AsNoTracking().FirstOrDefault(i => i.Codigo.Equals(p.VATId)).IVA;
            }

            int? seleccionado = HttpContext.Session.GetInt32(Consts.SESSION_EJERCICIO);
            if (seleccionado.HasValue && expense.InvoiceDate.HasValue && expense.InvoiceDate.Value.Year != seleccionado)
            {
                List<ValidationResult> businessErrors = new List<ValidationResult>();
                businessErrors.Add(new ValidationResult("La fecha de factura no concuerda con el ejercicio seleccionado. Por favor revise la fecha de factura o bien seleccione otro ejercicio.", new string[] { "InvoiceDate" }));
                this.AddValidationErrors(businessErrors);

                TempData["ErrorMessage"] = businessErrors.First().ErrorMessage;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var client = await _context.Clients.FirstOrDefaultAsync(c => c.NIF.ToLower().Trim().Equals(expense.Client.NIF.ToLower().Trim()));
                    if (client == null)
                    {
                        client = expense.Client;
                    }

                    if (expense.Client != null)
                    {
                        client.Email = expense.Client.Email;
                        client.Email2 = expense.Client.Email2;
                    }
                    expense.ClientId = client.Id;

                    //if (expense.BillingLocation != null)
                    //{
                    //    expense.BillingLocation.ClientId = client.Id;
                    //}
                    //if (expense.BillingLocationId == null && expense.BillingLocation != null)
                    //{
                    //    expense.BillingLocationId = expense.BillingLocation.Id;
                    //}
                    if (await _context.ClientLocations.FirstOrDefaultAsync(c => c.Id.Equals(expense.BillingLocationId)) == null)
                    {
                        _context.Add(expense.BillingLocation);
                    }
                    if (await _context.Clients.FirstOrDefaultAsync(c => c.Id.Equals(client.Id)) == null)
                    {
                        _context.Add(expense.Client);
                    }
                    foreach (var p in expense.Products)
                    {
                        if (!_context.Products.AsNoTracking().Any(e => e.Id == p.Id))
                        {
                            _context.Entry(p).State = EntityState.Added;
                        }
                        else if (p.Deleted != null)
                        {
                            _context.Entry(p).State = EntityState.Deleted;
                        }
                        else
                        {
                            _context.Entry(p).State = EntityState.Modified;
                        }
                    }
                    expense.Products = null;
                    expense.BillingLocation = null;
                    expense.Client = null;
                    expense.Modified = DateTime.Now;
                    expense.Account = null;

                    _context.Update(expense);
                    await _context.SaveChangesAsync(true);
                }
                catch (Exception)
                {
                    if (!ExpenseExists(expense.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            //ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name");
            //ViewData["BillingLocationId"] = new SelectList(_context.ClientLocations.Where(c => c.ClientId.Equals(expense.ClientId)), "Id", "Address");
            //ViewData["FPag"] = new SelectList(_sage.FPag.AsNoTracking(), "Codigo", "Nombre", expense.FPag);

            //ViewData["DocumentTypes"] = new SelectList(_context.DocumentTypes, "Id", "Name", expense.DocumentTypeId);
            //ViewData["ClientId"] = new SelectList(_context.Clients.AsNoTracking(), "Id", "FullName", expense.ClientId);
            //ViewData["VATId"] = new SelectList(_sage.Tipo_IVA.AsNoTracking(), "Codigo", "Nombre", expense.VATId);
            //ViewData["Products"] = _sage.Articulo.AsNoTracking().Where(a => a.Tipo_Art < 3);
            //ViewData["Serie"] = new SelectList(_sageComu.Letras.AsNoTracking(), "Codigo", "Display", expense.Serie);
            //ViewData["CongressId"] = new SelectList(_context.Congresses.OrderByDescending(a => a.Number).AsNoTracking().Where(c => !c.HideRegistrations), "Id", "DisplayName", expense.CongressId);

            return RedirectToAction("Edit", new { id = id });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            expense.Deleted = DateTime.Now;
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(Guid id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}

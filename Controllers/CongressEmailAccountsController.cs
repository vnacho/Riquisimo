using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ferpuser.Data;
using Ferpuser.Models;
using Z.EntityFramework.Plus;
using Ferpuser.Models.Consts;

namespace Ferpuser.Controllers
{
    public class CongressEmailAccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CongressEmailAccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CongressEmailAccounts
        public async Task<IActionResult> Index()
        {

            var hasAdmin = HttpContext.User.Claims.Any(c => c.Type.Equals(Consts.CLAIM_PERMISO_ADMINISTRACION));
            if (hasAdmin)
            {
                var applicationDbContext = _context.CongressEmailAccounts
                    .Select(r => new CongressEmailAccounts
                    {
                        Id = r.Id,
                        MailUser = r.MailUser,
                        AccountId = r.AccountId,
                        Account = new Account { Name = r.Account.Name },
                        CongressId = r.CongressId,
                        Congress = new Congress { Name = r.Congress.Name, Number = r.Congress.Number }
                    })
                    .AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            } else
            {
                var applicationDbContext = _context.CongressEmailAccounts
                    .Select(r => new CongressEmailAccounts
                    {
                        Id = r.Id,
                        MailUser = r.MailUser,
                        AccountId = r.AccountId,
                        Account = new Account { Name = r.Account.Name },
                        CongressId = r.CongressId,
                        Congress = new Congress { Name = r.Congress.Name, Number = r.Congress.Number }
                    }).Where(r => r.Account.Name.ToLower().Equals(HttpContext.User.Identity.Name.ToLower()))
                    .AsNoTracking();
                return View(await applicationDbContext.ToListAsync());
            }

        }

        // GET: CongressEmailAccounts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congressEmailAccounts = await _context.CongressEmailAccounts
                .Include(c => c.Account)
                .Include(c => c.Congress)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (congressEmailAccounts == null)
            {
                return NotFound();
            }

            return View(congressEmailAccounts);
        }

        // GET: CongressEmailAccounts/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name");
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).AsNoTracking().Where(c => !c.HideRegistrations), "Id", "DisplayName");
            return View(new CongressEmailAccounts());
        }

        // POST: CongressEmailAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CongressId,AccountId,IncomingMailServer,OutgoingMailServer,IncomingMailPort,OutgoingMailPort,MailUser,MailPassword,SignatureBefore,Signature,SignatureAfter,Id,Created,Modified,Deleted")] CongressEmailAccounts congressEmailAccounts)
        {
            if (ModelState.IsValid)
            {
                congressEmailAccounts.Id = Guid.NewGuid();
                _context.Add(congressEmailAccounts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", congressEmailAccounts.AccountId);
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).AsNoTracking().Where(c => !c.HideRegistrations), "Id", "DisplayName", congressEmailAccounts.CongressId);
            return View(congressEmailAccounts);
        }

        // GET: CongressEmailAccounts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congressEmailAccounts = await _context.CongressEmailAccounts.FindAsync(id);
            if (congressEmailAccounts == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", congressEmailAccounts.AccountId);
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).AsNoTracking().Where(c => !c.HideRegistrations), "Id", "DisplayName", congressEmailAccounts.CongressId);
            return View(congressEmailAccounts);
        }

        // POST: CongressEmailAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CongressId,AccountId,IncomingMailServer,OutgoingMailServer,IncomingMailPort,OutgoingMailPort,MailUser,MailPassword,SignatureBefore,Signature,SignatureAfter,Id,Created,Modified,Deleted")] CongressEmailAccounts congressEmailAccounts)
        {
            if (id != congressEmailAccounts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(congressEmailAccounts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CongressEmailAccountsExists(congressEmailAccounts.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Name", congressEmailAccounts.AccountId);
            ViewData["CongressId"] = new SelectList(_context.Congresses.Where(c => c.Deleted == null).AsNoTracking().Where(c => !c.HideRegistrations), "Id", "DisplayName", congressEmailAccounts.CongressId);
            return View(congressEmailAccounts);
        }

        // GET: CongressEmailAccounts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congressEmailAccounts = await _context.CongressEmailAccounts
                .Include(c => c.Account)
                .Include(c => c.Congress)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (congressEmailAccounts == null)
            {
                return NotFound();
            }

            return View(congressEmailAccounts);
        }

        // POST: CongressEmailAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var congressEmailAccounts = await _context.CongressEmailAccounts.FindAsync(id);
            _context.CongressEmailAccounts.Remove(congressEmailAccounts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CongressEmailAccountsExists(Guid id)
        {
            return _context.CongressEmailAccounts.Any(e => e.Id == id);
        }
    }
}

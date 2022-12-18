using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Congress")]
    public class RegistrantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistrantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Registrants
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Registrant.Include(r => r.Location).Include(r => r.Treatment);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Registrants/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrant = await _context.Registrant
                .Include(r => r.Location)
                .Include(r => r.Treatment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registrant == null)
            {
                return NotFound();
            }

            return View(registrant);
        }

        // GET: Registrants/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.RegistrantLocations, "Id", "Address");
            ViewData["TreatmentId"] = new SelectList(_context.Treatments, "Id", "Name");
            return View();
        }

        // POST: Registrants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Surnames,TreatmentId,Position,ProfessionalCategory,Workplace,LocationId,NIF,Email,Email2,Id,Created,Modified,Deleted")] Registrant registrant)
        {
            if (ModelState.IsValid)
            {
                registrant.Id = Guid.NewGuid();
                _context.Add(registrant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.RegistrantLocations, "Id", "Address", registrant.LocationId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(t => t.Deleted == null), "Id", "Name", registrant.TreatmentId);
            return View(registrant);
        }
        // GET: Registrants/Edit/5
        public async Task<IActionResult> Editor(Guid? id)
        {
            if (id == null)
            {
                ViewData["LocationId"] = new SelectList(_context.RegistrantLocations, "Id", "Address");
                ViewData["TreatmentId"] = new SelectList(_context.Treatments, "Id", "Name");
                var reg = new Registrant();
                reg.Location = new RegistrantLocation
                {
                    RegistrantId = reg.Id
                };
                return View(reg);
            }

            var registrant = await _context.Registrant.Include(r => r.Location).FirstOrDefaultAsync(r => r.Id.Equals(id));
            if (registrant == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.RegistrantLocations, "Id", "Address", registrant.LocationId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(t => t.Deleted == null), "Id", "Name", registrant.TreatmentId);
            return View(registrant);
        }

        // GET: Registrants/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrant = await _context.Registrant.FindAsync(id);
            if (registrant == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.RegistrantLocations, "Id", "Address", registrant.LocationId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(t => t.Deleted == null), "Id", "Name", registrant.TreatmentId);
            return View(registrant);
        }

        // POST: Registrants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Surnames,TreatmentId,Position,ProfessionalCategory,Workplace,LocationId,NIF,Email,Email2,Id,Created,Modified,Deleted")] Registrant registrant)
        {
            if (id != registrant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registrant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrantExists(registrant.Id))
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
            ViewData["LocationId"] = new SelectList(_context.RegistrantLocations, "Id", "Address", registrant.LocationId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(t => t.Deleted == null), "Id", "Name", registrant.TreatmentId);
            return View(registrant);
        }

        public async Task<IActionResult> Search(string id)
        {
            if (id == null)
            {
                return View(new List<Client>());
            }
            var text = id.Trim().ToLower();
            if (id.Length > 3)
            {
                var clients = (await _context.Registrant.ToListAsync()).Where(c => c.FullName.ToLower().Trim().Contains(text) || c.NIF.ToLower().Trim().Contains(text)).OrderBy(c => c.Surnames);
                return View(clients);
            }
            return View(new List<Registrant>());
        }

        // GET: Registrants/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrant = await _context.Registrant
                .Include(r => r.Location)
                .Include(r => r.Treatment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registrant == null)
            {
                return NotFound();
            }

            return View(registrant);
        }

        // POST: Registrants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var registrant = await _context.Registrant.FindAsync(id);
            _context.Registrant.Remove(registrant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrantExists(Guid id)
        {
            return _context.Registrant.Any(e => e.Id == id);
        }
    }
}

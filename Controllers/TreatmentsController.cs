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
    public class TreatmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreatmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Treatments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Treatments.Where(t => t.Deleted == null).ToListAsync());
        }

        // GET: Treatments/Create
        public IActionResult Create()
        {
            return View(new Treatment());
        }

        // POST: Treatments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,Created,Modified,Deleted")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                treatment.Id = Guid.NewGuid();
                _context.Add(treatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(treatment);
        }

        // GET: Treatments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatments.FindAsync(id);
            if (treatment == null)
            {
                return NotFound();
            }
            ViewData["CanRemove"] = !_context.Registrant.Any(r => r.TreatmentId == id && r.Deleted == null);

            return View(treatment);
        }

        // POST: Treatments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Id,Created,Modified,Deleted")] Treatment treatment)
        {
            if (id != treatment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treatment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreatmentExists(treatment.Id))
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
            ViewData["CanRemove"] = !_context.Registrant.Any(r => r.TreatmentId == id && r.Deleted == null);

            return View(treatment);
        }

        // GET: Treatments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (treatment == null)
            {
                return NotFound();
            }

            return View(treatment);
        }

        // POST: Treatments/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var treatment = await _context.Treatments.FindAsync(id);
            treatment.Deleted = DateTime.Now;
            _context.Treatments.Update(treatment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreatmentExists(Guid id)
        {
            return _context.Treatments.Any(e => e.Id == id);
        }
    }
}

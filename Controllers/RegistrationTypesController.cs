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
    public class RegistrationTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistrationTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RegistrationTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.RegistrationTypes.Where(rt => rt.Deleted == null).ToListAsync());
        }

        // GET: RegistrationTypes/Create
        public IActionResult Create()
        {
            return View(new RegistrationType());
        }

        // POST: RegistrationTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Id,Created,Modified,Deleted")] RegistrationType registrationType)
        {
            if (ModelState.IsValid)
            {
                registrationType.Id = Guid.NewGuid();
                _context.Add(registrationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registrationType);
        }

        // GET: RegistrationTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var registrationType = await _context.RegistrationTypes.FindAsync(id);
            if (registrationType == null)
            {
                return NotFound();
            }

            ViewData["CanRemove"] = !_context.Registrations.Any(r => r.RegistrationTypeId == id && r.Deleted == null);
            return View(registrationType);
        }

        // POST: RegistrationTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Description,Id,Created,Modified,Deleted")] RegistrationType registrationType)
        {
            if (id != registrationType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registrationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationTypeExists(registrationType.Id))
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
            ViewData["CanRemove"] = !_context.Registrations.Any(r => r.RegistrationTypeId == id && r.Deleted == null);

            return View(registrationType);
        }

        // GET: RegistrationTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrationType = await _context.RegistrationTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registrationType == null)
            {
                return NotFound();
            }

            return View(registrationType);
        }

        // POST: RegistrationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var registrationType = await _context.RegistrationTypes.FindAsync(id);
            registrationType.Deleted = DateTime.Now;
            _context.RegistrationTypes.Update(registrationType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationTypeExists(Guid id)
        {
            return _context.RegistrationTypes.Any(e => e.Id == id);
        }
    }
}
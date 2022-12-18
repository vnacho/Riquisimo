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
    public class RoomTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RoomTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.RoomTypes.Where(rt => rt.Deleted == null).ToListAsync());
        }

        // GET: RoomTypes/Create
        public IActionResult Create()
        {
            return View(new RoomType());
        }

        // POST: RoomTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomType roomType)
        {
            if (ModelState.IsValid)
            {
                roomType.Id = Guid.NewGuid();
                _context.Add(roomType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        // GET: RoomTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }

            ViewData["CanRemove"] = !_context.Accommodations.Any(a => a.RoomTypeId == id && a.Deleted == null);
            return View(roomType);
        }

        // POST: RoomTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, RoomType roomType)
        {
            if (id != roomType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    roomType.Modified = DateTime.Now;
                    _context.Update(roomType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomTypeExists(roomType.Id))
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
            ViewData["CanRemove"] = !_context.Accommodations.Any(a => a.RoomTypeId == id && a.Deleted == null);

            return View(roomType);
        }

        // GET: RoomTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        // POST: RoomTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var roomType = await _context.RoomTypes.FindAsync(id);
            roomType.Deleted = DateTime.Now;
            _context.RoomTypes.Update(roomType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomTypeExists(Guid id)
        {
            return _context.RoomTypes.Any(e => e.Id == id);
        }
    }
}

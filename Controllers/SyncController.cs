using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ferpuser.BLL.Filters;
using Ferpuser.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Congress")]
    public class SyncController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SyncController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult FromWeb()
        {
            var congresses = _context.Congresses.Where(c => c.Deleted == null && !c.Ended && c.IsCongress);
            ViewBag.Congress = congresses;
            return View(congresses.ToList());
        }

        [HttpPost, ActionName("FromWeb")]
        public async Task<IActionResult> FromWebConfirmed()
        {
            CongressFilter filter = new CongressFilter();
            await TryUpdateModelAsync<CongressFilter>(filter, "filter", f => f.Number);

            var congresses = _context.Congresses.Where(c => c.Deleted == null && !c.Ended && c.IsCongress);
            ViewBag.Congress = congresses;
            return View(congresses.Where(filter.ExpressionFilter()).ToList());
        }

        public IActionResult ToSage()
        {
            return View(_context.Congresses.Where(c => c.Deleted == null && !c.Ended && c.IsCongress).ToList());
        }
        public IActionResult SetCongress(Guid? id)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Name.ToLower().Equals(User.Identity.Name));
            var congress = _context.Congresses.Find(id);
            if (account == null || congress == null)
            {
                return NotFound();
            }

            account.CongressId = id;
            _context.Update(account);
            _context.SaveChanges();
            return Ok();
        }
    }
}
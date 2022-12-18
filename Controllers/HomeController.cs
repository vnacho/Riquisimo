using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ferpuser.Models;
using Ferpuser.Data;
using Ferpuser.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Z.EntityFramework.Plus;
using Ferpuser.Models.Enums;
using Microsoft.AspNetCore.Http;
using Ferpuser.Models.Consts;

namespace Ferpuser.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HttpContext.Session.SetString("MENU", "");

            var hasAdmin = HttpContext.User.Claims.Any(c => c.Type.Equals(Consts.CLAIM_PERMISO_ADMINISTRACION));
            var hasBudgetControl = HttpContext.User.Claims.Any(c => c.Type.Equals(Consts.CLAIM_PERMISO_CONTROL_PRESUPUESTARIO));
            var hasCongress = HttpContext.User.Claims.Any(c => c.Type.Equals("AccessCongress"));            
            var hasCollaboration = HttpContext.User.Claims.Any(c => c.Type.Equals("AccessCollaborations"));
            
            if (hasCongress && !hasAdmin && !hasBudgetControl && !hasCollaboration)
            {
                return RedirectToAction("Congress", "Home");
            }
            if (!hasCongress && !hasAdmin && !hasBudgetControl && hasCollaboration)
            {
                return RedirectToAction("Index", "Expenses");
            }
            return View();
        }
        public IActionResult Congress()
        {
            //var newClients = _context.Clients.Where(c => c.SageCode == null).AsNoTracking().ToList().Where(c =>
            //    (_context.Registrations.Include(r => r.Congress).Any(r => r.Deleted == null && r.ClientId.Equals(c.Id) && !r.Congress.HideRegistrations) ||
            //     _context.Accommodations.Include(r => r.Congress).Any(r => r.Deleted == null && r.ClientId.Equals(c.Id) && !r.Congress.HideRegistrations))).ToList();

            //var newClientRegistrations = _context.Registrations
            //    .Include(r => r.Registrant)
            //    .Include(r => r.Client).Where(r => r.Deleted == null && newClients.Contains(r.Client)).Select(r => Selector.SelectRegistration(r)).ToList().GroupBy(r => r.ClientId);
            //var newClientAccommodations = _context.Accommodations
            //    .Include(r => r.Registrant)
            //    .Include(r => r.Client).Where(r => r.Deleted == null && newClients.Contains(r.Client)).Select(r => Selector.SelectAccommodation(r)).ToList().GroupBy(r => r.ClientId);

            var unsent = _context.Registrations
                .Include(r => r.BillingLocation)
                .Include(r => r.Client)
                .Include(r => r.Congress)
                .Include(r => r.Registrant)
                .Include(r => r.RegistrationType).Where(r => r.Deleted == null && !r.Congress.HideRegistrations && !r.Imported && r.Reviewed)
                .Select(r => Selector.SelectRegistration(r)).AsNoTracking();


            var unreviewed = _context.Registrations
                .Include(r => r.BillingLocation)
                .Include(r => r.Client)
                .Include(r => r.Congress)
                .Include(r => r.Registrant)
                .Include(r => r.RegistrationType).Where(r => r.Deleted == null && !r.Congress.HideRegistrations && !r.Reviewed)
                .Select(r => Selector.SelectRegistration(r)).AsNoTracking();


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
                Unsent = unsent.ToList(),
                Unreviewed = unreviewed.ToList(),
                //NewClients = newClients,
                //NewClientRegistrations = newClientRegistrations.ToList(),
                //NewClientAccommodations = newClientAccommodations.ToList(),
                UnsentAcc = unsentAcc.ToList(),
                UnreviewedAcc = unreviewedAcc.ToList(),
            };
            return View(vm);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

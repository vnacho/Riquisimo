using Ferpuser.Data;
using Ferpuser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize]
    public class CongressController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CongressController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //Es la pantalla de acceso al menú de eventos
            HttpContext.Session.SetString("MENU", "MENU_EVENTOS");

            var newClients = _context.Clients.Where(c => c.SageCode == null).Where(c =>
                (_context.Registrations.Include(r => r.Congress).Any(r => r.Deleted == null && r.ClientId.Equals(c.Id) && !r.Congress.HideRegistrations) ||
                 _context.Accommodations.Include(r => r.Congress).Any(r => r.Deleted == null && r.ClientId.Equals(c.Id) && !r.Congress.HideRegistrations)))
                .AsNoTracking().ToList();

            var newClientRegistrations = _context.Registrations
                .Include(r => r.Registrant)
                .Include(r => r.Client).Where(r => r.Deleted == null && newClients.Contains(r.Client)).Select(r => Selector.SelectRegistration(r)).ToList().GroupBy(r => r.ClientId);
            var newClientAccommodations = _context.Accommodations
                .Include(r => r.Registrant)
                .Include(r => r.Client).Where(r => r.Deleted == null && newClients.Contains(r.Client)).Select(r => Selector.SelectAccommodation(r)).ToList().GroupBy(r => r.ClientId);

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
                NewClients = newClients,
                NewClientRegistrations = newClientRegistrations.ToList(),
                NewClientAccommodations = newClientAccommodations.ToList(),
                UnsentAcc = unsentAcc.ToList(),
                UnreviewedAcc = unreviewedAcc.ToList(),
            };
            return View(vm);
        }
    }
}

using Ferpuser.Models;
using iText.Kernel.Events;
using Microsoft.AspNetCore.Mvc;


namespace Ferpuser.Controllers
{
    public class EventoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calendar()
        {
            ViewData["events"] = new[]
            {
                new Evento { Id = 1, Title = "Video for Marisa", StartDate = "2022-11-14"},
                new Evento { Id = 2, Title = "Preparation", StartDate = "2022-11-12"},
            };

            return View();
        }
    }
}

using Ferpuser.Data;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Ferpuser.BLL.Managers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Ferpuser.BLL.BusinessValidators;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Controllers
{
    public class RestauracionController : Controller
    {
        private readonly ApplicationDbContext db;
        private IOptions<AppSettings> options;
        private RestauracionManager manager;

        public RestauracionController(ApplicationDbContext db, IOptions<AppSettings> options)
        {
            this.db = db;
            this.options = options;
            manager = new RestauracionManager(db);
        }

        public IActionResult Index(Guid CongressId, bool Continuar = false)
        {
            Congress congress = db.Congresses.AsNoTracking().SingleOrDefault(f => f.Id == CongressId);
            if (congress == null)
                return NotFound();
            if (!db.Encuentros.Any(f => f.CongressId == CongressId))
                return View("EventoSinEncuentros", congress);

            ViewBag.Continuar = Continuar;

            return View(congress);
        }
        
        public IActionResult Nif(Guid CongressId)
        {
            RestauracionNifViewModel model = new RestauracionNifViewModel() { CongressId = CongressId };
            Congress congress = db.Congresses.AsNoTracking().SingleOrDefault(f => f.Id == CongressId);
            if (congress == null)
                return NotFound();

            model.Congress = congress;
            return View(model);
        }

        [HttpPost, ActionName("Nif")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NifConfirmed()
        {
            RestauracionNifViewModel model = new RestauracionNifViewModel();
            await TryUpdateModelAsync<RestauracionNifViewModel>(model);

            Congress congress = db.Congresses.AsNoTracking().SingleOrDefault(f => f.Id == model.CongressId);
            if (congress == null)
                return NotFound();

            model.Congress = congress;

            if (ModelState.IsValid)
            {
                RestauracionEncuentrosViewModel item = new RestauracionEncuentrosViewModel();
                item.Inscripcion = manager.GetInscripcion(model.NIF, model.CongressId);
                if (item.Inscripcion == null)
                {
                    TempData["WarningMessage"] = "La persona con el NIF indicado no está inscrito en el evento.";
                    return View(model);
                }

                item.Encuentros = manager.GetEncuentrosActivos(model.CongressId);
                if (item.Encuentros == null || !item.Encuentros.Any())
                {
                    TempData["WarningMessage"] = "Lo sentimos, actualmente no hay encuentros disponibles para este evento.";
                    return View(model);
                }

                return RedirectToAction(nameof(EncuentrosReservados), "Restauracion", new { nif = model.NIF, congressId = model.CongressId });
            }
            return View(model);
        }

        public IActionResult EncuentrosReservados(string nif, Guid congressId)
        {
            Congress congress = db.Congresses.AsNoTracking().SingleOrDefault(f => f.Id == congressId);
            if (congress == null)
                return NotFound();

            RestauracionEncuentrosViewModel model = new RestauracionEncuentrosViewModel()
            {
                CongressId = congressId,
                Congress = congress,
                Inscripcion = manager.GetInscripcion(nif, congressId),
                NIF = nif
            };

            var restauraciones = db.Restauraciones.Where(f => f.NIF == nif).Select(f => f.EncuentroId);
            model.Encuentros = db.Encuentros.Where(f => restauraciones.Contains(f.Id) && f.CongressId == congressId).ToList();
            return View(model);
        }

        public async Task<IActionResult> BajaReserva(string Nif, Guid CongressId, int EncuentroId)
        {            
            var inscripcion = manager.GetInscripcion(Nif, CongressId);
            await manager.DeleteReserva(inscripcion.Id, EncuentroId);
            if (manager.EnviarMailReservas(CongressId, inscripcion.Id, this.ControllerContext, options.Value))
                TempData["SuccessMessage"] = "Su reserva se ha dado de baja correctamente y se ha enviado el mail actualizado con las reservas.";
            else
                TempData["SuccessMessage"] = "Su reserva se ha dado de baja correctamente pero no ha podido enviarse el mail actualizado con sus reservas.";
            return RedirectToAction(nameof(EncuentrosReservados), "Restauracion", new { nif = Nif, congressId = CongressId });
        }

        public IActionResult Encuentros(Guid congressId)
        {
            Congress congress = db.Congresses.AsNoTracking().SingleOrDefault(f => f.Id == congressId);
            if (congress == null)
                return NotFound();

            RestauracionEncuentrosViewModel model = new RestauracionEncuentrosViewModel()
            {
                CongressId = congressId,
                Congress = congress
            };

            model.Encuentros = db.Encuentros.Where(f => f.CongressId == congressId && f.Fecha >= DateTime.Now).ToList();
            return View(model);
        }

        public IActionResult ReservaSeleccionada(Guid CongressId, int EncuentroId)
        {
            Congress congress = db.Congresses.AsNoTracking().SingleOrDefault(f => f.Id == CongressId);
            if (congress == null)
                return NotFound();

            Encuentro encuentro = db.Encuentros.AsNoTracking().SingleOrDefault(f => f.Id == EncuentroId);

            RestauracionReservaSeleccionadaViewModel model = new RestauracionReservaSeleccionadaViewModel()
            {
                CongressId = CongressId,
                Congress = congress,
                EncuentroId = EncuentroId,
                Encuentro = encuentro
            };
            return View(model);
        }

        [HttpPost, ActionName("ReservaSeleccionada")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReservaSeleccionadaConfirmed()
        {
            RestauracionReservaSeleccionadaViewModel model = new RestauracionReservaSeleccionadaViewModel();
            await TryUpdateModelAsync(model);
            model.Congress = db.Congresses.AsNoTracking().SingleOrDefault(f => f.Id == model.CongressId);
            model.Encuentro = db.Encuentros.AsNoTracking().SingleOrDefault(f => f.Id == model.EncuentroId);

            if (ModelState.IsValid)
            {
                //Validación de negocio            
                RestauracionValidator validator = new RestauracionValidator(db);
                List<ValidationResult> businessErrors = new List<ValidationResult>();

                if (model.Encuentro.ReservaMesa)
                    businessErrors.AddRange(validator.ValidateNifs((model.NIFs ?? string.Empty).Split(','), model.CongressId, model.EncuentroId));
                else
                    businessErrors.AddRange(validator.ValidateNif(model.NIFs ?? string.Empty, model.CongressId, model.EncuentroId));

                this.AddValidationErrors(businessErrors);

                if (ModelState.IsValid)
                {
                    await manager.CreateReservas(model.NIFs.Split(','), model.EncuentroId);
                    var NIFsErroresEnvio = manager.EnviarMailReservas(model.CongressId, model.NIFs.Split(','), this.ControllerContext, options.Value);

                    if (NIFsErroresEnvio.Any())
                        TempData["SuccessMessage"] = $"Las reservas se han creado correctamente. No se han podido enviar los mails de confirmación a los siguientes asistentes: {string.Join(", ", NIFsErroresEnvio)}";
                    else
                        TempData["SuccessMessage"] = "Las reservas se han creado correctamente y se han enviado los mails de confirmación.";
                    return RedirectToAction(nameof(Index), new { CongressId = model.CongressId });
                }
            }

            return View(model);
        }

    }   
}

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

namespace Ferpuser.Controllers
{
    public class RestauracionController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly RestauracionManager manager;

        public RestauracionController(ApplicationDbContext db, IOptions<AppSettings> options)
        {
            this.db = db;
            this.manager = new RestauracionManager(db, options.Value);
        }

        public IActionResult Index(Guid CongressId)
        {
            Congress congress = db.Congresses.AsNoTracking().FirstOrDefault(f => f.Id == CongressId);
            RestauracionIndexViewModel model = new RestauracionIndexViewModel()
            {
                CongressId = CongressId,
                Congress = congress
            };

            var encuentros = db.Encuentros.Where(f => f.CongressId == CongressId);
            if (encuentros == null || !encuentros.Any())
                return View("EventoSinEncuentros", model);
            
           
            return View(model);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexConfirmed()
        {
            RestauracionIndexViewModel model = new RestauracionIndexViewModel();
            await TryUpdateModelAsync<RestauracionIndexViewModel>(model);

            Congress congress = db.Congresses.AsNoTracking().FirstOrDefault(f => f.Id == model.CongressId);
            model.Congress = congress;

            if (ModelState.IsValid)
            {
                RestauracionEncuentrosViewModel item = Get(model.NIF, model.CongressId);
                
                if (item.Inscripcion == null)
                {
                    TempData["WarningMessage"] = "La persona con el NIF indicado no está inscrito en el evento.";
                    return View(model);
                }

                if (item.Encuentros == null || !item.Encuentros.Any())
                {
                    TempData["WarningMessage"] = "Lo sentimos, actualmente no hay encuentros disponibles para este evento.";
                    return View(model);
                }

                return RedirectToAction(nameof(Encuentros), "Restauracion", new { nif = model.NIF, congressId = model.CongressId });
            }

            
            return View(model);
        }

        public IActionResult Encuentros(string nif, Guid congressId)
        {
            RestauracionEncuentrosViewModel model = Get(nif, congressId);
            return View(model);
        }

        [HttpPost, ActionName(nameof(Encuentros))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EncuentrosConfirmed(string NIF, Guid congressId, int[] EncuentrosSeleccionados)
        {
            try
            {
                RestauracionEncuentrosViewModel model = Get(NIF, congressId);
                IEnumerable<int> EncuentrosCancelados = model.Encuentros.Select(f => f.Id).Except(EncuentrosSeleccionados);
                
                StatusActualizarReservas status = await manager.ActualizarReservas(
                    NIF, congressId, EncuentrosSeleccionados, EncuentrosCancelados, this.ControllerContext
                );
                switch (status)
                {
                    case StatusActualizarReservas.Correcto:
                        TempData["SuccessMessage"] = "Sus reservas se han actualizado correctamente y se ha enviado el mail de la reserva.";
                        break;
                    case StatusActualizarReservas.ErrorEnvioMail:
                        TempData["SuccessMessage"] = "Sus reservas se han actualizado correctamente pero no ha podido enviarse el mail de la reserva.";
                        break;
                }
            }
            catch
            {
                TempData["WarningMessage"] = "Ha ocurrido un error en el proceso de actualización de reservas.";
            }            
            return RedirectToAction(nameof(Encuentros), new { nif = NIF, congressId = congressId });            
        }

        private RestauracionEncuentrosViewModel Get(string nif, Guid CongressId)
        {
            RestauracionEncuentrosViewModel model = new RestauracionEncuentrosViewModel()
            {
                NIF = nif,
                CongressId = CongressId
            };            

            //Quitar los espacios y pasarlo a minúsculas
            nif = nif.Replace(" ", "").ToUpper().Trim();

            //Buscar el cliente por nif            
            Registration inscripcion = db.Registrations.AsNoTracking()
                .Include(f => f.Registrant)
                .Include(f => f.Congress)
                .FirstOrDefault(f => f.CongressId == CongressId && f.Registrant.NIF.Trim().ToUpper() == nif);
            if (inscripcion == null)
                return model;

            model.Inscripcion = inscripcion;

            List<Encuentro> encuentros = db.Encuentros.AsNoTracking().Include(f => f.Congress)
                .Where(f => 
                    (f.CongressId == inscripcion.CongressId) && 
                    (f.Fecha >= DateTime.Now)
                ).ToList();
            if (!encuentros.Any())
                return model;

            model.Encuentros = encuentros;

            var reservas = db.Restauraciones.AsNoTracking().Where(f => f.NIF == nif);
            foreach (var encuentro in encuentros)
            {
                if (reservas.Any(f => f.EncuentroId == encuentro.Id))
                    model.EncuentrosReservados.Add(encuentro.Id);
            }

            return model;
        }
    }   
}

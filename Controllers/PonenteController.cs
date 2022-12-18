using CsvHelper;
using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Congress")]
    public class PonenteController : Controller
    {
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComuContext;
        private readonly ApplicationDbContext db;

        public PonenteController(SageContext sageContext, SageComuContext sageComuContext, ApplicationDbContext dbContext)
        {
            _sageContext = sageContext;
            _sageComuContext = sageComuContext;
            db = dbContext;
        }

        public async Task<IActionResult> Index(int? pag, string sort = "", string currentsort = "", bool reset = false, bool export = false)
        {
            if (reset)
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(sort))
            {
                if (string.IsNullOrWhiteSpace(currentsort))
                    sort = "Id";
                else
                    sort = currentsort;
            }

            PonenteFilter filter = new PonenteFilter();
            await TryUpdateModelAsync<PonenteFilter>(filter, "filter",
                f => f.Nombre,
                f => f.CongressId);

            Pager pager = new Pager(await db.Ponentes.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<Ponente> list = await db.Ponentes.Where(filter.ExpressionFilter())
                .Include(p => p.Congreso)
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .Select(f => new Ponente()
                {
                    Id = f.Id,
                    NIF = f.NIF,
                    Apellidos = f.Apellidos,
                    Nombre = f.Nombre,
                    CentroTrabajo = f.CentroTrabajo,
                    Mail = f.Mail,
                    Telefono = f.Telefono,
                    Congreso = new Congress()
                    {
                        Id = f.Congreso.Id,
                        Number = f.Congreso.Number,
                        Name = f.Congreso.Name
                    }
                })
                .ToListAsync();

            LoadViewBagEventos();
            return View(list);
        }

        private void LoadViewBagEventos()
        {
            ViewBag.Eventos = db.Congresses.OrderBy(f => f.Number).ThenBy(f => f.Name).Select(f =>
                new Congress
                {
                    Id = f.Id,
                    Name = f.Name,
                    Number = f.Number
                });
        }

        public IActionResult Create()
        {
            ViewBag.TiposComite = db.TiposComite.OrderBy(f => f.Id);
            ViewBag.PuestosComite = db.PuestosComite.OrderBy(f => f.Id);
            LoadViewBagEventos();
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed()
        {
            Ponente model = new Ponente();
            await TryUpdateModelAsync<Ponente>(model, "",
                f => f.CongressId,
                f => f.Login,
                f => f.Password,
                f => f.Nombre,
                f => f.Apellidos,
                f => f.NIF,
                f => f.Localidad,
                f => f.Provincia,
                f => f.Cargo,
                f => f.CentroTrabajo,
                f => f.Telefono,
                f => f.Movil,
                f => f.Mail,
                f => f.Mail2,
                f => f.Tratamiento,
                f => f.AmbitoComite,
                f => f.Activo,
                f => f.Visible,
                f => f.UltimoAcceso,
                f => f.Comentarios,
                f => f.Superevaluador,
                f => f.Visualizador,
                f => f.JuntaDirectiva,
                f => f.FechaRegistro,
                f => f.TipoComiteId,
                f => f.PuestoComiteId
                );

            int controlRepetidos = db.Ponentes.Where(f => f.CentroTrabajo.Equals(model.CentroTrabajo)).Count();


            //Validación de negocio            
            PonenteValidator validator = new PonenteValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid && controlRepetidos == 0)
            {
                PonenteManager manager = new PonenteManager(db);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }

            if (controlRepetidos > 0)
                TempData["ErrorMessage"] = string.Concat("Existen ", controlRepetidos.ToString(), " registro/s con el numero centro de trabajo ", model.CentroTrabajo, ".");

            ViewBag.TiposComite = db.TiposComite.OrderBy(f => f.Id);
            ViewBag.PuestosComite = db.PuestosComite.OrderBy(f => f.Id);
            LoadViewBagEventos();

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = db.Ponentes.Single(f => f.Id == id);
            ViewBag.TiposComite = db.TiposComite.OrderBy(f => f.Id);
            ViewBag.PuestosComite = db.PuestosComite.OrderBy(f => f.Id);
            LoadViewBagEventos();
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            Ponente model = db.Ponentes.Find(id);

            await TryUpdateModelAsync<Ponente>(model, "",
                f => f.CongressId,
                f => f.Login,
                f => f.Password,
                f => f.Nombre,
                f => f.Apellidos,
                f => f.NIF,
                f => f.Localidad,
                f => f.Provincia,
                f => f.Cargo,
                f => f.CentroTrabajo,
                f => f.Telefono,
                f => f.Movil,
                f => f.Mail,
                f => f.Mail2,
                f => f.Tratamiento,
                f => f.AmbitoComite,
                f => f.Activo,
                f => f.Visible,
                f => f.UltimoAcceso,
                f => f.Comentarios,
                f => f.Superevaluador,
                f => f.Visualizador,
                f => f.JuntaDirectiva,
                f => f.FechaRegistro,
                f => f.TipoComiteId,
                f => f.PuestoComiteId
                );

            int controlRepetidos = db.Ponentes.Where(f => f.CentroTrabajo.Equals(model.CentroTrabajo) && f.Id!=id).Count();

            PonenteValidator validator = new PonenteValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid && controlRepetidos == 0)
            {
                PonenteManager manager = new PonenteManager(db);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }

            if (controlRepetidos > 0)
                TempData["ErrorMessage"] = string.Concat("Existen ", controlRepetidos.ToString(), " registro/s con el numero centro de trabajo ", model.CentroTrabajo, ".");

            ViewBag.TiposComite = db.TiposComite.OrderBy(f => f.Id);
            ViewBag.PuestosComite = db.PuestosComite.OrderBy(f => f.Id);
            LoadViewBagEventos();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            PonenteValidator validator = new PonenteValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                PonenteManager manager = new PonenteManager(db);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        public async Task<FileResult> ExportCsv(string currentsort = "")
        {
            PonenteFilter filter = new PonenteFilter();
            await TryUpdateModelAsync<PonenteFilter>(filter, "filter",
                f => f.Nombre,
                f => f.CongressId);

            IEnumerable<Ponente> list = await db.Ponentes
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoPonentes.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<PonenteViewModelMap>();
                await csv.WriteRecordsAsync<Ponente>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

    }
}
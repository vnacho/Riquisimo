using CsvHelper;
using CsvHelper.Configuration;
using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Core;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class EncuentroController : Controller
    {
        private readonly ApplicationDbContext dbContext;        

        public EncuentroController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int? pag, string sort = "", string currentsort = "", bool reset = false, bool export = false)
        {
            if (reset)
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(sort))
            {
                if (string.IsNullOrWhiteSpace(currentsort))
                    sort = "Id desc";
                else
                    sort = currentsort;
            }

            EncuentroFilter filter = new EncuentroFilter();
            await TryUpdateModelAsync<EncuentroFilter>(filter, "filter",
                f => f.CongressId);

            Pager pager = new Pager(await dbContext.Encuentros.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<Encuentro> list = await dbContext.Encuentros.AsNoTracking().Include(f => f.Congress).Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .Select(y => new Encuentro
                {
                    Id = y.Id,
                    Nombre = y.Nombre,
                    Fecha = y.Fecha,
                    Lugar = y.Lugar,
                    NumeroMesas = y.NumeroMesas,
                    ComensalesMesa = y.ComensalesMesa,
                    Congress = new Congress
                    {
                        Id = y.Congress.Id,
                        Name = y.Congress.Name,
                        Number = y.Congress.Number
                    }
                })
                .ToListAsync();

            LoadViewBag();
            return View(list);
        }

        public IActionResult Create()
        {
            LoadViewBag();
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed()
        {
            Encuentro model = new Encuentro();
            await TryUpdateModelAsync<Encuentro>(model, "",
                f => f.Nombre,
                f => f.CongressId,
                f => f.Fecha,
                f => f.Lugar,
                f => f.NumeroMesas,
                f => f.ComensalesMesa,
                f => f.ReservaMesa
            );

            //Validación de negocio            
            EncuentroValidator validator = new EncuentroValidator(dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                EncuentroManager manager = new EncuentroManager(dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }
            LoadViewBag();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = dbContext.Encuentros.AsNoTracking().Single(f => f.Id == id);
            LoadViewBag();
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            Encuentro model = dbContext.Encuentros.Find(id);

            await TryUpdateModelAsync<Encuentro>(model, "",
                f => f.Nombre,
                f => f.CongressId,
                f => f.Fecha,
                f => f.Lugar,
                f => f.NumeroMesas,
                f => f.ComensalesMesa,
                f => f.ReservaMesa
            );

            EncuentroValidator validator = new EncuentroValidator(dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                EncuentroManager manager = new EncuentroManager(dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }
            LoadViewBag();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            EncuentroValidator validator = new EncuentroValidator(dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                EncuentroManager manager = new EncuentroManager(dbContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Inscritos(int id)
        {
            InscritosEncuentroViewModel model = new InscritosEncuentroViewModel();
            model.Filter.EncuentroId = id;

            model.Encuentro = dbContext.Encuentros.Include(f => f.Congress).AsNoTracking().FirstOrDefault(f => f.Id == id);
            var restauraciones = dbContext.Restauraciones.Include(f => f.Registration.Registrant).AsNoTracking().Where(model.Filter.ExpressionFilter());

            foreach (var restauracion in restauraciones)
            {
                model.Inscritos.Add(new InscritosEncuentroDto()
                {
                    NumeroMesa = restauracion.NumeroMesa,
                    registration = restauracion.Registration,
                });
            }            

            return View(model);
        }

        [HttpPost, ActionName("Inscritos")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InscritosConfirmed()
        {
            InscritosEncuentroViewModel model = new InscritosEncuentroViewModel();
            await TryUpdateModelAsync<RestauracionFilter>(model.Filter, "Filter");

            model.Encuentro = dbContext.Encuentros.Include(f => f.Congress).AsNoTracking().FirstOrDefault(f => f.Id == model.Filter.EncuentroId);

            var restauraciones = dbContext.Restauraciones.Include(f => f.Registration.Registrant).AsNoTracking().Where(model.Filter.ExpressionFilter());

            foreach (var restauracion in restauraciones)
            {
                model.Inscritos.Add(new InscritosEncuentroDto()
                {
                    NumeroMesa = restauracion.NumeroMesa,
                    registration = restauracion.Registration,
                });
            }

            return View(model);
        }

        public async Task<FileResult> InscritosCsv()
        {
            InscritosEncuentroViewModel model = new InscritosEncuentroViewModel();
            await TryUpdateModelAsync<RestauracionFilter>(model.Filter, "Filter");

            model.Encuentro = dbContext.Encuentros.Include(f => f.Congress).AsNoTracking().FirstOrDefault(f => f.Id == model.Filter.EncuentroId);

            var restauraciones = dbContext.Restauraciones.Include(f => f.Registration.Registrant).AsNoTracking().Where(model.Filter.ExpressionFilter());

            foreach (var restauracion in restauraciones)
            {
                model.Inscritos.Add(new InscritosEncuentroDto()
                {
                    NumeroMesa = restauracion.NumeroMesa,
                    registration = restauracion.Registration,
                });
            }
            
            //GetUnpaidExpensesSage(start, end, out List<UnpaidExpensesViewModel> records, Cliente, Vendedor, Almacen, FilterEstado, FilterRetencion);
            string fileName = "Listado de inscritos - Restauracion.csv";
            return await GetCsvAsync<InscritosEncuentroDto, InscritosEncuentroDtoMap>(model.Inscritos, fileName);
        }

        private async Task<FileResult> GetCsvAsync<T, E>(IEnumerable<T> records, string fileName) where E : ClassMap<T>
        {
            if (!records.Any())
                return null;

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.GetCultureInfo("es-ES")))
            {
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<E>();
                await csv.WriteRecordsAsync<T>(records);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);
        }

        private void LoadViewBag()
        {
            ViewBag.Eventos = dbContext.Congresses.OrderBy(f => f.Number).ThenBy(f => f.Name).Select(f =>
                new Congress
                {
                    Id = f.Id,
                    Name = f.Name,
                    Number = f.Number
                });
        }
    }
}

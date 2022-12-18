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
    public class SociedadCientificaController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public SociedadCientificaController(ApplicationDbContext dbContext)
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

            SociedadCientificaFilter filter = new SociedadCientificaFilter();
            await TryUpdateModelAsync<SociedadCientificaFilter>(filter, "filter",
                f => f.Nombre);

            Pager pager = new Pager(await dbContext.SociedadesCientificas.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<SociedadCientifica> list = await dbContext.SociedadesCientificas.AsNoTracking().Where(filter.ExpressionFilter())
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .Select(y => new SociedadCientifica
                {
                    Id = y.Id,
                    Nombre = y.Nombre                    
                })
                .ToListAsync();

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed()
        {
            SociedadCientifica model = new SociedadCientifica();
            await TryUpdateModelAsync<SociedadCientifica>(model, "",
                f => f.Nombre
            );

            //Validación de negocio            
            SociedadCientificaValidator validator = new SociedadCientificaValidator(dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                SociedadCientificaManager manager = new SociedadCientificaManager(dbContext);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = dbContext.SociedadesCientificas.AsNoTracking().Single(f => f.Id == id);            
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            SociedadCientifica model = dbContext.SociedadesCientificas.Find(id);

            await TryUpdateModelAsync<SociedadCientifica>(model, "",
                f => f.Nombre
            );

            SociedadCientificaValidator validator = new SociedadCientificaValidator(dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                SociedadCientificaManager manager = new SociedadCientificaManager(dbContext);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            SociedadCientificaValidator validator = new SociedadCientificaValidator(dbContext);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                SociedadCientificaManager manager = new SociedadCientificaManager(dbContext);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Socios(int id)
        {
            SociosSociedadCientificaViewModel model = new SociosSociedadCientificaViewModel();
            model.SociedadCientificaId = id;
            model.SociedadCientifica = dbContext.SociedadesCientificas.AsNoTracking().FirstOrDefault(f => f.Id == id);
            model.Socios = dbContext.SociosSociedadCientifica.AsNoTracking().Include(f => f.CargoJuntaDirectivaSociedadCientifica).Where(f => f.SociedadCientificaId == id).ToList();
            return View(model);
        }

        //[HttpPost, ActionName("Socios")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SociosConfirmed()
        //{
        //    SociosSociedadCientificaViewModel model = new SociosSociedadCientificaViewModel();
        //    await TryUpdateModelAsync<SociosSociedadCientificaViewModel>(model);
        //    model.SociedadCientifica = dbContext.SociedadesCientificas.AsNoTracking().FirstOrDefault(f => f.Id == model.SociedadCientificaId);
        //    model.Socios = dbContext.SociosSociedadCientifica.AsNoTracking().Include(f => f.CargoJuntaDirectivaSociedadCientifica).Where(f => f.SociedadCientificaId == model.SociedadCientificaId).ToList();
        //    return View(model);
        //}

        public async Task<FileResult> SociosCsv()
        {
            SociosSociedadCientificaViewModel model = new SociosSociedadCientificaViewModel();
            await TryUpdateModelAsync<SociosSociedadCientificaViewModel>(model);
            model.SociedadCientifica = dbContext.SociedadesCientificas.AsNoTracking().FirstOrDefault(f => f.Id == model.SociedadCientificaId);
            model.Socios = dbContext.SociosSociedadCientifica.AsNoTracking().Include(f => f.CargoJuntaDirectivaSociedadCientifica).Where(f => f.SociedadCientificaId == model.SociedadCientificaId).ToList();            

            string fileName = "Listado de socios.csv";
            return await GetCsvAsync<SocioSociedadCientifica, SocioSociedadCientificaMap>(model.Socios, fileName);
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
    }
}

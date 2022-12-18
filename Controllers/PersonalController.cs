using CsvHelper;
using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Interfaces;
using Ferpuser.BLL.Managers;
using Ferpuser.BLL.Services;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Core;
using Ferpuser.Models.Enums;
using Ferpuser.Models.SessionObjects;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Collaborations")]
    public class PersonalController : ControlPresupuestarioBaseController
    {
        private readonly ApplicationDbContext db;
        private IWebHostEnvironment _hostEnvironment;

        public PersonalController(ApplicationDbContext dbContext, IWebHostEnvironment environment)
        {
            db = dbContext;
            _hostEnvironment = environment;
        }

        public async Task<IActionResult> Index(int? pag, string sort = "", string currentsort = "", bool reset = false, bool export = false)
        {
            //Guardar estado de listado con paginación, ordenación y filtros
            string url = HttpContext.Session.GetString(Consts.PRESERVE_PERSONAL_URL);
            if (!string.IsNullOrWhiteSpace(url))
            {
                HttpContext.Session.Remove(Consts.PRESERVE_PERSONAL_URL);
                if (url.Contains("Personal")) //Puede venir de otra página por lo que realizamos esa comprobación
                    return Redirect(url);
            }
            if (reset)             
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(sort))
            {
                if (string.IsNullOrWhiteSpace(currentsort))
                    sort = "Id desc";
                else
                    sort = currentsort;
            }

            PersonalFilter filter = new PersonalFilter();
            await TryUpdateModelAsync<PersonalFilter>(filter, "filter",
                f => f.Nombre,
                f => f.Categoria,
                f => f.TieneFechaAlta,
                f => f.TieneFechaApto,
                f => f.TieneFechaBaja,
                f => f.TieneFechaEPI,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.FechaDesdeEntEPI,
                f => f.FechaHastaEntEPI,
                f => f.TieneIBAN,
                f => f.CT,
                f => f.ObraId,
                f => f.TipoUltimoContratoId);            
            
            Pager pager = new Pager(await db.Personal.CountAsync(filter.ExpressionFilter()), pag, 100, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<Personal> list = await db.Personal
                .Include(f => f.CentroCoste)
                .Include(f => f.Obra)
                .Include(f => f.TipoTarifa)
                .Include(f => f.TipoUltimoContrato)
                .Where(filter.ExpressionFilter()) 
                .OrderBy(sort)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToListAsync();

            CargarCombos();

            return View(list);
        }

        public async Task<FileResult> ExportCsv(string currentsort = "")
        {
            PersonalFilter filter = new PersonalFilter();
            await TryUpdateModelAsync<PersonalFilter>(filter, "filter",
                f => f.Nombre,
                f => f.Categoria,
                f => f.TieneFechaAlta,
                f => f.TieneFechaApto,
                f => f.TieneFechaBaja,
                f => f.TieneFechaEPI,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.FechaDesdeEntEPI,
                f => f.FechaHastaEntEPI,
                f => f.TieneIBAN,
                f => f.CT,
                f => f.ObraId,
                f => f.TipoUltimoContratoId
                );

            IEnumerable<Personal> list = await db.Personal
                .Include(f => f.CentroCoste)
                .Include(f => f.Obra)
                .Include(f => f.TipoTarifa)
                .Include(f => f.TipoUltimoContrato)
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_Personal.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<PersonalMap>();
                await csv.WriteRecordsAsync<Personal>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

        public async Task<FileResult> ExportPdf(string currentsort = "")
        {
            PersonalFilter filter = new PersonalFilter();
            await TryUpdateModelAsync<PersonalFilter>(filter, "filter",
                f => f.Nombre,
                f => f.Categoria,
                f => f.TieneFechaAlta,
                f => f.TieneFechaApto,
                f => f.TieneFechaBaja,
                f => f.TieneFechaEPI,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.FechaDesdeEntEPI,
                f => f.FechaHastaEntEPI,
                f => f.TieneIBAN,
                f => f.CT,
                f => f.ObraId,
                f => f.TipoUltimoContratoId
                );

            IEnumerable<Personal> list = await db.Personal
                .Include(f => f.CentroCoste)
                .Include(f => f.Obra)
                .Include(f => f.TipoTarifa)
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string table = "<table class='w-100'><tr class='font-weight-bold'>" +
                "<td>Id</td><td>NIF</td><td>Nombre</td><td>Val. NIF</td>" + 
                "<td>Categoría</td><td>Rev. médica</td>" +
                "<td>Última Rev.</td><td>SAP</td><td>Fecha apto</td>" + 
                "<td>IBAN</td><td>CT</td><td>Fecha alta</td>" + 
                "<td>Fecha baja</td><td>Cód. Obra</td><td>Obra</td>" +
                "</tr>{0}</table>";

            string rows = string.Empty;
            foreach (var item in list)
            {
                rows += $"<tr><td>{item.Id}</td><td>{item.NIF}</td><td>{item.Nombre}</td><td>{item.FechaValidezNIF.ToShortDateString()}</td>" + 
                        $"<td>{item.Categoria}</td><td>{item.RevisionMedica}</td>" + 
                        $"<td>{item.FechaUltimaRevisionMedica?.ToShortDateString()}</td><td>{item.SAP}</td><td>{item.FechaApto}</td>" + 
                        $"<td>{item.IBAN_Display}</td><td>{item.CT}</td><td>{item.FechaAlta?.ToShortDateString()}</td>" + 
                        $"<td>{item.FechaBaja?.ToShortDateString()}</td><td>{item.Obra?.NivelAnalitico2}</td><td>{item.Obra?.Nombre}</td>" +
                        $"</tr>";
            }

            var html = PrintService.GetHtmlTheme("Listado de personal", string.Format(table, rows));
            var pdf = PrintService.GetBytesLandscape(html);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_Personal.pdf";
            return File(pdf, "application/pdf", fileName);
        }

        public IActionResult Create()
        {
            var url = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            if (url.EndsWith(nameof(Personal)) || url.EndsWith($"{nameof(Personal)}/") || url.Contains("Index") || url.Contains("Personal?")) //Guardar filtros y paginación de index
                HttpContext.Session.SetString(Consts.PRESERVE_PERSONAL_URL, url);

            ViewBag.TiposTarifa = db.TiposTarifa.OrderBy(f => f.Nombre);
            ViewBag.CentrosCoste = db.CentrosCoste.OrderBy(f => f.Nombre);
            ViewBag.TiposContratos = db.TipoContrato.OrderBy(f => f.Nombre);
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed()
        {
            Personal model = new Personal();
            await TryUpdateModelAsync<Personal>(model, "",
                f => f.Nombre,
                f => f.NIF,
                f => f.FechaNacimiento,
                f => f.FechaValidezNIF,
                f => f.RevisionMedica,
                f => f.SAP,
                f => f.FechaApto,
                f => f.FechaEntregaEpi,
                f => f.CT,
                f => f.Venta,
                f => f.Categoria,
                f => f.CosteEstandar,
                f => f.FechaAlta,
                f => f.FechaUltimaRevisionMedica,
                f => f.FechaBaja,
                f => f.IBAN,
                f => f.TipoTarifaId,
                f => f.CentroCosteId,
                f => f.ObraId,
                f => f.PrecioHora,
                f => f.FechaUltimoContrato,
                f => f.TipoUltimoContratoId);

            //Validación de negocio            
            PersonalValidator validator = new PersonalValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                PersonalManager manager = new PersonalManager(db);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.TiposTarifa = db.TiposTarifa.OrderBy(f => f.Nombre);
            ViewBag.CentrosCoste = db.CentrosCoste.OrderBy(f => f.Nombre);
            ViewBag.TiposContratos = db.TipoContrato.OrderBy(f => f.Nombre);
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var url = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            if (url != null && (url.EndsWith(nameof(Personal)) || url.EndsWith($"{nameof(Personal)}/") || url.Contains("Index") || url.Contains("Personal?"))) //Guardar filtros y paginación de index
                HttpContext.Session.SetString(Consts.PRESERVE_PERSONAL_URL, url);

            var model = db.Personal.Include(f => f.Documentos).Single(f => f.Id == id);
            ViewBag.TiposTarifa = db.TiposTarifa.OrderBy(f => f.Nombre);
            ViewBag.CentrosCoste = db.CentrosCoste.OrderBy(f => f.Nombre);
            ViewBag.TiposContratos = db.TipoContrato.OrderBy(f => f.Nombre);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id)
        {
            Personal model = db.Personal.Find(id);

            await TryUpdateModelAsync<Personal>(model, "",
                f => f.Nombre,
                f => f.NIF,
                f => f.FechaNacimiento,
                f => f.FechaValidezNIF,
                f => f.RevisionMedica,
                f => f.SAP,
                f => f.FechaApto,
                f => f.FechaEntregaEpi,
                f => f.CT,
                f => f.Venta,
                f => f.Categoria,
                f => f.CosteEstandar,
                f => f.FechaAlta,
                f => f.FechaUltimaRevisionMedica,
                f => f.FechaBaja,
                f => f.IBAN,
                f => f.TipoTarifaId,
                f => f.CentroCosteId,
                f => f.ObraId,
                f => f.PrecioHora,
                f => f.FechaUltimoContrato,
                f => f.TipoUltimoContratoId
                );

            PersonalValidator validator = new PersonalValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                PersonalManager manager = new PersonalManager(db);
                await manager.Edit(model);
                TempData["Message"] = "El registro se ha modificado correctamente.";
                return RedirectToAction("Index");
            }
            ViewBag.TiposTarifa = db.TiposTarifa.OrderBy(f => f.Nombre);
            ViewBag.CentrosCoste = db.CentrosCoste.OrderBy(f => f.Nombre);
            ViewBag.TiposContratos = db.TipoContrato.OrderBy(f => f.Nombre);

            db.Entry(model).Collection(b => b.Documentos).Load();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DescargarDocumentacion(int id)
        {
            DocumentosViewModel model = new DocumentosViewModel();
            await TryUpdateModelAsync<DocumentosViewModel>(model);

            if (ModelState.IsValid)
            {
                string html, nombre, nombrePlantilla, nombrePdf;
                html = nombre = nombrePlantilla = nombrePdf = string.Empty;

                var trabajador = db.Personal.Find(model.IdPersonal);
                var obra = db.CentrosCoste.FirstOrDefault(f => f.Id == model.IdObra);

                Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();

                //INF PS
                nombre = "INF PS";
                nombrePlantilla = "DocumentoInfPS.html";
                AddEntry(dictionary, nombre, nombrePlantilla, trabajador, obra, model.Fecha.Value);

                //INF COVID
                nombre = "INF COVID";
                nombrePlantilla = "DocumentoInfCOVID.html";
                AddEntry(dictionary, nombre, nombrePlantilla, trabajador, obra, model.Fecha.Value);

                //EPIS
                nombre = "EPIS";
                nombrePlantilla = "DocumentoEPIS.html";
                AddEntry(dictionary, nombre, nombrePlantilla, trabajador, obra, model.Fecha.Value);

                //EPIS
                nombre = "MAQUINARIA";
                nombrePlantilla = "DocumentoMaquinaria.html";
                AddEntry(dictionary, nombre, nombrePlantilla, trabajador, obra, model.Fecha.Value);

                //EPIS
                nombre = "INF RLAB";
                nombrePlantilla = "DocumentoInfRLAB.html";
                AddEntry(dictionary, nombre, nombrePlantilla, trabajador, obra, model.Fecha.Value);

                using (var compressedFileStream = new MemoryStream())
                {
                    //Create an archive and store the stream in memory.
                    using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, false))
                    {
                        foreach (var entry in dictionary)
                        {
                            var zipEntry = zipArchive.CreateEntry(entry.Key);
                            using (var originalFileStream = new MemoryStream(entry.Value))
                            using (var zipEntryStream = zipEntry.Open())
                            {
                                originalFileStream.CopyTo(zipEntryStream);
                            }
                        }                        
                    }

                    return File(compressedFileStream.ToArray(), "application/zip", "Filename.zip");
                }
            }

            ViewBag.Personal = db.Personal.OrderBy(f => f.Nombre);
            ViewBag.CentrosCoste = db.CentrosCoste.OrderBy(f => f.Nombre);
            return PartialView("_Documentacion", model);
        }

        private void AddEntry(Dictionary<string, byte[]> dictionary, string nombre, string nombrePlantilla, Personal trabajador, CentroCoste obra, DateTime fecha)
        {
            string html, nombrePdf;
            using (StreamReader reader = new StreamReader(Path.Combine("HtmlTemplates", nombrePlantilla)))
            {
                html = reader.ReadToEnd();
            }

            ParametroManager manager = new ParametroManager(db);
            var datos_empresa = manager.GetDatosEmpresa();
            html = html.Replace("#URL_LOGO", datos_empresa.Logo);
            html = html.Replace("#URL_FIRMA", datos_empresa.Firma);
            html = html.Replace("#FECHA", fecha.ToLongDateString());
            html = html.Replace("#OBRA", obra?.Nombre?.ToUpper());
            html = html.Replace("#TRABAJADOR", trabajador.Nombre?.ToUpper());
            html = html.Replace("#DNI", trabajador.NIF?.ToUpper());
            html = html.Replace("#CATEGORIA#", trabajador.Categoria);
            html = html.Replace("#EMPRESA#", datos_empresa.Nombre);
            nombrePdf = $"{trabajador.Nombre?.ToUpper()?.Trim()}-{nombre}.pdf";
            dictionary.Add(nombrePdf, PrintService.GetBytes(html));
        }

        public async Task<IActionResult> Delete(int id)
        {
            PersonalValidator validator = new PersonalValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                PersonalManager manager = new PersonalManager(db);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocumento(
            int Id, 
            IFormFile fileDocumentacion, 
            IFormFile fileFormacion, 
            IFormFile filePlantillas, 
            IFormFile fileContratos)
        {
            IFileUploader _fileUploader = (IFileUploader)HttpContext.RequestServices.GetService(typeof(IFileUploader));
            Documento model = new Documento() { PersonalId = Id };
            string url = string.Empty;
            if (fileDocumentacion != null)
            {
                model.Tipo = TipoDocumento.Documentacion;
                model.FicheroNombre = fileDocumentacion.FileName;
                model.FicheroUrl = await _fileUploader.UploadFile(fileDocumentacion);                
            }
            else if (fileFormacion != null)
            {
                model.Tipo = TipoDocumento.Formacion;
                model.FicheroNombre = fileFormacion.FileName;
                model.FicheroUrl = await _fileUploader.UploadFile(fileFormacion);
            }
            else if (filePlantillas != null)
            {
                model.Tipo = TipoDocumento.Plantilla;
                model.FicheroNombre = filePlantillas.FileName;
                model.FicheroUrl = await _fileUploader.UploadFile(filePlantillas);
            }
            else if (fileContratos != null)
            {
                model.Tipo = TipoDocumento.Contrato;
                model.FicheroNombre = fileContratos.FileName;
                model.FicheroUrl = await _fileUploader.UploadFile(fileContratos);
            }

            db.Documento.Add(model);
            db.SaveChanges();

            TempData["Message"] = "El fichero se ha añadido correctamente.";
            return RedirectToAction("Edit", new { Id = Id });
        }

        public IActionResult BorrarDocumento(int Id, int PersonalId)
        {
            db.Documento.Remove(db.Documento.Find(Id));
            db.SaveChanges();
            TempData["Message"] = "El fichero se ha borrado correctamente.";
            return RedirectToAction("Edit", new { Id = PersonalId });
        }

        private void CargarCombos()
        {
            ViewBag.CentrosCoste = db.CentrosCoste.OrderBy(f => f.Nombre);
            ViewBag.TiposContrato = db.TipoContrato.OrderBy(f => f.Nombre);
        }

        [HttpGet]
        public PartialViewResult BuscarDocumentacion(int id, TipoDocumento tipo, string search = "")
        {
            ViewData["Tipo"] = tipo;
            var model = db.Personal.Single(f => f.Id == id);
            model.Documentos = db.Documento.Where(f => f.PersonalId == id && f.Tipo == tipo && f.FicheroNombre.Contains(search)).ToList();            
            return PartialView("_Documentos", model);
        }
    }
}

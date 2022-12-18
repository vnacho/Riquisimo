using Ferpuser.BLL.Filters;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.SessionObjects;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

using Ferpuser.BLL.Managers;
using Ferpuser.BLL.Interfaces;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Ferpuser.Controllers
{
    [Authorize]
    public class CongressesController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly SageContext _sage;

        public CongressesController(ApplicationDbContext context, SageContext sage)
        {
            db = context;
            _sage = sage;
        }

        public  async Task<IActionResult> Index(int? pag, string sort = "", string currentsort = "", bool reset = false, bool export = false)
        {
            if (reset)
            {
                HttpContext.Session.Remove(Consts.SESSION_REGISTRATION_LIST_STATE);
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString("MENU", "MENU_EVENTOS");

            CongressesFilter filter;
            string sSesion = HttpContext.Session.GetString(Consts.SESSION_REGISTRATION_LIST_STATE);
            var previous = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            CongressesSession objSesion;

            if (!string.IsNullOrWhiteSpace(previous) && previous.Contains("/Congresses/", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(sSesion))
            {
                objSesion = JsonConvert.DeserializeObject<CongressesSession>(sSesion);
                sort = objSesion.sort;
                filter = objSesion.filter;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(sort))
                {
                    if (string.IsNullOrWhiteSpace(currentsort))
                        sort = "Id desc";
                    else
                        sort = currentsort;
                }

                filter = new CongressesFilter();

                await TryUpdateModelAsync<CongressesFilter>(filter, "filter",
                    f => f.TipoCongreso);
            }

            ViewData["Sort"] = sort;
            ViewData["Filter"] = filter;

            objSesion = new CongressesSession()
            {
                filter = filter,
                sort = sort
            };

            HttpContext.Session.SetString(Consts.SESSION_REGISTRATION_LIST_STATE, JsonConvert.SerializeObject(objSesion));

            IEnumerable<Congress> list;
            if (filter.TipoCongreso != Consts.CODIGO_TIPO_EVENTO_OBRA)
            {
                list = await db.Congresses.Where(filter.ExpressionFilter())
                    .Select(y => new Congress
                    {
                        Id = y.Id,
                        Number = y.Number,
                        Code = y.Code,
                        Name = y.Name,
                        Place = y.Place,
                        StartDate = y.StartDate,
                        EndDate = y.EndDate,
                        TipoCongress = y.TipoCongress
                    })
                    .OrderBy(sort)
                    .ToListAsync();

               
            }
            else //Tipo obra
            {
                list = await db.Congresses.Where(filter.ExpressionFilter())
                    .Select(y => new Congress
                    {
                        Id = y.Id,
                        Number = y.Number,
                        Code = y.Code,
                        Name = y.Name,
                        NombreCliente = y.NombreCliente,
                        PoblacionObra = y.PoblacionObra,
                        FechaInicio = y.FechaInicio,
                        ContratosObra = y.ContratosObra,
                        Finalizada = y.Finalizada
                    })
                    .OrderBy(sort)
                    .ToListAsync();

                foreach (var item in list)
                {
                    item.NombreCliente = _sage.Clientes.FirstOrDefault(f => f.Codigo == item.NombreCliente)?.Nombre;
                }
            }

            return View(list);
        }

        public IActionResult RegistrantSelector(Guid? id)
        {

            if (!id.HasValue)
            {
                return NotFound();
            }
            if (!db.Congresses.Where(c => c.Deleted == null).Any(m => (Guid?)m.Id == id))
            {
                return NotFound();
            }
            ViewData["CongressId"] = id.Value;
            return View(db.Registrations.Include(r => r.Registrant).Include(r => r.Client).Where(r => r.Deleted == null && r.CongressId.Equals(id.Value)).ToList());
        }
        [HttpPost]
        public IActionResult Credentials(Guid? id, List<Guid> registrationIds, string mode, bool debug)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            List<Registration> registrations = db.Registrations
                .Include(r => r.Registrant)
                    .ThenInclude(rr => rr.Treatment)
                .Include(r => r.Client)
                .Include(r => r.BillingLocation)
                .Where(r => r.Deleted == null && registrationIds.Contains(r.Id)).ToList();
            var congress = db.Congresses.Find(id.Value);

            var useBarcode = mode.Equals("useBarcode");
            var isApp = mode.Equals("isApp");
            var isWallet = mode.Equals("isWallet");
            var isEnvelope = mode.Equals("isEnvelope");

            ViewData["UseBarcode"] = useBarcode;
            ViewData["IsApp"] = isApp;
            ViewData["IsWallet"] = isWallet;
            ViewData["IsEnvelope"] = isEnvelope;
            var regs = new List<Registration>(registrations);

            if (debug)
            {
                for (int i = 0; i < 50; i++)
                {
                    regs.AddRange(registrations);
                }
                regs = regs.OrderBy(a => Guid.NewGuid()).ToList();
            }
            else
            {
                regs = regs.OrderBy(r => r.Registrant.Surnames).ThenBy(r => r.Registrant.Name).ToList();
            }
            var vm = new Credentials
            {
                Registrations = regs,
                Congress = congress
            };
            return View(vm);
        }

        public IActionResult ClientsPending(Guid? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            IEnumerable<IGrouping<Client, Registration>> pending = db.Registrations
                .Include(r => r.Client)
                .Include(r => r.Registrant)
                .Include(r => r.Congress)
                .Where(r => r.Deleted == null && r.CongressId.Equals(id) && !r.Paid).ToList().GroupBy(r => r.Client);
            return View(pending);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }
            Congress congress = await db.Congresses.Where(c => c.Deleted == null).FirstOrDefaultAsync(m => (Guid?)m.Id == id);
            return congress != null ? View(congress) : (IActionResult)NotFound();
        }

        public IActionResult Create()
        {
            CargarCombosComunes();
            //this

            string sort = "";
            CargarFiltro(sort);

            return View(new Congress()
            {
                Number = 0,
                IsCongress = true   
            });
        }
        public IActionResult CreateCostCenter()
        {
            ViewData["Almacen"] = _sage.Almacen.OrderBy(a => a.Codigo).AsNoTracking();
            return View("Create", new Congress()
            {
                Number = 0,
                IsCongress = false,
                StartDate = new DateTime(0),
                EndDate = new DateTime(0),
                Code = "FACT"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Congress congress)
        {
            CargarCombosComunes("46015");

            if (!ModelState.IsValid)
                return View(congress);
            
            if (db.Congresses.Where(c => c.Deleted == null).Any(x => x.Code.Equals(congress.Code) && x.Number == congress.Number))
            {
                ModelState.AddModelError("", "Ya existe un congreso con el código y la clave proporcionados");
                return View(congress);
            }

            switch (congress.TipoCongress)
            {
                case "O":
                    congress.IsCongress = false;
                    break;
                case "T":
                    congress.IsCongress = true;
                    break;
            }

            CongressesManager manager = new CongressesManager(db);
            manager.Create(congress);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();            

            Congress congress = await db.Congresses
                                        .Include(f => f.DocumentosObra)
                                        .Include(f => f.ContratosObra)
                                        .Where(f => f.Id == id).FirstOrDefaultAsync();

            if (congress == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(congress.ConnectionString) && string.IsNullOrWhiteSpace(congress.DatabaseServer))
            {
                var s = congress.ConnectionString.Trim().Split(";");
                //Server=36enfermeriatraumatologia.com;Database=cot36_bdatos;User=cot36_user;Password="g7RcU@$;Advk";
                congress.DatabaseServer = s.FirstOrDefault(i => i.ToLower().StartsWith("server")).Split("=").Last();
                congress.Database = s.FirstOrDefault(i => i.ToLower().StartsWith("database")).Split("=").Last();
                congress.DatabaseUser = s.FirstOrDefault(i => i.ToLower().StartsWith("user")).Split("=").Last();
                congress.DatabasePassword = s.FirstOrDefault(i => i.ToLower().StartsWith("password")).Split("=").Last();
            }

            if (db.Congresses.Any(x => x.Deleted == null && x.Code.Equals(congress.Code) && x.Number == congress.Number && x.Id != id))
                ModelState.AddModelError("", "Ya existe un congreso con el código y la clave proporcionados");

            ViewData["totalContratos"] = congress.TotalContratos().ToString("C");
            ViewData["Registered"] = db.Registrations.Count(r => r.Deleted == null && r.CongressId.Equals(id));
            
            CargarCombosComunes(congress.CodigoPostalObra);

            string sort = "";

            CargarFiltro(sort);
 
            return View(congress);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Congress congress)
        {
            if (id != congress.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["Registered"] = db.Registrations.Count(r => r.Deleted == null && r.CongressId.Equals(id));
                CargarCombosComunes();
                return View(congress);
            }

            try
            {
                CongressesManager manager = new CongressesManager(db);
                congress.Modified = DateTime.Now;

                switch (congress.TipoCongress)
                {
                    case "O":
                        congress.IsCongress = false;
                        break;
                    case "T":
                        congress.IsCongress = true;
                        break;
                    default:
                        congress.IsCongress = true;
                        break;
                }

                manager.Edit(congress);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CongressExists(congress.Id))
                    return NotFound();
                throw;
            }

            ViewData["Registered"] = db.Registrations.Count(r => r.Deleted == null && r.CongressId.Equals(id));
            
            CargarCombosComunes();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                Congress congress = await db.Congresses
                    .IncludeFilter(c => c.Registrations.FirstOrDefault())
                    .IncludeFilter(c => c.Accommodations.FirstOrDefault())
                    .IncludeFilter(c => c.Expenses.FirstOrDefault())
                    .IncludeFilter(c => c.CongressEmailAccounts.FirstOrDefault())
                    .FirstOrDefaultAsync(c => c.Id.Equals(id));
                if (congress == null)
                {
                    return NotFound();
                }
                if (congress.Registrations == null || congress.Registrations.Any() || 
                    congress.Accommodations == null || congress.Accommodations.Any() ||
                    congress.Expenses == null || congress.Expenses.Any() ||
                    congress.CongressEmailAccounts == null || congress.CongressEmailAccounts.Any()
                    )
                {
                    return StatusCode(405);
                }
                CongressesManager manager = new CongressesManager(db);
                congress.Deleted = DateTime.Now;
                await manager.Delete(congress.Id);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }

        }

        private bool CongressExists(Guid id)
        {
            return db.Congresses.Where(c => c.Deleted == null).Any(e => e.Id == id);
        }

        public JsonResult CheckIfCodeExists(string Code, Guid Id, int Number)
        {
            return Json(db.Congresses.Where(c => c.Deleted == null).FirstOrDefault(x => x.Code.Equals(Code) && x.Number == Number && x.Id != Id) == null);
        }

        public async Task<IActionResult> LogoAsync(string id)
        {
            Congress congress = await db.Congresses.FindAsync(Guid.Parse(id.Split(".")[0]));
            if (congress == null)
            {
                return NotFound();
            }

            byte[] imageBytes = Convert.FromBase64String(congress.LogoBase64.Split(";base64,")[1]);
            return File(imageBytes, congress.LogoBase64.Split("base64,")[0].Substring(5));
        }

        [HttpPost]
        public async Task<PartialViewResult> Refrescar()
        {
            //Congress model = new Congress();
            Congress model;
            string numero = Request.Form["Id"];
            string archivoVistaParcial = "~/Views/Congresses/Create.cshtml";

            Guid elId;

            if (string.IsNullOrEmpty(numero))
            {
                model = new Congress();
            }
            else
            {
                elId = new Guid(numero);

                model = db.Congresses.Where(f => f.Id == elId)
                    .Include(f => f.ContratosObra)
                    .Include(f => f.DocumentosObra)
                    .FirstOrDefault();
                archivoVistaParcial = "~/Views/Congresses/_EditCongress.cshtml";

                if (model == null)
                {
                    archivoVistaParcial = "~/Views/Congresses/Create.cshtml";
                    model = new Congress();
                }
            }

            CargarCombosComunes();

            await TryUpdateModelAsync<Congress>(model, "");

            ModelState.Clear();
            return PartialView(archivoVistaParcial, model);
        }

        [HttpPost]
        public async Task<string> Guardar()
        {
            //this
            Congress model;
            string numero = Request.Form["Id"];
            Guid elId = new Guid(numero);
            bool registroNuevo = false;
            string sort = "";
            
            model = db.Congresses.Where(f => f.Id == elId)
                                    .Include(f => f.ContratosObra)
                                    .Include(f => f.DocumentosObra)
                                    .FirstOrDefault();
            
            CargarFiltro(sort);

            if (model == null)
            {
                model = new Congress();
                registroNuevo = true;
            }

            await TryUpdateModelAsync<Congress>(model, "");

            if (!ModelState.IsValid)
            {
                string errorEnCampos = "ERROR:\n";
                foreach(var item in ModelState.ToArray())
                {
                    if (item.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                    {
                        errorEnCampos += item.Value.Errors[0].ErrorMessage.ToString() + "\n";
                    }
                }

                if (registroNuevo)
                    return errorEnCampos;
                else
                {
                    if (errorEnCampos.Length > 7){
                        TempData["ErrorMessage"] = errorEnCampos;
                    }
                    return "/Congresses/Edit/" + elId;
                }
            }

            if (registroNuevo)
                db.Add(model);
            else
                db.Update(model);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return "/Congresses";// RedirectToAction("Index");
        }

        public PartialViewResult AddContrato([FromQuery] Guid CodigoCongresses)
        {
            return PartialView("~/Views/Shared/EditorTemplates/ContratoObra.cshtml", new ContratoObra() { CodigoContrato = "AñadiendoNuevoContrato", CongressID = CodigoCongresses, DocumentosContratoObra = new List<DocumentoContratoObra>(), ImporteContrato = 0 });
        }

        public PartialViewResult EditContrato([FromQuery] Guid idContrato)
        {
            ContratoObra contrato = db.ContratoObras
                .Include(f=> f.DocumentosContratoObra)
                .Where(f=> f.Id==idContrato).FirstOrDefault();

            return PartialView("~/Views/Shared/EditorTemplates/ContratoObra.cshtml", contrato);
        }

        [HttpPost]
        public async Task<JsonResult> SaveContrato()
        {
            ContratoObra contratoObra = new ContratoObra();
            
            //this.Request.Form

            await TryUpdateModelAsync<ContratoObra>(contratoObra, "contratoObra",
                f => f.Congress,
                f => f.CongressID,
                f => f.CodigoContrato,
                f => f.FechaContratoFin,
                f => f.ImporteContrato,
                f => f.FechaContratoInicio,
                f => f.Id);

            if (ModelState.IsValid)
            {
                Congress model = new Congress(); 

                await TryUpdateModelAsync<Congress>(model, "");
                
                if (model.ContratosObra == null)
                    model.ContratosObra = new List<ContratoObra>();

                if (model.ContratosObra.Count > 0 && model.ContratosObra.Where(f => f.Id == contratoObra.Id).Count() > 0)
                {
                    var list = model.ContratosObra.ToList();
                    list[list.FindIndex(f => f.Id == contratoObra.Id)] = contratoObra;
                    model.ContratosObra = list;
                    db.Update(contratoObra);
                }
                else
                {
                    model.ContratosObra.Add(contratoObra);
                    db.Add(contratoObra);
                }


                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                ViewData["totalContratos"] = model.TotalContratos().ToString("C");
                CargarCombosComunes();

                var responseValid = new ResponseObject()
                {
                    Status = 1,
                    Data = await this.RenderPartialViewToString("_EditCongress", model)
                };

                return Json(responseValid);
            }

            var responseInvalid = new ResponseObject()
            {
                Status = 0,
                Data = await this.RenderPartialViewToString("~/Shared/EditorTemplates/ContratoObra", contratoObra)
            };

            return Json(responseInvalid);
        }



        [HttpPost]
        public async Task<IActionResult> EnviarFichero(Guid IdcontratoObra, IFormFile fileDocumentacion)
        {
            IFileUploader _fileUploader = (IFileUploader)HttpContext.RequestServices.GetService(typeof(IFileUploader));

            DocumentoContratoObra model = new DocumentoContratoObra() { ContratoObraId = IdcontratoObra };

            model.FicheroNombre = fileDocumentacion.FileName;
            model.FicheroUrl = await _fileUploader.UploadFile(fileDocumentacion);

            db.Add(model);
            db.SaveChanges();

            TempData["Message"] = "El fichero se ha añadido correctamente.";

            dynamic data = new { Path = await _fileUploader.UploadFile(fileDocumentacion), FileName = fileDocumentacion.FileName };

            return Ok(JsonConvert.SerializeObject(data));
        }

        [HttpPost]
        public async Task<IActionResult> EnviarFicheroObra(Guid IdCongressObra, IFormFile fileDocumentacionObra)
        {
            IFileUploader _fileUploader = (IFileUploader)HttpContext.RequestServices.GetService(typeof(IFileUploader));

            DocumentoObra model = new DocumentoObra() {  CongressId = IdCongressObra };

            model.FicheroNombre = fileDocumentacionObra.FileName;
            model.FicheroUrl = await _fileUploader.UploadFile(fileDocumentacionObra);

            db.Add(model);
            db.SaveChanges();

            TempData["Message"] = "El fichero se ha añadido correctamente.";

            dynamic data = new { Path = await _fileUploader.UploadFile(fileDocumentacionObra), FileName = fileDocumentacionObra.FileName };

            return Ok(JsonConvert.SerializeObject(data));
        }

        public IActionResult BorrarDocumento(int Id, Guid IdContrato)
        {
            db.DocumentoContratoObras.Remove(db.DocumentoContratoObras.Find(Id));
            db.SaveChanges();
            TempData["Message"] = "El fichero se ha borrado correctamente.";
            return RedirectToAction("Edit", new { Id = IdContrato });
        }

        public IActionResult BorrarDocumentoObra(int Id, Guid IdCongressObra)
        {
            db.DocumentoObras.Remove(db.DocumentoObras.Find(Id));
            db.SaveChanges();
            TempData["Message"] = "El fichero se ha borrado correctamente.";
            return RedirectToAction("Edit", new { Id = IdCongressObra });
        }

        public async Task<IActionResult> DeleteContrato(Guid ContratoObraId, Guid IdCongressObra)
        {
            ContratoObra contratoObra = db.ContratoObras.Where(f => f.Id == ContratoObraId).FirstOrDefault();
            if (contratoObra != null)
            {
                try
                {
                    ICollection<DocumentoContratoObra> documentosContratoObrasAborrar = db.DocumentoContratoObras.Where(f => f.ContratoObraId == ContratoObraId).ToList();
                    foreach(DocumentoContratoObra documentoContratoObra in documentosContratoObrasAborrar)
                    {
                        db.DocumentoContratoObras.Remove(documentoContratoObra);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                }
                
                TempData["Message"] = "Se han borrado correctamente los documentos.";
            }

            Congress model = await db.Congresses
                                    .Include(f => f.ContratosObra)
                                    .Where(f => f.Id == contratoObra.CongressID).FirstOrDefaultAsync();
                                
            db.ContratoObras.Remove(contratoObra);
            db.SaveChanges();

            CargarCombosComunes();

            return RedirectToAction("Edit", new { Id = IdCongressObra });

        }

        private void CargarCombosComunes(string? codPostalObra = "none")
        {
            ViewData["Almacen"] = _sage.Almacen.OrderBy(a => a.Codigo).AsNoTracking();
            ViewBag.Clientes = _sage.Clientes.OrderBy(f => f.Nombre).AsNoTracking();
            //ViewBag.CodPost = _sage.codpos.GroupBy(y => new { y.CODIGO, DISPLAY = y.CODIGO + ' ' + y.POBLACION }).Select(Z => Z.Key).ToList().OrderBy(f => f.CODIGO);            
            ViewBag.CodPost = _sage.codpos.Select(y => new { y.CODIGO, DISPLAY = y.CODIGO.Trim() }).Where(f=>f.CODIGO.Contains(codPostalObra)).AsNoTracking().ToList().OrderBy(f => f.CODIGO);
            ViewBag.SociedadesCientificas = db.SociedadesCientificas.OrderBy(f => f.Nombre);
        }  

        private void CargarFiltro(string sort)
        {
            CongressesFilter filter = new CongressesFilter();
            string sSesion = HttpContext.Session.GetString(Consts.SESSION_REGISTRATION_LIST_STATE);
            var previous = Request.HttpContext.GetServerVariable("HTTP_REFERER");
            CongressesSession objSesion;

            if (sSesion != null)
            {
                objSesion = JsonConvert.DeserializeObject<CongressesSession>(sSesion);
                sort = objSesion.sort;
                filter = objSesion.filter;
            }

            ViewData["Sort"] = sort;
            ViewData["Filter"] = filter;
        }

        public JsonResult Get(string q, int count)
        {            
            var results = db.Congresses
                .Where(f => f.Number.ToString().Contains(q) || f.Name.Contains(q))
                .OrderBy(f => f.Number).ThenBy(f => f.Name)
                .Take(count);
            return Json(results);
        }


        [HttpPost]
        public async Task<string> FiltrarCodigosPostales()
        
        {
            string resultado = "";
            try
            {
                vistaQuery objetos = JsonConvert.DeserializeObject<vistaQuery>(this.Request.Form.Keys.FirstOrDefault().ToString());

                List<vistaCodpos> model = await _sage.codpos
                    .Where(f => f.PROVINCIA.ToLower().Contains(objetos.q.ToLower()) || f.POBLACION.ToLower().Contains(objetos.q.ToLower()) || f.CODIGO.Contains(objetos.q.ToString())).OrderBy(f=>f.CODIGO)
                    .Select(f=> new vistaCodpos { id = f.CODIGO.Trim(), text = f.CODIGO.Trim() + " " + f.POBLACION.Trim() + "->" +f.PROVINCIA.Trim() })
                    .ToListAsync();

                model.Add(new vistaCodpos { id = objetos.q.Substring(0, objetos.q.IndexOf(' ')>0? objetos.q.IndexOf(' '):objetos.q.Length), text = objetos.q });


                resultado = System.Text.Json.JsonSerializer.Serialize(model);

            } catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
                
        
            //var resuetlo = Json(model);
            //resuetlo.ContentType = "application/json; chasert=utf-8";

            return resultado;
            //return resuetlo;
        }

        private partial class vistaQuery
        {
            public string q { get; set; }
            public string page_limit { get; set; }
        }

        private partial class vistaCodpos
        {
            public string id { get; set; }
            public string text{ get; set; }
        }

    }
}

using Ferpuser.BLL.BusinessValidators;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Core;
using Ferpuser.Models.Sage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using CsvHelper;
using System.Globalization;
using Ferpuser.Models.Dtos;

namespace Ferpuser.Controllers
{
    [Authorize]
    public class ProveedorController : Controller
    {
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComu;
        private readonly ApplicationDbContext db;

        public ProveedorController(SageContext sageContext, ApplicationDbContext dbContext, SageComuContext sageComu)
        {
            _sageContext = sageContext;
            _sageComu = sageComu;
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

            ProveedorFilter filter = new ProveedorFilter();
            await TryUpdateModelAsync<ProveedorFilter>(filter, "filter");

            Pager pager = new Pager(await db.Proveedores.CountAsync(filter.ExpressionFilter()), pag, 10, 5);
            ViewData["Pager"] = pager;
            ViewData["Sort"] = sort;

            IEnumerable<Proveedor> list = null;
            try
            {
                list = await db.Proveedores.Where(filter.ExpressionFilter())
                    .OrderBy(sort)
                    .Skip((pager.Page - 1) * pager.PageSize)
                    .Take(pager.PageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ViewBag.Eventos = db.Congresses.OrderBy(f => f.Number);

            return View(list);
        }

        public async Task<IActionResult> Import()
        {

            var listaLocalProveedores = await db.Proveedores.Select(pr => new { CODIGO = pr.CUENTACONTABLE, pr.Deleted }).Where(pr => pr.Deleted == null).ToListAsync();

            string proveedoresLocal = "";

            foreach (var proveedorLocal in listaLocalProveedores)
            {
                proveedoresLocal += string.Format("{0},", proveedorLocal.CODIGO);
            }

            IEnumerable<ProveedorSage> proveedoresSage = await _sageContext.Proveedores
                .Where(prsa => !proveedoresLocal.Contains(prsa.CODIGO.Trim())
                        && (prsa.CIF != null && prsa.CIF != "")
                        && (prsa.CODIGO.Trim().StartsWith("4000") || prsa.CODIGO.Trim().StartsWith("4001")))
                .ToListAsync();

            int cantidad = 0;

            IEnumerable<ProveedorSage> masDeUnCIF = null;

            string unCIF = "";

            foreach (var sageProveedor in proveedoresSage)//_sageContext.Proveedores)
            {
                try
                {
                    unCIF = sageProveedor.CIF.Trim().ToUpper();

                    masDeUnCIF = await _sageContext.Proveedores.Where(prsa => prsa.CIF.Contains(unCIF))
                                                                .OrderBy(pr => pr.CODIGO)
                                                                .ToListAsync();

                    ContlfPro telef_Pro = await _sageContext.ContlfPro.Where(tel => tel.Proveedor.Trim().Contains(masDeUnCIF.First().CODIGO.Trim()) && tel.Predet == true).FirstOrDefaultAsync();

                    string telPpro = "";
                    string contacto = "";

                    if (telef_Pro != null)
                    {
                        if (telef_Pro.Telefono != null)
                            telPpro = telef_Pro.Telefono;

                        if (telef_Pro.Persona != null)
                            contacto = telef_Pro.Persona;
                    }
                    var Proveedor = db.Proveedores.FirstOrDefault(c_ => c_.CUENTACONTABLE.Equals(masDeUnCIF.First().CODIGO));

                    if (Proveedor == null)
                    {
                        await AddSageProveedor(db, masDeUnCIF.First(), telPpro, contacto);
                        cantidad++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            //await db.SaveChangesAsync();

            if (cantidad > 0)
                TempData["Message"] = string.Format("Se han importado {0} correctamente.", cantidad.ToString());

            return RedirectToAction("Index");
        }

        public ActionResult CrearProveedorFactura(string? origen = "")
        {
            return RedirectToAction("Create", new { origen = origen });
        }

        public IActionResult Create(string? origen = "")
        {
            ViewBag.origen = string.IsNullOrEmpty(origen) ? "proveedor" : origen;

            var ultimoProveedorSage = _sageContext.Proveedores.Where(prv => prv.CODIGO.StartsWith("4000")).OrderBy(prv => prv.CODIGO).LastOrDefault();

            var ultimoProveedor = db.Proveedores.Where(prv => prv.CUENTACONTABLE.StartsWith("4000")).OrderBy(prv => prv.CUENTACONTABLE).LastOrDefault();

            long ultimoCodigo = 0;
            long ultimoCodigoSage = 0;

            if (ultimoProveedorSage != null)
                ultimoCodigoSage = long.Parse(ultimoProveedorSage.CODIGO.ToString());

            if (ultimoProveedor != null)
                ultimoCodigo = long.Parse(ultimoProveedor.CUENTACONTABLE.ToString());

            var cadena = "";

            if (ultimoCodigoSage > ultimoCodigo)
                ultimoCodigo = ultimoCodigoSage;

            if (ultimoCodigo > 0)
                cadena = (ultimoCodigo + 1).ToString();
            else
                cadena = "400000000";

            var modelo = new Proveedor { CUENTACONTABLE = cadena };

            CargaCombos();

            return View(modelo);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed(string? origen = "")
        {

            Proveedor model = new Proveedor();
            await TryUpdateModelAsync<Proveedor>(model, "");

            VerificarCamposProveedor(model);

            //Validación de negocio            
            ProveedorValidator validator = new ProveedorValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Create(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                ProveedorManager manager = new ProveedorManager(db);
                await manager.Create(model);
                TempData["Message"] = "El registro se ha creado correctamente.";

                var resultado = await AddProveedorSage(model);
                if (resultado != null)
                {
                    AddTelProveedor(model);

                    _sageContext.SaveChangesAsync();

                    TempData["Message"] = "El registro se ha actualizado en SAGE correctamente.";
                }
                else
                    TempData["ErrorMessage"] = "Se ha producido un ERROR y el registro NO ha actualizado en SAGE.";

                return await Volver(origen);
            }

            string elCodigoPostal = model.CODPOST;

            if (elCodigoPostal == null || elCodigoPostal == "")
                elCodigoPostal = "none";

            ViewBag.origen = origen;

            CargaCombos(elCodigoPostal);

            return View(model);
        }

        public async Task<IActionResult> Volver(string origen)
        {
            string[] tipoOrigen = origen != "" ? origen.Split('-') : new[] { "proveedor" };

            if (tipoOrigen[0] == "proveedor")
                return RedirectToAction("Index");

            if (tipoOrigen[1] == "Factura")
            {
                CompraFactura facturaModel = null;
                if (tipoOrigen[0] == "0")
                {
                    facturaModel = new CompraFactura();
                    return RedirectToAction("Create", "ComprasFactura", facturaModel);
                }
                else
                {
                    facturaModel = db.CompraFacturas.Where(f => f.Id == Int32.Parse(tipoOrigen[0])).FirstOrDefault();
                    return RedirectToAction("Edit", "ComprasFactura", facturaModel);
                }
            } else
            {
                CompraPedido pedidoModel = null;
                if (tipoOrigen[0] == "0")
                {
                    pedidoModel = new CompraPedido();
                    return RedirectToAction("Create", "ComprasPedido", pedidoModel);
                }
                else
                {
                    pedidoModel = db.CompraPedidos.Where(f => f.Id == Int32.Parse(tipoOrigen[0])).FirstOrDefault();
                    return RedirectToAction("Edit", "ComprasPedido", pedidoModel);
                }
            }
        }

        public IActionResult Edit(Guid id)
        {
            Proveedor model = db.Proveedores.Single(f => f.Id == id);

            if (model == null)
                return NotFound();

            string elCodigoPostal = model.CODPOST;

            if (elCodigoPostal == null || elCodigoPostal == "")
                elCodigoPostal = "none";

            CargaCombos(elCodigoPostal);

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(Guid id)
        {
            Proveedor model = db.Proveedores.Find(id);

            await TryUpdateModelAsync<Proveedor>(model, "");

            VerificarCamposProveedor(model);

            ProveedorValidator validator = new ProveedorValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Edit(model);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                //Borrar Campos en caso de que no lleve retención
                if (!model.TIPO_RET)
                {
                    model.RETENCION = "";
                    model.MODO_RET = false;
                }

                ProveedorManager manager = new ProveedorManager(db);
                await manager.Edit(model);

                TempData["Message"] = "El registro se ha actualizado correctamente.";

                var resultado = AddProveedorSage(model);
                if (resultado != null)
                {
                    TempData["Message"] = "El registro se ha actualizado en SAGE correctamente.";
                    AddTelProveedor(model);

                }
                else
                    TempData["ErrorMessage"] = "Se ha producido un ERROR y el registro NO ha actualizado en SAGE.";

                return RedirectToAction("Index");
            }

            string elCodigoPostal = model.CODPOST;

            if (elCodigoPostal == null || elCodigoPostal == "")
                elCodigoPostal = "none";

            CargaCombos(elCodigoPostal);

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            ProveedorValidator validator = new ProveedorValidator(db);
            IEnumerable<ValidationResult> businessErrors = validator.Delete(id);
            this.AddValidationErrors(businessErrors);

            if (ModelState.IsValid)
            {
                ProveedorManager manager = new ProveedorManager(db);
                await manager.Delete(id);
                TempData["Message"] = "El registro se ha eliminado correctamente.";
            }

            return RedirectToAction("Index");
        }

        public async Task<string> ProveedorNIFExistsCreate(string NIF)
        {
            Proveedor model = new Proveedor();
            await TryUpdateModelAsync<Proveedor>(model, "");

            if (NIF.Length < 4)
                return "false";

            var proveedorLocal = await db.Proveedores.FirstOrDefaultAsync(c => c.NIF.Trim().ToLower().Equals(NIF.ToLower().Trim()));

            if (proveedorLocal != null)
            {
                //TempData["ErrorMessage"] = "El NIF ya existe.";
                return "borrar";
            }

            ProveedorSage proveedorSage = await _sageContext.Proveedores.FirstOrDefaultAsync(c => c.CIF.Trim().ToLower().Equals(NIF.ToLower().Trim()));


            if (proveedorSage == null)
                return "false";

            //telf_pro telf_Pro = await _sageContext.telf_pro.Where(tel=>tel.PROVEEDOR.Equals(proveedorSage.CODIGO) && !tel.TEXTO.Contains("FAX")).FirstOrDefaultAsync();
            ContlfPro elContacto = await _sageContext.ContlfPro.Where(cto => cto.Proveedor.Equals(proveedorSage.CODIGO) && cto.Predet == true).FirstOrDefaultAsync();

            string telPpro = "";
            string contacto = "";

            if (elContacto != null)
            {
                telPpro = elContacto.Telefono;
                contacto = elContacto.Persona;
            }

            var _proveedor = await UpdateSageProveedor(db, proveedorSage, telPpro, contacto);

            return System.Text.Json.JsonSerializer.Serialize(_proveedor);
        }

        private bool ClientExists(Guid id)
        {
            return db.Clients.Any(e => e.Id == id);
        }

        public async Task<FileResult> ExportCsv(string currentsort = "")
        {
            ProveedorFilter filter = new ProveedorFilter();
            await TryUpdateModelAsync<ProveedorFilter>(filter, "filter",
                f => f.NIF,
                f => f.Domicilio,
                f => f.NombreComercial);

            IEnumerable<Proveedor> list = await db.Proveedores
                .Where(filter.ExpressionFilter())
                .OrderBy(currentsort)
                .ToListAsync();

            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_ListadoProveedores.csv";

            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Configuration.CultureInfo = new CultureInfo("es-ES");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<ProveedorViewModelMap>();
                await csv.WriteRecordsAsync<Proveedor>(list);
            }
            return File(memoryStream.ToArray(), "text/csv", fileName);

        }

        [HttpPost]
        public async Task<string> FiltrarCodigosPostales()

        {
            string resultado = "";
            try
            {
                Select2Request objetos = JsonConvert.DeserializeObject<Select2Request>(this.Request.Form.Keys.FirstOrDefault().ToString());

                List<Select2Response> model = await _sageContext.codpos
                    .Where(f => f.PROVINCIA.ToLower().Contains(objetos.q.ToLower()) || f.POBLACION.ToLower().Contains(objetos.q.ToLower()) || f.CODIGO.Contains(objetos.q.ToString())).OrderBy(f => f.CODIGO)
                    .Select(f => new Select2Response { id = f.CODIGO.Trim(), text = f.CODIGO.Trim() + " " + f.POBLACION.Trim() + "->" + f.PROVINCIA.Trim() })
                    .ToListAsync();

                model.Add(new Select2Response { id = objetos.q.Substring(0, objetos.q.IndexOf(' ') > 0 ? objetos.q.IndexOf(' ') : objetos.q.Length), text = objetos.q });


                resultado = System.Text.Json.JsonSerializer.Serialize(model);

            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }


            //var resuetlo = Json(model);
            //resuetlo.ContentType = "application/json; chasert=utf-8";

            return resultado;
            //return resuetlo;
        }

        private async Task<Proveedor> UpdateSageProveedor(ApplicationDbContext context, ProveedorSage sageProveedor, string telPro, string contacto)
        {
            var Proveedor = await context.Proveedores.FirstOrDefaultAsync(c_ => c_.CUENTACONTABLE.Equals(sageProveedor.CODIGO));

            if (Proveedor == null)
            {
                Proveedor = new Proveedor
                {
                    CUENTACONTABLE = sageProveedor.CODIGO.Trim() ?? "",
                    NIF = sageProveedor.CIF.Trim() ?? "",
                    RAZONSOCIAL = sageProveedor.NOMBRE2.Trim() ?? "",
                    NOMBRECOMERCIAL = sageProveedor.NOMBRE.Trim() ?? "",
                    DIRECCION = sageProveedor.DIRECCION.Trim() ?? "",
                    CODPOST = sageProveedor.CODPOST.Trim() ?? "",
                    LOCALIDAD = sageProveedor.POBLACION.Trim() ?? "",
                    PROVINCIA = sageProveedor.PROVINCIA.Trim() ?? "",
                    PAIS = sageProveedor.PAIS.Trim() ?? "",
                    PERSONACONTACTO = contacto ?? "",
                    CARGO = "",
                    EMAIL = sageProveedor.EMAIL.Trim() ?? "",
                    TELEFONO = telPro ?? "",
                    TELEFONO2 = "",
                    PAGINAWEB = sageProveedor.HTTP.Trim() ?? "",
                    FORMAPAGO = sageProveedor.FPAG.Trim() ?? "",
                    RETENCION = sageProveedor.TIPO_RET ?? "",// sageProveedor.RETENCION,
                    MODO_RET = sageProveedor.MODO_RET,
                    TIPO_RET = sageProveedor.RETENCION,
                    COMISIONES = "",
                    OBSERVACIONES = ""
                };

                //[Display(Name = "TIPO RETENCIÓN")]
                //public string? RETENCION { get; set; }
                //[Display(Name = "TIENE RETENCIÓN")]
                //public bool? TIPO_RET { get; set; }
                //[Display(Name = "MODO RETENCIÓN")]
                //public bool MODO_RET { get; set; }
                //context.Add(Proveedor);
            }
            else
            {
                Proveedor.CUENTACONTABLE = sageProveedor.CODIGO.Trim();
                Proveedor.NIF = sageProveedor.CIF.Trim();
                Proveedor.RAZONSOCIAL = sageProveedor.NOMBRE2 != null ? sageProveedor.NOMBRE2.Trim() : "";
                Proveedor.NOMBRECOMERCIAL = sageProveedor.NOMBRE != null ? sageProveedor.NOMBRE.Trim() : "";
                Proveedor.DIRECCION = sageProveedor.DIRECCION != null ? sageProveedor.DIRECCION.Trim() : "";
                Proveedor.CODPOST = sageProveedor.CODPOST != null ? sageProveedor.CODPOST.Trim() : "";
                Proveedor.LOCALIDAD = sageProveedor.POBLACION != null ? sageProveedor.POBLACION.Trim() : "";
                Proveedor.PROVINCIA = sageProveedor.PROVINCIA != null ? sageProveedor.PROVINCIA.Trim() : "";
                Proveedor.PAIS = sageProveedor.PAIS.Trim();
                Proveedor.PERSONACONTACTO = contacto ?? "";
                Proveedor.CARGO = "";
                Proveedor.EMAIL = sageProveedor.EMAIL != null ? sageProveedor.EMAIL.Trim() : "";
                Proveedor.TELEFONO = telPro ?? "";
                Proveedor.TELEFONO2 = "";
                Proveedor.PAGINAWEB = sageProveedor.HTTP != null ? sageProveedor.HTTP.Trim() : "";
                Proveedor.FORMAPAGO = sageProveedor.FPAG != null ? sageProveedor.FPAG.Trim() : "";
                Proveedor.RETENCION = sageProveedor.TIPO_RET != null ? sageProveedor.TIPO_RET.Trim() : "";
                Proveedor.MODO_RET = sageProveedor.MODO_RET;
                Proveedor.TIPO_RET = sageProveedor.RETENCION;
                Proveedor.COMISIONES = "";
                Proveedor.OBSERVACIONES = "";

                //context.Update(Proveedor);
            }
            //await context.SaveChangesAsync();

            return Proveedor;
        }
        private async Task<Proveedor> AddSageProveedor(ApplicationDbContext context, ProveedorSage sageProveedor, string telPro, string contacto)
        {
            var Proveedor = context.Proveedores.FirstOrDefault(c_ => c_.CUENTACONTABLE.Equals(sageProveedor.CODIGO));

            if (Proveedor == null)
            {
                Proveedor = new Proveedor
                {
                    CUENTACONTABLE = sageProveedor.CODIGO.Trim() ?? "",
                    NIF = sageProveedor.CIF.Trim() ?? "",
                    RAZONSOCIAL = sageProveedor.NOMBRE2.Trim() ?? "",
                    NOMBRECOMERCIAL = sageProveedor.NOMBRE.Trim() ?? "",
                    DIRECCION = sageProveedor.DIRECCION.Trim() ?? "",
                    CODPOST = sageProveedor.CODPOST.Trim() ?? "",
                    LOCALIDAD = sageProveedor.POBLACION.Trim() ?? "",
                    PROVINCIA = sageProveedor.PROVINCIA.Trim() ?? "",
                    PAIS = sageProveedor.PAIS.Trim() ?? "",
                    PERSONACONTACTO = contacto ?? "",
                    CARGO = "",
                    EMAIL = sageProveedor.EMAIL.Trim() ?? "",
                    TELEFONO = telPro,
                    TELEFONO2 = "",
                    PAGINAWEB = sageProveedor.HTTP.Trim() ?? "",
                    FORMAPAGO = sageProveedor.FPAG.Trim() ?? "",
                    RETENCION = sageProveedor.TIPO_RET.Trim() ?? "",
                    COMISIONES = "",
                    TIPO_RET = sageProveedor.RETENCION,
                    MODO_RET = sageProveedor.MODO_RET,
                    OBSERVACIONES = ""
                };
                context.Add(Proveedor);
            }
            else
            {
                Proveedor.CUENTACONTABLE = sageProveedor.CODIGO.Trim() ?? "";
                Proveedor.NIF = sageProveedor.CIF.Trim() ?? "";
                Proveedor.RAZONSOCIAL = sageProveedor.NOMBRE2.Trim() ?? "";
                Proveedor.NOMBRECOMERCIAL = sageProveedor.NOMBRE.Trim() ?? "";
                Proveedor.DIRECCION = sageProveedor.DIRECCION.Trim() ?? "";
                Proveedor.CODPOST = sageProveedor.CODPOST.Trim() ?? "";
                Proveedor.LOCALIDAD = sageProveedor.POBLACION.Trim() ?? "";
                Proveedor.PROVINCIA = sageProveedor.PROVINCIA.Trim() ?? "";
                Proveedor.PAIS = sageProveedor.PAIS.Trim() ?? "";
                Proveedor.PERSONACONTACTO = contacto ?? "";
                Proveedor.CARGO = "";
                Proveedor.EMAIL = sageProveedor.EMAIL.Trim() ?? "";
                Proveedor.TELEFONO = telPro;
                Proveedor.TELEFONO2 = "";
                Proveedor.PAGINAWEB = sageProveedor.HTTP.Trim() ?? "";
                Proveedor.FORMAPAGO = sageProveedor.FPAG.Trim() ?? "";
                Proveedor.RETENCION = sageProveedor.TIPO_RET.Trim() ?? "";
                Proveedor.MODO_RET = sageProveedor.MODO_RET;
                Proveedor.TIPO_RET = sageProveedor.RETENCION;
                Proveedor.COMISIONES = "";
                Proveedor.OBSERVACIONES = "";

                context.Update(Proveedor);
            }
            context.SaveChanges();

            return Proveedor;
        }
        private async Task<ProveedorSage> AddProveedorSage(Proveedor _proveedor)
        {
            bool crearCuentaProveedor = false;

            ProveedorSage ProveedorSage = _sageContext.Proveedores
                .FirstOrDefault(
                c_ => c_.CODIGO.Trim().Equals(_proveedor.CUENTACONTABLE.Trim())
                && c_.CIF.Trim().ToUpper().Equals(_proveedor.NIF.Trim().ToUpper()));

            //ret_emp retencion = await context.Ret_Emps.FirstOrDefaultAsync(ret => ret.CUENTA.Equals(_proveedor.CUENTACONTABLE));

            try
            {
                //ProveedorSageManager manager = new ProveedorSageManager(context);

                if (ProveedorSage == null)
                {
                    ProveedorSage = new ProveedorSage
                    {
                        CODIGO = _proveedor.CUENTACONTABLE != null ? _proveedor.CUENTACONTABLE.Trim() : "",
                        CIF = _proveedor.NIF != null ? _proveedor.NIF.Trim() : "",
                        NOMBRE2 = _proveedor.RAZONSOCIAL != null ? _proveedor.RAZONSOCIAL.Trim() : "",
                        NOMBRE = _proveedor.NOMBRECOMERCIAL != null ? _proveedor.NOMBRECOMERCIAL.Trim() : "",
                        DIRECCION = _proveedor.DIRECCION != null ? _proveedor.DIRECCION.Trim() : "",
                        CODPOST = _proveedor.CODPOST != null ? _proveedor.CODPOST.Trim() : "",
                        POBLACION = _proveedor.LOCALIDAD != null ? _proveedor.LOCALIDAD.Trim() : "",
                        PROVINCIA = _proveedor.PROVINCIA != null ? _proveedor.PROVINCIA.Trim() : "",
                        PAIS = _proveedor.PAIS != null ? _proveedor.PAIS : "",
                        NOMBRE3ERP = _proveedor.PERSONACONTACTO != null ? _proveedor.PERSONACONTACTO.Trim() : "",
                        EMAIL = _proveedor.EMAIL != null ? _proveedor.EMAIL.Trim() : "",
                        HTTP = _proveedor.PAGINAWEB != null ? _proveedor.PAGINAWEB.Trim() : "",
                        FPAG = _proveedor.FORMAPAGO != null ? _proveedor.FORMAPAGO.Trim() : "",
                        RETENCION = _proveedor.TIPO_RET,
                        MODO_RET = _proveedor.MODO_RET,
                        TIPO_RET = _proveedor.RETENCION.Trim() ?? "",
                        REFER_CAT = ""
                    };
                    try
                    {
                        //await manager.Create(ProveedorSage);
                        
                        crearCuentaProveedor = _sageContext.Add(ProveedorSage) !=null ? true : false ;
                        //await context.SaveChangesAsync();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    ProveedorSage.CODIGO = _proveedor.CUENTACONTABLE != null ? _proveedor.CUENTACONTABLE.Trim() : "";
                    ProveedorSage.CIF = _proveedor.NIF != null ? _proveedor.NIF.Trim() : "";
                    ProveedorSage.NOMBRE2 = _proveedor.RAZONSOCIAL != null ? _proveedor.RAZONSOCIAL.Trim() : "";
                    ProveedorSage.NOMBRE = _proveedor.NOMBRECOMERCIAL != null ? _proveedor.NOMBRECOMERCIAL.Trim() : "";
                    ProveedorSage.DIRECCION = _proveedor.DIRECCION != null ? _proveedor.DIRECCION.Trim() : "";
                    ProveedorSage.CODPOST = _proveedor.CODPOST != null ? _proveedor.CODPOST.Trim() : "";
                    ProveedorSage.POBLACION = _proveedor.LOCALIDAD != null ? _proveedor.LOCALIDAD.Trim() : "";
                    ProveedorSage.PROVINCIA = _proveedor.PROVINCIA != null ? _proveedor.PROVINCIA.Trim() : "";
                    ProveedorSage.PAIS = _proveedor.PAIS != null ? _proveedor.PAIS : "";
                    ProveedorSage.NOMBRE3ERP = _proveedor.PERSONACONTACTO != null ? _proveedor.PERSONACONTACTO.Trim() : "";
                    ProveedorSage.EMAIL = _proveedor.EMAIL != null ? _proveedor.EMAIL.Trim() : "";
                    ProveedorSage.HTTP = _proveedor.PAGINAWEB != null ? _proveedor.PAGINAWEB.Trim() : "";
                    ProveedorSage.FPAG = _proveedor.FORMAPAGO != null ? _proveedor.FORMAPAGO.Trim() : "0";
                    ProveedorSage.MODO_RET = _proveedor.MODO_RET;
                    ProveedorSage.RETENCION = _proveedor.TIPO_RET;
                    ProveedorSage.TIPO_RET = _proveedor.RETENCION != null ? _proveedor.RETENCION.Trim() : "";
                    ProveedorSage.MODIFIED = DateTime.Now;
                    try
                    {
                        _sageContext.Update(ProveedorSage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                _sageContext.SaveChanges();
                
                if (crearCuentaProveedor)
                {
                    try
                    {
                        var resultado = AddCuentaProveedorASage(ProveedorSage);
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "Se ha producido un ERROR ceando la CUENTA del proveedor en SAGE.";
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return ProveedorSage;
        }
        private async Task AddTelProveedor(Proveedor Proveedor)
        {
            if (Proveedor.TELEFONO != null && !Proveedor.Equals(""))
            {
                ContlfPro telefono = null;
                try
                {
                    telefono = _sageContext.ContlfPro.FirstOrDefault(tel => tel.Proveedor.Trim().Equals(Proveedor.CUENTACONTABLE.Trim()) && tel.Predet == true);
                    if (telefono == null)
                    {
                        telefono = new ContlfPro
                        {
                            Proveedor = Proveedor.CUENTACONTABLE.Trim(),
                            Guid = Proveedor.CUENTACONTABLE.Trim(),
                            Telefono = Proveedor.TELEFONO.Trim(),
                            Predet = true,
                            Persona = Proveedor.PERSONACONTACTO.Trim(),
                            Tipo = 1,
                            GuidId = Guid.NewGuid().ToString()
                        };
                        _sageContext.Add(telefono);
                    }
                    else
                    {
                        telefono.Modified = DateTime.Now;
                        telefono.Telefono = Proveedor.TELEFONO.Trim();
                        telefono.Predet = true;
                        telefono.Persona = Proveedor.PERSONACONTACTO.Trim();
                        _sageContext.Update(telefono);
                    }
                    _sageContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async Task AddCuentaProveedorASage(ProveedorSage proveedorSage)
        {
            cuentas nuevaCuenta = new cuentas
            { 
                CODIGO = proveedorSage.CODIGO,
                NOMBRE = proveedorSage.NOMBRE.Trim(),
                CIF = proveedorSage.CIF.Trim(),
                VARIA = "",
                SECUNDARIA = "N",
                OBSERVACIO = "NUEVO",
                DIVISA = "000",
                VISTA = true,
                DESCRIP = "",
                FCREADO = DateTime.Now,
                BABELENV = false,
                CLIENTEERP = "",
                GUID_ID = Guid.NewGuid().ToString(),
                CREATED = DateTime.Now,
                MODIFIED = DateTime.Now,
                TIPOIVA = "",
                CLAVEIRPF = 0,
                RECARGO = false,
                CONCEPTO = "",
                CONGASING = "",
                CONIRPF = "",
                TELEFONO = "",
                RETENCION = false,
                TIPO_RET = "",
                PORCEN_RET = 0,
                ANTICIPREM = "",
                BANCO_PREV = "",
                CSB = true,
                DIAPAG = 0,
                DIAPAG2 = 0,
                FPAG = "",
                CODPOST = proveedorSage.CODPOST,
                DIRECCION = "",
                EMAIL = "",
                FAX = "",
                PAIS = proveedorSage.PAIS,
                POBLACION = proveedorSage.POBLACION,
                PROVINCIA = proveedorSage.PROVINCIA
            };
            try
            {
                _sageContext.Add(nuevaCuenta);
                _sageContext.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Se ha producido un ERROR ceando la CUENTA del proveedor en SAGE.";
            }
        }

        private void CargaCombos(string? codPostalObra = "none")
        {
            ViewBag.TipoRet = _sageContext.tipo_ret.OrderBy(f => f.NOMBRE);
            ViewBag.CodPost = _sageContext.codpos.Select(y => new { y.CODIGO, DISPLAY = y.CODIGO.Trim() +" " + y.CODIGO.Trim() +"->" + y.CODIGO.Trim() }).Where(f => f.CODIGO.Contains(codPostalObra)).AsNoTracking().ToList().OrderBy(f => f.CODIGO);
            ViewBag.FPago = _sageContext.FPag.OrderBy(f => f.Nombre);
            ViewBag.Paises = _sageComu.paises.OrderBy(f => f.NOMBRE);
            ViewBag.TiposComite = db.TiposComite.OrderBy(f => f.Id);
            ViewBag.PuestosComite = db.PuestosComite.OrderBy(f => f.Id);
            ViewBag.Eventos = db.Congresses.OrderBy(f => f.Number);
        }

        private void VerificarCamposProveedor(Proveedor model)
        {
            model.CARGO = model.CARGO == null ? "": model.CARGO;
            model.CODPOST = model.CODPOST == null ? "":model.CODPOST;
            model.COMISIONES = model.COMISIONES == null ? "" : model.COMISIONES;
            model.CUENTACONTABLE = model.CUENTACONTABLE == null ? "" : model.CUENTACONTABLE;
            model.DIRECCION = model.DIRECCION == null ? "" : model.DIRECCION;
            model.EMAIL = model.EMAIL == null ? "" : model.EMAIL;
            model.FORMAPAGO = model.FORMAPAGO == null ? "0" : model.FORMAPAGO;
            model.LOCALIDAD = model.LOCALIDAD == null ? "" : model.LOCALIDAD;
            //model.MODO_RET = model.MODO_RET == null ? false : model.MODO_RET;
            model.NOMBRECOMERCIAL = model.NOMBRECOMERCIAL == null ? "" : model.NOMBRECOMERCIAL;
            model.OBSERVACIONES = model.OBSERVACIONES == null ? "" : model.OBSERVACIONES;
            model.PAGINAWEB = model.PAGINAWEB == null ? "" : model.PROVINCIA;  
            model.PAIS = model.PAIS == null ? "" :model.PAIS;
            model.PERSONACONTACTO = model.PERSONACONTACTO == null ? "" : model.PERSONACONTACTO;
            model.PROVINCIA = model.PROVINCIA == null ? "" : model.PROVINCIA;
            model.RAZONSOCIAL = model.RAZONSOCIAL == null ?"" : model.RAZONSOCIAL;
            model.RETENCION = model.RETENCION == null ? "" : model.RETENCION;
            model.TELEFONO = model.TELEFONO == null ? "" : model.TELEFONO;
            model.TELEFONO2 = model.TELEFONO2 == null ? "" : model.TELEFONO2;
            //model.TIPO_RET = model.TIPO_RET == null ? false: model.TIPO_RET;
        }
    }
}


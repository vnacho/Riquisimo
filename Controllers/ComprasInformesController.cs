using CsvHelper;
using CsvHelper.Configuration;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Compras")]
    public class ComprasInformesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly SageContext sage_context;        

        public ComprasInformesController(
            ApplicationDbContext db,
            SageContext sage_context)
        {
            this.db = db;
            this.sage_context = sage_context;            
        }

        #region Informe Facturas

        public IActionResult Facturas()
        {
            ComprasInformesFacturas model = new ComprasInformesFacturas();            

            ViewBag.Proveedores = sage_context.Proveedores.OrderBy(f => f.CODIGO).ThenBy(f => f.NOMBRE).ThenBy(f => f.CIF);
            ViewBag.Vendedores = sage_context.Vendedor.OrderBy(f => f.Codigo).ThenBy(f => f.Nombre);
            ViewBag.Eventos = sage_context.Almacen.OrderBy(f => f.Codigo).ThenBy(f => f.Nombre);

            return View(model);
        }

        [HttpPost, ActionName("Facturas")]
        public async Task<IActionResult> FacturasConfirmed()
        {
            ComprasInformesFacturas model = new ComprasInformesFacturas();
            await TryUpdateModelAsync<CompraFacturaFilter>(model.Filter, "Filter",
                f => f.CodigoProveedor,
                f => f.CodigoOperario,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.CodigoEvento,
                f => f.Pagada,
                f => f.TieneRetencion);
            model.Items = db.CompraFacturas.Where(model.Filter.ExpressionFilter());

            ViewBag.Proveedores = sage_context.Proveedores.OrderBy(f => f.CODIGO).ThenBy(f => f.NOMBRE).ThenBy(f => f.CIF);
            ViewBag.Vendedores = sage_context.Vendedor.OrderBy(f => f.Codigo).ThenBy(f => f.Nombre);
            ViewBag.Eventos = sage_context.Almacen.OrderBy(f => f.Codigo).ThenBy(f => f.Nombre);

            return View(model);
        }

        public async Task<FileResult> FacturasCsv()
        {
            ComprasInformesFacturas model = new ComprasInformesFacturas();
            await TryUpdateModelAsync<CompraFacturaFilter>(model.Filter, "Filter",
                f => f.CodigoProveedor,
                f => f.CodigoOperario,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.CodigoEvento,
                f => f.Pagada,
                f => f.TieneRetencion);
            model.Items = db.CompraFacturas.Where(model.Filter.ExpressionFilter());
            return await GetCsvAsync<CompraFactura, CompraFacturaMap>(model.Items, "informe.csv");
        }

        #endregion

        private async Task<FileResult> GetCsvAsync<T, E>(IEnumerable<T> records, string fileName) where E : ClassMap<T>
        {
            if (records == null || !records.Any())
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

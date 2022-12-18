using ClosedXML.Excel;
using CsvHelper;
using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Services;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Collaborations")]
    public class InformeTotalHorasPersonalController : ControlPresupuestarioBaseController
    {
        private readonly ApplicationDbContext _dbContext;

        public InformeTotalHorasPersonalController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.CentrosCoste = await _dbContext.CentrosCoste.OrderBy(f => f.Nombre).ToListAsync();
            return View();
            //InformeTotalHorasPersonalViewModel model = new InformeTotalHorasPersonalViewModel()
            //{
            //    FechaDesde = DateTime.Now.Date.AddDays(-31),
            //    FechaHasta = DateTime.Now.Date
            //};
            //return View(model);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexConfirmed()
        {
            InformeTotalHorasPersonalViewModel model = new InformeTotalHorasPersonalViewModel();

            await TryUpdateModelAsync<InformeTotalHorasPersonalViewModel>(model, "");

            if (ModelState.IsValid)
            {
                IEnumerable<PartePersonal> list = await _dbContext.PartePersonal.Where(model.ExpressionFilter())
                .Include(f => f.Personal)
                .Include(f => f.CentroCoste)
                .OrderBy(f => f.Fecha)
                .ToListAsync();

                model.Items = list;
            }

            ViewBag.CentrosCoste = _dbContext.CentrosCoste.OrderBy(f => f.Nombre);
            return View(model);
        }

        public async Task<IActionResult> ExportCsv()
        {
            InformeTotalHorasPersonalViewModel model = new InformeTotalHorasPersonalViewModel();

            await TryUpdateModelAsync<InformeTotalHorasPersonalViewModel>(model, "");

            if (!ModelState.IsValid)
                return RedirectToAction("Index", model);
            
            IEnumerable<PartePersonal> list = await _dbContext.PartePersonal.Where(model.ExpressionFilter())
            .Include(f => f.Personal)
            .Include(f => f.CentroCoste)
            .OrderBy(f => f.Fecha)
            .ToListAsync();

            model.Items = list;            

            int fila, columna;
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_InformeHorasPersonal.csv";

            XLWorkbook wb = new XLWorkbook();
            IXLRange range;
            var worksheet = wb.Worksheets.Add("Informe");

            string filtros = string.Empty;
            if (model.CentroCosteId.HasValue)
                filtros += $"Centro de coste: {_dbContext.CentrosCoste.Find(model.CentroCosteId).Display}. ";
            if (model.FechaDesde.HasValue)
                filtros += $"Fecha desde: {model.FechaDesde?.ToShortDateString()}. ";
            if (model.FechaHasta.HasValue)
                filtros += $"Fecha hasta: {model.FechaHasta?.ToShortDateString()}. ";
            if (model.DesglosadoPorObra)
                filtros += "Desglosado por obra. ";

            worksheet.Cell("A1").Value = filtros;
            range = worksheet.Range("A1:Z1");
            worksheet.Range("A1:Z1").Merge();

            range = worksheet.Range("B2:BA2");
            range.Style.Font.Bold = true;
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("A2").Value = "TRABAJADOR";
            DateTime tmp = model.FechaDesde.Value;
            columna = 2;
            while (tmp <= model.FechaHasta.Value)
            {
                worksheet.Cell(2, columna).Value = tmp.Day;
                tmp = tmp.AddDays(1);
                columna++;
            }
            worksheet.Cell(2, columna).Value = "TOT";

            fila = 3;
            columna = 2;
            if (model.DesglosadoPorObra) //Desglosado por obra
            {
                var obras = model.Items.OrderBy(f => f.CentroCoste.Nombre).Select(f => f.CentroCosteId).Distinct();
                foreach (var obra in obras)
                {
                    var personal = model.Items.Where(f => f.CentroCosteId == obra).OrderBy(f => f.Personal.Nombre).Select(f => f.PersonalId).Distinct();
                    foreach (var p in personal)
                    {
                        tmp = model.FechaDesde.Value;
                        worksheet.Cell(fila, 1).Value = model.Items.First(f => f.PersonalId == p).Personal.Nombre;
                        columna = 2;
                        while (tmp <= model.FechaHasta.Value)
                        {
                            worksheet.Cell(fila, columna).DataType = XLDataType.Number;
                            worksheet.Cell(fila, columna).Value = Convert.ToDecimal(model.Items.Where(f => f.CentroCosteId == obra && f.PersonalId == p && f.Fecha == tmp).Sum(f => f.Unidades).ToString("N1"));
                            columna++;
                            tmp = tmp.AddDays(1);
                        }
                        worksheet.Cell(fila, columna).Value = model.Items.Where(f => f.CentroCosteId == obra && f.PersonalId == p).Sum(f => f.Unidades);
                        fila++;
                    }

                    worksheet.Row(fila).Style.Font.FontColor = XLColor.Brown;
                    worksheet.Row(fila).Style.Font.Bold = true;
                    worksheet.Cell(fila, 1).Value = model.Items.First(f => f.CentroCosteId == obra).CentroCoste.Nombre;
                    tmp = model.FechaDesde.Value;
                    columna = 2;
                    while (tmp <= model.FechaHasta.Value)
                    {
                        worksheet.Cell(fila, columna).DataType = XLDataType.Number;
                        worksheet.Cell(fila, columna).Value = Convert.ToDecimal(model.Items.Where(f => f.CentroCosteId == obra && f.Fecha == tmp).Sum(f => f.Unidades).ToString("N1"));
                        columna++;
                        tmp = tmp.AddDays(1);
                    }
                    worksheet.Cell(fila, columna).Value = model.Items.Where(f => f.CentroCosteId == obra).Sum(f => f.Unidades);
                    fila++;
                }
            }
            else //Normal (no desglosado por obra)
            {
                var personal = model.Items.OrderBy(f => f.Personal.Nombre).Select(f => f.PersonalId).Distinct();
                foreach (var p in personal)
                {
                    worksheet.Cell(fila, 1).Value = model.Items.First(f => f.PersonalId == p).Personal.Nombre;
                    tmp = model.FechaDesde.Value;
                    while (tmp <= model.FechaHasta.Value)
                    {
                        worksheet.Cell(fila, columna).DataType = XLDataType.Number;
                        worksheet.Cell(fila, columna).Value = Convert.ToDecimal(model.Items.Where(f => f.PersonalId == p && f.Fecha == tmp).Sum(f => f.Unidades).ToString("N1"));
                        tmp = tmp.AddDays(1);
                        columna++;
                    }
                    worksheet.Cell(fila, columna).DataType = XLDataType.Number;
                    worksheet.Cell(fila, columna).Value = model.Items.Where(f => f.PersonalId == p).Sum(f => f.Unidades);

                    fila++;
                    columna = 2;
                }
            }

            //Totales pie
            tmp = model.FechaDesde.Value;
            columna = 2;
            while (tmp <= model.FechaHasta.Value)
            {
                worksheet.Row(fila).Style.Font.Bold = true;
                worksheet.Cell(fila, columna).DataType = XLDataType.Number;
                worksheet.Cell(fila, columna).Value = Convert.ToDecimal(model.Items.Where(f => f.Fecha == tmp).Sum(f => f.Unidades).ToString("N1"));
                tmp = tmp.AddDays(1);
                columna++;
            }
            worksheet.Cell(fila, columna).DataType = XLDataType.Number;
            worksheet.Cell(fila, columna).Value = model.Items.Sum(f => f.Unidades);

            byte[] workbookBytes;
            using (var ms = new MemoryStream())
            {
                wb.SaveAs(ms);
                workbookBytes = ms.ToArray();
            }
            return File(workbookBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public async Task<IActionResult> ExportPdf()
        {
            InformeTotalHorasPersonalViewModel model = new InformeTotalHorasPersonalViewModel();

            await TryUpdateModelAsync<InformeTotalHorasPersonalViewModel>(model, "");

            if (!ModelState.IsValid)
                return RedirectToAction("Index", model);

            IEnumerable<PartePersonal> list = await _dbContext.PartePersonal.Where(model.ExpressionFilter())
            .Include(f => f.Personal)
            .Include(f => f.CentroCoste)
            .OrderBy(f => f.Fecha)
            .ToListAsync();

            model.Items = list;

            DateTime tmp = model.FechaDesde.Value;
            string rows = string.Empty;
            string cells = string.Empty;

            while (tmp <= model.FechaHasta.Value)
            {
                cells += $"<td class='text-center'>{tmp.Day}</td>";
                tmp = tmp.AddDays(1);
            }
            rows += $"<tr class='font-weight-bold'><td>TRABAJADOR</td>{cells}<td>TOT</td></tr>";
            
            if (model.DesglosadoPorObra) //Desglosado por obra
            {

            }
            else //No desglosado por obra
            {
                var personal = model.Items.Select(f => f.PersonalId).Distinct();
                foreach (var p in personal)
                {
                    cells = string.Empty;
                    tmp = model.FechaDesde.Value;

                    while (tmp <= model.FechaHasta.Value)
                    {
                        cells += $"<td>{model.Items.Where(f => f.PersonalId == p && f.Fecha == tmp).Sum(f => f.Unidades).ToString("N1")}</td>";
                        tmp = tmp.AddDays(1);
                    }

                    rows += $"<tr><td>{model.Items.First(f => f.PersonalId == p).Personal.Nombre}</td>{cells}<td class='font-weight-bold'>{model.Items.Where(f => f.PersonalId == p).Sum(f => f.Unidades)}</td></tr>";
                }
            }

            tmp = model.FechaDesde.Value;
            cells = "<th scope='col' class='text-left pl-2 font-weight-bold'></th>";
            while (tmp <= model.FechaHasta.Value)
            {
                cells += $"<th>{model.Items.Where(f => f.Fecha == tmp).Sum(f => f.Unidades).ToString("N1")}</th>";
                tmp = tmp.AddDays(1);
            }
            cells += $"<th class='text-center px-0 font-weight-bold'>{model.Items.Sum(f => f.Unidades)}</th>";
            rows += $"<tr>{cells}</tr>";

            string table = $"<table class='w-100'>{rows}</table>";

            var html = PrintService.GetHtmlTheme("Informe horas personal", table);
            var pdf = PrintService.GetBytesLandscape(html);
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_InformeHorasPersonal.pdf";
            return File(pdf, "application/pdf", fileName);
        }
    }
}

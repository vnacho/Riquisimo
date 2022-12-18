using ClosedXML.Excel;
using Ferpuser.BLL.Services;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Collaborations")]
    public class InformePartesPersonalValoradoController : ControlPresupuestarioBaseController
    {
        private readonly ApplicationDbContext _dbContext;
        private IWebHostEnvironment _hostEnvironment;

        public InformePartesPersonalValoradoController(ApplicationDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _hostEnvironment = environment;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.CentrosCoste = await _dbContext.CentrosCoste.OrderBy(f => f.Nombre).ToListAsync();
            return View();
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexConfirmed()
        {
            InformePartesPersonalValoradoViewModel model = new InformePartesPersonalValoradoViewModel();

            await TryUpdateModelAsync<InformePartesPersonalValoradoViewModel>(model, "",
                f => f.CentroCosteId,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.DesglosadoPorObra);

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
            InformePartesPersonalValoradoViewModel model = new InformePartesPersonalValoradoViewModel();

            await TryUpdateModelAsync<InformePartesPersonalValoradoViewModel>(model, "",
                f => f.CentroCosteId,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.DesglosadoPorObra);

            if (!ModelState.IsValid)
                return RedirectToAction("Index", model);

            IEnumerable<PartePersonal> list = await _dbContext.PartePersonal.Where(model.ExpressionFilter())
                .Include(f => f.Personal)
                .Include(f => f.CentroCoste)
                .OrderBy(f => f.Fecha)
                .ToListAsync();

            model.Items = list;

            int fila;
            decimal total = 0;
            //string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_InformeHorasPersonal.csv";
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_InformeHorasPersonal.xlsx";

            XLWorkbook wb = new XLWorkbook();
            IXLRange range;
            var ws = wb.Worksheets.Add("Informe");

            //Logotipo
            fila = 1;
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img\\logo_informes.png");
            //ws.AddPicture(imagePath).MoveTo(ws.Cell(fila, 1), ws.Cell(fila + 4, 4));
            ws.AddPicture(imagePath).MoveTo(ws.Cell(fila, 1)).Scale(0.5);
            range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila + 4, 4));
            range.Merge();

            fila = fila + 5;
            ws.Cell(fila, 1).Value = "INFORME DE PARTES DE PERSONAL VALORADO";            
            range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila, 1));
            range.Style.Font.Bold = true;
            range.Merge();

            fila++;
            string filtros = string.Empty;
            if (model.CentroCosteId.HasValue)
                filtros += $"Centro de coste: {_dbContext.CentrosCoste.Find(model.CentroCosteId).Display}. ";
            if (model.FechaDesde.HasValue)
                filtros += $"Fecha desde: {model.FechaDesde?.ToShortDateString()}. ";
            if (model.FechaHasta.HasValue)
                filtros += $"Fecha hasta: {model.FechaHasta?.ToShortDateString()}. ";
            if (model.DesglosadoPorObra)
                filtros += "Desglosado por obra. ";
            ws.Cell(fila, 1).Value = filtros;
            range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila, 1));
            range.Merge();

            fila++;
            range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila, 4));
            range.Style.Font.Bold = true;
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            range = ws.Range(ws.Cell(fila, 2), ws.Cell(fila, 4));
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Cell(fila,1).Value = "TRABAJADOR";
            ws.Cell(fila, 2).Value = "HORAS";
            ws.Cell(fila, 3).Value = "€/hora";
            ws.Cell(fila, 4).Value = "IMPORTE";

            if (model.DesglosadoPorObra) //Desglosado por obra
            {
                var obras = model.Items.OrderBy(f => f.CentroCoste.Nombre).Select(f => f.CentroCosteId).Distinct();
                foreach (var obra in obras)
                {
                    var personal = model.Items.Where(f => f.CentroCosteId == obra).OrderBy(f => f.Personal.Nombre).Select(f => f.PersonalId).Distinct();
                    decimal totalobra = 0;

                    foreach (var p in personal)
                    {
                        fila++;
                        ws.Cell(fila, 1).Value = model.Items.First(f => f.PersonalId == p).Personal.Nombre;
                        ws.Cell(fila, 2).DataType = XLDataType.Number;
                        ws.Cell(fila, 2).Value = model.Items.Where(f => f.PersonalId == p && f.CentroCosteId == obra).Sum(f => f.Unidades);
                        ws.Cell(fila, 3).DataType = XLDataType.Number;
                        ws.Cell(fila, 3).Value = model.Items.First(f => f.PersonalId == p).Personal.PrecioHora;

                        decimal? totalpersona = null;
                        if (model.Items.First(f => f.PersonalId == p).Personal.PrecioHora.HasValue)
                        {
                            totalpersona = model.Items.Where(f => f.PersonalId == p && f.CentroCosteId == obra).Sum(f => f.Unidades) * model.Items.First(f => f.PersonalId == p).Personal.PrecioHora.Value;
                            total += totalpersona.Value;
                            totalobra += totalpersona.Value;

                            ws.Cell(fila, 4).DataType = XLDataType.Number;
                            ws.Cell(fila, 4).Value = Math.Round(totalpersona.Value, 2);
                        }                        
                    }

                    fila++;
                    range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila, 4));
                    range.Style.Font.FontColor = XLColor.FromArgb(126, 0, 0);
                    range.Style.Font.Bold = true;
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    ws.Cell(fila, 1).Value = model.Items.First(f => f.CentroCosteId == obra).CentroCoste.Nombre;
                    ws.Cell(fila, 2).DataType = XLDataType.Number;
                    ws.Cell(fila, 2).Value = model.Items.Where(f => f.CentroCosteId == obra).Sum(f => f.Unidades);
                    ws.Cell(fila, 4).DataType = XLDataType.Number;
                    ws.Cell(fila, 4).Value = Math.Round(totalobra,2);
                }
            }
            else //Normal (no desglosado por obra)
            {
                var personal = model.Items.OrderBy(f => f.Personal.Nombre).Select(f => f.PersonalId).Distinct();
                foreach (var p in personal)
                {
                    fila++;
                    ws.Cell(fila, 1).Value = model.Items.First(f => f.PersonalId == p).Personal.Nombre;
                    ws.Cell(fila, 2).DataType = XLDataType.Number;
                    ws.Cell(fila, 2).Value = model.Items.Where(f => f.PersonalId == p).Sum(f => f.Unidades);
                    ws.Cell(fila, 3).DataType = XLDataType.Number;
                    ws.Cell(fila, 3).Value = model.Items.First(f => f.PersonalId == p).Personal.PrecioHora;
                    
                    if (model.Items.First(f => f.PersonalId == p).Personal.PrecioHora.HasValue)
                    {
                        decimal totalPersona = model.Items.First(f => f.PersonalId == p).Personal.PrecioHora.Value *
                            model.Items.Where(f => f.PersonalId == p).Sum(f => f.Unidades);
                        ws.Cell(fila, 4).DataType = XLDataType.Number;
                        ws.Cell(fila, 4).Value = Math.Round(totalPersona, 2);
                        total += totalPersona;
                    }                    
                }
            }

            //Totales pie
            fila++;
            range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila, 4));
            range.Style.Font.Bold = true;
            ws.Cell(fila, 2).DataType = XLDataType.Number;
            ws.Cell(fila, 2).Value = model.Items.Sum(f => f.Unidades);
            ws.Cell(fila, 4).DataType = XLDataType.Number;
            ws.Cell(fila, 4).Value = Math.Round(total,2);

            //Otros formatos, alineación y bordes
            range = ws.Range(ws.Cell(9, 2), ws.Cell(fila, 4));
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            //range = ws.Range(ws.Cell(8, 1), ws.Cell(fila, 1));
            ws.Column(1).Width = 60;

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
            InformePartesPersonalValoradoViewModel model = new InformePartesPersonalValoradoViewModel();

            await TryUpdateModelAsync<InformePartesPersonalValoradoViewModel>(model, "",
                f => f.CentroCosteId,
                f => f.FechaDesde,
                f => f.FechaHasta,
                f => f.DesglosadoPorObra);

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
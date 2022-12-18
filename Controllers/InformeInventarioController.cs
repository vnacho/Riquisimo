using Ferpuser.Data;
using Ferpuser.Models.PartialModels;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Ferpuser.Models;
using Ferpuser.Models.Dtos;
using Ferpuser.BLL.Filters;
using Ferpuser.Models.Enums;
using Ferpuser.BLL.Managers;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Almacen")]
    public class InformeInventarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly SageContext _sage;
        public ControlPresupuestarioManager _manager { get; set; }

        private IWebHostEnvironment _hostEnvironment;
        public InformeInventarioController(ControlPresupuestarioManager manager, ApplicationDbContext context, SageContext sage, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _sage = sage;
            _manager = manager;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            InformeInventarioViewModel model = new InformeInventarioViewModel();// { Year = DateTime.Now.Year };
            CargarCombos();
            return View(model);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexConfirmed()
        {
            CargarCombos();

            InformeInventarioViewModel model = new InformeInventarioViewModel();

            await TryUpdateModelAsync<InformeInventarioViewModel>(model, "");

            //DateTime FechaDesde = new DateTime(model.Year.Value, model.Month.Value, 1);
            //DateTime FechaHasta = new DateTime(model.Year.Value, model.Month.Value, 1).AddMonths(1).AddSeconds(-1);

            if (!ModelState.IsValid || model.FechaDesde == null)// || model.CentroCosteId == null) //Salir, no continuar
                return View(model);

            model.ItemsMovimientos = await _context.MovimientosArticulosAlmacens.Where(f => f.FechaMovimiento >= model.FechaDesde)// && f.CentroCosteId == model.CentroCosteId)
                .ToListAsync();
            //COMPANY.Select(x => x.Id == 5 && x.Id != EMPLOYEE.Where(z => z.USERNAME == "ADMIN").Select(g => g.COMPANY_ID).FirstOrDefault());
            //            model.Items = _context.InventarioArticulosAlmacen.Where(f => f.Modified >= FechaDesde && f.Deleted == null)
            model.Items = _context.InventarioArticulosAlmacen.Where(f => f.Deleted == null && model.ItemsMovimientos.Select(x => x.ArticulosAlmacenId).ToList().Contains(f.ArticulosAlmacenId))
                .Include(f => f.ArticulosAlmacen)
                .Select(y => new MovimientosInventarioModel
                {
                    ProductCode = y.ArticulosAlmacen.ProductCode,
                    ProductDescription = y.ArticulosAlmacen.ProductDescription,
                    ExInicial = y.Unidades,
                    Salidas = 0,
                    Entradas = 0,
                    ExFinal = 0
                })
                .ToList();

            model.Calculate();

            return View(model);
        }

        public async Task<IActionResult> ExportCsv()
        {
            InformeInventarioViewModel model = new InformeInventarioViewModel();

            await TryUpdateModelAsync<InformeInventarioViewModel>(model, "");

            if (!ModelState.IsValid)
                return RedirectToAction("Index", model);

            int fila;
            //string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_InformeHorasPersonal.csv";
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_InformeInventario.xlsx";

            XLWorkbook wb = new XLWorkbook();
            IXLRange range;
            var ws = wb.Worksheets.Add("Informe");

            //Logotipo
            fila = 1;
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "img\\logo_informes.png");
            //ws.AddPicture(imagePath).MoveTo(ws.Cell(fila, 1), ws.Cell(fila + 4, 4));
            ws.AddPicture(imagePath).MoveTo(ws.Cell(fila, 1)).Scale(0.5);
            range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila + 4, 2));
            range.Merge();

            range = ws.Range(ws.Cell(fila, 3), ws.Cell(fila, 6));
            range.Style.Fill.BackgroundColor = XLColor.LightGray;
            range.Style.Font.FontColor = XLColor.Black;
            range.Merge();
            range.DataType = XLDataType.Text;
            range.Value = "OCTIVI LEVANTINA, S.L.";
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            fila = fila + 5;

            //ws.Cell(fila, 6).DataType = XLDataType.Text;
            //ws.Cell(fila, 6).Value = "Cartera de Obra a fecha:";
            range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila, 6));
            //range.Merge();
            range.DataType = XLDataType.Text;
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell(fila, 2).Value = "INFORME ALMACÉN";
            ws.Cell(fila, 6).Value = "'" + model.FechaDesde.Value.ToString("dd-MM-yyyy");
            ws.Cell(fila, 6).DataType = XLDataType.Text;

            //range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila, 1));
            //range.Style.Font.Bold = true;
            //range.Merge();

            fila++;
            //string filtros = "aqui van los filtros";
            ////if (model.CentroCosteId.HasValue)
            ////    filtros += $"Centro de coste: {_dbContext.CentrosCoste.Find(model.CentroCosteId).Display}. ";
            ////if (model.FechaDesde.HasValue)
            ////    filtros += $"Fecha desde: {model.FechaDesde?.ToShortDateString()}. ";
            ////if (model.FechaHasta.HasValue)
            ////    filtros += $"Fecha hasta: {model.FechaHasta?.ToShortDateString()}. ";
            ////if (model.DesglosadoPorObra)
            ////    filtros += "Desglosado por obra. ";
            ////ws.Cell(fila, 1).Value = filtros;
            //range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila, 1));
            //range.Merge();

            fila++;
            range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila, 6));
            range.Style.Font.Bold = true;
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            //range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila, 8));
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Cell(fila, 1).Value = "CODIGO";
            ws.Cell(fila, 2).Value = "DESCRIPCIÓN ARTÍCULO";
            ws.Cell(fila, 3).Value = "EX INICIAL";
            ws.Cell(fila, 4).Value = "SALIDAS";
            ws.Cell(fila, 5).Value = "ENTRADAS";
            ws.Cell(fila, 6).Value = "EX FINALES";

            var InicioTabla = fila + 1;
            if (model.Items != null)
            {
                foreach (var p in model.Items)
                {
                    fila++;
                    ws.Cell(fila, 1).DataType = XLDataType.Text;
                    ws.Cell(fila, 1).Value = model.Items.First(f => f.ProductCode == p.ProductCode).ProductCode;

                    ws.Cell(fila, 2).DataType = XLDataType.Text;
                    ws.Cell(fila, 2).Value = model.Items.First(f => f.ProductDescription == p.ProductDescription).ProductDescription;

                    ws.Cell(fila, 3).DataType = XLDataType.Number;
                    ws.Cell(fila, 3).Value = model.Items.First(f => f.ExInicial == p.ExInicial).ExInicial;

                    ws.Cell(fila, 4).DataType = XLDataType.Number;
                    ws.Cell(fila, 4).Value = model.Items.First(f => f.Salidas == p.Salidas).Salidas;

                    ws.Cell(fila, 5).DataType = XLDataType.Number;
                    ws.Cell(fila, 5).Value = model.Items.First(f => f.Entradas == p.Entradas).Entradas;

                    ws.Cell(fila, 6).DataType = XLDataType.Number;
                    ws.Cell(fila, 6).Value = model.Items.First(f => f.ExFinal == p.ExFinal).ExFinal;

                }
            }

            InicioTabla = fila - InicioTabla;

            range = ws.Range(ws.Cell(fila - InicioTabla, 3), ws.Cell(fila, 6));
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            range = ws.Range(ws.Cell(fila - InicioTabla, 2), ws.Cell(fila, 2));
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            range = ws.Range(ws.Cell(fila - InicioTabla, 1), ws.Cell(fila, 1));
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            range = ws.Range(ws.Cell(fila - InicioTabla, 1), ws.Cell(fila + 1, 6));
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            //Totales pie
            //fila++;
            //range = ws.Range(ws.Cell(fila, 3), ws.Cell(fila, 6));
            //range.Style.Font.Bold = true;

            //Otros formatos, alineación y bordes

            //range = ws.Range(ws.Cell(8, 1), ws.Cell(fila, 1));
            ws.Column(1).Width = 15;
            ws.Column(2).Width = 40;
            ws.Column(3).Width = 40;
            ws.Column(4).Width = 15;
            ws.Column(5).Width = 15;
            ws.Column(6).Width = 15;
            //ws.Column(7).Width = 15;
            //ws.Column(8).Width = 15;

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
            //InformePartesPersonalValoradoViewModel model = new InformePartesPersonalValoradoViewModel();

            //await TryUpdateModelAsync<InformePartesPersonalValoradoViewModel>(model, "",
            //    f => f.CentroCosteId,
            //    f => f.FechaDesde,
            //    f => f.FechaHasta,
            //    f => f.DesglosadoPorObra);

            //if (!ModelState.IsValid)
            //    return RedirectToAction("Index", model);

            //IEnumerable<PartePersonal> list = await _dbContext.PartePersonal.Where(model.ExpressionFilter())
            //.Include(f => f.Personal)
            //.Include(f => f.CentroCoste)
            //.OrderBy(f => f.Fecha)
            //.ToListAsync();

            //model.Items = list;

            //DateTime tmp = model.FechaDesde.Value;
            //string rows = string.Empty;
            //string cells = string.Empty;

            //while (tmp <= model.FechaHasta.Value)
            //{
            //    cells += $"<td class='text-center'>{tmp.Day}</td>";
            //    tmp = tmp.AddDays(1);
            //}
            //rows += $"<tr class='font-weight-bold'><td>TRABAJADOR</td>{cells}<td>TOT</td></tr>";

            //if (model.DesglosadoPorObra) //Desglosado por obra
            //{

            //}
            //else //No desglosado por obra
            //{
            //    var personal = model.Items.Select(f => f.PersonalId).Distinct();
            //    foreach (var p in personal)
            //    {
            //        cells = string.Empty;
            //        tmp = model.FechaDesde.Value;

            //        while (tmp <= model.FechaHasta.Value)
            //        {
            //            cells += $"<td>{model.Items.Where(f => f.PersonalId == p && f.Fecha == tmp).Sum(f => f.Unidades).ToString("N1")}</td>";
            //            tmp = tmp.AddDays(1);
            //        }

            //        rows += $"<tr><td>{model.Items.First(f => f.PersonalId == p).Personal.Nombre}</td>{cells}<td class='font-weight-bold'>{model.Items.Where(f => f.PersonalId == p).Sum(f => f.Unidades)}</td></tr>";
            //    }
            //}

            //tmp = model.FechaDesde.Value;
            //cells = "<th scope='col' class='text-left pl-2 font-weight-bold'></th>";
            //while (tmp <= model.FechaHasta.Value)
            //{
            //    cells += $"<th>{model.Items.Where(f => f.Fecha == tmp).Sum(f => f.Unidades).ToString("N1")}</th>";
            //    tmp = tmp.AddDays(1);
            //}
            //cells += $"<th class='text-center px-0 font-weight-bold'>{model.Items.Sum(f => f.Unidades)}</th>";
            //rows += $"<tr>{cells}</tr>";

            //string table = $"<table class='w-100'>{rows}</table>";

            //var html = PrintService.GetHtmlTheme("Informe horas personal", table);
            //var pdf = PrintService.GetBytesLandscape(html);
            //string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_InformeHorasPersonal.pdf";
            return null; //File(pdf, "application/pdf", fileName);
        }


        private void CargarCombos()
            {
                ViewBag.CentrosCoste = _context.CentrosCoste.OrderBy(f => f.Nombre).ToList();
            }
        }
}

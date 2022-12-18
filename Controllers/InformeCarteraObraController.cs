using ClosedXML.Excel;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models.PartialModels;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    public class InformeCarteraObraController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly SageContext _sage;
        public ControlPresupuestarioManager _manager { get; set; }

        private IWebHostEnvironment _hostEnvironment;

        public InformeCarteraObraController(ControlPresupuestarioManager manager, ApplicationDbContext context, SageContext sage, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _sage = sage;
            _manager = manager;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            InformeCarteraObraViewModel model = new InformeCarteraObraViewModel() { Year = DateTime.Now.Year };
            return View(model);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexConfirmed()
        {
            InformeCarteraObraViewModel model = new InformeCarteraObraViewModel();

            await TryUpdateModelAsync<InformeCarteraObraViewModel>(model, "",
                f => f.Month,
                f => f.Year);

            if (!ModelState.IsValid || model.Month == null || model.Year == null ) //Salir, no continuar
                return View(model);


            //Borrar los registros de origen del mes seleccionado, se recalcularán siempre
            model.Origenes = _context.Origen.Where(f => (f.Mes <= model.Month && f.Anio == model.Year) || f.Anio < model.Year).ToList();

            DateTime FechaHasta = new DateTime(model.Year.Value, model.Month.Value, 1).AddMonths(1).AddSeconds(-1);

            try
            {
                model.Items = _context.Congresses
                    .Where(f => f.FechaInicio <= FechaHasta && !f.Finalizada && f.Deleted == null)
                    .Include(f => f.ContratosObra)
                    .Select(y => new CongressCarteraObraModel
                    {
                        Id = y.Id,
                        Number = y.Number,
                        Code = y.Code,
                        Name = y.Name,
                        Place = y.Place,
                        StartDate = y.StartDate,
                        EndDate = y.EndDate,
                        TipoCongress = y.TipoCongress,
                        NombreCliente = y.NombreCliente,
                        FechaInicio = y.FechaInicio,
                        Finalizada = y.Finalizada,
                        ContratosObra = y.ContratosObra
                    })
                    .OrderBy(x => x.Number)
                    .ToList();

                //Recuperar el nombre del cliente
                foreach (var item in model.Items)
                {
                    //var client_db = _context.Clients.FirstOrDefault(x => x.SageCode.Trim() == item.NombreCliente.Trim());
                    //if (client_db != null)
                    //{
                    //    item.NombreCliente = client_db.BusinessName;
                    //    continue;
                    //}

                    var client_sage = _sage.Clientes.FirstOrDefault(f => f.Codigo.Trim() == item.NombreCliente.Trim());
                    if (client_sage != null)
                        item.NombreCliente = client_sage.Nombre;
                }                

                model.Calculate();

            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return View(model);
        }

        public async Task<IActionResult> ExportCsv()
        {
            InformeCarteraObraViewModel model = new InformeCarteraObraViewModel();
            
            await TryUpdateModelAsync<InformeCarteraObraViewModel>(model, "");

            if (!ModelState.IsValid)
                return RedirectToAction("Index", model);

            int fila;
            //string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_InformeHorasPersonal.csv";
            string fileName = DateTime.Now.ToString("yyyyMMddHHss") + "_InformeCarteraObra.xlsx";

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

            range = ws.Range(ws.Cell(fila, 3), ws.Cell(fila, 8));
            range.Style.Fill.BackgroundColor = XLColor.LightGray;
            range.Style.Font.FontColor = XLColor.Black;
            range.Merge();
            range.DataType = XLDataType.Text;
            range.Value = "OCIVI LEVANTINA, S.L.";
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            fila = fila + 5;

            range = ws.Range(ws.Cell(fila, 6), ws.Cell(fila, 7));
            range.Merge();
            range.DataType = XLDataType.Text;
            range.Value = "Cartera de Obra a fecha:";

            ws.Cell(fila, 8).Value = "'" + model.Month.ToString() + '-' + model.Year.ToString();
            ws.Cell(fila, 8).DataType = XLDataType.Text;

            fila++;
            fila++;

            range = ws.Range(ws.Cell(fila, 1), ws.Cell(fila, 8));
            range.Style.Font.Bold = true;
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Cell(fila, 1).Value = "CODIGO";
            ws.Cell(fila, 2).Value = "NOMBRE";
            ws.Cell(fila, 3).Value = "CLIENTE";
            ws.Cell(fila, 4).Value = "PRESUPUESTO";
            ws.Cell(fila, 5).Value = "EJECUTADO";
            ws.Cell(fila, 6).Value = "RESULTADO";
            ws.Cell(fila, 7).Value = "%";
            ws.Cell(fila, 8).Value = "PENDIENTE";

            var InicioTabla = fila;
            if (model.Items != null)
            {
                foreach (var p in model.Items)
                {
                    fila++;
                    ws.Cell(fila, 1).DataType = XLDataType.Text;
                    ws.Cell(fila, 1).Value = model.Items.First(f => f.Number == p.Number).Number.ToString();

                    ws.Cell(fila, 2).DataType = XLDataType.Text;
                    ws.Cell(fila, 2).Value = model.Items.First(f => f.Name == p.Name).Name; 

                    ws.Cell(fila, 3).DataType = XLDataType.Text;
                    ws.Cell(fila, 3).Value = model.Items.First(f => f.NombreCliente == p.NombreCliente).NombreCliente;

                    ws.Cell(fila, 4).DataType = XLDataType.Number;
                    ws.Cell(fila, 4).Value = model.Items.First(f => f.ImportePresupuesto == p.ImportePresupuesto).ImportePresupuesto;

                    ws.Cell(fila, 5).DataType = XLDataType.Number;
                    ws.Cell(fila, 5).Value = model.Items.First(f => f.ImporteEjecutado == p.ImporteEjecutado).ImporteEjecutado;

                    ws.Cell(fila, 6).DataType = XLDataType.Number;
                    ws.Cell(fila, 6).Value = model.Items.First(f => f.ImporteResultado == p.ImporteResultado).ImporteResultado;

                    ws.Cell(fila, 7).DataType = XLDataType.Number;
                    ws.Cell(fila, 7).Value = model.Items.First(f => f.ImportePorcentaje == p.ImportePorcentaje).ImportePorcentaje/100;

                    ws.Cell(fila, 8).DataType =  XLDataType.Number;
                    ws.Cell(fila, 8).Value = model.Items.First(f => f.ImportePendiente == p.ImportePendiente).ImportePendiente;
                }
            }

            InicioTabla = fila - InicioTabla;

            range = ws.Range(ws.Cell(fila - InicioTabla, 4), ws.Cell(fila, 8));
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            range = ws.Range(ws.Cell(fila - InicioTabla, 2), ws.Cell(fila, 3));
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            range = ws.Range(ws.Cell(fila - InicioTabla, 1), ws.Cell(fila, 1));
            range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            range = ws.Range(ws.Cell(fila - InicioTabla, 1), ws.Cell(fila+1, 8));
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            //Totales pie
            fila++;
            range = ws.Range(ws.Cell(fila, 3), ws.Cell(fila, 8));
            range.Style.Font.Bold = true;

            ws.Cell(fila, 3).DataType = XLDataType.Text;
            ws.Cell(fila, 3).Value = "TOTAL";

            ws.Cell(fila, 4).DataType = XLDataType.Number;
            ws.Cell(fila, 4).Value = model.TotalPresupuesto;

            ws.Cell(fila, 5).DataType = XLDataType.Number;
            ws.Cell(fila, 5).Value = model.TotalEjecutado;

            ws.Cell(fila, 6).DataType = XLDataType.Number;
            ws.Cell(fila, 6).Value = model.TotalResultado;

            //ws.Cell(fila, 7).DataType = XLDataType.Number;
            //ws.Cell(fila, 7).Value = model.TotalPorcentaje/100;

            ws.Cell(fila, 8).DataType = XLDataType.Number;
            ws.Cell(fila, 8).Value = model.TotalPendiente;

            ws.Range(ws.Cell(9, 7), ws.Cell(fila, 7)).Style.NumberFormat.Format = "###.##%";

            ws.Column(1).Width = 15;
            ws.Column(2).Width = 40;
            ws.Column(3).Width = 40;
            ws.Column(4).Width = 15;
            ws.Column(5).Width = 15;
            ws.Column(6).Width = 15;
            ws.Column(7).Width = 15;
            ws.Column(8).Width = 15;

            byte[] workbookBytes;
            using (var ms = new MemoryStream())
            {
                wb.SaveAs(ms);
                workbookBytes = ms.ToArray();
            }
            return  File(workbookBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        private XLDataType IXLNumberFormatBase(string v)
        {
            throw new NotImplementedException();
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
            return null;//File(pdf, "application/pdf", fileName);
        }
    }
}

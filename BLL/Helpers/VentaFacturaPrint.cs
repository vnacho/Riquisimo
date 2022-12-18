using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Enums;
using Ferpuser.Models.Sage;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ferpuser.BLL.Helpers
{
    public class VentaFacturaPrint
    {
        protected readonly IHttpContextAccessor httpContextAccessor;

        protected readonly ApplicationDbContext dbContext;
        protected readonly SageContext sageContext;

        private FormatoImpresion formato { get; set; }
        private VentaFactura model { get; set; }
        private Clientes cliente { get; set; }
        private Congress evento { get; set; }

        protected empresa empresa { get; set; }
        protected modconfi datos_registro_empresa { get; set; }

        PdfPage page;
        PdfDocument pdfDoc;
        Document document;
        Paragraph p;

        public VentaFacturaPrint(ApplicationDbContext dbContext, SageContext sageContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.sageContext = sageContext;
            this.httpContextAccessor = httpContextAccessor;

            empresa = sageContext.empresa.Find(Consts.CODIGO_EMPRESA);
            datos_registro_empresa = sageContext.modconfi.Find(Consts.CODIGO_EMPRESA);
        }

        public byte[] GetFactura(int Id, FormatoImpresion formato)
        {
            model = dbContext.VentaFacturas.Include(f => f.FacturaLineas).AsNoTracking().SingleOrDefault(f => f.Id == Id);
            cliente = sageContext.Clientes.AsNoTracking().FirstOrDefault(f => f.Codigo == model.CodigoCliente);
            evento = dbContext.Congresses.AsNoTracking().FirstOrDefault(f => f.Number.ToString() == model.CodigoEvento);
            this.formato = formato;
            model.Calcular();

            MemoryStream ms = new MemoryStream();
            PdfWriter writer = new PdfWriter(ms);

            pdfDoc = new PdfDocument(writer);
            document = new Document(pdfDoc, PageSize.A4, false);

            EndPageEventHandler endpagehandler = new EndPageEventHandler(document, formato, model, cliente, evento, empresa, datos_registro_empresa, httpContextAccessor);
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, endpagehandler);

            // Calculate top margin to be sure that the table will fit the margin.
            float topMargin = 20 + endpagehandler.HeaderHeight;
            float bottomMargin = 20 + endpagehandler.FooterHeight;
            document.SetMargins(topMargin, IText7Helper.DEFAULT_MARGIN, bottomMargin, IText7Helper.DEFAULT_MARGIN);
            document.SetFont(IText7Helper.GetCalibriFont());
            document.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
            document.SetProperty(Property.LEADING, new Leading(Leading.MULTIPLIED, 1.0f)); //Establece el line height de cada párrafo

            writer.SetCloseStream(false);

            page = pdfDoc.AddNewPage();

            ImprimirLineas();
            ImprimirDatosEvento();
            ImprimirDesglose();
            ImprimirRetencion();
            ImprimirFormaPago();
            ImprimirNotasFactura();

            document.Close();
            return ms.ToArray();
        }

        private void ImprimirLineas()
        {
            Table table;
            Cell cell;

            if (formato == FormatoImpresion.C)
                table = new Table(UnitValue.CreatePercentArray(new float[] { 60, 5, 5, 10, 5, 5, 10 }));
            else
                table = new Table(UnitValue.CreatePercentArray(new float[] { 50, 5, 10, 5, 5, 10 }));
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetBorderTop(Border.NO_BORDER);

            cell = new Cell();
            cell.Add(new Paragraph("DESCRIPCIÓN"));
            cell.SetBold();
            cell.SetBorderRight(Border.NO_BORDER);
            cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(cell);

            cell = new Cell();
            cell.Add(new Paragraph("UDS").SetTextAlignment(TextAlignment.RIGHT));
            cell.SetBold();
            cell.SetBorderLeft(Border.NO_BORDER);
            cell.SetBorderRight(Border.NO_BORDER);
            cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(cell);

            if (formato == FormatoImpresion.C)
            {
                cell = new Cell();
                cell.Add(new Paragraph("TP").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBold();
                cell.SetBorderLeft(Border.NO_BORDER);
                cell.SetBorderRight(Border.NO_BORDER);
                cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                table.AddHeaderCell(cell);
            }

            cell = new Cell();
            cell.Add(new Paragraph("PRECIO").SetTextAlignment(TextAlignment.RIGHT));
            cell.SetBold();
            cell.SetBorderLeft(Border.NO_BORDER);
            cell.SetBorderRight(Border.NO_BORDER);
            cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(cell);

            cell = new Cell();
            cell.Add(new Paragraph("DTO").SetTextAlignment(TextAlignment.RIGHT));
            cell.SetBold();
            cell.SetBorderLeft(Border.NO_BORDER);
            cell.SetBorderRight(Border.NO_BORDER);
            cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(cell);

            cell = new Cell();
            cell.Add(new Paragraph("IVA").SetTextAlignment(TextAlignment.RIGHT));
            cell.SetBold();
            cell.SetBorderLeft(Border.NO_BORDER);
            cell.SetBorderRight(Border.NO_BORDER);
            cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(cell);

            cell = new Cell();
            cell.Add(new Paragraph("IMPORTE").SetTextAlignment(TextAlignment.RIGHT));
            cell.SetBold();
            cell.SetBorderLeft(Border.NO_BORDER);
            cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddHeaderCell(cell);

            //Líneas de la factura
            int colspan = formato == FormatoImpresion.C ? 7 : 6;
            foreach (var item in model.FacturaLineas.OrderBy(f => f.Orden))
            {
                cell = new Cell();
                cell.Add(new Paragraph($"{item.NombreArticulo}"));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                cell = new Cell();
                cell.Add(new Paragraph($"{item.Unidades}").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                if (formato == FormatoImpresion.C)
                {
                    cell = new Cell();
                    cell.Add(new Paragraph($"{item.Tiempo}").SetTextAlignment(TextAlignment.RIGHT));
                    cell.SetBorder(Border.NO_BORDER);
                    cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                    table.AddCell(cell);
                }

                cell = new Cell();
                cell.Add(new Paragraph($"{item.BaseImponiblePrecioUnitario.ToString("N2")}").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                cell = new Cell();
                cell.Add(new Paragraph($"{item.Descuento}").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                cell = new Cell();
                cell.Add(new Paragraph($"{item.IVA_Porcentaje}").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                cell = new Cell();
                cell.Add(new Paragraph($"{item.BaseImponibleTotal.ToString("N2")}").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                if (!string.IsNullOrWhiteSpace(item.ObservacionesFacturaLinea))
                {
                    cell = new Cell(1, colspan);
                    cell.Add(new Paragraph($"{item.ObservacionesFacturaLinea}"));
                    cell.SetBorder(Border.NO_BORDER);
                    cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                    table.AddCell(cell);
                }
                if (item.DescripcionAmpliada && !string.IsNullOrWhiteSpace(item.TextoDescripcionAmpliada))
                {
                    cell = new Cell(1, colspan);
                    cell.SetPaddingLeft(15);
                    cell.Add(new Paragraph($"{item.TextoDescripcionAmpliada}").SetItalic());
                    cell.SetBorder(Border.NO_BORDER);
                    cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                    table.AddCell(cell);
                }
            }

            if (formato == FormatoImpresion.D)
            {
                //Fila vacía
                for (int i = 0; i < colspan; i++)
                {
                    table.AddCell(new Cell());
                }

                //Fila
                cell = new Cell(1, colspan - 1);
                cell.Add(new Paragraph("CERTIFICADO A ORIGEN DE MES"));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                cell = new Cell(1, colspan - 1);
                cell.Add(new Paragraph($"{model.FacturaLineas.Sum(f => f.BaseImponibleTotal).ToString("N2")}").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                //Fila
                cell = new Cell();
                cell.Add(new Paragraph("A DEDUCIR CERTIFICACIÓN ANTERIOR"));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                cell = new Cell();
                //cell.Add(new Paragraph("-1,00").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                cell = new Cell();
                //cell.Add(new Paragraph($"{model.OrigenImporte.Value.ToString("N2")}").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                cell = new Cell();
                //cell.Add(new Paragraph("0,00%").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                cell = new Cell();
                //cell.Add(new Paragraph("0,00%").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);

                cell = new Cell();
                cell.Add(new Paragraph($"-{model.OrigenImporte.Value.ToString("N2")}").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_SM);
                table.AddCell(cell);
            }

            document.Add(table);

            //Espacio posterior
            document.Add(new Paragraph());

        }
        private void ImprimirDatosEvento()
        {
            if (formato == FormatoImpresion.A && evento != null)
            {
                string datos_evento;
                if (evento.EndDate != evento.StartDate)
                    datos_evento = $"{evento.Name}. A celebrar en {evento.Place} del {evento.StartDate.ToShortDateString()} al {evento.EndDate.ToShortDateString()}.";
                else
                    datos_evento = $"{evento.Name}. A celebrar en {evento.Place} el día {evento.StartDate.ToShortDateString()}.";
                document.Add(new Paragraph($"{datos_evento}").SetFontSize(IText7Helper.FONT_SIZE_SM));

                //Espacio posterior
                document.Add(new Paragraph());
            }
        }
        private void ImprimirDesglose()
        {
            Cell cell;

            //Desglose totales            
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 35, 65 }));
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetBorderTop(Border.NO_BORDER);
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER));

            Table tableDesglose = new Table(UnitValue.CreatePercentArray(new float[] { 28, 16, 28, 28 }));
            tableDesglose.SetWidth(UnitValue.CreatePercentValue(100));

            cell = new Cell();
            cell.Add(new Paragraph("BASE IMPONIBLE").SetTextAlignment(TextAlignment.RIGHT).SetUnderline());
            cell.SetBorder(Border.NO_BORDER);
            cell.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
            tableDesglose.AddHeaderCell(cell);

            cell = new Cell();
            cell.Add(new Paragraph("IVA %").SetTextAlignment(TextAlignment.RIGHT).SetUnderline());
            cell.SetBorder(Border.NO_BORDER);
            cell.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
            tableDesglose.AddHeaderCell(cell);

            cell = new Cell();
            cell.Add(new Paragraph("TOTAL IVA").SetTextAlignment(TextAlignment.RIGHT).SetUnderline());
            cell.SetBorder(Border.NO_BORDER);
            cell.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
            tableDesglose.AddHeaderCell(cell);

            cell = new Cell();
            cell.Add(new Paragraph("TOTAL").SetTextAlignment(TextAlignment.CENTER).SetUnderline().SetBold());
            cell.SetBorderBottom(Border.NO_BORDER);
            cell.SetBorderRight(Border.NO_BORDER);
            cell.SetBorderTop(Border.NO_BORDER);
            cell.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
            tableDesglose.AddHeaderCell(cell);

            var ivas = model.FacturaLineas.Select(f => f.IVA_Porcentaje).Distinct().OrderBy(f => f);
            string totalAntesRetencion = model.TotalAntesRetencion?.ToString("C");
            foreach (var iva in ivas)
            {
                var totalBI = model.FacturaLineas.Where(f => f.IVA_Porcentaje == iva).Sum(f => f.BaseImponibleTotal);
                if (formato == FormatoImpresion.D && iva == 0)
                    totalBI = totalBI - model.OrigenImporte.Value;

                var totalIVA = model.FacturaLineas.Where(f => f.IVA_Porcentaje == iva).Sum(f => f.ImporteIVA);
                var totalLinea = totalBI + totalIVA;

                cell = new Cell();
                cell.Add(new Paragraph($"{totalBI.ToString("N2")}").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
                tableDesglose.AddCell(cell);

                cell = new Cell();
                cell.Add(new Paragraph($"{iva} %").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
                tableDesglose.AddCell(cell);

                cell = new Cell();
                cell.Add(new Paragraph($"{totalIVA.ToString("N2")}").SetTextAlignment(TextAlignment.RIGHT));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
                tableDesglose.AddCell(cell);

                if (!string.IsNullOrWhiteSpace(totalAntesRetencion)) //El total solo se muestra en la primera fila, por eso se vacía dentro de este if
                {
                    cell = new Cell(ivas.Count(), 1);
                    cell.Add(new Paragraph($"{totalAntesRetencion}").SetTextAlignment(TextAlignment.CENTER).SetBold());
                    cell.SetBorderBottom(Border.NO_BORDER);
                    cell.SetBorderRight(Border.NO_BORDER);
                    cell.SetBorderTop(Border.NO_BORDER);
                    cell.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
                    cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    tableDesglose.AddCell(cell);

                    totalAntesRetencion = string.Empty;
                }
            }

            table.AddCell(tableDesglose);
            document.Add(table);
        }        
        private void ImprimirRetencion()
        {
            Cell cell;
            //Retención
            if (model.TieneRetencion)
            {
                string porcentaje_retencion = model.Retencion_Porcentaje.HasValue ? model.Retencion_Porcentaje.Value.ToString("N2") : string.Empty;
                string total_retencion = model.TotalRetencion.HasValue ? model.TotalRetencion.Value.ToString("N2") : string.Empty;
                string tipo = string.Empty;
                string modo = string.Empty;

                if (model.EsRetencionFiscal.HasValue && model.EsRetencionFiscal.Value)
                    tipo = "Fisc.";
                else if (model.EsRetencionNoFiscal.HasValue && model.EsRetencionNoFiscal.Value)
                    tipo = "No Fisc.";
                if (model.ModoRetencion.HasValue)
                    modo = model.ModoRetencion.Value == ModoRetencion.SobreTotal ? "Total" : "Base";

                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 45, 55 }));
                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.SetBorder(Border.NO_BORDER);
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER));

                Table tableRetencion = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 }));
                tableRetencion.SetWidth(UnitValue.CreatePercentValue(100));
                tableRetencion.SetBorder(Border.NO_BORDER);

                cell = new Cell();
                cell.Add(new Paragraph($"Ret. {tipo}/{modo} ({porcentaje_retencion})").SetUnderline().SetTextAlignment(TextAlignment.CENTER));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
                tableRetencion.AddHeaderCell(cell);

                cell = new Cell();
                cell.Add(new Paragraph($"TOTAL NETO").SetUnderline().SetTextAlignment(TextAlignment.CENTER));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
                tableRetencion.AddHeaderCell(cell);

                cell = new Cell();
                cell.Add(new Paragraph($"{total_retencion}").SetTextAlignment(TextAlignment.CENTER));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
                tableRetencion.AddCell(cell);

                cell = new Cell();
                cell.Add(new Paragraph($"{model.Total.ToString("N2")}").SetTextAlignment(TextAlignment.CENTER));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetFontSize(IText7Helper.FONT_SIZE_NORMAL);
                tableRetencion.AddCell(cell);

                cell = new Cell();
                cell.SetBorder(Border.NO_BORDER);
                cell.Add(tableRetencion);
                table.AddCell(cell);
                document.Add(table);
            }
        }
        private void ImprimirFormaPago()
        {
            string sFormaPago = string.Empty;
            p = new Paragraph().SetFontSize(IText7Helper.FONT_SIZE_SM);

            if (formato == FormatoImpresion.A && evento != null)
            {
                //11/07/2022 JIR. Se solicita que no se imprima el nombre del evento
                //p.Add(new Text($"Transferencia a: {empresa.NOMBRE.Trim()} - {evento.Name.Trim()}"));
                p.Add(new Text($"Transferencia a: {empresa.NOMBRE.Trim()}"));
                p.Add(new Text("\n"));
                p.Add(new Text($"IBAN: {evento.IBAN} SWIFT: {evento.SwiftCode}"));
                document.Add(p);                
            }
            else
            {
                var forma_pago = sageContext.FPag.Find(cliente.FPAG);
                if (forma_pago == null)
                    forma_pago = sageContext.FPag.Find("00"); //Forma de pago por defecto 

                p.Add(new Text($"Forma pago: ").SetBold());
                p.Add(new Text($"{forma_pago.Nombre}"));
                document.Add(p);
            }
        }
        private void ImprimirNotasFactura()
        {
            //NOTAS_FACTURA => Notas iva exento y inversión sujeto pasivo •	Se pueden dar los 2 avisos al mismo tiempo
            List<string> notas_factura = new List<string>();
            if (model.FacturaLineas.Any(f => f.CodigoTipoIVA.Trim() == "07")) //Tiene que aparecer cuando el código del IVA sea el “07”, y siempre va a ser ese código
                notas_factura.Add("Operación de Inversión del Sujeto Pasivo de acuerdo al Art. 84 Uno 2º f de la Ley 37/1992 de 28 de diciembre sobre el Impuesto del Valor Añadido");
            else if (model.FacturaLineas.Any(f => f.IVA_Porcentaje == 0)) //8/2/2022 => En el caso de que el porcentaje de iva sea 0% ya debo mostrar el primer mensaje ¿correcto? SI
                notas_factura.Add("Factura exenta de IVA según el artículo 20.1 de la Ley 37/1992 de 28 de diciembre sobre el Impuesto del Valor Añadido.");

            foreach (var nota in notas_factura)
            {
                document.Add(new Paragraph(nota).SetBold().SetItalic().SetFontSize(IText7Helper.FONT_SIZE_XS).SetFontColor(ColorConstants.GRAY));
            }           
        }

        //private void ImprimirPie()
        //{
        //    //float alto_total_pie = 0;
        //    //float width, height;
        //    if (!string.IsNullOrWhiteSpace(evento.TailBase64))
        //    {
        //        string sBase64 = evento.TailBase64.Substring(evento.TailBase64.LastIndexOf(',') + 1); //Hay que quitar la primera parte del string 
        //        ImageData imageData = ImageDataFactory.Create(Convert.FromBase64String(sBase64));
        //        Image image = new Image(imageData);
        //        //width = page.GetPageSize().GetWidth() - document.GetLeftMargin() - document.GetRightMargin();
        //        //height = width * image.GetImageHeight() / image.GetImageWidth();
        //        //image.ScaleToFit(width, height);

        //        //pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
        //        //area = new Rectangle(document.GetLeftMargin(), 0, image.GetImageScaledWidth(), image.GetImageScaledHeight());

        //        image = image.SetFixedPosition(pdfDoc.GetNumberOfPages(), 0, 0, page.GetPageSize().GetWidth());
        //        document.Add(image);
        //        //new Canvas(pdfCanvas, area).Add(image);
        //        //alto_total_pie = image.GetImageScaledHeight();
        //    }
        //}       

        //private void ImprimirNumerosPaginas()
        //{
            ////Page numbers
            //int n = pdfDoc.GetNumberOfPages();
            //if (n > 1)
            //{
            //    for (int i = 1; i <= n; i++)
            //    {
            //        //Pie de página numeración
            //        document.ShowTextAligned(
            //            new Paragraph($"Página {i} de {n}").SetFontSize(IText7Helper.FONT_SIZE_SM),
            //            559,
            //            10,
            //            i,
            //            TextAlignment.RIGHT,
            //            VerticalAlignment.BOTTOM,
            //            0);
            //    }
            //}
        //}
    }

    public class EndPageEventHandler : IEventHandler
    {
        IHttpContextAccessor httpContextAccessor;

        private Document doc;
        private FormatoImpresion formato;
        private VentaFactura model;
        private Clientes cliente;
        private Congress evento;
        protected empresa empresa { get; set; }
        protected modconfi datos_registro_empresa { get; set; }

        private Table HeaderTable;
        private Table FooterTable;

        public float HeaderHeight { get; set; }
        public float FooterHeight { get; set; }

        public EndPageEventHandler(Document doc, 
            FormatoImpresion formato, 
            VentaFactura model, 
            Clientes cliente, 
            Congress evento,
            empresa empresa, 
            modconfi datos_registro_empresa,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this.doc = doc;
            this.formato = formato;
            this.model = model;
            this.cliente = cliente;
            this.evento = evento;
            this.empresa = empresa;
            this.datos_registro_empresa = datos_registro_empresa;
            this.httpContextAccessor = httpContextAccessor;

            InitHeader();
            InitFooter();

            TableRenderer renderer;
            LayoutResult result;

            // Simulate the positioning of the renderer to find out how much space the header table will occupy.
            renderer = (TableRenderer)HeaderTable.CreateRendererSubTree();
            renderer.SetParent(new DocumentRenderer(doc));            
            result = renderer.Layout(new LayoutContext(new LayoutArea(0, PageSize.A4)));
            HeaderHeight = result.GetOccupiedArea().GetBBox().GetHeight();

            // Simulate the positioning of the renderer to find out how much space the header table will occupy.
            renderer = (TableRenderer)FooterTable.CreateRendererSubTree();
            renderer.SetParent(new DocumentRenderer(doc));
            result = renderer.Layout(new LayoutContext(new LayoutArea(0, PageSize.A4)));
            FooterHeight = result.GetOccupiedArea().GetBBox().GetHeight();
        }
        public void HandleEvent(Event e)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)e;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc);
            Rectangle area = page.GetPageSize();

            //Header
            pdfCanvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc);
            new Canvas(pdfCanvas, area).Add(HeaderTable);

            //Footer
            pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
            area = new Rectangle(0, 20, page.GetPageSize().GetWidth(), FooterHeight);
            new Canvas(pdfCanvas, area).Add(FooterTable).Close();
        }

        private void InitHeader()
        {
            Cell cell;

            HeaderTable = new Table(1).UseAllAvailableWidth();
            HeaderTable.SetFont(IText7Helper.GetCalibriFont());
            HeaderTable.SetBorder(Border.NO_BORDER);
            HeaderTable.SetMargins(0, IText7Helper.DEFAULT_MARGIN, 0, IText7Helper.DEFAULT_MARGIN);

            HeaderTable.SetProperty(Property.LEADING, new Leading(Leading.MULTIPLIED, 1.0f));
            
            if (formato == FormatoImpresion.A)
            {
                //Cabecera de evento
                if (!string.IsNullOrWhiteSpace(evento.LogoBase64))
                {   
                    string sBase64 = evento.LogoBase64.Substring(evento.LogoBase64.LastIndexOf(',') + 1); //Hay que quitar la primera parte del string 
                    ImageData imageData = ImageDataFactory.Create(Convert.FromBase64String(sBase64));
                    Image image = new Image(imageData);

                    cell = new Cell();
                    cell.SetBorder(Border.NO_BORDER);
                    cell.Add(image.SetAutoScale(true));
                    HeaderTable.AddCell(cell);
                }
            }

            //*********************** Datos de cliente ********************************
            Table tableCliente = new Table(UnitValue.CreatePercentArray(new float[] { 25, 25, 50 }));
            tableCliente.SetWidth(UnitValue.CreatePercentValue(100));
            tableCliente.SetBorder(Border.NO_BORDER);

            if (formato == FormatoImpresion.A)
            {
                tableCliente.AddCell(new Cell().SetBorder(Border.NO_BORDER)); //Celda primera (sin nada)            
                tableCliente.AddCell(new Cell().SetBorder(Border.NO_BORDER)); //Celda segunda (sin nada)            
            }
            else
            {
                //Dejar un espacio antes
                tableCliente.SetMarginTop(20);

                string url = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/img/logo.png";
                ImageData imageData = ImageDataFactory.Create(new Uri(url));
                Image image = new Image(imageData);

                cell = new Cell();
                cell.SetBorder(Border.NO_BORDER);
                cell.Add(image.SetAutoScale(true));
                tableCliente.AddCell(cell);

                tableCliente.AddCell(new Cell().SetBorder(Border.NO_BORDER)); //Celda segunda (sin nada)     
            }

            cell = new Cell();
            cell.SetBorder(Border.NO_BORDER);
           
            Paragraph p = new Paragraph()
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(IText7Helper.FONT_SIZE_NORMAL);

            //p.SetProperty(Property.LEADING, new Leading(Leading.MULTIPLIED, 1.0f));

            //Nombre
            p.Add(new Text($"{cliente.Nombre?.Trim()}").SetBold());
            p.Add(new Text("\n"));
            //Dirección
            p.Add(new Text($"{model.Direccion?.Trim()}"));
            p.Add(new Text("\n"));
            //CP + Población
            p.Add(new Text($"{model.CodigoPostal?.Trim()} {model.Poblacion?.Trim()}"));
            p.Add(new Text("\n"));
            //Provincia
            p.Add(new Text($"{model.Provincia?.Trim()}"));
            p.Add(new Text("\n"));
            //CIF
            p.Add(new Text($"{cliente.CIF?.Trim()}"));

            cell.Add(p);

            tableCliente.AddCell(cell);
            cell = new Cell();
            cell.SetBorder(Border.NO_BORDER);
            cell.Add(tableCliente);
            HeaderTable.AddCell(cell);


            //*********************** Datos de factura ********************************
            Table tableDatosFactura = new Table(UnitValue.CreatePercentArray(new float[] { 75, 25 }));
            tableDatosFactura.SetWidth(UnitValue.CreatePercentValue(100));
            tableDatosFactura.SetBorder(Border.NO_BORDER);

            cell = new Cell();
            p = new Paragraph();
            p.Add(new Text($"Factura Nº: ").SetBold());
            p.Add(new Text($"{model.CodigoDisplay}     "));
            p.Add(new Text($"Evento: ").SetBold());

            if (formato == FormatoImpresion.A)
                p.Add(new Text($"{model.CodigoEvento}"));
            else
                p.Add(new Text($"{model.CodigoEvento} {model.NombreEvento}"));

            cell.Add(p);
            cell.SetBorder(Border.NO_BORDER);
            tableDatosFactura.AddCell(cell);

            cell = new Cell();
            p = new Paragraph();
            p.Add(new Text($"Fecha: ").SetBold());
            p.Add(new Text($"{model.Fecha.ToShortDateString()}"));
            cell.Add(p);
            cell.SetBorder(Border.NO_BORDER);
            cell.SetTextAlignment(TextAlignment.RIGHT);
            tableDatosFactura.AddCell(cell);

            if (!string.IsNullOrWhiteSpace(model.Observaciones))
            {
                cell = new Cell();
                cell.Add(new Paragraph($"{model.Observaciones}").SetFontSize(IText7Helper.FONT_SIZE_SM).SetFontColor(ColorConstants.GRAY));
                cell.SetBorder(Border.NO_BORDER);
                tableDatosFactura.AddCell(cell);
            }

            cell = new Cell();            
            cell.SetBorderLeft(Border.NO_BORDER);
            cell.SetBorderRight(Border.NO_BORDER);
            cell.SetBorderBottom(Border.NO_BORDER);
            cell.Add(tableDatosFactura);
            HeaderTable.AddCell(cell);
        }

        private void InitFooter()
        {
            Cell cell;

            FooterTable = new Table(1).UseAllAvailableWidth();
            FooterTable.SetFont(IText7Helper.GetCalibriFont());
            FooterTable.SetBorder(Border.NO_BORDER);
            FooterTable.SetMargins(0, IText7Helper.DEFAULT_MARGIN, 0, IText7Helper.DEFAULT_MARGIN);

            //Texto legal
            if (empresa.NOMBRE.Trim().ToLower().Contains("ferpuser"))
            {
                string TextoLegal = "En cumplimiento de lo previsto en la Ley Orgánica 15/1999, de 13 de diciembre de Protección de Datos de " +
                "Caracter Personal, le comunicamos que sus datos constan en un fichero titularidad de " +
                "FERPUSER - SANICONGRESS necesario para la gestión contable y fiscal de la empresa. " +
                "Puede ejercer los derechos de acceso, rectificación, cancelación y oposición enviando una solicitud por " +
                "escrito al siguiente correo electrónico: lopd @ferpuser.com";

                cell = new Cell();
                cell.SetBorder(Border.NO_BORDER);
                cell.Add(new Paragraph(TextoLegal).SetFontSize(IText7Helper.FONT_SIZE_XS).SetTextAlignment(TextAlignment.JUSTIFIED));
                cell.SetTextAlignment(TextAlignment.JUSTIFIED);
                FooterTable.AddCell(cell);
            }

            //Imagen faldón
            if (formato == FormatoImpresion.A)
            {
                if (!string.IsNullOrWhiteSpace(evento.TailBase64))
                {
                    string sBase64 = evento.TailBase64.Substring(evento.TailBase64.LastIndexOf(',') + 1); //Hay que quitar la primera parte del string 
                    ImageData imageData = ImageDataFactory.Create(Convert.FromBase64String(sBase64));
                    Image image = new Image(imageData);

                    cell = new Cell();
                    cell.SetBorder(Border.NO_BORDER);
                    cell.Add(image.SetAutoScale(true));
                    FooterTable.AddCell(cell);
                }
            }

            //Pie de datos de empresa, registro mercantil, etc
            string Provincia = string.IsNullOrWhiteSpace(empresa.PROVINCIA) ? empresa.POBLACION.Trim() : empresa.PROVINCIA.Trim();
            string DatosEmpresa = $"Registro {datos_registro_empresa.REG_PUB.Trim()} {datos_registro_empresa.REG_MER.Trim()} - " +
                $"Tomo {datos_registro_empresa.TOMO.Trim()}, Libro {datos_registro_empresa.LIBRO.Trim()}, " +
                $"folio {datos_registro_empresa.FOLIO.Trim()}, Hoja {datos_registro_empresa.HOJA.Trim()} inscripción {datos_registro_empresa.SECCION.Trim()} - " +
                $"{empresa.NOMBRE.Trim()} - {empresa.DIRECCION.Trim()} - {empresa.CODPOS.Trim()} {Provincia} C.I.F. {empresa.CIF.Trim()}";
            cell = new Cell();
            cell.SetBorder(Border.NO_BORDER);
            cell.Add(new Paragraph(DatosEmpresa).SetFontSize(IText7Helper.FONT_SIZE_XS).SetTextAlignment(TextAlignment.CENTER).SetFontColor(ColorConstants.GRAY));
            cell.SetTextAlignment(TextAlignment.CENTER);
            FooterTable.AddCell(cell);
        }

    }

    public class IText7Helper
    {
        public const int FONT_SIZE_XS = 8;
        public const int FONT_SIZE_SM = 10;
        public const int FONT_SIZE_NORMAL = 11;

        public const int DEFAULT_MARGIN = 36;

        public static PdfFont GetCalibriFont()
        {
            string pathFonts = System.IO.Path.Combine("Fonts", "calibri.ttf");
            FontProgram fontProgram = FontProgramFactory.CreateFont(pathFonts);
            return PdfFontFactory.CreateFont(fontProgram, PdfEncodings.WINANSI);
        }
    }
}

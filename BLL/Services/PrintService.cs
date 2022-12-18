using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Services
{
    public class PrintService
    {
        public static string GetHtmlTheme(string titulo, string contenidoHtml)
        {            
            string styles = @"<style>
                body {
                    font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; 
                    font-size: 14pt;
                }
                .header {
                    margin-top: 20px;
                    text-align: left;
                    font-weight: bold;
                    line-height: 1px;
                    margin-bottom: 2em;
                }
                .w-100 {
                    width: 100% !important;
                }
                .font-weight-bold
                {
                    font-weight:bold !important;
                }
                .text-right {
                    text-align:right !important;
                }
                .text-nowrap
                {
                    white-space: nowrap !important;
                }
            </style>";            

            var html = string.Format(@"<html>
                    <head>
                        <meta charset='UTF-8'>
                        {0}
                    </head>
                    <body>
                        <div class='header'>#TITULO</div>
                        <div>#CONTENIDO</div>
                    </body>
                    </html>", styles);

            html = html.Replace("#TITULO", titulo);
            html = html.Replace("#CONTENIDO", contenidoHtml);
            return html;
        }       

        public static byte[] GetBytes(string html)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Blink;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 40;
            converter.Options.MarginRight = 40;
            converter.Options.MarginTop = 30;
            converter.Options.MarginBottom = 30;
            converter.Options.PageBreaksEnhancedAlgorithm = true;
            PdfDocument doc = converter.ConvertHtmlString(html);
            //for (int i = 1; i < doc.Pages.Count; i++)
            //{
            //    doc.RemovePageAt(i);
            //}
            MemoryStream pdfStream = new MemoryStream();
            doc.Save(pdfStream);
            var array = pdfStream.ToArray();
            return array;
        }

        public static byte[] GetBytesLandscape(string html)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Blink;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
            converter.Options.MarginLeft = 40;
            converter.Options.MarginRight = 40;
            converter.Options.MarginTop = 30;
            converter.Options.MarginBottom = 30;
            converter.Options.PageBreaksEnhancedAlgorithm = true;
            PdfDocument doc = converter.ConvertHtmlString(html);
            for (int i = 1; i < doc.Pages.Count; i++)
            {
                doc.RemovePageAt(i);
            }
            MemoryStream pdfStream = new MemoryStream();
            doc.Save(pdfStream);
            var array = pdfStream.ToArray();
            return array;
        }

        public static byte[] GetBytes(string html, string html_footer, bool mostrar_paginas = false, int footerHeight = 130)
        {            
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.RenderingEngine = RenderingEngine.Blink;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 40;
            converter.Options.MarginRight = 40;
            converter.Options.MarginTop = 30;
            converter.Options.MarginBottom = 30;
            converter.Options.PageBreaksEnhancedAlgorithm = true;
            converter.Options.DisplayFooter = true;
            converter.Footer.Height = footerHeight;
         
            PdfHtmlSection footerHtml = new PdfHtmlSection(html_footer, string.Empty);
            //footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footerHtml);

            if (mostrar_paginas)
            {
                PdfTextSection text = new PdfTextSection(0, 10,
                    "Pág.: {page_number} de {total_pages}  ",
                    new System.Drawing.Font("Arial", 8));
                text.HorizontalAlign = PdfTextHorizontalAlign.Right;
                converter.Footer.Add(text);
            }

            PdfDocument doc = converter.ConvertHtmlString(html);
            MemoryStream pdfStream = new MemoryStream();
            doc.Save(pdfStream);
            var array = pdfStream.ToArray();
            return array;
        }
    }
}

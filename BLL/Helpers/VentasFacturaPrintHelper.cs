using Ferpuser.BLL.Services;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ferpuser.BLL.Helpers
{
    public class VentasFacturaPrintHelper : VentasPrintHelper
    {
        private VentaFactura model { get; set; }

        public VentasFacturaPrintHelper(
            ApplicationDbContext dbContext, 
            SageContext sageContext, 
            IHttpContextAccessor httpContextAccessor) : base(dbContext, sageContext, httpContextAccessor)
        {
        }

        public byte[] GetFactura(int Id, FormatoImpresion formato)
        {
            string html, html_footer, template;
            _formato = formato;

            model = _dbContext.VentaFacturas.Include(f => f.FacturaLineas).SingleOrDefault(f => f.Id == Id);
            if (model == null)
                return null;

            model.Calcular();
            evento = _dbContext.Congresses.AsNoTracking().FirstOrDefault(f => f.Number.ToString() == model.CodigoEvento && !f.Deleted.HasValue);
            cliente = _sageContext.Clientes.AsNoTracking().FirstOrDefault(f => f.Codigo == model.CodigoCliente);

            html = string.Empty;
            template = "VentaFacturaFormatoBD.html";
            if (_formato == FormatoImpresion.A)
                template = "VentaFacturaFormatoA.html";
            else if (_formato == FormatoImpresion.C)
                template = "VentaFacturaFormatoC.html";

            using (StreamReader reader = new StreamReader(System.IO.Path.Combine("HtmlTemplates", template)))
            {
                html = reader.ReadToEnd();
            }

            html = ReplaceDataEmpresa(html);
            html = ReplaceDataCliente(html);
            html = ReplaceDataEvento(html);
            html = ReplaceDataGeneral(html);

            html = html.Replace("#CLIENTE_DIRECCION#", model.Direccion);
            html = html.Replace("#CLIENTE_POBLACION#", $"{model.CodigoPostal + " "}{model.Poblacion}");
            html = html.Replace("#CLIENTE_PROVINCIA#", model.Provincia);

            html = html.Replace("#ID#", model.CodigoDisplay);
            html = html.Replace("#CODIGO_EVENTO#", model.CodigoEvento);
            html = html.Replace("#NOMBRE_EVENTO#", model.NombreEvento);
            html = html.Replace("#FECHA#", $"{model.Fecha.ToShortDateString()}");
            html = html.Replace("#OBSERVACIONES#", model.Observaciones?.Replace("\r\n", "<br/>"));

            ////NOTAS_FACTURA => Notas iva exento y inversión sujeto pasivo •	Se pueden dar los 2 avisos al mismo tiempo
            //List<string> notas_factura = new List<string>();
            //if (model.FacturaLineas.Any(f => f.CodigoTipoIVA.Trim() == "07")) //Tiene que aparecer cuando el código del IVA sea el “07”, y siempre va a ser ese código
            //    notas_factura.Add("Operación de Inversión del Sujeto Pasivo de acuerdo al Art. 84 Uno 2º f de la Ley 37/1992 de 28 de diciembre sobre el Impuesto del Valor Añadido");
            //else if (model.FacturaLineas.Any(f => f.IVA_Porcentaje == 0)) //8/2/2022 => En el caso de que el porcentaje de iva sea 0% ya debo mostrar el primer mensaje ¿correcto? SI
            //    notas_factura.Add("Factura exenta de IVA según el artículo 20.1 de la Ley 37/1992 de 28 de diciembre sobre el Impuesto del Valor Añadido.");

            //if (notas_factura.Any())
            //    html = html.Replace("#NOTAS_FACTURA#", string.Join("<br/>", notas_factura));
            //else
            //    html = html.Replace("#NOTAS_FACTURA#", string.Empty);

            string rows = string.Empty;
            if (model.FacturaLineas == null)
                model.FacturaLineas = new List<VentaFacturaLinea>();

            foreach (var item in model.FacturaLineas.OrderBy(f => f.Orden))
            {
                string tdTieneTiempo = string.Empty;
                if (_formato == FormatoImpresion.C)
                    tdTieneTiempo = $"<td class='text-right'>{item.Tiempo}</td>";

                rows += $"<tr>" +
                    $"<td>{item.NombreArticulo}</td>" +
                    $"<td class='text-right'>{item.Unidades}</td>" +
                    tdTieneTiempo +
                    $"<td class='text-right'>{item.BaseImponiblePrecioUnitario.ToString("N2")}</td>" +
                    $"<td class='text-right'>{item.Descuento} %</td>" +
                    $"<td class='text-right'>{item.IVA_Porcentaje} %</td>" +
                    $"<td class='text-right'>{item.BaseImponibleTotal.ToString("N2")}</td></tr>";
                if (!string.IsNullOrWhiteSpace(item.ObservacionesFacturaLinea))
                {
                    var sObservacionesLinea = item.ObservacionesFacturaLinea.Replace("\r\n", "<br/>");
                    rows += $"<tr><td>{sObservacionesLinea}</td></tr>";
                }
                if (item.DescripcionAmpliada && !string.IsNullOrWhiteSpace(item.TextoDescripcionAmpliada))
                {
                    var sTextoDescripcionAmpliada = item.TextoDescripcionAmpliada.Replace("\r\n", "<br/>");
                    rows += $"<tr><td style='padding-left:10px;'><i>{sTextoDescripcionAmpliada}</i></td></tr>";
                }
            }

            if (_formato == FormatoImpresion.D)
            {
                int numero_columnas = 6;
                rows += $"<tr><td colspan='{numero_columnas}'>&nbsp;</td></tr>";
                rows += $"<tr><td colspan='{numero_columnas - 1}'>CERTIFICADO A ORIGEN DE MES</td><td class='text-right'>{model.FacturaLineas.Sum(f => f.BaseImponibleTotal).ToString("N2")}</td></tr>";
                rows += $"<tr>" +
                    $"<td>A DEDUCIR CERTIFICACIÓN ANTERIOR</td>" +
                    $"<td class='text-right'>-1,00</td>" +
                    $"<td class='text-right'>{model.OrigenImporte.Value.ToString("N2")}</td>" +
                    $"<td class='text-right'>0,00%</td>" +
                    $"<td class='text-right'>0,00%</td>" +
                    $"<td class='text-right'>-{model.OrigenImporte.Value.ToString("N2")}</td>" +
                    $"</tr>";
            }

            html = html.Replace("#ROWS#", rows);

            //DESGLOSE TOTALES
            string sDesglose = string.Empty;
            var ivas = model.FacturaLineas.Select(f => f.IVA_Porcentaje).Distinct().OrderBy(f => f);

            //En este total 
            //decimal dTotalSinRentencion = (model.Total - model.TotalRetencion ?? 0);
            string totalAntesRetencion = model.TotalAntesRetencion?.ToString("C");

            foreach (var iva in ivas)
            {
                var totalBI = model.FacturaLineas.Where(f => f.IVA_Porcentaje == iva).Sum(f => f.BaseImponibleTotal);
                if (_formato == FormatoImpresion.D && iva == 0)
                    totalBI = totalBI - model.OrigenImporte.Value;

                var totalIVA = model.FacturaLineas.Where(f => f.IVA_Porcentaje == iva).Sum(f => f.ImporteIVA);
                var totalLinea = totalBI + totalIVA;

                string sContentFila =
                    $"<td class='text-right'>{totalBI.ToString("N2")}</td>" +
                    $"<td class='text-right'>{iva} %</td>" +
                    $"<td class='text-right'>{totalIVA.ToString("N2")}</td>";

                if (!string.IsNullOrWhiteSpace(totalAntesRetencion)) //El total solo se muestra en la primera fila, por eso se vacía dentro de este if
                {
                    sContentFila += $"<th class='text-center' rowspan='{ivas.Count()}' style='border-left:solid 1px black'><b>{totalAntesRetencion}</b></td>";
                    totalAntesRetencion = string.Empty;
                }

                sDesglose += $"<tr>{sContentFila}</tr>";
            }
            html = html.Replace("#TOTAL_DESGLOSE#", sDesglose);

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

                string total_desglose_retencion =
                    "<div class='w-100' style='margin-top:10px'>" +
                    "<table class='text-right' style='width: 55%; margin-left: 45%;'>" +
                    "<tr>" +
                    $"<td class='text-center'><u>Ret. {tipo}/{modo} ({porcentaje_retencion})</u></td>" +
                    $"<td class='text-center'><u>TOTAL NETO</u></td>" +
                    "</tr>" +
                    "<tbody>" +
                    "</tbody>" +
                    "<tr>" +
                    $"<td class='text-center'>{total_retencion}</td>" +
                    $"<td class='text-center'>{model.Total.ToString("N2")}</td>" +
                    "</tr>" +
                    "</table>" +
                    "</div>";

                html = html.Replace("#TOTAL_DESGLOSE_RETENCION#", total_desglose_retencion);
            }
            else
            {
                html = html.Replace("#TOTAL_DESGLOSE_RETENCION#", string.Empty);
            }

            html_footer = GetHtmlPie();

            int footer_height = 100;
            if (formato == FormatoImpresion.A)
                footer_height = 150;

            return PrintService.GetBytes(html, html_footer, false, footer_height);
        }

        protected override string GetHtmlPie(bool esfactura = true)
        {
            string html = base.GetHtmlPie(esfactura);

            //NOTAS_FACTURA => Notas iva exento y inversión sujeto pasivo •	Se pueden dar los 2 avisos al mismo tiempo
            List<string> notas_factura = new List<string>();
            if (model.FacturaLineas.Any(f => f.CodigoTipoIVA.Trim() == "07")) //Tiene que aparecer cuando el código del IVA sea el “07”, y siempre va a ser ese código
                notas_factura.Add("Operación de Inversión del Sujeto Pasivo de acuerdo al Art. 84 Uno 2º f de la Ley 37/1992 de 28 de diciembre sobre el Impuesto del Valor Añadido");
            else if (model.FacturaLineas.Any(f => f.IVA_Porcentaje == 0)) //8/2/2022 => En el caso de que el porcentaje de iva sea 0% ya debo mostrar el primer mensaje ¿correcto? SI
                notas_factura.Add("Factura exenta de IVA según el artículo 20.1 de la Ley 37/1992 de 28 de diciembre sobre el Impuesto del Valor Añadido.");

            if (notas_factura.Any())
                html = html.Replace("#NOTAS_FACTURA#", string.Join("<br/>", notas_factura));
            else
                html = html.Replace("#NOTAS_FACTURA#", string.Empty);
            
            return html;
        }
        
    }
}

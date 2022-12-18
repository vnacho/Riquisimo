using Ferpuser.BLL.Managers;
using Ferpuser.BLL.Services;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ferpuser.BLL.Helpers
{
    public class VentasPedidoPrintHelper : VentasPrintHelper
    {
        private VentaPedido model { get; set; }

        public VentasPedidoPrintHelper(
            ApplicationDbContext dbContext, 
            SageContext sageContext,
            ParametroManager parametroManager,
            IHttpContextAccessor httpContextAccessor) : base(dbContext, sageContext, parametroManager, httpContextAccessor)
        {
        }

        public byte[] GetAlbaran(int Id, FormatoImpresion formato)
        {
            string html, html_footer, template;

            _formato = formato;
            model = _dbContext.VentaPedidos.Include(f => f.PedidoLineas).SingleOrDefault(f => f.Id == Id);
            if (model == null)
                return null;

            model.Calcular();
            evento = _dbContext.Congresses.AsNoTracking().FirstOrDefault(f => f.Number.ToString() == model.CodigoEvento && !f.Deleted.HasValue);
            cliente = _sageContext.Clientes.AsNoTracking().FirstOrDefault(f => f.Codigo == model.CodigoCliente);
            
            html = string.Empty;
            template = "VentaPedidoFormatoB.html";
            if (_formato == FormatoImpresion.A)
                template = "VentaPedidoFormatoA.html";
            else if (_formato == FormatoImpresion.C)
                template = "VentaPedidoFormatoC.html";

            using (StreamReader reader = new StreamReader(Path.Combine("HtmlTemplates", template)))
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

            if (model.TipoDocumentoVenta == null)
                model.TipoDocumentoVenta = _dbContext.TiposDocumentoVenta.AsNoTracking().Single(f => f.Id == model.TipoDocumentoVentaId);
            html = html.Replace("#TIPO_DOCUMENTO_VENTA#", model.TipoDocumentoVenta.Descripcion);

            //NOTAS_FACTURA => Notas iva exento y inversión sujeto pasivo •	Se pueden dar los 2 avisos al mismo tiempo
            List<string> notas_factura = new List<string>();
            if (model.PedidoLineas.Any(f => f.CodigoTipoIVA.Trim() == "07")) //Tiene que aparecer cuando el código del IVA sea el “07”, y siempre va a ser ese código
                notas_factura.Add("Operación de Inversión del Sujeto Pasivo de acuerdo al Art. 84 Uno 2º f de la Ley 37/1992 de 28 de diciembre sobre el Impuesto del Valor Añadido");
            else if (model.PedidoLineas.Any(f => f.IVA_Porcentaje == 0)) //8/2/2022 => En el caso de que el porcentaje de iva sea 0% ya debo mostrar el primer mensaje ¿correcto? SI
                notas_factura.Add("Factura exenta de IVA según el artículo 20.1 de la Ley 37/1992 de 28 de diciembre sobre el Impuesto del Valor Añadido.");

            if (notas_factura.Any())
                html = html.Replace("#NOTAS_FACTURA#", string.Join("<br/>", notas_factura));
            else
                html = html.Replace("#NOTAS_FACTURA#", string.Empty);

            string rows = string.Empty;
            if (model.PedidoLineas == null)
                model.PedidoLineas = new List<VentaPedidoLinea>();

            foreach (var item in model.PedidoLineas.OrderBy(f => f.Orden))
            {
                string tdTieneTiempo = string.Empty;
                if (_formato == FormatoImpresion.C)
                    tdTieneTiempo = $"<td class='text-right'>{item.Tiempo}</td>";
                   
                rows += $"<tr><td>{item.NombreArticulo}</td>" +
                    $"<td class='text-right'>{item.Unidades}</td>" +
                    tdTieneTiempo +
                    $"<td class='text-right'>{item.PrecioUnitario.ToString("N2")}</td>" +
                    $"<td class='text-right'>{item.Descuento} %</td>" +
                    $"<td class='text-right'>{item.IVA_Porcentaje} %</td>" +
                    $"<td class='text-right'>{item.TotalPedidoLinea.ToString("N2")}</td></tr>";
                if (!string.IsNullOrWhiteSpace(item.ObservacionesPedidoLinea))
                {
                    var sObservacionesLinea = item.ObservacionesPedidoLinea.Replace("\r\n", "<br/>");
                    rows += $"<tr><td>{sObservacionesLinea}</td></tr>";
                }
                if (item.DescripcionAmpliada && !string.IsNullOrWhiteSpace(item.TextoDescripcionAmpliada))
                {
                    var sTextoDescripcionAmpliada = item.TextoDescripcionAmpliada.Replace("\r\n", "<br/>");
                    rows += $"<tr><td style='padding-left:10px;'><i>{sTextoDescripcionAmpliada}</i></td></tr>";
                }
            }
            html = html.Replace("#ROWS#", rows);

            //DESGLOSE TOTALES
            string sDesglose = string.Empty;
            var ivas = model.PedidoLineas.Select(f => f.IVA_Porcentaje).Distinct().OrderBy(f => f);
            string total = model.Total.ToString("C");
            foreach (var iva in ivas)
            {
                var totalBI = model.PedidoLineas.Where(f => f.IVA_Porcentaje == iva).Sum(f => f.TotalPedidoLinea);
                var totalIVA = model.PedidoLineas.Where(f => f.IVA_Porcentaje == iva).Sum(f => f.ImporteIVA);

                string sContentFila =
                    $"<td class='text-right'>{totalBI.ToString("N2")}</td>" +
                    $"<td class='text-right'>{iva} %</td>" +
                    $"<td class='text-right'>{totalIVA.ToString("N2")}</td>";

                if (!string.IsNullOrWhiteSpace(total))
                {
                    sContentFila += $"<th class='text-center' rowspan='{ivas.Count()}' style='border-left:solid 1px black'><b>{total}</b></td>";
                    total = string.Empty;
                }

                sDesglose += $"<tr>{sContentFila}</tr>";
            }
            html = html.Replace("#TOTAL_DESGLOSE#", sDesglose);
            html_footer = GetHtmlPie();

            int footer_height = 100;
            if (formato == FormatoImpresion.A)
                footer_height = 150;

            return PrintService.GetBytes(html, html_footer, false, footer_height);
        }
    }
}

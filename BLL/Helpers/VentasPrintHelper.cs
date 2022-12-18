using Ferpuser.BLL.Managers;
using Ferpuser.BLL.Services;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Dtos;
using Ferpuser.Models.Enums;
using Ferpuser.Models.Sage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ferpuser.BLL.Helpers
{
    public class VentasPrintHelper
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly SageContext _sageContext;
        protected readonly ParametroManager parametroManager;
        protected FormatoImpresion _formato;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected empresa empresa { get; set; }
        protected modconfi datos_registro_empresa { get; set; }
        protected Congress evento { get; set; }
        protected Clientes cliente { get; set; }  
        protected DatosEmpresaDto datos_empresa { get; set; }

        public VentasPrintHelper(
            ApplicationDbContext dbContext, 
            SageContext sageContext,
            ParametroManager parametroManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _sageContext = sageContext;
            _httpContextAccessor = httpContextAccessor;
            this.parametroManager = parametroManager;

            empresa = _sageContext.empresa.Find(Consts.CODIGO_EMPRESA);
            datos_registro_empresa = _sageContext.modconfi.Find(Consts.CODIGO_EMPRESA);

            datos_empresa = parametroManager.GetDatosEmpresa();
        }

        //protected string GetImagenCabeceraEvento(string CodigoEvento)
        //{
        //    var congress = _dbContext.Congresses.AsNoTracking().FirstOrDefault(f => f.Number.ToString() == CodigoEvento);
        //    return congress.TailBase64;
        //}

        //protected string ReplaceDataEmpresa(string html)
        //{   
        //    html = html.Replace("#EMPRESA_NOMBRE#", empresa.NOMBRE);
        //    html = html.Replace("#EMPRESA_DIRECCION#", empresa.DIRECCION);
        //    html = html.Replace("#EMPRESA_POBLACION#", $"{empresa.CODPOS + " "}{empresa.POBLACION}");
        //    html = html.Replace("#EMPRESA_PROVINCIA#", string.IsNullOrWhiteSpace(empresa.PROVINCIA) ? empresa.POBLACION : empresa.PROVINCIA);
        //    html = html.Replace("#EMPRESA_CIF#", empresa.CIF);
        //    html = html.Replace("#EMPRESA_CP#", empresa.CODPOS);

        //    html = html.Replace("#REG_PUB#", datos_registro_empresa.REG_PUB);
        //    html = html.Replace("#REG_MER#", datos_registro_empresa.REG_MER);
        //    html = html.Replace("#TOMO#", datos_registro_empresa.TOMO);
        //    html = html.Replace("#LIBRO#", datos_registro_empresa.LIBRO);
        //    html = html.Replace("#FOLIO#", datos_registro_empresa.FOLIO);
        //    html = html.Replace("#HOJA#", datos_registro_empresa.HOJA);
        //    html = html.Replace("#SECCION#", datos_registro_empresa.SECCION);

        //    string direccion_completa = $"{empresa.NOMBRE} - {empresa.DIRECCION} - {empresa.CODPOS + " "}{empresa.POBLACION} C.I.F. {empresa.CIF}";
        //    html = html.Replace("#DIRECCION_EMPRESA#", direccion_completa);

        //    return html;
        //}

        protected string ReplaceDataEmpresa(string html)
        {
            html = html.Replace("#EMPRESA_NOMBRE#", datos_empresa.Nombre);
            html = html.Replace("#EMPRESA_DIRECCION#", datos_empresa.Direccion);
            html = html.Replace("#EMPRESA_POBLACION#", $"{datos_empresa.CodigoPostal + " "}{datos_empresa.Poblacion}");
            html = html.Replace("#EMPRESA_PROVINCIA#", string.IsNullOrWhiteSpace(datos_empresa.Provincia) ? datos_empresa.Poblacion : datos_empresa.Provincia);
            html = html.Replace("#EMPRESA_CIF#", datos_empresa.NifCif);
            html = html.Replace("#EMPRESA_CP#", datos_empresa.CodigoPostal);

            html = html.Replace("#REG_PUB#", datos_registro_empresa.REG_PUB);
            html = html.Replace("#REG_MER#", datos_registro_empresa.REG_MER);
            html = html.Replace("#TOMO#", datos_registro_empresa.TOMO);
            html = html.Replace("#LIBRO#", datos_registro_empresa.LIBRO);
            html = html.Replace("#FOLIO#", datos_registro_empresa.FOLIO);
            html = html.Replace("#HOJA#", datos_registro_empresa.HOJA);
            html = html.Replace("#SECCION#", datos_registro_empresa.SECCION);

            string direccion_completa = $"{datos_empresa.Nombre} - {datos_empresa.Direccion} - {datos_empresa.CodigoPostal + " "}{datos_empresa.Poblacion} C.I.F. {datos_empresa.NifCif}";
            html = html.Replace("#DIRECCION_EMPRESA#", direccion_completa);

            return html;
        }

        protected string ReplaceDataCliente(string html)
        {   
            html = html.Replace("#CLIENTE_NOMBRE#", cliente.Nombre);
            html = html.Replace("#CLIENTE_CIF#", cliente.CIF);
            return html;
        }

        protected string ReplaceDataEvento(string html)
        {
            string sFormaPago = string.Empty;
            
            if (_formato == FormatoImpresion.A && evento != null)
            {
                sFormaPago = $"Transferencia a: {datos_empresa.Nombre} - {evento.Name}<br/>IBAN: {evento.IBAN} SWIFT: {evento.SwiftCode} ";

                //Indicar los datos del evento
                string datos_evento;
                if (evento.EndDate != evento.StartDate)
                    datos_evento = $"{evento.Name}. A celebrar en {evento.Place} del {evento.StartDate.ToShortDateString()} al {evento.EndDate.ToShortDateString()}.";
                else
                    datos_evento = $"{evento.Name}. A celebrar en {evento.Place} el día {evento.StartDate.ToShortDateString()}.";
                html = html.Replace("#DATOS_EVENTO#", datos_evento);
            }
            else
            {
                html = html.Replace("#DATOS_EVENTO#", string.Empty);
            }

            if (string.IsNullOrWhiteSpace(sFormaPago))
            {
                var forma_pago = _sageContext.FPag.Find(cliente.FPAG);
                if (forma_pago == null)
                    forma_pago = _sageContext.FPag.Find("00"); //Forma de pago por defecto 
                sFormaPago = $"<b>Forma pago:</b> {forma_pago.Nombre}";
            }

            html = html.Replace("#FORMA_PAGO#", sFormaPago);

            if (_formato == FormatoImpresion.A)
            {
                html = html.Replace("#HEADER#", evento.LogoBase64);
                html = html.Replace("#FOOTER#", evento.TailBase64);
            }

            return html;
        }

        protected string ReplaceDataGeneral(string html)
        {
            html = html.Replace("#BASEURL#", $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}");
            html = html.Replace("#URL_LOGO#", datos_empresa.Logo);
            //html = html.Replace("#URL_LOGO#", $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/img/logo.png");
            return html;
        }

        protected virtual string GetHtmlPie(bool esfactura = false)
        {
            string template = string.Empty;
            
            if (_formato == FormatoImpresion.A)
            {
                template = "PieFormatoA.html";
                if (datos_empresa.Nombre.Trim().ToLower().Contains("ferpuser"))
                    template = "PieFormatoAFerpuser.html";
            }
            else
            {
                template = "Pie.html";
                if (datos_empresa.Nombre.Trim().ToLower().Contains("ferpuser"))
                    template = "PieFerpuser.html";
            }

            string htmlpie;
            using (StreamReader reader = new StreamReader(Path.Combine("HtmlTemplates", template)))
            {
                htmlpie = reader.ReadToEnd();
            }

            htmlpie = ReplaceDataEmpresa(htmlpie);
            htmlpie = ReplaceDataEvento(htmlpie);            

            if (!esfactura)
                htmlpie = htmlpie.Replace("#NOTAS_FACTURA#", string.Empty);

            return htmlpie;
        }
    }
}

using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Sage;
using Ferpuser.Models.Transfer;
using Ferpuser.ViewFunctions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.RandomExtension;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Ferpuser.Controllers
{
    [Authorize]
    public class TransferController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComuContext;
        
        public TransferController(ApplicationDbContext context, SageContext sageContext, SageComuContext sageComuContext)
        {
            _context = context;
            _sageContext = sageContext;
            _sageComuContext = sageComuContext;
        }
        
        public async Task<IActionResult> ClientToSage(Guid? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var client = _context.Clients.Include(c => c.Locations).FirstOrDefault(c => c.Deleted == null && c.SageCode == null && c.Id.Equals(id));
            if (client == null)
                return NotFound();

            var codes = _sageContext.Clientes.Where(c => c.Codigo.StartsWith("4300")).OrderBy(c => c.Codigo).Select(c => int.Parse(c.Codigo));
            var codigo = Enumerable.Range(430000001, 430099999).Except(codes).First();
            //var codigo = int.Parse().Max(c => c.Codigo)) + 1;            

            string sAddress = string.Empty;
            string sZipCode = string.Empty;
            string sCity = string.Empty;
            string sProvince = string.Empty;
            string sCountry = string.Empty;

            var address = client.Locations.OrderBy(l => l.Created).FirstOrDefault();

            if (address == null)
            {
                var registration = _context.Registrations.Include(f => f.Registrant.Location).FirstOrDefault(f => f.ClientId == id);
                if (registration != null && registration.Registrant != null && registration.Registrant.Location != null)
                {
                    sAddress = registration.Registrant.Location.Address;
                    sZipCode = registration.Registrant.Location.ZipCode;
                    sCity = registration.Registrant.Location.City;
                    sProvince = registration.Registrant.Location.Province;
                    sCountry = registration.Registrant.Location.Country;
                }
            }
            else
            {
                sAddress = address.Address;
                sZipCode = address.ZipCode;
                sCity = address.City;
                sProvince = address.Province;
                sCountry = address.Country;
            }

            string cod_country = string.Empty;
            if (sCountry.Trim().ToLower().Equals("españa"))
                cod_country = "034";

            var divisa = "000";
            try
            {
                if (string.IsNullOrWhiteSpace(client.SageCode) && _sageContext.Clientes.Find(client.SageCode) == null)
                {
                    FormattableString query = $@"INSERT INTO [dbo].[clientes]([CODIGO],[CIF],[NOMBRE],[DIRECCION],[CODPOST],[POBLACION],[PROVINCIA],[PAIS],[EMAIL],[EMAIL_F],[F_ALTA],[IDIOMA],[TARIFA]) 
                                                     VALUES('{codigo}', '{client.NIF}', '{client.BusinessName}', '{sAddress}', '{sZipCode}', '{sCity}', '{sProvince}', '{cod_country}', '{client.Email}', '{client.Email2}', CAST('{DateTime.Now}' AS smalldatetime), '{divisa}', 'TD')";
                    await _sageContext.Database.ExecuteSqlRawAsync(query.ToString());
                }

                int linea = 1;
                var lines = _sageContext.Env_Cli.Where(e => e.Cliente.Equals(codigo));

                FormattableString cuentasQuery = $@"INSERT INTO [dbo].[cuentas]([CODIGO],[NOMBRE],[CIF],[DIVISA],[VISTA]) 
                                                     VALUES('{codigo}', '{client.BusinessName}', '{client.NIF}', '{divisa}', 1)";
                await _sageContext.Database.ExecuteSqlRawAsync(cuentasQuery.ToString());

                if (lines.Count() > 0)                
                    linea = lines.Max(e => e.Linea) + 1;

                foreach (var l in client.Locations.Where(l => l.Deleted == null && l.SageLine == null).ToList())
                {
                    var phone = l.Phone;
                    var phone2 = l.Phone2 ?? "";
                    var lCountry = cod_country;
                    FormattableString locationQuery = $@"INSERT INTO [dbo].[env_cli]([LINEA],[CLIENTE],[DIRECCION],[CODPOS],[POBLACION],[PROVINCIA],[PAIS],[TELEFONO],[FAX]) 
                                                 VALUES({linea}, '{codigo}', '{l.Address}', '{l.ZipCode}', '{l.City}', '{l.Province}', '{lCountry}', '{phone}', '{phone2}')";
                    await _sageContext.Database.ExecuteSqlRawAsync(locationQuery.ToString());

                    l.SageLine = linea;
                    l.SageClient = codigo.ToString();
                    _context.Update(l);
                    _context.SaveChanges();

                    linea++;
                }
            }
            catch (Exception)
            {
                throw;
            }

            client.SageCode = codigo.ToString();
            client.Modified = DateTime.Now;

            _context.Update(client);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public CostCenterProduct GetCostCenterProduct(Guid id)
        {
            CostCenterProduct reg;

            reg = _context.Expenses
                .Include(r => r.Client)
                .Include(r => r.BillingLocation)
                .Include(r => r.Account)
                .Include(r => r.Congress).FirstOrDefault(r => r.Id.Equals(id));
            if (reg != null)
            {
                return reg;
            }
            reg = _context.Accommodations
                .Include(r => r.Client)
                .Include(r => r.BillingLocation)
                .Include(r => r.Account)
                .Include(r => r.Congress).FirstOrDefault(r => r.Id.Equals(id));
            if (reg != null)
            {
                return reg;
            }
            reg = _context.Registrations
                .Include(r => r.Client)
                .Include(r => r.BillingLocation)
                .Include(r => r.Account)
                .Include(r => r.Congress).FirstOrDefault(r => r.Id.Equals(id));
            if (reg != null)
            {
                return reg;
            }
            return null;
        }
        public async Task<IActionResult> CostCenterProductToSage(Guid? id, DateTime? date = null)
        {
            if (!id.HasValue)
                return BadRequest();

            if (!date.HasValue)
                date = DateTime.Now;
            
            CostCenterProduct reg = null;

            try
            {               
                reg = GetCostCenterProduct(id.Value);
                if (reg == null)
                    return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest("ERROR FINDING CostCenterProduct: " + e.ToString());
            }

            var formatter = new QueryFormatter(reg, _sageContext, _context, date.Value);

            try
            {
                using var transaction = await _sageContext.Database.BeginTransactionAsync();

                _sageContext.Database.ExecuteSqlRaw(formatter.HeaderQuery);
                int line = 0;
                var detailQuery = formatter.DetailQuery(++line);
                _sageContext.Database.ExecuteSqlRaw(detailQuery);

                //_sageContext.Database.ExecuteSqlInterpolated(formatter.FormattableDetailQuery(++line));

                for (int i = 0; i < formatter.products.Count(); i++)
                {
                    var productDetailQuery = formatter.DetailQuery(++line, i);
                    _sageContext.Database.ExecuteSqlRaw(productDetailQuery);
                    if (!string.IsNullOrWhiteSpace(formatter.products[i].notes))
                    {
                        var productDetailNotesQuery = formatter.DetailNotesQuery(++line, i);
                        _sageContext.Database.ExecuteSqlRaw(productDetailNotesQuery);
                    }
                }
                if (formatter.hasHotel)
                {
                    _sageContext.Database.ExecuteSqlRaw(formatter.HotelQuery(++line));
                }
                if (reg.ShowCostCenterInfoOnInvoice)
                {
                    _sageContext.Database.ExecuteSqlRaw(formatter.CostCenterQuery(++line));
                    _sageContext.Database.ExecuteSqlRaw(formatter.CelebrationQuery(++line));
                }
                if (formatter.hasRegistrant)
                {
                    _sageContext.Database.ExecuteSqlRaw(formatter.RegistrantQuery(++line));
                }

                _sageContext.Database.ExecuteSqlRaw(formatter.UpdateSeriesQuery());

                reg.InvoiceNumber = formatter.factura;
                reg.Exported = true;
                reg.InvoiceDate = date;
                reg.DocumentTypeId = new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39");
                _context.Update(reg);
                _context.SaveChanges();

                //JIR 23/02/2021
                var previ = _sageComuContext.Previ_Cl.FirstOrDefault(p => p.Factura.Equals(reg.InvoiceNumber));
                if (previ != null)
                {
                    previ.FEC_OPER = date;
                    //_sageComuContext.Update(previ); Será necesaria esta línea? No creo ya que el tracking tiene que estar activado
                    _sageComuContext.SaveChanges();
                }

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, "Error al facturar un centro de coste.");
                return BadRequest("ERROR SAVING CHANGES: " + e.ToString());
            }
            try
            {
                if (reg is Expense)
                    await SendExpense(id);
                
                if (reg is Registration)                
                    await SendRegistration(id);
                
                if (reg is Accommodation)
                    await SendAccommodation(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
            return Ok();
        }
        private async Task SendAccommodation(Guid? id)
        {
            var registration = await _context.Accommodations
                .Include(r => r.Congress)
                .Include(r => r.Client)
                .Include(r => r.BillingLocation)
                .Include(r => r.DocumentType)
                .Include(r => r.Registrant)
                    .ThenInclude(t => t.Treatment)
                .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            decimal vat = _sageContext.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(registration.VATId)).IVA;

            var html = MailSender.MailSender.InvoiceMailAttachment(registration.Congress, registration, vat);
            var pdf = MailSender.MailSender.HtmlToPdf(html, registration.InvoiceDate, registration.InvoiceNumber);
        }
        private async Task SendRegistration(Guid? id)
        {
            var registration = await _context.Registrations
               .Include(r => r.Congress)
               .Include(r => r.Client)
               .Include(r => r.BillingLocation)
               .Include(r => r.DocumentType)
               .Include(r => r.Registrant)
                   .ThenInclude(t => t.Treatment)
               .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            decimal vat = _sageContext.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(registration.VATId)).IVA;

            var html = MailSender.MailSender.InvoiceMailAttachment(registration.Congress, registration, vat);
            var pdf = MailSender.MailSender.HtmlToPdf(html, registration.InvoiceDate, registration.InvoiceNumber);

        }
        private async Task SendExpense(Guid? id)
        {
            var expense = await _context.Expenses
                               .Include(r => r.Congress)
                               .Include(r => r.Client)
                               .Include(r => r.BillingLocation)
                               .Include(r => r.Products)
                               .Include(r => r.DocumentType)
                               .FirstOrDefaultAsync(r => r.Deleted == null && r.Id.Equals(id));

            decimal vat = _sageContext.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(expense.VATId)).IVA;

            var html = MailSender.MailSender.InvoiceMailAttachment(expense.Congress, expense, vat);
            string number = "";
            if (expense.DocumentType.IsInvoice())
            {
                number = expense.InvoiceNumber;
            }
            else
            {
                number = ViewHelpers.PadCongress(expense.Number);
            }
            var pdf = MailSender.MailSender.HtmlToPdf(html, expense.InvoiceDate, number);
        }

        public async Task<IActionResult> RegistrationToSage(Guid? id, DateTime? date = null)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            if (!date.HasValue)
            {
                date = DateTime.Now;
            }
            Registration reg = null;

            var codArticulo = "70100";
            var articulo = _sageContext.Articulo.Find(codArticulo);
            var description = "Inscripción al Evento";
            try
            {
                if (articulo != null)
                {
                    description = articulo.Nombre;
                }

                reg = _context.Registrations
                .Include(r => r.Client)
                .Include(r => r.Registrant)
                    .ThenInclude(rr => rr.Treatment)
                .Include(r => r.BillingLocation)
                .Include(r => r.Congress).FirstOrDefault(r => r.Id.Equals(id)/* && r.Deleted == null && r.Reviewed && !r.Exported && !string.IsNullOrWhiteSpace(r.Client.SageCode)*/);
                if (reg == null)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR FINDING REGISTRATION: " + e.ToString());
            }

            var empresa = "01";
            var serie = "I ";
            var nFactura = _sageContext.Serie.FirstOrDefault(s => s.Serie.Equals(serie) && s.Tipodoc == 7).Contador + 1;
            var factura = serie + "  " + nFactura;
            var vat = reg.VATId;
            var iva = _sageContext.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(reg.VATId)).IVA;
            reg.VAT = iva;
            var baseImp = (reg.Fee / (1 + (iva / 100))).ToString("F6").Replace(",", ".");
            var baseImp2d = Math.Round(reg.Fee / (1 + (iva / 100)), 2).ToString("F6").Replace(",", ".");
            var importe = reg.Fee.ToString("F6").Replace(",", ".");
            var importe2d = Math.Round(reg.Fee, 2).ToString("F6").Replace(",", ".");
            var cliente = reg.Client.SageCode;
            var registrant = reg.Registrant.Treatment.Name + " " + reg.Registrant.Name + " " + reg.Registrant.Surnames;
            var notes = "";
            if (reg.VATId.Equals("00"))
            {
                notes = "* Factura exenta de IVA según el artículo 20.1 de la ley 37/1992 de 28 de diciembre sobre el Impuesto del Valor Añadido";
            }

            try
            {
                using var transaction = await _sageContext.Database.BeginTransactionAsync();
                FormattableString query = $@"INSERT INTO [dbo].[c_albven]([EMPRESA],[NUMERO],[FACTURA],[CLIENTE],[IMPORTE],[IMPDIVISA],[TOTALDOC],[TOTALDIV],[FECHA],[FECHA_FAC],[FRADIRECTA],[LETRA],[DIVISA],[ALMACEN],[FPAG],[VENDEDOR],[BANC_CLI],[OBSERVACIO]) 
                                                 VALUES('{empresa}','{factura}', '{factura}', '{cliente}', {baseImp}, {baseImp}, {importe}, {importe}, CAST('{date.Value}' AS smalldatetime), CAST('{date.Value}' AS smalldatetime), 1,'{serie}', '000', '{reg.Congress.Number}', '00', '01','99', '{notes}')";
                _sageContext.Database.ExecuteSqlRaw(query.ToString());

                FormattableString detailQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[ARTICULO],[TIPO_IVA],[CLIENTE],[FECHA],[DEFINICION],[UNIDADES],[LINIA],[PRECIO],[IMPORTE],[PRECIOIVA],[IMPORTEIVA],[FAMILIA],[PRECIODIV],[IMPORTEDIV],[LETRA],[IMPDIVIVA],[PREDIVIVA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{codArticulo}', '{vat}', '{cliente}', CAST('{date.Value}' AS smalldatetime), '{description}', 1, 1, {baseImp}, {baseImp2d}, {importe}, {importe2d}, '{codArticulo}', {baseImp}, {baseImp2d}, '{serie}', {importe2d}, {importe}, '01')";
                _sageContext.Database.ExecuteSqlRaw(detailQuery.ToString());

                FormattableString congressQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date.Value}' AS smalldatetime), '{reg.Congress.Name}', 2, '{serie}', '01')";
                _sageContext.Database.ExecuteSqlRaw(congressQuery.ToString());

                var toCelebrate = "A celebrar en ";
                var celebrateLocation = reg.Congress.GetDayAndTimePrint();
                if (toCelebrate.Length + celebrateLocation.Length <= 50)
                {
                    toCelebrate += celebrateLocation;
                }
                else if (celebrateLocation.Length <= 50)
                {
                    toCelebrate = celebrateLocation;
                }
                else
                {
                    toCelebrate = celebrateLocation.Substring(0, 50);
                }
                FormattableString dateQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date.Value}' AS smalldatetime), '{toCelebrate}', 3, '{serie}', '01')";
                _sageContext.Database.ExecuteSqlRaw(dateQuery.ToString());


                FormattableString registrantQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date.Value}' AS smalldatetime), '{registrant}', 4, '{serie}', '01')";
                _sageContext.Database.ExecuteSqlRaw(registrantQuery.ToString());



                FormattableString seriesQuery = $@"UPDATE [dbo].[series] SET [CONTADOR] = {nFactura} WHERE [SERIE] = '{serie}' AND [TIPODOC] = 7";
                _sageContext.Database.ExecuteSqlRaw(seriesQuery.ToString());

                await transaction.CommitAsync();

            }
            catch (Exception e)
            {
                return BadRequest("ERROR SENDING REGISTRATION TO SAGE: " + e.ToString());
            }

            try
            {
                reg.InvoiceNumber = factura;
                reg.Exported = true;
                reg.InvoiceDate = date;
                _context.Update(reg);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest("ERROR SAVING CHANGES: " + e.ToString());
            }

            return Ok();
        }

        public async Task<IActionResult> AccommodationToSage(Guid? id, DateTime? date = null)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            if (!date.HasValue)
            {
                date = DateTime.Now;
            }
            Accommodation reg = null;
            var codArticulo = "70800";
            var articulo = _sageContext.Articulo.Find(codArticulo);
            var description = "Alojamiento para el Evento";
            try
            {

                if (articulo != null)
                {
                    description = articulo.Nombre;
                }

                reg = _context.Accommodations
                    .Include(r => r.Client)
                    .Include(r => r.Registrant)
                        .ThenInclude(rr => rr.Treatment)
                    .Include(r => r.BillingLocation)
                    .Include(r => r.Congress).FirstOrDefault(r => r.Id.Equals(id)/* && r.Deleted == null && r.Reviewed && !r.Exported && !string.IsNullOrWhiteSpace(r.Client.SageCode)*/);
                if (reg == null)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest("ERROR FINDING ACCOMMODATION: " + e.ToString());
            }

            var empresa = "01";
            var serie = "A ";
            var nFactura = _sageContext.Serie.FirstOrDefault(s => s.Serie.Equals(serie) && s.Tipodoc == 7).Contador + 1;
            var factura = serie + "  " + nFactura;
            var vat = reg.VATId;

            var iva = _sageContext.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(reg.VATId)).IVA;
            reg.VAT = iva;

            var baseImp = (reg.Fee / (1 + (iva / 100))).ToString("F6").Replace(",", ".");
            var baseImp2d = Math.Round(reg.Fee / (1 + (iva / 100)), 2).ToString("F6").Replace(",", ".");
            var importe = reg.Fee.ToString("F6").Replace(",", ".");
            var importe2d = Math.Round(reg.Fee, 2).ToString("F6").Replace(",", ".");

            var cliente = reg.Client.SageCode;
            var registrant = reg.Registrant.Treatment.Name + " " + reg.Registrant.Name + " " + reg.Registrant.Surnames;
            var hotel = reg.Hotel;
            var notes = "";
            if (reg.VATId.Equals("00"))
                notes = "* Factura exenta de IVA según el artículo 20.1 de la ley 37/1992 de 28 de diciembre sobre el Impuesto del Valor Añadido";
            
            try
            {
                using var transaction = await _sageContext.Database.BeginTransactionAsync();
                FormattableString query = $@"INSERT INTO [dbo].[c_albven]([EMPRESA],[NUMERO],[FACTURA],[CLIENTE],[IMPORTE],[IMPDIVISA],[TOTALDOC],[TOTALDIV],[FECHA],[FECHA_FAC],[FRADIRECTA],[LETRA],[DIVISA],[ALMACEN],[FPAG],[VENDEDOR],[BANC_CLI],[OBSERVACIO])
                                                 VALUES('{empresa}','{factura}', '{factura}', '{cliente}', {baseImp}, {baseImp}, {importe}, {importe}, CAST('{date.Value}' AS smalldatetime), CAST('{date.Value}' AS smalldatetime), 1,'{serie}', '000', '{reg.Congress.Number}', '00', '01', '99', '{notes}')";
                _sageContext.Database.ExecuteSqlRaw(query.ToString());

                FormattableString detailQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[ARTICULO],[TIPO_IVA],[CLIENTE],[FECHA],[DEFINICION],[UNIDADES],[LINIA],[PRECIO],[IMPORTE],[PRECIOIVA],[IMPORTEIVA],[FAMILIA],[PRECIODIV],[IMPORTEDIV],[LETRA],[IMPDIVIVA],[PREDIVIVA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{codArticulo}', '{vat}', '{cliente}', CAST('{date.Value}' AS smalldatetime), '{description}', 1, 1, {baseImp}, {baseImp2d}, {importe}, {importe2d}, '{codArticulo}', {baseImp}, {baseImp2d}, '{serie}', {importe2d}, {importe}, '01')";
                _sageContext.Database.ExecuteSqlRaw(detailQuery.ToString());


                FormattableString hotelQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date.Value}' AS smalldatetime), 'Hotel: {hotel}', 2, '{serie}', '01')";
                _sageContext.Database.ExecuteSqlRaw(hotelQuery.ToString());

                FormattableString registrantQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date.Value}' AS smalldatetime), '{registrant}', 3, '{serie}', '01')";
                _sageContext.Database.ExecuteSqlRaw(registrantQuery.ToString());

                FormattableString congressQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date.Value}' AS smalldatetime), '{reg.Congress.Name}', 4, '{serie}', '01')";
                _sageContext.Database.ExecuteSqlRaw(congressQuery.ToString());


                FormattableString dateQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date.Value}' AS smalldatetime), 'A celebrar en {reg.Congress.GetDayAndTimePrint()}', 5, '{serie}', '01')";
                _sageContext.Database.ExecuteSqlRaw(dateQuery.ToString());



                FormattableString seriesQuery = $"UPDATE [dbo].[series] SET [CONTADOR] = {nFactura} WHERE [SERIE] = '{serie}' AND [TIPODOC] = 7";
                _sageContext.Database.ExecuteSqlRaw(seriesQuery.ToString());

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                return BadRequest("ERROR SENDING ACCOMMODATION TO SAGE: " + e.ToString());
            }

            try
            {
                reg.InvoiceNumber = factura;
                reg.Exported = true;
                reg.InvoiceDate = date;
                _context.Update(reg);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest("ERROR SAVING CHANGES: " + e.ToString());
            }
            return Ok();
        }
        public async Task<IActionResult> ToSage(Guid? id, DateTime? date = null)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            if (_context.Congresses.Find(id.Value) == null)
            {
                return NotFound();
            }
            if (!date.HasValue)
            {
                date = DateTime.Now;
            }
            var codArticulo = "70100";
            var articulo = _sageContext.Articulo.Find(codArticulo);
            var description = "Inscripción al Evento";

            if (articulo != null)
            {
                description = articulo.Nombre;
            }


            foreach (var reg in _context.Accommodations.Include(r => r.Client).Include(r => r.Registrant).ThenInclude(rr => rr.Treatment).Include(r => r.BillingLocation).Include(r => r.Congress).Where(r => r.Deleted == null && r.CongressId.Equals(id) && r.Reviewed && !r.Exported && !string.IsNullOrWhiteSpace(r.Client.SageCode)).ToList())
            {
                var empresa = "01";
                var serie = "I ";
                var nFactura = _sageContext.Serie.FirstOrDefault(s => s.Serie.Equals(serie) && s.Tipodoc == 7).Contador + 1;
                var factura = serie + "  " + nFactura;
                var vat = reg.VATId;
                var iva = _sageContext.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(reg.VATId)).IVA;
                var baseImp = (reg.Fee / (1 + (iva / 100))).ToString("F6").Replace(",", ".");
                var importe = reg.Fee.ToString("F6").Replace(",", ".");
                var cliente = reg.Client.SageCode;
                var registrant = reg.Registrant.Treatment.Name + " " + reg.Registrant.Name + " " + reg.Registrant.Surnames;
                try
                {
                    using var transaction = await _sageContext.Database.BeginTransactionAsync();
                    FormattableString query = $@"INSERT INTO [dbo].[c_albven]([EMPRESA],[NUMERO],[FACTURA],[CLIENTE],[IMPORTE],[IMPDIVISA],[TOTALDOC],[TOTALDIV],[FECHA],[FECHA_FAC],[FRADIRECTA],[LETRA],[DIVISA],[ALMACEN],[FPAG],[VENDEDOR],[BANC_CLI]) 
                                                 VALUES('{empresa}','{factura}', '{factura}', '{cliente}', {baseImp}, {baseImp}, {importe}, {importe}, CAST('{date.Value}' AS smalldatetime),CAST('{date.Value}' AS smalldatetime), 1,'{serie}', '000', '{reg.Congress.Number}', '00', '01','99')";
                    _sageContext.Database.ExecuteSqlRaw(query.ToString());

                    FormattableString detailQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[ARTICULO],[TIPO_IVA],[CLIENTE],[FECHA],[DEFINICION],[UNIDADES],[LINIA],[PRECIO],[IMPORTE],[PRECIOIVA],[IMPORTEIVA],[FAMILIA],[PRECIODIV],[IMPORTEDIV],[LETRA],[IMPDIVIVA],[PREDIVIVA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{codArticulo}', '{vat}', '{cliente}', CAST('{date.Value}' AS smalldatetime), '{description}', 1, 1, {baseImp}, {baseImp}, {importe}, {importe}, '{codArticulo}', {baseImp}, {baseImp}, '{serie}', {importe}, {importe}, '01')";
                    _sageContext.Database.ExecuteSqlRaw(detailQuery.ToString());

                    FormattableString congressQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date.Value}' AS smalldatetime), '{reg.Congress.Name}', 2, '{serie}', '01')";
                    _sageContext.Database.ExecuteSqlRaw(congressQuery.ToString());


                    FormattableString dateQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date.Value}' AS smalldatetime), 'A celebrar en {reg.Congress.GetDayAndTimePrint()}', 3, '{serie}', '01')";
                    _sageContext.Database.ExecuteSqlRaw(dateQuery.ToString());


                    FormattableString registrantQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date.Value}' AS smalldatetime), 'Persona inscrita: {registrant}', 4, '{serie}', '01')";
                    _sageContext.Database.ExecuteSqlRaw(registrantQuery.ToString());

                    FormattableString seriesQuery = $@"UPDATE [dbo].[series] SET [CONTADOR] = {nFactura} WHERE [SERIE] = 'I ' AND [TIPODOC] = 7";
                    _sageContext.Database.ExecuteSqlRaw(seriesQuery.ToString());

                    await transaction.CommitAsync();

                    reg.InvoiceNumber = factura;
                    reg.Exported = true;

                    //Alojamientos Articulo 70800
                    _context.Update(reg);
                    _context.SaveChanges();
                }
                catch
                {
                    throw;
                }

            }

            return Ok();
        }
        public async Task<ValueTuple<BadRequestObjectResult, CostCenterProduct, string>> FindCostCenterProduct(Guid id)
        {
            var tuple = new ValueTuple<BadRequestObjectResult, CostCenterProduct, string>(null, null, "");
            try
            {
                tuple.Item2 = await _context.Registrations.Include(r => r.Client).Include(r => r.Congress).FirstOrDefaultAsync(r => r.Id.Equals(id));
                if (tuple.Item2 == null)
                {
                    tuple.Item2 = await _context.Accommodations.Include(r => r.Client).Include(r => r.Congress).FirstOrDefaultAsync(r => r.Id.Equals(id));
                }
                if (tuple.Item2 == null)
                {
                    tuple.Item2 = await _context.Expenses.Include(r => r.Client).Include(r => r.Congress).FirstOrDefaultAsync(r => r.Id.Equals(id));
                }
                if (tuple.Item2 == null)
                {
                    tuple.Item1 = BadRequest("ERROR FINDING ID IN DATABASE");
                }
                tuple.Item3 = tuple.Item2.Congress.SageAccount;
            }
            catch (Exception e)
            {
                tuple.Item1 = BadRequest("ERROR FINDING ID IN DATABASE: " + e.ToString());
            }
            return tuple;
        }
        public async Task<IActionResult> IsCollectedInSage(Guid? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var tuple = await FindCostCenterProduct(id.Value);
            if (tuple.Item1 != null)
                return tuple.Item1;

            var reg = tuple.Item2;
            var previ = _sageComuContext.Previ_Cl.FirstOrDefault(p => p.Factura.Equals(reg.InvoiceNumber));
            if (previ != null && previ.Cobro.HasValue)
                return Ok();
            else            
                return NotFound();            
        }
        public async Task<IActionResult> CollectBillToSage(Guid? id, DateTime? date = null)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            if (!date.HasValue)
            {
                date = DateTime.Now;
            }

            var tuple = await FindCostCenterProduct(id.Value);
            if (tuple.Item1 != null)
            {
                return tuple.Item1;
            }
            var reg = tuple.Item2;
            var cuenta = tuple.Item3;

            var previ = _sageComuContext.Previ_Cl.FirstOrDefault(p => p.Factura.Equals(reg.InvoiceNumber));
            if (previ == null)
            {
                return BadRequest("ERROR FINDING PREVI_CL");
            }

            var albven = _sageContext.C_Albven.FirstOrDefault(c => c.Numero == reg.InvoiceNumber);

            if (albven == null)
            {
                return BadRequest("ERROR FINDING THE INVOICE");
            }
            var random = new Random();
            var divisa = "000";
            var asi = RandomAsi(random);
            var asi2 = RandomAsi(random);
            previ.Banco = cuenta;
            previ.Asi = asi;
            previ.Cliente = reg.Client.SageCode;

            //previ.Factura = reg.InvoiceNumber;
            previ.Asiento = true;
            previ.Cobro = date;
            previ.Divisa = divisa;
            previ.FEC_OPER = reg.InvoiceDate; //25/01/2021 => programadorburgos.es se solicita que se actualice este campo con la fecha de factura

            //var sageClient = await _sageContext.Clientes.FirstOrDefaultAsync(c => c.Codigo.Equals(reg.Client.SageCode));


            var number = _sageContext.Asientos.Max(a => a.Numero) + 1;
            var sageGuid = Guid.NewGuid().ToString().ToUpper();
            var invoiceNumber = reg.InvoiceNumber;
            var ammount = albven.Totaldoc;
            var clientName = reg.Client.BusinessName;

            _sageComuContext.Update(previ);
            _sageComuContext.SaveChanges();

            var asiento2 = new Asientos
            {
                Empresa = "01",
                Numero = number,
                Asi = asi2,
                Guid = sageGuid,
                Definicion = "COBRO FRA. " + invoiceNumber + "/1",
                Linea = 2,
                Debe = 0,
                Haber = ammount,
                Debediv = 0,
                Haberdiv = ammount,
                Importediv = ammount,
                Archivo = "",
                Conciconcep = "",
                Cuenta = albven.Cliente.ToString(), //FIXME: find client account
                Divisa = divisa,
                Factura = "",
                Libro = "",
                Proveedor = "",
                Referencia = number.ToString(),
                Tipo = "0",
                Usuario = "",

                Fecha = date,
                Punteo = false,
                Vista = true,
                Cambio = 1,
                Arqueo = 0,
                Cierre89 = false,
                Fcreado = DateTime.Now,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Operacion = 0,
                Conciaut = false,
                Isv = false,
                Suplido = false,
                Dts = 0,
                Manual = true,
                Entradaman = false,
                Capture = false

            };
            var asiento1 = new Asientos
            {
                Empresa = "01",
                Numero = number,
                Asi = asi,
                Guid = sageGuid,
                Definicion = "COBRO FRA. " + invoiceNumber + "/1 " + clientName,
                Linea = 1,
                Debe = ammount,
                Haber = 0,
                Debediv = ammount,
                Haberdiv = 0,
                Importediv = ammount,
                Archivo = "",
                Conciconcep = "",
                Cuenta = cuenta,
                Divisa = divisa,
                Factura = "",
                Libro = "",
                Proveedor = "",
                Referencia = number.ToString(),
                Tipo = "0",
                Usuario = "",

                Fecha = date,
                Punteo = false,
                Vista = true,
                Cambio = 1,
                Arqueo = 0,
                Cierre89 = false,
                Fcreado = DateTime.Now,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Operacion = 0,
                Conciaut = false,
                Isv = false,
                Suplido = false,
                Dts = 0,
                Manual = true,
                Entradaman = false,
                Capture = false
            };
            _sageContext.Add(asiento2);
            _sageContext.Add(asiento1);
            bool saved = false;
            while (!saved)
            {
                try
                {
                    _sageContext.SaveChanges();
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Asientos)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
            }
            reg.Paid = true;
            reg.PaidDate = date;
            _context.Update(reg);
            _context.SaveChanges();
            return Ok();
        }

        private static string RandomAsi(Random random)
        {
            ulong range = 10000000000000000000;
            var asiS = random.NextULong(range).ToString();

            while (asiS.Length < 20)
            {
                asiS = random.NextUInt(10).ToString() + asiS;
            }
            return asiS;
        }

        public class QueryFormatterProduct
        {
            public string codArticulo;
            public string familia;
            public string description;

            public string vat;
            public decimal iva;

            public string precioIva;
            public string precioBase;
            public string importeIva;
            public string importeBase;
            public string notes;

            public double units;
        }
        public class QueryFormatter
        {
            string codArticulo;
            string familia;
            string description;
            string empresa = "01";
            string serie;

            decimal? nFactura;
            public string factura;
            string vat;
            decimal iva;

            string precioIva;
            string precioBase;
            string importeIva;
            string importeBase;
            string cliente;
            string registrant;
            string vendedor;
            string notes;
            string costCenterName;
            int costCenterNumber;

            string units;
            string fpag;
            DateTime date;
            string celebrateLocation;
            string hotel;
            public bool hasRegistrant = false;
            public bool hasHotel = false;
            public List<QueryFormatterProduct> products = new List<QueryFormatterProduct>();

            public QueryFormatter(CostCenterProduct reg, SageContext sageContext, ApplicationDbContext context, DateTime date_)
            {
                codArticulo = reg.Product;
                Articulo articulo = sageContext.Articulo.Find(codArticulo);

                if (string.IsNullOrWhiteSpace(reg.ProductDescription))
                    description = articulo?.Nombre;
                else
                    description = reg.ProductDescription;

                familia = articulo?.Familia;
                empresa = "01";
                serie = reg.Serie;

                var objSerie = sageContext.Serie.FirstOrDefault(s => s.Serie.Equals(serie) && s.Tipodoc == 7);
                nFactura = objSerie == null ? 1 : objSerie.Contador + 1;
                factura = serie + "  " + nFactura;
                vat = reg.VATId;
                iva = sageContext.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(reg.VATId)).IVA;
                units = reg.Units.ToString("F6").Replace(",", ".");

                precioBase = reg.BasePriceStr;
                importeBase = reg.TotalPriceStr;
                precioIva = reg.BasePriceVATStr;
                importeIva = reg.TotalPriceStr;
                cliente = reg.Client.SageCode;

                if (reg.Account == null || reg.Account.Vendedor == null)
                    vendedor = "01";
                else
                    vendedor = reg.Account.Vendedor;

                notes = "";
                if (reg.VATId.Equals("00"))
                {
                    notes = "* Factura exenta de IVA según el artículo 20.1 de la ley 37/1992 de 28 de diciembre sobre el Impuesto del Valor Añadido";
                }
                date = date_;
                costCenterName = reg.Congress.Name;
                costCenterNumber = reg.Congress.Number;
                //warehouse = reg.Congress.Warehouse;

                celebrateLocation = reg.Congress.GetDayAndTimePrint();
                fpag = reg.FPag;
                hasRegistrant = SetRegistrant(reg.Id, context);
                hasHotel = SetHotel(reg.Id, context);
                SetProducts(reg.Id, context, sageContext);
            }
            public bool SetRegistrant(Guid id, ApplicationDbContext context)
            {

                RegistrationBase reg = null;
                reg = context.Registrations.AsNoTracking().Include(r => r.Registrant).ThenInclude(rr => rr.Treatment).FirstOrDefault(r => r.Id.Equals(id));
                if (reg != null)
                {
                    registrant = reg.Registrant.Treatment.Name + " " + reg.Registrant.Name + " " + reg.Registrant.Surnames;
                    return true;
                }
                reg = context.Accommodations.AsNoTracking().Include(r => r.Registrant).ThenInclude(rr => rr.Treatment).FirstOrDefault(r => r.Id.Equals(id));
                if (reg != null)
                {
                    registrant = reg.Registrant.Treatment.Name + " " + reg.Registrant.Name + " " + reg.Registrant.Surnames;
                    return true;
                }
                return false;
            }
            public bool SetHotel(Guid id, ApplicationDbContext context)
            {
                Accommodation reg = null;
                reg = context.Accommodations.AsNoTracking().FirstOrDefault(r => r.Id.Equals(id));
                if (reg != null)
                {
                    hotel = reg.Hotel;

                    return true;
                }
                return false;
            }
            public bool SetProducts(Guid id, ApplicationDbContext context, SageContext sageContext)
            {
                Expense exp = null;
                exp = context.Expenses.Include(e => e.Products).AsNoTracking().FirstOrDefault(r => r.Id.Equals(id));
                if (exp != null)
                {
                    foreach (var p in exp.Products)
                    {
                        Articulo articulo = sageContext.Articulo.Find(p.ProductCode);
                        p.VAT = sageContext.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(p.VATId)).IVA;
                        products.Add(new QueryFormatterProduct
                        {
                            codArticulo = p.ProductCode,
                            familia = articulo.Familia,
                            description = p.ProductDescription,
                            vat = p.VATId,
                            iva = sageContext.Tipo_IVA.FirstOrDefault(i => i.Codigo.Equals(p.VATId)).IVA,
                            units = p.Units,
                            precioBase = p.BasePriceStr,
                            importeBase = p.TotalPriceStr,
                            precioIva = p.BasePriceVATStr,
                            importeIva = p.TotalPriceStr,
                            notes = p.ProductNotes
                        });
                    }

                    return true;
                }
                return false;
            }
            public string HeaderQuery
            {
                get
                {
                    FormattableString query = $@"INSERT INTO [dbo].[c_albven]([EMPRESA],[NUMERO],[FACTURA],[CLIENTE],[IMPORTE],[IMPDIVISA],[TOTALDOC],[TOTALDIV],[FECHA],[FECHA_FAC],[FRADIRECTA],[LETRA],[DIVISA],[ALMACEN],[FPAG],[VENDEDOR],[BANC_CLI],[OBSERVACIO]) 
                                                 VALUES('{empresa}', '{factura}', '{factura}', '{cliente}', {importeBase}, {importeBase}, {importeIva}, {importeIva}, CAST('{date}' AS smalldatetime),CAST('{date}' AS smalldatetime), 1,'{serie}', '000', '{costCenterNumber}',    '{fpag}', '{vendedor}','99', '{notes}')";
                    //VALUES('{empresa}', '{factura}', '{factura}', '{cliente}', {baseImp}, {baseImp}, {importe}, {importe}, CAST('{date.Value}' AS smalldatetime), 1,'{serie}', '000', '{reg.Congress.Number}', '00', '01',        '99', '{notes}')";

                    return query.ToString();
                }
            }

            public string DetailQuery(int line)
            {
                FormattableString detailQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[ARTICULO],[TIPO_IVA],[CLIENTE],[FECHA],[DEFINICION],[UNIDADES],[LINIA],[PRECIO],[IMPORTE],[PRECIOIVA],[IMPORTEIVA],[FAMILIA],[PRECIODIV],[IMPORTEDIV],[LETRA],[IMPDIVIVA],[PREDIVIVA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{codArticulo}', '{vat}', '{cliente}', CAST('{date}' AS smalldatetime), '{description}', {units}, {line}, {precioBase}, {importeBase}, {precioIva}, {importeIva}, '{familia}', {precioBase}, {importeBase}, '{serie}', {importeIva}, {precioIva}, '{vendedor}')";
                return detailQuery.ToString();

            }
            public FormattableString FormattableDetailQuery(int line)
            {
                return $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[ARTICULO],[TIPO_IVA],[CLIENTE],[FECHA],[DEFINICION],[UNIDADES],[LINIA],[PRECIO],[IMPORTE],[PRECIOIVA],[IMPORTEIVA],[FAMILIA],[PRECIODIV],[IMPORTEDIV],[LETRA],[IMPDIVIVA],[PREDIVIVA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{codArticulo}', '{vat}', '{cliente}', CAST('{date}' AS smalldatetime), '{description}', {units}, {line}, {precioBase}, {importeBase}, {precioIva}, {importeIva}, '{codArticulo}', {precioBase}, {importeBase}, '{serie}', {importeIva}, {precioIva}, '{vendedor}')";
            }
            public string DetailQuery(int line, int product)
            {
                var p = products[product];
                FormattableString detailQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[ARTICULO],[TIPO_IVA],[CLIENTE],[FECHA],[DEFINICION],[UNIDADES],[LINIA],[PRECIO],[IMPORTE],[PRECIOIVA],[IMPORTEIVA],[FAMILIA],[PRECIODIV],[IMPORTEDIV],[LETRA],[IMPDIVIVA],[PREDIVIVA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{p.codArticulo}', '{p.vat}', '{cliente}', CAST('{date}' AS smalldatetime), '{p.description}', {p.units}, {line}, {p.precioBase}, {p.importeBase}, {p.precioIva}, {p.importeIva}, '{p.familia}', {p.precioBase}, {p.importeBase}, '{serie}', {p.importeIva}, {p.precioIva}, '{vendedor}')";

                return detailQuery.ToString();

            }
            public string DetailNotesQuery(int line, int product)
            {
                var p = products[product];
                FormattableString detailQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date}' AS smalldatetime), '{p.notes}', {line}, '{serie}', '{vendedor}')";
                return detailQuery.ToString();

            }
            public string CostCenterQuery(int line)
            {
                string sCostCenterName = costCenterName;
                if (costCenterName.Length > 100)
                    sCostCenterName = sCostCenterName.Substring(0, 100);

                FormattableString costCenterQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date}' AS smalldatetime), '{sCostCenterName}', {line}, '{serie}', '{vendedor}')";
                return costCenterQuery.ToString();
            }

            public string CelebrationQuery(int line)
            {

                var toCelebrate = "A celebrar en ";
                if (toCelebrate.Length + celebrateLocation.Length <= 50)
                {
                    toCelebrate += celebrateLocation;
                }
                else if (celebrateLocation.Length <= 50)
                {
                    toCelebrate = celebrateLocation;
                }
                else
                {
                    toCelebrate = celebrateLocation.Substring(0, 50);
                }
                FormattableString dateQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date}' AS smalldatetime), '{toCelebrate}', {line}, '{serie}', '{vendedor}')";
                return dateQuery.ToString();
            }

            public string RegistrantQuery(int line)
            {

                FormattableString registrantQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date}' AS smalldatetime), '{registrant}', {line}, '{serie}', '{vendedor}')";
                return registrantQuery.ToString();
            }
            public string HotelQuery(int line)
            {
                FormattableString hotelQuery = $@"INSERT INTO [dbo].[d_albven]([EMPRESA],[NUMERO],[CLIENTE],[FECHA],[DEFINICION],[LINIA],[LETRA],[VENDEDOR]) 
                                                 VALUES('{empresa}','{factura}', '{cliente}', CAST('{date}' AS smalldatetime), 'Hotel: {hotel}', {line}, '{serie}', '{vendedor}')";
                return hotelQuery.ToString();
            }
            public string UpdateSeriesQuery()
            {
                FormattableString seriesQuery = $@"UPDATE [dbo].[series] SET [CONTADOR] = {nFactura} WHERE [SERIE] = '{serie}' AND [TIPODOC] = 7";
                return seriesQuery.ToString();
            }
        }
    }
}
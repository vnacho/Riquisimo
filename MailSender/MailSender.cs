using Ferpuser.Models;
using Ferpuser.ViewFunctions;
using SelectPdf;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Ferpuser.MailSender
{

    public enum EmailType
    {
        All = 0,
        Notification = 1,
        Invoice = 2,
    }
    public static class MailSender
    {
        [Obsolete("Do not use this in Production code!!!", false)]
        static void NEVER_EAT_POISON_Disable_CertificateValidation()
        {
            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                )
                {
                    return true;
                };
        }
        static void Disable_CertificateValidation()
        {
            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
        }
        private static bool CertificateValidationCallBack(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are 
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }
        public static void Send(string documentType, CongressEmailAccounts email, CostCenterProduct registration, dynamic request, decimal vat, EmailType emailType)
        {
            SmtpClient smtp = new SmtpClient(email.OutgoingMailServer)
            {
                EnableSsl = true,
                Port = email.OutgoingMailPort,
                Credentials = new NetworkCredential(email.MailUser, email.MailPassword),
            };
            MailMessage message = new MailMessage
            {
                Sender = new MailAddress(email.MailUser, email.Account.Name),
                From = new MailAddress(email.MailUser, email.Account.Name),
                Subject = documentType + ": " + email.Congress.Name,
                Body = request.body,
                IsBodyHtml = true,
            };
            if (email.Account.SendCopyTo && !string.IsNullOrWhiteSpace(email.Account.Email))
                message.To.Add(new MailAddress(email.Account.Email, email.Account.Name));            

            //Se pidió que se mandara
            //message.To.Add(new MailAddress(registration.Client.Email, registration.Client.BusinessName));

            var mails = (string)request.mails;
            if (!string.IsNullOrWhiteSpace(mails))
            {
                if (mails.Contains(';'))
                {
                    foreach (var mail in (mails).Split(";"))
                    {
                        try
                        {
                            message.To.Add(new MailAddress(mail));
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    message.To.Add(new MailAddress(mails));
                }
            }

            if (emailType == EmailType.All || emailType == EmailType.Notification)
            {
                var attachmentBody = NotificationMailAttachment(email.Congress, registration);
                MemoryStream stream = new MemoryStream(HtmlToPdf(attachmentBody, registration.InvoiceDate, registration.InvoiceNumber));
                Attachment attachment = new Attachment(stream, "confirmación.pdf");
                message.Attachments.Add(attachment);
            }
            if (emailType == EmailType.All || emailType == EmailType.Invoice)
            {
                var attachmentBody = InvoiceMailAttachment(email.Congress, registration, vat);
                MemoryStream stream = new MemoryStream(HtmlToPdf(attachmentBody, registration.InvoiceDate, registration.InvoiceNumber));
                Attachment attachment = new Attachment(stream, "factura.pdf");
                message.Attachments.Add(attachment);
            }
            //Disable_CertificateValidation();
            try
            {
                smtp.Send(message);
            }
            catch (Exception e)
            {
                try
                {
                    NEVER_EAT_POISON_Disable_CertificateValidation();
                    smtp.Send(message);
                }
                catch (Exception e2)
                {
                    throw new Exception("First exception: " + e.ToString() + ". Second Exception: " + e2.ToString());
                }
            }
        }

        public static void SendRegistration(string documentType, CongressEmailAccounts email, CostCenterProduct registration, dynamic request, decimal vat, EmailType emailType)
        {
            SmtpClient smtp = new SmtpClient(email.OutgoingMailServer)
            {
                EnableSsl = true,
                Port = email.OutgoingMailPort,
                Credentials = new NetworkCredential(email.MailUser, email.MailPassword),
            };
            MailMessage message = new MailMessage
            {
                Sender = new MailAddress(email.MailUser, email.Account.Name),
                From = new MailAddress(email.MailUser, email.Account.Name),
                Subject = documentType + ": " + email.Congress.Name,
                Body = request.body,
                IsBodyHtml = true,
            };
            if (email.Account.SendCopyTo && !string.IsNullOrWhiteSpace(email.Account.Email))
                message.To.Add(new MailAddress(email.Account.Email, email.Account.Name));

            //Se pidió que se mandara
            //message.To.Add(new MailAddress(registration.Client.Email, registration.Client.BusinessName));

            var mails = (string)request.mails;
            if (!string.IsNullOrWhiteSpace(mails))
            {
                if (mails.Contains(';'))
                {
                    foreach (var mail in (mails).Split(";"))
                    {
                        try
                        {
                            message.To.Add(new MailAddress(mail));
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    message.To.Add(new MailAddress(mails));
                }
            }

            if (emailType == EmailType.All || emailType == EmailType.Notification)
            {
                var attachmentBody = NotificationMailAttachment(email.Congress, registration);
                MemoryStream stream = new MemoryStream(HtmlToPdf(attachmentBody, registration.InvoiceDate, registration.InvoiceNumber));
                Attachment attachment = new Attachment(stream, "confirmación.pdf");
                message.Attachments.Add(attachment);
            }
            if (emailType == EmailType.All || emailType == EmailType.Invoice)
            {
                var attachmentBody = InvoiceMailAttachment(email.Congress, registration, vat);
                MemoryStream stream = new MemoryStream(HtmlToPdf(attachmentBody, registration.InvoiceDate, registration.InvoiceNumber));
                Attachment attachment = new Attachment(stream, "factura.pdf");
                message.Attachments.Add(attachment);
            }

            smtp.Send(message);            
        }

        public static string MailBody(string signatureAfter, CostCenterProduct registration, EmailType emailType)
        {
            string custom = "Adjuntamos ";
            if (emailType == EmailType.Notification || emailType == EmailType.All)
            {
                custom += "confirmación ";
            }
            if (emailType == EmailType.All)
            {
                custom += "y ";
            }
            if (emailType == EmailType.Invoice || emailType == EmailType.All)
            {
                if (registration.DocumentType != null)
                {
                    custom += registration.DocumentType.Name.ToLower();
                }
                else
                {
                    custom += "factura ";
                }
            }
            if (registration is Registration)
            {
                custom += "de inscripción";
            }
            else if (registration is Accommodation)
            {
                custom += "de alojamiento";
            }

            var header = @"<html><head><meta charset='UTF-8'></head><body>
                            <div class='main'>
                                <p>";
            var content = "";
            if (registration is RegistrationBase reg)
            {
                content = @"Estimado/a " + reg.Registrant.Treatment.Name + " " + reg.Registrant.Name + " " + reg.Registrant.Surnames;
            }

            var footer = @"</p>
                            <p>
                                " + custom + @".
                            </p>
                            <p>
                                Atentamente,
                            </p>
                        </div>
                        " + signatureAfter + "</body></html>";
            return header + content + footer;
        }
        public static string InvoiceMailAttachment(Congress congress, CostCenterProduct registration, decimal vat)
        {
            var date = registration.InvoiceDate ?? DateTime.Now;
            var description = "";
            if (registration is Registration)
            {
                description = "Cuota de inscripción al evento";
            }
            if (registration is Accommodation a)
            {
                description = "Alojamiento para el evento en el hotel: <br />" + a.Hotel;
            }


            decimal baseImp2d = Math.Round(registration.Fee / (1 + (vat / 100)), 2);
            var notes = "";
            if (registration.VATId.Equals("00"))
            {
                notes = "* Factura exenta de IVA según el artículo 20.1 de la ley 37/1992 de 28 de diciembre sobre el Impuesto del Valor Añadido";
            }
            #region header
            var header = @"<html>
                        <head>
                            <meta charset='UTF-8'>
                            <style>
                                body {
                                    font-family: Calibri, Candara, Segoe, Segoe UI, Optima, Arial, sans-serif;
                                    font-size: 13pt;
                                }

                                .header {
                                    margin-top: 5%;
                                    padding-left: 50%;
                                    font-size: 13pt;
                                    line-height: 16px;
                                    margin-bottom: 2em;
                                }

                                .name {
                                    font-weight: bold;
                                    font-size: 14pt;
                                }

                                .date {
                                    font-weight: bold;
                                    text-align: right;
                                    margin-bottom: 3em;
                                }

                                .main {
                                    width: 50%;
                                    margin-bottom: 1%;
                                    display: flex;
                                }

                                .main.first>div {
                                    display: flex;
                                    width: 95%;
                                    margin-right: 5%;
                                    justify-content: space-between;
                                }

                                .main.second>div {
                                    display: flex;
                                    width: 45%;
                                    margin-right: 5%;
                                    justify-content: space-between;
                                }

                                .main-title {
                                    color: #81003d;
                                    text-transform: uppercase;
                                    font-weight: bold;
                                    font-style: italic;
                                }

                                .date,
                                .main,
                                .header {
                                    margin-left: 2em;
                                    margin-right: 2em;
                                }

                                tr,
                                th,
                                td,
                                table,
                                thead,
                                tbody {
                                    padding: 0;
                                    margin: 0;
                                    border: 0;
                                    border-collapse: collapse;
                                }

                                .concept {
                                    margin-left: 2em;
                                    width: calc(100% - 4em);
                                    text-align: right;
                                }

                                .concept>thead {
                                    border: 1px solid black;
                                    background-color: lightgray;
                                }

                                .concept th {
                                    padding: 5px;
                                }

                                .concept td {
                                    padding: 5px;
                                }

                                .concept tr>td:first-child,
                                .concept tr>th:first-child {
                                    text-align: left;
                                }

                                .lower-container {
                                    width: 100%;
                                    height: 60%;
                                    display: flex;
                                    margin-top: 20%;
                                }

                                .lower-left {
                                    font-size: 10pt;
                                }

                                .lower-left>div {
                                    display: inline-block;
                                    -webkit-writing-mode: vertical-lr;
                                    -ms-writing-mode: tb-lr;
                                    writing-mode: vertical-lr;
                                    -moz-transform: rotate(180deg);
                                    -o-transform: rotate(180deg);
                                    -webkit-transform: rotate(180deg);
                                    -ms-transform: rotate(180deg);
                                    transform: rotate(180deg);
                                }

                                .lower-right {
                                    width: 100%;
                                }

                                .lower-right-table-container {
                                    display: flex;
                                    justify-content: flex-end;
                                }

                                .lower-right-table {
                                    text-align: right;
                                    margin: 10%;
                                    margin-right: 0em;
                                    width: 50%;
                                    border: 1px solid black;
                                }

                                .lower-right-table th,
                                .lower-right-table td {
                                    padding-left: 5%;
                                    padding-right: 5%;
                                    padding-top: 3%;
                                    padding-bottom: 3%;
                                }

                                .iban {
                                    display: flex;
                                    margin-bottom: 5%;
                                }

                                .iban-label {
                                    font-weight: bolder;
                                    margin-left: 5%;
                                    margin-right: 5%;
                                }
                                .bold {
                                    font-weight: bold;
                                }
                                .legal {
                                    font-size: 13px;
                                    margin-left: 5%;
                                    margin-right: 5%;
                                    margin-top: 5%;
                                    margin-bottom: 10%;
                                }

                                img {
                                    width: 100%;
                                }
                            </style>
                        </head>

                        <body>
                            <img style='width: 100%' src='";
            if (!string.IsNullOrWhiteSpace(congress.LogoBase64))
            {
                header += congress.LogoBase64;
            }
            else
            {
                header += "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAA+wAAABaCAYAAAAiotx9AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAAZdEVYdFNvZnR3YXJlAEFkb2JlIEltYWdlUmVhZHlxyWU8AAADZGlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS4wLWMwNjAgNjEuMTM0Nzc3LCAyMDEwLzAyLzEyLTE3OjMyOjAwICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1wTU06T3JpZ2luYWxEb2N1bWVudElEPSJ4bXAuZGlkOkQ4MzYxOTEwMjMzMkUyMTFCOEVCRDI4NUU2N0I3NDlBIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjg0Q0EyNDIxQTc3NjExRTJCRUJFQzJGNEFFQUY1NzlEIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjg0Q0EyNDIwQTc3NjExRTJCRUJFQzJGNEFFQUY1NzlEIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzUgV2luZG93cyI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjNBRkJDRTVCNzk5QkUyMTE4OTY2QjdGNTk5RkFGNjE1IiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOkQ4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";
            }

            header += @"' />
                            <div class='header'>
                                <p class='name'>
                                    " + registration.Client.BusinessName + @"
                                </p>
                                <p>
                                    " + registration.Client.NIF + @"
                                </p>
                                <p>
                                    " + registration.BillingLocation.Address + @"
                                </p>
                                <p>
                                    " + registration.BillingLocation.ZipCode + " " + registration.BillingLocation.City + @"
                                </p>
                                <p>
                                    " + registration.BillingLocation.Province + @"
                                </p>
                            </div>

                            <div class='main first'>
                                <div>
                                    <div class='main-title'>";

            if (registration.DocumentType.Name.Equals("Factura proforma"))
            {
                header += "Fac. proforma";
            }
            else
            {
                header += registration.DocumentType.Name;
            }

            header += " nº:</div><div>";

            if (registration.DocumentType.IsInvoice())
            {
                header += registration.InvoiceNumber;
            }
            else
            {
                header += ViewHelpers.PadCongress(registration.Number);
            }
            header += @"</div>
                                </div>

                                <div>
                                    <div class='main-title'>
                                        GF:
                                    </div>
                                    <div>
                                        " + congress.Number + @"
                                    </div>
                                    <div></div>
                                    <div></div>
                                    <div></div>
                                    <div></div>
                                </div>

                            </div>

                            <div class='main second'>
                                <div>
                                    <div class='main-title'>
                                        Fecha:
                                    </div>
                                    <div>
                                        " + date.ToShortDateString() + @"
                                    </div>
                                </div>
                            </div>";
            #endregion
            var content = "";
            #region registration content
            var congressData = @"<tr>
                            <td>
                                " + congress.Name + @"
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                A celebrar en " + congress.GetDayAndTimePrint() + @"
                            </td>
                            <td></td>
                        </tr>";
            if (registration is RegistrationBase reg)
                content = @"<table class='concept'>
                                <thead>
                                    <tr>
                                        <th>
                                            Descripción
                                        </th>
                                        <th>
                                            Importe
                                        </th>
                                    </tr>
                                </thead>
                                <tbody><tr>
                            <td>
                                " + description + @"
                            </td>
                            <td>
                                " + registration.Fee.ToString("N") + @"
                            </td>
                        </tr>"
                        + congressData +
                        @"<tr>
                            <td>
                                " + reg.Registrant.Treatment.Name + " " + reg.Registrant.Name + " " + reg.Registrant.Surnames + @"
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                " + notes + @"
                            </td>
                            <td></td>
                        </tr>";
            #endregion
            #region expense content
            if (registration is Expense e)
            {
                content = @"<table class='concept'>
                                <thead>
                                    <tr>
                                        <th>
                                            Descripción
                                        </th>
                                        <th>
                                            Unidades
                                        </th>
                                        <th>
                                            Precio
                                        </th>
                                        <th>
                                            Importe
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>";
                content += @"<tr><td>" + e.ProductDescription + "</td><td>" + e.Units.ToString("N") + "</td><td>" + e.GetTaxBase().ToString("N") + "</td><td>" + ((decimal)registration.Units * e.GetTaxBase()).ToString("N") + "</td></tr>";
                if (!string.IsNullOrWhiteSpace(e.ProductNotes))
                {
                    content += @"<tr><td>" + e.ProductNotes + "</td><td></td><td></td><td></td></tr>";
                }
                foreach (var product in e.Products)
                {
                    content += @"<tr><td>" + product.ProductDescription + "</td><td>" + product.Units.ToString("N") + "</td><td>" + product.BasePrice.ToString("N") + "</td><td>" + product.TotalPrice.ToString("N") + "</td></tr>";
                    if (!string.IsNullOrWhiteSpace(product.ProductNotes))
                    {
                        content += @"<tr><td>" + product.ProductNotes + "</td><td></td><td></td><td></td></tr>";
                    }
                }
                if (e.ShowCostCenterInfoOnInvoice)
                {
                    content += congressData;
                }
            }
            #endregion

            #region footer
            var footerTop = @"</tbody>
                </table>
                    <div class='lower-container'>
                            <div class='lower-left'>
                                <div>
                                    Registro Mercantil Valencia - Tomo 5.640, Libro 2947, folio 151, Hoja V 51.058 inscripción 1ª
                                    <br />
                                    Ferpuser S.L. - Santiago de Les, 8 - 46014 Valencia C.I.F. B96575550
                                </div>
                            </div>

                            <div class='lower-right'>
                                <div class='lower-right-table-container'>
                                    <table class='lower-right-table'>
                                        <thead>
                                            <tr>
                                                <th>
                                                    Base
                                                </th>
                                                <th>
                                                    %
                                                </th>
                                                <th>
                                                    IVA
                                                </th>
                                                <th>
                                                    TOTAL
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>";
            var footerContent = "";
            if (registration is RegistrationBase)
            {
                footerContent = @"<tr>
                                    <td>
                                        " + baseImp2d.ToString("N") + @"
                                    </td>
                                    <td>
                                        " + vat + @"
                                    </td>
                                    <td>
                                        " + (registration.Fee - baseImp2d).ToString("N") + @"
                                    </td>
                                    <td class='bold'>
                                        " + registration.Fee.ToString("N") + @"€
                                    </td>
                                </tr>";
            }
            else if (registration is Expense exp)
            {
                var first = true;
                var vats = exp.GetDistinctVATs();
                foreach (var pvat in vats)
                {
                    footerContent += @"<tr><td>" + exp.TotalOfVAT(pvat).ToString("N") + @"</td><td>" + pvat.ToString("N") + @"</td><td>" + exp.TotalVATOfVAT(pvat).ToString("N") + @"</td>";
                    if (first)
                    {
                        footerContent += "<td + rowspan='" + vats.Count + "' class='bold'>" + exp.TotalVAT.ToString("N") + @"€</td>";
                        first = false;
                    }
                    footerContent += "</tr>";
                }
            }

            //Si tiene código swift lo ponemos debajo del IBAN
            string sSwift = string.Empty;
            if (!string.IsNullOrWhiteSpace(congress.SwiftCode))
                sSwift = $"<span style='margin-left:50px;'>SWIFT: {congress.SwiftCode}</span>";
            
            var footerBottom = @"</tbody>
                                    </table>
                                </div>
                                <div class='iban'>
                                    <div class='iban-label'>
                                        Forma pago:
                                    </div>
                                    <div>
                                        Transferencia a: FERPUSER, S.L. - " + congress.Name + @"
                                        <br />
                                        IBAN: " + congress.IBAN + sSwift + @" 
                                    </div>
                                </div>
                                <div class='legal'>
                                    En cumplimiento de lo previsto en la Ley Orgánica 15/1999, de 13 de diciembre de Protección de Datos de
                                    Caracter Personal, le comunicamos que sus datos constan en un fichero titularidad de
                                    FERPUSER-SANICONGRESS necesario para la gestión contable y fiscal de la empresa.
                                    Puede ejercer los derechos de acceso, rectificación, cancelación y oposición enviando una solicitud por
                                    escrito al siguiente correo electrónico: lopd@ferpuser.com
                                </div>
                                <img style='width: 100%' src='";
            if (!string.IsNullOrWhiteSpace(congress.TailBase64))            
                footerBottom += congress.TailBase64;
            else
                footerBottom += "data:image/jpeg;base64,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aqH1Whe41pNwrkPGvwO0H4ha+l9q0er3WY44jZ/2zex2Ev8A10svM+zSf9tIq64JinE4rOjW9l/CDEYdVtK5j6B4J03w1DqEdjb/AGX+1LiS9uP3kn72ST/WSVXh+G2jQeG9H0mOwii0/QJLeTTo45JP9F+zf6uugoo9tVD6vROb8NfCLw/4NHh3+zbD7L/wiujSaBpX+kyS/ZbKT7N+7/6af8edv/rf+edHwu+EehfBvw1JpPhuw/szTpbiS5+z+ZJJ+8k/66f+i66XI9KMj0rT21UPq1ASiiisToCiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAqv8A2Pa/YPsv2W2+x/8APv5f7qrFFAFe80211Ly/tVrbXPlSeZH5kfmeVJRNo1rNqkd9Ja20l5FH5cc/l/vYo/8ArpViigDJh8BaHZ61/aUejaRHqHmeZ9rjs4/N8z/rpVjU/DenaxqFvdXVjY3Nxa/8e8k9vHJLF/1zq9RR7YPqxT1LQbHWPs/261trn7LJ5lv58fmeVJ/z0jqSbR7W8iuI5LW2k+3x+XceZH/x9R/9NKsUUAV5tHtZpZJJLW1kklkjkk/d/wCtkj/1dR/2BY/b/tX2Gx+2eZ5nn/Z4/N8zy/L/APRclXKKACqWseG9N16W3kvrGxvpLWTzbfz7eOTyv+udXaKAI/skH2nz/Li+0eX5e/y/3vl1H/ZsAtZIPIj+zy+Z5kfl/upfM/1lWKKAK/8AZFj/AM+tt/rPtP8Aq/8Alp/q/Moh0e1s7+4uoLW2jvLry/tE6R/vZfL/ANX5lWKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigBL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iiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigArD+HHiLV/FXhGC+13w9ceFtTklnjk02e7huniVJnSN/MhZkIkjVJAAcqJAGAYEDcopdbjvpYKKKKYgooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooA/9k=";

            footerBottom += @"' />
                            </div>
                        </div>
                    </body>
                    </html>";
            #endregion
            var all = header + content + footerTop + footerContent + footerBottom;
            return all;
        }
        public static string NotificationMailAttachment(Congress congress, CostCenterProduct registration)
        {
            string custom = "";
            if (registration is Registration r)
            {
                string category = r.Registrant.CategoriaInscrito == null ? r.Registrant.Category : r.Registrant.CategoriaInscrito.Nombre;
                custom = "Tiene confirmada su inscripción y por ello participará como " + category + " en el evento '" + congress.Name + "', que se celebrará en " + congress.GetDayAndTimePrint() + ".";
            }
            else if (registration is Accommodation a)
            {
                custom = "Tiene confirmado su alojamiento en el hotel " + a.Hotel + ", " + a.RoomType?.Description + ", fecha entrada " + a.StartDate.ToShortDateString() + ", fecha salida " + a.EndDate.ToShortDateString() + ", en el evento '" + congress.Name + "'.";
            }
            var header = @"<html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            body {
                                font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; 
                                font-size: 14pt;
                            }
                            .header {
                                margin-top: 25%;
                                text-align: right;
                                font-weight: bold;
                                line-height: 1px;
                                margin-bottom: 2em;
                            }

                            .date {
                                font-weight: bold;
                                text-align: right;
                                margin-bottom: 3em;
                            }

                            .main {
                                margin-bottom: 3em;
                            }
                            .date, .main, .header {
                                margin-left: 2em;
                                margin-right: 2em;
                            }
                            .signature {
                                text-align: center;
                                margin-bottom: 50%;
                            }
                            img {
                                width: 100%;
                            }
                        </style>
                    </head>
                    <body>
                        <img style='width: 100%' src='" + congress.LogoBase64 + @"' />
                        <div class='header'>";
            var content = "";
            if (registration is RegistrationBase reg)
            {
                content = @"<p>
                                " + reg.Registrant.Treatment.Name + " " + reg.Registrant.Name + " " + reg.Registrant.Surnames + @"
                            </p>
                            <p>
                                " + reg.Registrant.Workplace + @"
                            </p>
                        </div>
                        <div class='date'>
                                " + congress.Place + ", " + DateTime.Now.ToString("d 'de' MMMM 'de' yyyy") + @"
                        </div>
                        <div class='main'>
                            <p>
                                Por el presente escrito le comunicamos que:
                            </p>
                            <p>
                                " + reg.Registrant.Treatment.Name + " " + reg.Registrant.Name + " " + reg.Registrant.Surnames + @", con N.I.F. " + reg.Registrant.NIF + @"
                            </p>
                            <p>
                                " + custom + @"
                            </p>
                            <br />
                            <p>
                                Para que sirva a los efectos oportunos de permisos, billetaje, justificación, etc.
                            </p>";
            }

            var footer = @"</div>
                        <div class='signature'>
                            Secretaría Técnica.
                        </div>
                        <img style='width: 100%' src='" + congress.TailBase64 + @"'/>
                    </body>
                    </html>";

            return header + content + footer;
        }
        public static byte[] HtmlToPdf(string html, DateTime? date, string number)
        {
            //byte[] res = null;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
            //    pdf.Save(ms);
            //    res = ms.ToArray();
            //}
            //return res;


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
            for (int i = 1; i < doc.Pages.Count; i++)
            {
                doc.RemovePageAt(i);
            }
            MemoryStream pdfStream = new MemoryStream();
            doc.Save(pdfStream);
            var array = pdfStream.ToArray();
            if (date.HasValue && !string.IsNullOrWhiteSpace(number))
            {
                string subPath = @"C:\sage50c\sage50cserv\MODULOS\gestdoc\Docs0001\Documentos\VENTAS\PDF " + date.Value.Year + @"\";
                string path = subPath + number + ".pdf";
                try
                {
                    Directory.CreateDirectory(subPath);

                    // Create the file, or overwrite if the file exists.
                    using FileStream fs = File.Create(path);
                    // Add some information to the file.
                    fs.Write(array, 0, array.Length);
                }

                catch (Exception)
                {
                }
            }

            return array;
        }
    }
}

using Ferpuser.Models.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Services
{
    public class EmailService
    {
        private SmtpConfig SmtpConfig { get; set; }

        public EmailService(SmtpConfig smtpConfig)
        {
            SmtpConfig = smtpConfig;
        }

        public void Send(string subject, string body, bool isBodyHtml, string[] toAdresses, string mailFrom = "", List<Attachment> attachments = null)
        {
            if (string.IsNullOrWhiteSpace(mailFrom))
                mailFrom = SmtpConfig.SmtpUser;

            SmtpClient smtp = new SmtpClient(SmtpConfig.SmtpServer, SmtpConfig.SmtpPort)
            {
                EnableSsl = SmtpConfig.EnableSsl,
                Credentials = new NetworkCredential(SmtpConfig.SmtpUser, SmtpConfig.SmtpPassword)
            };

            MailMessage message = new MailMessage
            {
                Sender = new MailAddress(SmtpConfig.SmtpUser, mailFrom),
                From = new MailAddress(SmtpConfig.SmtpUser, mailFrom),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };

            foreach (string to in toAdresses)
            {
                message.To.Add(new MailAddress(to));
            }

            if (attachments != null)
            {
                foreach(var attach in attachments)
                {
                    message.Attachments.Add(attach);
                }
            }

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
    }

    
}

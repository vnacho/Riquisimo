using Ferpuser.Models.Consts;
using Ferpuser.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Helpers
{
    public class UserMailHelper
    {
        /// <summary>
        /// Obtiene la configuración smtp del usuario en el caso de que exista.
        /// Si no existe devuelve null
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public static SmtpConfig GetStmpConfig(ClaimsPrincipal User)
        {
            try
            {
                //Obtenemos los datos de smtp del usuario activo
                string sSmtpServer = User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_SMTP_SERVER))?.Value;
                string sSmtpPort = User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_SMTP_PORT))?.Value;
                string sSmtpMailuser = User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_MAIL_USER))?.Value;
                string sSmtpMailPassword = User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_MAIL_PASSWORD))?.Value;

                if (string.IsNullOrWhiteSpace(sSmtpServer) ||
                    string.IsNullOrWhiteSpace(sSmtpPort) ||
                    string.IsNullOrWhiteSpace(sSmtpMailuser) ||
                    string.IsNullOrWhiteSpace(sSmtpMailPassword))
                {
                    return null;
                }

                return new SmtpConfig()
                {
                    SmtpServer = sSmtpServer,
                    SmtpPort = Convert.ToInt32(sSmtpPort),
                    SmtpUser = sSmtpMailuser,
                    SmtpPassword = sSmtpMailPassword
                };
            }
            catch
            {
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Helpers
{
    public static class NIFHelper
    {
        /// <summary>
        /// Formatea el nif para coger solo caracteres válidos
        /// [0-9][A-Z]
        /// </summary>
        /// <param name="nif"></param>
        /// <returns></returns>
        public static string Format(string nif)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in nif)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                    sb.Append(c);
            }
            return sb.ToString().ToUpper();
        }
    }
}

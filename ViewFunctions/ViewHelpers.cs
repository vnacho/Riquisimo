using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferpuser.ViewFunctions
{
    public static class ViewHelpers
    {
        public static string SetActive(ViewContext viewContext, string name) {
            if (viewContext.RouteData.Values["controller"].ToString().Equals(name))
            {
                return "active";
            } else
            {
                return "";
            }
        }
        public static string PadCongress(int value)
        {
            return Pad(value, 5);
        }
        public static string Pad(int value, int size)
        {
            string fmt = new string('0', size);
            return value.ToString(fmt);
        }
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}

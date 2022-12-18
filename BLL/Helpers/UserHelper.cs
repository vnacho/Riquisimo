using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ferpuser.Models.Consts;
using Microsoft.AspNetCore.Mvc;

namespace Ferpuser.BLL.Helpers
{
    public static class UserHelper
    {
        public static bool AccesoAdmin(ClaimsPrincipal User)
        { 
            return User.Claims.Any(c => c.Type.Equals(Consts.CLAIM_PERMISO_ADMINISTRACION));
        }        

        public static string CodigoOperario(ClaimsPrincipal User)
        {            
            return User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_CODIGO_OPERARIO))?.Value;            
        }

        public static string CodigoVendedor(ClaimsPrincipal User)
        {
            return User.Claims.FirstOrDefault(c => c.Type.Equals(Consts.CLAIM_CODIGO_VENDEDOR))?.Value;
        }
    }    
}

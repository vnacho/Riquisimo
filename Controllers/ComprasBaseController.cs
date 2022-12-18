using Ferpuser.Models.Consts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    public class ComprasBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            HttpContext.Session.SetString("MENU", "MENU_COMPRAS");
        }

        
    }
}

using Ferpuser.Data;
using Ferpuser.Models.Sage;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class SecundarManager
    {
        private readonly SageContext _sageContext;

        public SecundarManager(SageContext sageContext)
        {
            _sageContext = sageContext;
        }

        public List<secundar> GetTiposCoste()
        {
            return _sageContext.secundar.Where(f => f.NIVEL == 1)?.ToList();
        }

        public List<secundar> GetCentrosCoste(string codigoTipoCoste)
        {
            var codigosCentrosCoste = _sageContext.plan_d.Where(f => f.SECNIVEL1 == codigoTipoCoste).Select(f => f.SECNIVEL2);
            return _sageContext.secundar.Where(f => f.NIVEL == 2 && codigosCentrosCoste.Contains(f.CODIGO))?.ToList();
        }

        public List<SelectListItem> GetTiposCosteSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            //list.Add(new SelectListItem() { Value = "", Text = "-Seleccionar-" });
            foreach (secundar item in _sageContext.secundar.Where(f => f.NIVEL == 1)?.OrderBy(f => f.NOMBRE).ToList())
            {
                list.Add(new SelectListItem() { Value = item.CODIGO, Text = item.Display });
            }            
            return list;
        }

        public List<SelectListItem> GetCentrosCosteSelectList(string codigoTipoCoste)
        {
            var codigosCentrosCoste = _sageContext.plan_d.Where(f => f.SECNIVEL1 == codigoTipoCoste).Select(f => f.SECNIVEL2);
            
            List<SelectListItem> list = new List<SelectListItem>();
            if (string.IsNullOrWhiteSpace(codigoTipoCoste))
                return list;

            //list.Add(new SelectListItem() { Value = "", Text = "-Seleccionar-" });
            foreach (secundar item in _sageContext.secundar.Where(f => f.NIVEL == 2 && codigosCentrosCoste.Contains(f.CODIGO))?.OrderBy(f => f.NOMBRE).ToList())
            {
                list.Add(new SelectListItem() { Value = item.CODIGO, Text = item.Display });
            }
            return list;
        }
    }
}

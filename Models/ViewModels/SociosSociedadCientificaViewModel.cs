using Ferpuser.BLL.Filters;
using System.Collections.Generic;

namespace Ferpuser.Models.ViewModels
{
    public class SociosSociedadCientificaViewModel
    {
        public int SociedadCientificaId { get; set; }        
        public SociedadCientifica SociedadCientifica { get; set; }
        public IList<SocioSociedadCientifica> Socios { get; set; } = new List<SocioSociedadCientifica>();

    }
}

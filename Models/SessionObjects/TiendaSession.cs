using Ferpuser.BLL.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.SessionObjects
{
    public class TiendaSession
    {
        public TiendaFilter filter { get; set; }
        public string sort { get; set; }
        public int page { get; set; }
    }
}

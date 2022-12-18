using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser
{
    public class AppSettings
    {    
        public string Empresa { get; set; }
        public string DescripcionAplicacion { get; set; }
        public string PathDocSAGE { get; set; }
        public bool HabilitarCongresos { get; set; }
        public bool EncuentrosMailsPrueba { get; set; }
        public string EncuentrosMailsCopia { get; set; }
    }
}

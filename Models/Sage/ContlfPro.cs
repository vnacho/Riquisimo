using System;
using System.Collections.Generic;

namespace Ferpuser.Models.Sage
{
    public partial class ContlfPro
    {
        public string Proveedor { get; set; } = "";
        public int Linea { get; set; } = 0;
        public bool Predet { get; set; }= false;
        public string Persona { get; set; } = "";
        public string Cargo { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Observa { get; set; } = "";
        public string Email { get; set; } = "";
        public string Skype { get; set; } = "";
        public string Facebook { get; set; } = "";
        public string Twitter { get; set; } = "";
        public int Lincontpro { get; set; } = 0;
        public int Lintelfpro { get; set; } = 0;
        public bool? Vista { get; set; } = false;
        public string Guid { get; set; } = "";
        public string GuidExp { get; set; } = "";
        public DateTime? Exportar { get; set; }
        public DateTime? Importar { get; set; }
        public string GuidId { get; set; }  
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;
        public int Tipo { get; set; } = 0;
    }
}

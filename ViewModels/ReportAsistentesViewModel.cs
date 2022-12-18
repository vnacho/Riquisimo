using Ferpuser.BLL.Filters;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.ViewModels
{
    public class ReportAsistentesViewModel
    {
        [Display(Name = "Asistente")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = false)]
        public string NIF { get; set; }

        [Display(Name = "Evento")]
        public Guid? IdCongress { get; set; }

        public Asistente Asistente { get; set; }

        public List<ReportAsistentesItem> Items { get; set; }

        public ReportAsistentesViewModel()
        {
            Items = new List<ReportAsistentesItem>();
        }
    }

    public class ReportAsistentesItem
    {
        public int CodigoEvento { get; set; }
        public string NombreEvento { get; set; }
        public string Lugar { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TipoInscripcion { get; set; }
        public decimal? ImporteInscripcion { get; set; }
    }
}

using Ferpuser.BLL.Filters;
using System.Collections.Generic;

namespace Ferpuser.Models.ViewModels
{
    public class InscritosEncuentroViewModel
    {
        public RestauracionFilter Filter { get; set; } = new RestauracionFilter();
        public Encuentro Encuentro { get; set; }
        public IList<InscritosEncuentroDto> Inscritos { get; set; } = new List<InscritosEncuentroDto>();

    }

    public class InscritosEncuentroDto
    {
        public Registration registration { get; set; }
        public int NumeroMesa { get; set; }
    }
}

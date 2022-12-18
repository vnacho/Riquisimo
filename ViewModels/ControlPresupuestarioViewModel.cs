using Ferpuser.BLL.Filters;
using Ferpuser.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.ViewModels
{
    public class ControlPresupuestarioViewModel
    {
        public List<ControlPresupuestarioDto> Items { get; set; }
        public ControlPresupuestarioFilter Filter { get; set; }

    }
}

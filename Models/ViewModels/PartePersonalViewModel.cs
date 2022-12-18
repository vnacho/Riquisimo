using Ferpuser.BLL.Filters;
using Ferpuser.Models.Core;
using System.Collections.Generic;

namespace Ferpuser.Models.ViewModels
{
    public class PartePersonalViewModel
    {
        public Pager Pager { get; set; }
        public string Sort { get; set; } = "Id desc";
        public PartePersonalFilter Filter { get; set; }
        public IEnumerable<PartePersonal> Items { get; set; }
    }
}

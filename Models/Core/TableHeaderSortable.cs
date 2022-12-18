using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Core
{
    public class TableHeaderSortable
    {
        public string Property { get; set; }
        public string Display { get; set; }
        public string CurrentSort { get; set; }
    }
}

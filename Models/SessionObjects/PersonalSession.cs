using Ferpuser.BLL.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.SessionObjects
{
    public class PersonalSession
    {
        public PersonalFilter filter { get; set; }
        public string sort { get; set; }
        public int page { get; set; }
    }
}

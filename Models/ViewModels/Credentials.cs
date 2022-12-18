using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.ViewModels
{
    public class Credentials
    {
        public List<Registration> Registrations { get; internal set; }
        public Congress Congress { get; internal set; }
    }
}

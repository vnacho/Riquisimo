using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.ViewModels
{
    public class HomeViewModel
    {
        public List<Registration> Unsent { get; set; }
        public List<Registration> Unreviewed { get; set; }
        public List<Client> NewClients { get; set; }
        public List<Accommodation> UnsentAcc { get; set; }
        public List<Accommodation> UnreviewedAcc { get; set; }
        public List<IGrouping<Guid?, Registration>> NewClientRegistrations { get; internal set; }
        public List<IGrouping<Guid?, Accommodation>> NewClientAccommodations { get; internal set; }
    }
}

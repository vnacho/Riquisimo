using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class CostCenterEmailData
    {
        public CostCenter CostCenter { get; set; }
        public Guid CostCenterId { get; set; }
        
        public EmailData EmailData { get; set; }
        public Guid EmailDataId { get; set; }
    }
}

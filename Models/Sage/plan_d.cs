using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public partial class plan_d
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string PLANCONT { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string SECNIVEL1 { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string SECNIVEL2 { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }

        public bool VISTA { get; set; }
    }
}

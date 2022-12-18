using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public class alma_anali
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(5)]
        public string ALMACEN { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string PLANCONT { get; set; }

        public bool VISTA { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string SECNIVEL1 { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(8)]
        public string SECNIVEL2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PORCENTAJE { get; set; }
    }
}

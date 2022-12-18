using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{

    [Table("secundar")]
    public partial class secundar
    {
        [Key]
        [StringLength(8)]
        public string CODIGO { get; set; }

        [Required]
        [StringLength(75)]
        public string NOMBRE { get; set; }

        [Required]
        [StringLength(10)]
        public string LIBRE_1 { get; set; }

        [Required]
        [StringLength(10)]
        public string LIBRE_2 { get; set; }

        [Required]
        [StringLength(10)]
        public string LIBRE_3 { get; set; }

        public bool? VISTA { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }

        public int NIVEL { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID { get; set; }

        public string Display => $"{CODIGO} {NOMBRE}";
    }
}

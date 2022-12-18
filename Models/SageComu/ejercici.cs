using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.SageComu
{
    [Table("ejercici")]
    public class ejercici
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string ANY { get; set; }

        [Required]
        [StringLength(80)]
        public string RUTA { get; set; }

        [Required]
        [StringLength(80)]
        public string RUTASER { get; set; }

        [Required]
        [StringLength(15)]
        public string CONEXION { get; set; }

        public bool PREDET { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime PERIODOINI { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime PERIODOFIN { get; set; }

        public bool? VISTA { get; set; }

        [Required]
        [StringLength(15)]
        public string ANTERIOR { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string GRUPO { get; set; }

        public bool NO_ACT { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }

        [NotMapped]
        public bool Selected { get; set; }
    }
}

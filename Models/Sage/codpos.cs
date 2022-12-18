using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public partial class codpos
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string CODIGO { get; set; }

        [Required]
        [StringLength(30)]
        public string POBLACION { get; set; }

        [Required]
        [StringLength(30)]
        public string PROVINCIA { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string LINEA { get; set; }

        public bool? VISTA { get; set; }

        [Required]
        [StringLength(5)]
        public string CPOSTALM { get; set; }

        [Required]
        [StringLength(30)]
        public string LONGI { get; set; }

        [Required]
        [StringLength(30)]
        public string LATI { get; set; }

        [Required]
        [StringLength(10)]
        public string POBLACERP { get; set; }

        [Required]
        [StringLength(10)]
        public string PROVINERP { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }

        public string DisplayName => $"{CODIGO.Trim()} {POBLACION.Trim()} {PROVINCIA.Trim()}";
    }
}

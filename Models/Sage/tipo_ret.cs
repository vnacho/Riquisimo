using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public partial class tipo_ret
    {
        [Key]
        [StringLength(2)]
        public string CODIGO { get; set; }

        [Required]
        [StringLength(50)]
        public string NOMBRE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal RETENCION { get; set; }

        [Required]
        [StringLength(8)]
        public string CTA_RE_SOP { get; set; }

        [Required]
        [StringLength(8)]
        public string CTA_RE_REP { get; set; }

        public bool? VISTA { get; set; }

        [Required]
        [StringLength(2)]
        public string G_CONTRI { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID { get; set; }

        public DateTime? IMPORTAR { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }
    }
}

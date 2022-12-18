using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.SageComu
{
    public partial class paises
    {
        [Key]
        [StringLength(3)]
        public string CODIGO { get; set; }

        [Required]
        [StringLength(30)]
        public string NOMBRE { get; set; }

        [Required]
        [StringLength(3)]
        public string LETRA { get; set; }

        [Column(TypeName = "ntext")]
        public string BANDERA { get; set; }

        public bool CEE { get; set; }

        [Required]
        [StringLength(3)]
        public string MONEDA { get; set; }

        [Required]
        [StringLength(3)]
        public string IDIOMA { get; set; }

        public bool? VISTA { get; set; }

        [Required]
        [StringLength(3)]
        public string COD_INTRA { get; set; }

        [Required]
        [StringLength(2)]
        public string ISO { get; set; }

        [Required]
        [StringLength(6)]
        public string ESTAD { get; set; }

        [Required]
        [StringLength(3)]
        public string MATRICULA { get; set; }

        [Required]
        [StringLength(5)]
        public string PREFIJO { get; set; }

        public int MAXIBAN { get; set; }

        [Required]
        [StringLength(3)]
        public string ISO2 { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }
    }
}

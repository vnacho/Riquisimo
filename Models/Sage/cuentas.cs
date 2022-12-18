using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public partial class cuentas
    {
        [Key]
        [StringLength(8)]
        public string CODIGO { get; set; }

        [Required]
        [StringLength(120)]
        public string NOMBRE { get; set; }

        [Required]
        [StringLength(17)]
        public string CIF { get; set; }

        [Required]
        [StringLength(8)]
        public string VARIA { get; set; }

        [Required]
        [StringLength(1)]
        public string SECUNDARIA { get; set; }

        [Column(TypeName = "text")]
        public string OBSERVACIO { get; set; }

        [Required]
        [StringLength(3)]
        public string DIVISA { get; set; }

        public bool? VISTA { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string DESCRIP { get; set; }

        public DateTime FCREADO { get; set; }

        public bool BABELENV { get; set; }

        [Required]
        [StringLength(15)]
        public string CLIENTEERP { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }

        [Required]
        [StringLength(1)]
        public string TIPOIVA { get; set; }

        public int CLAVEIRPF { get; set; }

        public bool RECARGO { get; set; }

        [Required]
        [StringLength(2)]
        public string CONCEPTO { get; set; }

        [Required]
        [StringLength(2)]
        public string CONGASING { get; set; }

        [Required]
        [StringLength(3)]
        public string CONIRPF { get; set; }

        [Required]
        [StringLength(15)]
        public string TELEFONO { get; set; }
        public bool RETENCION { get; internal set; }
        
        [Required]
        [StringLength(3)]
        public string PAIS { get; internal set; }
        
        [Required]
        [StringLength(2)]
        public string TIPO_RET { get; internal set; }
        public int PORCEN_RET { get; internal set; }

        [Required]
        [StringLength(9)]
        public string ANTICIPREM { get; internal set; }

        [Required]
        [StringLength(9)]
        public string BANCO_PREV { get; internal set; }
        public bool CSB { get; internal set; }
        public int DIAPAG { get; internal set; }
        public int DIAPAG2 { get; internal set; }

        [Required]
        [StringLength(2)]
        public string FPAG { get; internal set; }

        [Required]
        [StringLength(10)]
        public string CODPOST { get; internal set; }

        [Required]
        [StringLength(80)]
        public string DIRECCION { get; internal set; }

        [Required]
        [StringLength(150)]
        public string EMAIL { get; internal set; }

        [Required]
        [StringLength(15)]
        public string FAX { get; internal set; }

        [Required]
        [StringLength(30)]
        public string POBLACION { get; internal set; }

        [Required]
        [StringLength(30)]
        public string PROVINCIA { get; internal set; }
    }
}

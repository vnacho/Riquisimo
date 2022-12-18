using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.SageComu
{
    public class previs
    {
        [Required]
        [StringLength(25)]
        public string USUARIO { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string EMPRESA { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string PROVEEDOR { get; set; }

        [Required]
        [StringLength(8)]
        public string BANCO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(24)]
        public string FACTURA { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NUMEREB { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime VENCIM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IMPORTE { get; set; }

        [Required]
        [StringLength(2)]
        public string TIPOPAG { get; set; }

        [Required]
        [StringLength(10)]
        public string NUM_PAG { get; set; }

        [Required]
        [StringLength(1)]
        public string PAGADA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IMPPAGARE { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? VENCIM2 { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PENDIENTE { get; set; }

        public bool ASIENTO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IMPORTEDIV { get; set; }

        [Required]
        [StringLength(3)]
        public string DIVISA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal CAMBIO { get; set; }

        public bool? VISTA { get; set; }

        [Required]
        [StringLength(20)]
        public string ASI { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "smalldatetime")]
        public DateTime EMISION { get; set; }

        public int PERIODO { get; set; }

        [Required]
        [StringLength(80)]
        public string CONCEPTO { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FECREME { get; set; }

        public int NUM_BANCO { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? PAGO { get; set; }

        public int? REMESA { get; set; }

        [Required]
        [StringLength(4)]
        public string EJER_PAG { get; set; }

        [Required]
        [StringLength(10)]
        public string LIBRE_1 { get; set; }

        [Required]
        [StringLength(4)]
        public string TIPOPREV { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FEC_OPER { get; set; }

        [Required]
        [StringLength(24)]
        public string REFUNDIR { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(1)]
        public string DOCCARGO { get; set; }

        [Required]
        [StringLength(10)]
        public string NUMCRED { get; set; }

        [Required]
        [StringLength(8)]
        public string BCO_ORG { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID { get; set; }

        public DateTime? IMPORTAR { get; set; }

        public bool RECC { get; set; }

        public bool CHEQUE { get; set; }

        public bool COBROAGRUP { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }
    }
}

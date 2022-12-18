using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public partial class c_pedive
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
        [StringLength(10)]
        public string NUMERO { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FECHA { get; set; }

        [Required]
        [StringLength(8)]
        public string CLIENTE { get; set; }

        public int ENV_CLI { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ENTREGA { get; set; }

        [Required]
        [StringLength(30)]
        public string NOTA { get; set; }

        [Required]
        [StringLength(2)]
        public string VENDEDOR { get; set; }

        [Required]
        [StringLength(2)]
        public string RUTA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRONTO { get; set; }

        public bool TRASPASADO { get; set; }

        public bool CANCELADO { get; set; }

        [Column(TypeName = "text")]
        public string OBSERVACIO { get; set; }

        public bool IVA_INC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TOTALPED { get; set; }

        [Required]
        [StringLength(3)]
        public string DIVISA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal CAMBIO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IMPDIVISA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PESO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LITROS { get; set; }

        public bool? VISTA { get; set; }

        [Required]
        [StringLength(2)]
        public string FPAG { get; set; }

        [Required]
        [StringLength(2)]
        public string OPERARIO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(2)]
        public string LETRA { get; set; }

        public bool COMMS { get; set; }

        [Required]
        [StringLength(10)]
        public string LIBRE_1 { get; set; }

        [Required]
        [StringLength(10)]
        public string LIBRE_2 { get; set; }

        [Required]
        [StringLength(10)]
        public string LIBRE_3 { get; set; }

        public DateTime HORA { get; set; }

        [Required]
        [StringLength(25)]
        public string REFERCLI { get; set; }

        [Required]
        [StringLength(5)]
        public string OBRA { get; set; }

        public bool EDI { get; set; }

        [Required]
        [StringLength(10)]
        public string DOC_TLR { get; set; }

        [Required]
        [StringLength(5)]
        public string ALMACEN { get; set; }

        public bool TRASPEJER { get; set; }

        public DateTime FECHASTOCK { get; set; }

        public bool FINALIZADO { get; set; }

        public DateTime? EXPORTAR { get; set; }

        [Required]
        [StringLength(35)]
        public string MANDATO { get; set; }

        [Required]
        [StringLength(15)]
        public string CLIENTEERP { get; set; }

        public bool TRASPERP { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_EXP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TOTALDOC { get; set; }

        [Required]
        [StringLength(10)]
        public string CODPOST { get; set; }

        [Required]
        [StringLength(10)]
        public string CANAL { get; set; }

        public bool IMPRESO { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TOTALDIV { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PORCEN_RET { get; set; }

        public int CALCULO { get; set; }

        [Required]
        [StringLength(2)]
        public string TARIFA { get; set; }
    }
}


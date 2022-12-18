namespace Ferpuser.Models.Sage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class c_albcom
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

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string PROVEEDOR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRONTO { get; set; }

        [Required]
        [StringLength(5)]
        public string ALMACEN { get; set; }

        [Required]
        [StringLength(24)]
        public string FACTURA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FECHA_FAC { get; set; }

        [Required]
        [StringLength(20)]
        public string ASI { get; set; }

        [Required]
        [StringLength(2)]
        public string FPAG { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IMPORTE { get; set; }

        [Column(TypeName = "text")]
        public string OBSERVACIO { get; set; }

        [Required]
        [StringLength(3)]
        public string DIVISA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IMPDIVISA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal CAMBIO { get; set; }

        public bool? VISTA { get; set; }

        public bool TRASPASADO { get; set; }

        public bool RECEQUIV { get; set; }

        [Required]
        [StringLength(5)]
        public string OBRA { get; set; }

        [Required]
        [StringLength(12)]
        public string ALBVEN { get; set; }

        [Required]
        [StringLength(2)]
        public string OPERARIO { get; set; }

        public bool COMMS { get; set; }

        [Required]
        [StringLength(15)]
        public string LIBRE_1 { get; set; }

        [Required]
        [StringLength(15)]
        public string LIBRE_2 { get; set; }

        [Required]
        [StringLength(15)]
        public string LIBRE_3 { get; set; }

        [Required]
        [StringLength(8)]
        public string GRUPO { get; set; }

        [Required]
        [StringLength(14)]
        public string KEYCOPY { get; set; }

        public int STOCK_COEF { get; set; }

        public bool FACTURABLE { get; set; }

        public DateTime FECHASTOCK { get; set; }

        public bool ISP { get; set; }

        public bool RECC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TOTALDOC { get; set; }

        [Required]
        [StringLength(30)]
        public string ENCARGADO { get; set; }

        public bool IMPRESO { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TOTALDIV { get; set; }

        [Column(TypeName = "numeric")]
        public decimal RETENCIO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal RET_DIV { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PORCEN_RET { get; set; }

        public bool TRASPEJER { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string DESCFAC { get; set; }

        public bool FRADIRECTA { get; set; }
    }
}

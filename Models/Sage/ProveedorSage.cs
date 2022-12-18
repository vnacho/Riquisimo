namespace Ferpuser.Models.Sage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("proveed")]
    public partial class ProveedorSage
    {
        [Key]
        [StringLength(9)]
        public string CODIGO { get; set; } = "";

        public int ENV_PRO { get; set; }

        [Required]
        [StringLength(120)]
        public string NOMBRE { get; set; } = "";

        [Required]
        [StringLength(120)]
        public string NOMBRE2 { get; set; } = "";

        [Required]
        [StringLength(80)]
        public string DIRECCION { get; set; } = "";

        [Required]
        [StringLength(10)]
        public string CODPOST { get; set; } = "";

        [Required]
        [StringLength(30)]
        public string POBLACION { get; set; } = "";

        [Required]
        [StringLength(30)]
        public string PROVINCIA { get; set; } = "";

        [Required]
        [StringLength(17)]
        public string CIF { get; set; } = "";

        [Required]
        [StringLength(8)]
        public string BANCO { get; set; } = "";

        [Required]
        [StringLength(2)]
        public string FPAG { get; set; } = "";

        [Column(TypeName = "numeric")]
        public decimal PRONTO { get; set; }

        [Required]
        [StringLength(2)]
        public string TIPO_IVA { get; set; } = "";

        public bool RECARGO { get; set; }

        public int COMUNITARI { get; set; }

        public bool RETENCION { get; set; }

        public bool MODO_RET { get; set; }

        [Required]
        [StringLength(2)]
        public string TIPO_RET { get; set; } = "";

        [Required]
        [StringLength(150)]
        public string EMAIL { get; set; } = "";

        [Required]
        [StringLength(60)]
        public string HTTP { get; set; } = "";

        public int DIAS_ENT { get; set; }

        [Column(TypeName = "text")]
        public string OBSERVACIO { get; set; } = "";

        [Column(TypeName = "numeric")]
        public decimal DESCU1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DESCU2 { get; set; }

        [Required]
        [StringLength(3)]
        public string IDIOMA { get; set; } = "";

        [Required]
        [StringLength(3)]
        public string PAIS { get; set; } = "";

        [Column(TypeName = "numeric")]
        public decimal DIAPAG { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DIAPAG2 { get; set; }

        public bool? VISTA { get; set; }

        public bool MOD349 { get; set; }

        [Required]
        [StringLength(8)]
        public string CONTRAPAR { get; set; } = "";

        [Required]
        [StringLength(3)]
        public string IDIOMA_IMP { get; set; } = "";

        [Required]
        [StringLength(3)]
        public string C_ENT { get; set; } = "";

        [Required]
        [StringLength(8)]
        public string COD_AGRUP { get; set; } = "";

        [Column(TypeName = "numeric")]
        public decimal COMISION { get; set; } = new decimal(0);

        public bool CSB { get; set; } = false;

        [Required]
        [StringLength(50)]
        public string MENSAJE { get; set; } = "";

        public bool NOCOMUSMS { get; set; } = false;

        public bool NOCOMUEMA { get; set; } = false;

        public bool NOCOMUCAR { get; set; } = false;

        [Column(TypeName = "smalldatetime")]
        public DateTime? FBLOQNOSMS { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FBLOQNOEMA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FBLOQNOCAR { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string NOCOMUOBS { get; set; } = "";

        public bool REGCAJA { get; set; } = false;

        [Required]
        [StringLength(50)]
        public string GUID { get; set; } = "";

        public DateTime? IMPORTAR { get; set; }

        public bool RECC { get; set; } = false;

        public bool RECC2 { get; set; } = false;

        [Required]
        [StringLength(15)]
        public string PROVEEDERP { get; set; } = "";

        [Required]
        [StringLength(15)]
        public string CTAERP { get; set; } = "";

        [Required]
        [StringLength(40)]
        public string NOMBRE3ERP { get; set; } = "";

        [Required]
        [StringLength(10)]
        public string POBLACERP { get; set; } = "";

        [Required]
        [StringLength(10)]
        public string PROVINERP { get; set; } = "";

        public int TERRITERP { get; set; } = 0;

        [Required]
        [StringLength(40)]
        public string DIRECC2ERP { get; set; } = "";

        [Required]
        [StringLength(10)]
        public string DELEGERP { get; set; } = "";

        [Required]
        [StringLength(40)]
        public string GUID_EXP { get; set; } = "";

        [Required]
        [StringLength(10)]
        public string CANAL { get; set; } = "";

        [Required]
        [StringLength(254)]
        public string FACEBOOK { get; set; } = "";

        [Required]
        [StringLength(254)]
        public string TWITTER { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string SKYPE { get; set; } = "";

        public bool SYNC_CTC { get; set; } = false;

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; } = "";

        public DateTime? CREATED { get; set; } = DateTime.Now;

        public DateTime? MODIFIED { get; set; } = DateTime.Now;

        [Column(TypeName = "numeric")]
        public decimal CAMBIO { get; set; } = new decimal (0);

        [Column(TypeName = "smalldatetime")]
        public DateTime? FEC_CAM { get; set; }

        public bool EXCLUIR349 { get; set; } = false;

        [Required]
        [StringLength(25)]
        public string REFER_CAT { get; set; } = "";

        public string DisplayName => $"{CODIGO.Trim()} {NOMBRE.Trim()} {CIF}";
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ferpuser.Models.Sage
{
    public partial class modconfi
    {
        [Key]
        [StringLength(2)]
        public string EMPRESA { get; set; }

        [Required]
        [StringLength(250)]
        public string RUTA { get; set; }

        public bool VISTA { get; set; }

        [Required]
        [StringLength(40)]
        public string C_NOMBRE { get; set; }

        [Required]
        [StringLength(40)]
        public string C_APELLIDO { get; set; }

        [Required]
        [StringLength(9)]
        public string C_TELEFONO { get; set; }

        [Required]
        [StringLength(120)]
        public string NOMBRE { get; set; }

        [Required]
        [StringLength(120)]
        public string APELLIDO { get; set; }

        [Required]
        [StringLength(17)]
        public string NIF { get; set; }

        [Required]
        [StringLength(2)]
        public string VIA { get; set; }

        [Required]
        [StringLength(20)]
        public string NOMVIA { get; set; }

        [Required]
        [StringLength(4)]
        public string NUMERO { get; set; }

        [Required]
        [StringLength(2)]
        public string ESCALERA { get; set; }

        [Required]
        [StringLength(2)]
        public string PISO { get; set; }

        [Required]
        [StringLength(2)]
        public string PUERTA { get; set; }

        [Required]
        [StringLength(10)]
        public string CPOSTAL { get; set; }

        [Required]
        [StringLength(30)]
        public string MUNICIPIO { get; set; }

        [Required]
        [StringLength(30)]
        public string PROVINCIA { get; set; }

        [Required]
        [StringLength(9)]
        public string TELEFONO { get; set; }

        [Required]
        [StringLength(2)]
        public string SIGLA { get; set; }

        [Required]
        [StringLength(4)]
        public string CTABANCO { get; set; }

        [Required]
        [StringLength(4)]
        public string CTASUCUR { get; set; }

        [Required]
        [StringLength(2)]
        public string CTADIGCON { get; set; }

        [Required]
        [StringLength(30)]
        public string CTACUENTA { get; set; }

        public bool INSCRITO { get; set; }

        [Required]
        [StringLength(5)]
        public string CODADMIN { get; set; }

        [Required]
        [StringLength(4)]
        public string LETRAS { get; set; }

        [Required]
        [StringLength(17)]
        public string C_NIF { get; set; }

        [Required]
        [StringLength(32)]
        public string C_MUNICI { get; set; }

        [Required]
        [StringLength(10)]
        public string C_CPOSTAL { get; set; }

        [Required]
        [StringLength(32)]
        public string C_PROVIN { get; set; }

        [Required]
        [StringLength(10)]
        public string C_FAX { get; set; }

        [Required]
        [StringLength(10)]
        public string C_TELEF { get; set; }

        [Required]
        [StringLength(6)]
        public string TOMO { get; set; }

        [Required]
        [StringLength(6)]
        public string LIBRO { get; set; }

        [Required]
        [StringLength(6)]
        public string SECCION { get; set; }

        [Required]
        [StringLength(6)]
        public string FOLIO { get; set; }

        [Required]
        [StringLength(10)]
        public string HOJA { get; set; }

        [Required]
        [StringLength(32)]
        public string OTROS { get; set; }

        [Required]
        [StringLength(32)]
        public string REG_PUB { get; set; }

        [Required]
        [StringLength(32)]
        public string REG_MER { get; set; }

        [Required]
        [StringLength(254)]
        public string RUTA_ORI { get; set; }

        [Required]
        [StringLength(254)]
        public string RUTA_DES { get; set; }

        [Required]
        [StringLength(32)]
        public string C_NOMVIA { get; set; }

        [Required]
        [StringLength(10)]
        public string FAX { get; set; }

        [Required]
        [StringLength(50)]
        public string NOM_S { get; set; }

        [Required]
        [StringLength(30)]
        public string APE1 { get; set; }

        [Required]
        [StringLength(30)]
        public string APE2 { get; set; }

        [Required]
        [StringLength(50)]
        public string C_NOM_S { get; set; }

        [Required]
        [StringLength(30)]
        public string C_APE1 { get; set; }

        [Required]
        [StringLength(30)]
        public string C_APE2 { get; set; }

        public bool M_NOMBRE2 { get; set; }

        public int PRORRATA { get; set; }

        public bool B_FECHA { get; set; }

        public int TIPO_I { get; set; }

        public int CLAVE_D_I { get; set; }

        public int P_OPER_I { get; set; }

        [Required]
        [StringLength(34)]
        public string D_BIEN_I { get; set; }

        [Required]
        [StringLength(32)]
        public string MOD3XX { get; set; }

        public bool TERCERAS { get; set; }

        public bool RETEN347 { get; set; }

        public bool PROVSIG347 { get; set; }

        [Required]
        [StringLength(50)]
        public string CUENTAIBAN { get; set; }

        [Required]
        [StringLength(11)]
        public string SWIFT { get; set; }

        [Required]
        [StringLength(4)]
        public string TIPOCTA { get; set; }

        [Required]
        [StringLength(4)]
        public string IBAN { get; set; }

        public bool EXONERADO { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }

        public bool SII { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SII_ALTA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SII_BAJA { get; set; }

        [Required]
        [StringLength(250)]
        public string SII_RUTA { get; set; }

        [Required]
        [StringLength(250)]
        public string SII_CNOM { get; set; }

        [Required]
        [StringLength(250)]
        public string SII_CEMI { get; set; }

        public bool SII_ENVIO { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SII_FINI { get; set; }

        public bool SII_AVISOS { get; set; }

        public bool SII_AVISOC { get; set; }

        public int SII_TERRI { get; set; }

        public bool SII_CSELLO { get; set; }

        public bool SII_REGIS { get; set; }

        [Required]
        [StringLength(250)]
        public string RUTA_ISV { get; set; }

        public bool SII_MACRO { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SII_ALTAT2 { get; set; }

        public int SII_TERRI2 { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SII_BAJAT2 { get; set; }

        public bool SII_ENVT2 { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? SII_FINIT2 { get; set; }

        [Required]
        [StringLength(250)]
        public string SII_RUTAT2 { get; set; }

        [Required]
        [StringLength(250)]
        public string SII_CNOMT2 { get; set; }

        [Required]
        [StringLength(250)]
        public string SII_CEMIT2 { get; set; }

        public bool SII_2TERR { get; set; }

        public bool RECM_ALTA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? RECM_FALTA { get; set; }

        public bool RECM_BAJA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? RECM_FBAJA { get; set; }

        [Required]
        [StringLength(150)]
        public string EMAIL { get; set; }

        [Required]
        [StringLength(150)]
        public string C_EMAIL { get; set; }

        public int FJURIDIC { get; set; }

        [Required]
        [StringLength(50)]
        public string FJURIOTR { get; set; }

        [Required]
        [StringLength(5)]
        public string CNAE { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string OBJSOCIA { get; set; }
    }
}

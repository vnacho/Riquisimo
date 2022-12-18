using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    [Table("empresa")]
    public class empresa
    {
        [Key]
        [StringLength(2)]
        public string CODIGO { get; set; }

        [Required]
        [StringLength(120)]
        public string NOMBRE { get; set; }

        [Required]
        [StringLength(120)]
        public string NOMBRE2 { get; set; }

        [Required]
        [StringLength(17)]
        public string CIF { get; set; }

        [Required]
        [StringLength(50)]
        public string DIRECCION { get; set; }

        [Required]
        [StringLength(10)]
        public string CODPOS { get; set; }

        [Required]
        [StringLength(15)]
        public string TELEFONO { get; set; }

        [Required]
        [StringLength(30)]
        public string POBLACION { get; set; }

        [Required]
        [StringLength(30)]
        public string PROVINCIA { get; set; }

        [Required]
        [StringLength(15)]
        public string FAX { get; set; }

        [Required]
        [StringLength(20)]
        public string TIPO { get; set; }

        public int PEDIVEN { get; set; }

        public int DEPVEN { get; set; }

        public int PRESUP { get; set; }

        public int ALBAVEN { get; set; }

        public int FACTUVEN { get; set; }

        public int TRASPASO { get; set; }

        public int REGULARIZA { get; set; }

        public int PEDICOM { get; set; }

        public int FACTUCOM { get; set; }

        public int ASIENTO { get; set; }

        public int IVASOPOR { get; set; }

        [Required]
        [StringLength(2)]
        public string TIPO_IVA { get; set; }

        [Required]
        [StringLength(5)]
        public string ALMACEN { get; set; }

        [Column(TypeName = "ntext")]
        public string LOGO { get; set; }

        [Required]
        [StringLength(80)]
        public string TXTALBA1 { get; set; }

        [Required]
        [StringLength(80)]
        public string TXTALBA2 { get; set; }

        [Required]
        [StringLength(80)]
        public string TXTFACTU1 { get; set; }

        [Required]
        [StringLength(80)]
        public string TXTFACTU2 { get; set; }

        [Required]
        [StringLength(150)]
        public string EMAIL { get; set; }

        [Required]
        [StringLength(60)]
        public string HTTP { get; set; }

        [Required]
        [StringLength(15)]
        public string MOBIL { get; set; }

        public int REMESA { get; set; }

        public bool USER_INF { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime PERIODOINI { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime PERIODOFIN { get; set; }

        public int DIGITOS { get; set; }

        [Required]
        [StringLength(3)]
        public string MONEDA { get; set; }

        [Required]
        [StringLength(15)]
        public string TPC { get; set; }

        public int ORDEN { get; set; }

        public bool PRES_ASI { get; set; }

        public int DECIMALES { get; set; }

        [Required]
        [StringLength(80)]
        public string PEDICO1 { get; set; }

        [Required]
        [StringLength(80)]
        public string PEDICO2 { get; set; }

        [Required]
        [StringLength(80)]
        public string FACTUCOM1 { get; set; }

        [Required]
        [StringLength(80)]
        public string FACTUCOM2 { get; set; }

        public int ARTICULO { get; set; }

        public int FAMILIA { get; set; }

        public int REFUNDIR { get; set; }

        public bool FACTURAR { get; set; }

        public int FRACEE { get; set; }

        public bool? VISTA { get; set; }

        public int VENDEDOR { get; set; }

        [Required]
        [StringLength(2)]
        public string LETRA { get; set; }

        [Required]
        [StringLength(5)]
        public string ALMFABRI { get; set; }

        public bool DATOS { get; set; }

        public bool MANTE { get; set; }

        public int VECES { get; set; }

        public int REMESAPA { get; set; }

        public bool DTOIMP { get; set; }

        [Required]
        [StringLength(50)]
        public string PASWORD { get; set; }

        public int PRECOM { get; set; }

        public int ALBACOM { get; set; }

        public int STCOEF_ENT { get; set; }

        public int STCOEF_SOR { get; set; }

        public int DEPCOM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal COLORCLI { get; set; }

        [Required]
        [StringLength(250)]
        public string TXTEFACTU1 { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string TXTEFACTU2 { get; set; }

        public int C_EMAIL { get; set; }

        public bool BLOQALBVTA { get; set; }

        public bool BLOQPEDVTA { get; set; }

        public bool BLOQPREVTA { get; set; }

        public bool BLOQDEPVTA { get; set; }

        public int COMUNICA { get; set; }

        public int REFUNDIRP { get; set; }

        [Required]
        [StringLength(80)]
        public string SUBFACOM { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string BODFACOM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal COLCLITEXT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal COLORTEXT { get; set; }

        public bool EMPEXPORT { get; set; }

        public bool EMPEXPDEL { get; set; }

        public bool EMPIMPSUB { get; set; }

        public bool EMPIMPADD { get; set; }

        public bool EMPEXPINV { get; set; }

        public int CRITEJRMIN { get; set; }

        public bool CRITCAJA { get; set; }

        [Required]
        [StringLength(2)]
        public string IVA_ISP { get; set; }

        [Required]
        [StringLength(100)]
        public string FOTO { get; set; }

        public int TERRITERP { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }

        public bool CONTACT { get; set; }

        public bool CAPTURE { get; set; }

        public bool USER_ROL { get; set; }

        [Required]
        [StringLength(2)]
        public string PYG_BAL { get; set; }

        public bool DASHBOARD { get; set; }

        public bool SUPLIDOS { get; set; }

        [Required]
        [StringLength(2)]
        public string IVASUPLIDOS { get; set; }

        public bool PRTAPLICAR { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? PRTFECHA { get; set; }

        public int PRTTIPO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRTPRC { get; set; }

        public bool PRTBAJA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? PRTFECHADS { get; set; }

        [Required]
        [StringLength(10)]
        public string ACRONIMO { get; set; }

        [Required]
        [StringLength(2)]
        public string ALBFADI { get; set; }

        public bool EOS { get; set; }

        [Required]
        [StringLength(2)]
        public string TARIFACF { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PORESTRUP { get; set; }

        public bool PCOSTES { get; set; }
    }
}

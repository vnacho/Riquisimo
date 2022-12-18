using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ferpuser.Models.Sage
{
    public class d_albven
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

        [Required]
        [StringLength(20)]
        public string ARTICULO { get; set; }

        [Required]
        [StringLength(100)]
        public string DEFINICION { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIDADES { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO2 { get; set; }

        [Required]
        [StringLength(2)]
        public string TIPO_IVA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal COSTE { get; set; }

        [Required]
        [StringLength(8)]
        public string CUENTA { get; set; }

        [Required]
        [StringLength(12)]
        public string PEDIDO { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FECHA { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LINIA { get; set; }

        [Required]
        [StringLength(8)]
        public string CLIENTE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IMPORTE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIOIVA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IMPORTEIVA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal CAJAS { get; set; }

        [Required]
        [StringLength(5)]
        public string FAMILIA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIODIV { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IMPORTEDIV { get; set; }

        [Required]
        [StringLength(15)]
        public string SERIE { get; set; }

        public int TIPO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal COMISION { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IMP_COM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PESO { get; set; }

        public int DOC { get; set; }

        [Required]
        [StringLength(12)]
        public string DOC_NUM { get; set; }

        public int DOC_LIN { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DOC_UNID { get; set; }

        public bool? VISTA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PVERDE { get; set; }

        public bool RECARG { get; set; }

        [Required]
        [StringLength(2)]
        public string COLOR { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(2)]
        public string LETRA { get; set; }

        [Required]
        [StringLength(4)]
        public string TALLA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IMPDIVIVA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PREDIVIVA { get; set; }

        public bool LOTE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PUNTOS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO3_IMP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO3_IMPDI { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO3_IVA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO3_IVADI { get; set; }

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
        [StringLength(30)]
        public string LIBRE_4 { get; set; }

        [Required]
        [StringLength(30)]
        public string LIBRE_5 { get; set; }

        public bool VENTASER { get; set; }

        [Required]
        [StringLength(20)]
        public string ASI { get; set; }

        [Required]
        [StringLength(8)]
        public string PROVEEDOR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ESCANEADO { get; set; }

        public bool STOCKNO { get; set; }

        [Required]
        [StringLength(2)]
        public string VENDEDOR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DIAS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ACTUAL { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ANTERIOR { get; set; }

        public int CONTADOR { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FLACTUAL { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FLANTERIOR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNID_DIAS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DOC_CAJA { get; set; }

        [Required]
        [StringLength(30)]
        public string ESCANDAL { get; set; }

        [Required]
        [StringLength(5)]
        public string ALMACEN { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TIPOPREC { get; set; }

        [Required]
        [StringLength(2)]
        public string TIPO_IVAV { get; set; }

        [Required]
        [StringLength(2)]
        public string TIPO_ART { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIMEDIDA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PREMEDIDA { get; set; }

        [Required]
        [StringLength(10)]
        public string NUMALBORI { get; set; }

        [Required]
        [StringLength(2)]
        public string LETALBORI { get; set; }

        public int EJEALBORI { get; set; }

        [Required]
        [StringLength(15)]
        public string CLIENTEERP { get; set; }

        [Required]
        [StringLength(10)]
        public string CODAGRUP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIAGRUP { get; set; }

        public bool? FACTURABLE { get; set; }

        public bool SUPLIDO { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }
    }
}


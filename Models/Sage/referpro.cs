using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Ferpuser.Models.Sage
{

    [Table("referpro")]
    public partial class referpro
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ARTICULO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string PROVEEDOR { get; set; }

        [Required]
        [StringLength(25)]
        public string REFERENCIA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PCOMPRA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO2 { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FECHA_ULT { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(3)]
        public string MONEDA { get; set; }

        public bool? VISTA { get; set; }

        public bool PREDET { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(2)]
        public string COLOR { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(4)]
        public string TALLA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO4 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO5 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO6 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DTO7 { get; set; }

        public bool CAMBIAR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal GASTO { get; set; }

        [Required]
        [StringLength(45)]
        public string NOTAS { get; set; }

        [Required]
        [StringLength(4)]
        public string ALBEJER { get; set; }

        [Required]
        [StringLength(10)]
        public string ALBNUM { get; set; }

        [Required]
        [StringLength(2)]
        public string EMPRESA { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }

        [Column(TypeName = "numeric")]
        public decimal COSTE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal CAMBIO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal C_BASE { get; set; }
    }
}

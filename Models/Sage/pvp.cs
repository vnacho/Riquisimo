using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Ferpuser.Models.Sage
{
    [Table("pvp")]
    public partial class pvp
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ARTICULO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string TARIFA { get; set; }

        [Column("PVP", TypeName = "numeric")]
        public decimal PVP1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PVPIVA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal MARGEN { get; set; }

        [Required]
        [StringLength(2)]
        public string TCP { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID { get; set; }

        public DateTime? IMPORTAR { get; set; }

        public bool? VISTA { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "smalldatetime")]
        public DateTime FECHAINI { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FECHAFIN { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIO1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIO2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIO3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIO4 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIO5 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIO6 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIO7 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIO8 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIO9 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PRECIO10 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIDMAX1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIDMAX2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIDMAX3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIDMAX4 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIDMAX5 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIDMAX6 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIDMAX7 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIDMAX8 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIDMAX9 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal UNIDMAX10 { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }
    }
}

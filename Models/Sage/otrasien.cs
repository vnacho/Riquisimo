using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    [Table("otrasien")]
    public partial class otrasien
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string EMPRESA { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string SECUNDAR { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string CODCUEN { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FECHA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DEBE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal HABER { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string ASI { get; set; }

        public bool? VISTA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DEBEDIV { get; set; }

        [Required]
        [StringLength(3)]
        public string DIVISA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal HABERDIV { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(8)]
        public string SECNIVEL2 { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(2)]
        public string PLANCONT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DEBEAGRUP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal HABERAGRUP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DEBEAGRUPDIV { get; set; }

        [Column(TypeName = "numeric")]
        public decimal HABERAGRUPDIV { get; set; }
    }
}

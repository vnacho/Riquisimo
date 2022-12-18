namespace Ferpuser.Models.SageComu
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("operario")]
    public partial class Operario
    {
        [Key]
        [StringLength(2)]
        public string CODIGO { get; set; }

        [Required]
        [StringLength(50)]
        public string NOMBRE { get; set; }

        public bool? VISTA { get; set; }

        [Required]
        [StringLength(6)]
        public string CLAVE { get; set; }

        [Required]
        [StringLength(50)]
        public string GUID_ID { get; set; }

        public DateTime CREATED { get; set; }

        public DateTime MODIFIED { get; set; }
    }
}

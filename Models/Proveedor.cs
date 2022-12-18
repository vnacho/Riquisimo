using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class Proveedor : BaseModel
    {
        [Display(Name = "PROVEEDOR")]
        [Required]
        //public string CODIGO { get; set; }
        public string CUENTACONTABLE { get; set; } = "";

        [Display(Name = "NIF/DNI")]
        [Required]
        [StringLength(17)]
        //public string CIF { get; set; }
        public string NIF { get; set; } = "";

        [Display(Name = "Nombre")]
        [StringLength(120)]
        public string RAZONSOCIAL { get; set; } = "";

        [Display(Name = "Razón Social")]
        [Required]
        [StringLength(120)]
        //public string NOMBRE2 { get; set; }
        public string? NOMBRECOMERCIAL { get; set; } = "";

        [StringLength(80)]
        public string? DIRECCION { get; set; } = "";

        [Display(Name = "CP")]
        [Required]
        [StringLength(10)]
        public string CODPOST { get; set; } = "";

        [Display(Name = "POBLACIÓN")]
        [StringLength(30)]
        //public string POBLACION { get; set; }
        public string? LOCALIDAD { get; set; } = "";

        [Display(Name = "PROVINCIA")]
        //[Required]
        [StringLength(30)]
        public string? PROVINCIA { get; set; } = "";

        [Required]
        [StringLength(3)]
        public string PAIS { get; set; } = "";

        [Display(Name = "CONTACTO")]
        [StringLength(40)]
        //public string NOMBRE3ERP { get; set; }
        public string? PERSONACONTACTO { get; set; } = "";

        [StringLength(50)]
        public string? CARGO { get; set; } = "";

        [StringLength(150)]
        public string? EMAIL { get; set; } = "";

        [StringLength(15)]
        public string? TELEFONO { get; set; } = "";

        [StringLength(15)]
        public string? TELEFONO2 { get; set; } = "";

        [StringLength(60)]
        //public string HTTP { get; set; }
        public string? PAGINAWEB { get; set; } = "";

        [StringLength(2)]
        public string? FORMAPAGO { get; set; } = "";

        [Display(Name = "TIPO RETENCIÓN")]
        [StringLength(2)]
        public string? RETENCION { get; set; } = "";

        [StringLength(100)]
        public string? COMISIONES { get; set; } = "";
        [Display(Name = "RETENCIÓN")]
        public bool TIPO_RET { get; set; } = false;
        [Display(Name = "MODO RETENCIÓN")]
        public bool MODO_RET { get; set; } = false;

        [DataType(DataType.MultilineText)]
        public string? OBSERVACIONES { get; set; } = "";
    }
}

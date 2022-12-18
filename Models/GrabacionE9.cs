using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class GrabacionE9 : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(80)]
        public string Descripcion { get; set; }

        [Display(Name = "Entrada/Salida")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(1)]
        public string EntradaSalida { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = true)]
        public decimal Importe { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Centro de coste")]
        public int CentroCosteId { get; set; }

        [Display(Name = "Centro de coste")]
        public CentroCoste CentroCoste { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Destino")]        
        public int DestinoId { get; set; }

        [Display(Name = "Destino")]
        //[ForeignKey("DestinoId")]
        public CentroCoste Destino { get; set; }

        public TipoGrabacionE9 TipoGrabacionE9 => EntradaSalida == "E" ? TipoGrabacionE9.Entrada : TipoGrabacionE9.Salida;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errores = new List<ValidationResult>();
            if (EntradaSalida != "E" && EntradaSalida != "S")
                errores.Add(new ValidationResult("El campo Entrada/Salida no tiene el valor correcto", new string[] { "EntradaSalida" }));
            return errores;
        }
    }
    public enum TipoGrabacionE9
    {
        Entrada,
        Salida
    }
}

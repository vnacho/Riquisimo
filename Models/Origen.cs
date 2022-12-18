using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Origen
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nivel analítico 1")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(3)]
        public string NivelAnalitico1 { get; set; }

        [Display(Name = "Nivel analítico 1")]
        public string NombreNivelAnalitico1 { get; set; }

        [Display(Name = "Nivel analítico 2")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(5)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El campo '{0}' debe ser numérico")]
        public string NivelAnalitico2 { get; set; }

        [Display(Name = "Nivel analítico 2")]
        public string NombreNivelAnalitico2 { get; set; }

        //[Display(Name = "Tipo de centro de coste")]
        //[Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        //public int TipoCentroCosteId { get; set; }
        //[Display(Name = "Tipo de centro de coste")]
        //public TipoCentroCoste TipoCentroCoste { get; set; }

        [Display(Name = "Año")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Range(2000, 2100, ErrorMessage = "El campo '{0}' debe estar entre {1} y {2}")]
        public int Anio { get; set; }

        [Display(Name = "Mes")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Range(1, 12, ErrorMessage = "El campo '{0}' debe estar entre {1} y {2}")]
        [UIHint("MonthSelector")]
        public int Mes { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Currency)]
        [UIHint("Currency")]
        public decimal Ingresos { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Currency)]
        [UIHint("Currency")]
        public decimal Gastos { get; set; }

        [Display(Name = "Fecha de informe")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaInforme { get; set; }

        //[Display(Name = "Añadir origen")]
        //public bool AddOrigen { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public TipoRegistro Tipo { get; set; }
    }

    public enum TipoRegistro
    {
        Directo,
        Indirecto,
        Estructuras,
        E9
    }
}

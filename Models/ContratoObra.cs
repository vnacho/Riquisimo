using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class ContratoObra : BaseModel
    {
        public Congress Congress { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Documento obra")]
        public Guid CongressID { get; set; }

        [Display(Name = "Código contrato")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [StringLength(20, ErrorMessage = "20 carácteres como máximo")]
        public string CodigoContrato { get; set; }

        [Display(Name = "Fecha contrato")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaContratoInicio { get; set; } = DateTime.Now;

        [Display(Name = "Importe del contrato")]
        public decimal ImporteContrato { get; set; } = 0;

        [Display(Name = "Fecha finalización")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [UIHint("Date")]
        public DateTime? FechaContratoFin { get; set; }

        public ICollection<DocumentoContratoObra> DocumentosContratoObra { get; set; } = new List<DocumentoContratoObra>();

        public void Calcular()
        {
            //ImporteDescuento = Math.Round(Unidades * BaseImponiblePrecioUnitario * Descuento / 100, 2);
            //BaseImponibleTotal = Unidades * BaseImponiblePrecioUnitario - ImporteDescuento;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            //if (UnidadesPendientes.HasValue && UnidadesPendientes.Value < Unidades)
            //    results.Add(new ValidationResult("No puede añadir más unidades que las que hay pendientes.", new string[] { "Unidades" }));
            return results;
        }
    }
}

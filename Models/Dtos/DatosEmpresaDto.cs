using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models.Dtos
{
    public class DatosEmpresaDto
    {
        [Display(Name = "Nombre de la empresa")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string Nombre { get; set; }

        [Display(Name = "Dirección de la empresa")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string Direccion { get; set; }

        [Display(Name = "Código postal")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(5)]
        [MinLength(5, ErrorMessage = "El campo '{0}' debe tener 5 caracteres.")]
        public string CodigoPostal { get; set; }

        [Display(Name = "Población")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string Poblacion { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string Provincia { get; set; }

        [Display(Name = "NIF/CIF")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string NifCif { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string Logo { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string Firma { get; set; }

        [Display(Name = "Nombre del representante")]
        public string NombreRepresentante { get; set; }

        [Display(Name = "NIF/CIF del representante")]
        public string NifCifRepresentante { get; set; }

    }
}

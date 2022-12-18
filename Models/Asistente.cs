using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Asistente
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "NIF/DNI")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(20)]
        [Index(IsUnique = true)]
        public string NIF { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [MaxLength(80)]
        public string Nombre { get; set; }
        
        [MaxLength(80)]
        public string Apellidos { get; set; }

        [Display(Name = "Centro de trabajo")]
        [MaxLength(80)]
        public string CentroTrabajo { get; set; }

        [Display(Name = "Categoría profesional")]
        [MaxLength(80)]
        public string CategoriaProfesional { get; set; }

        [MaxLength(80)]
        public string Cargo { get; set; }

        [Display(Name = "Fecha actualización del cargo")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaActualizacionCargo { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(80)]
        public string Direccion { get; set; }

        [Display(Name = "Población")]
        [MaxLength(80)]
        public string Poblacion { get; set; }

        [MaxLength(80)]
        public string Ciudad { get; set; }

        [Display(Name = "Teléfono 1")]
        [MaxLength(15)]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Telefono1 { get; set; }

        [Display(Name = "Teléfono 2")]
        [MaxLength(15)]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Telefono2 { get; set; }

        [Display(Name = "Correo electrónico 1")]
        [MaxLength(80)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email1 { get; set; }

        [Display(Name = "Correo electrónico 2")]
        [MaxLength(80)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email2 { get; set; }

        [Display(Name = "Asociación")]
        [MaxLength(80)]
        public string Asociacion { get; set; }

        [Display(Name = "Código país (SAGE)")]
        public string CodigoPais { get; set; }

        [Display(Name = "País")]
        public string NombrePais { get; set; }

        [Display(Name = "Código postal")]
        [MaxLength(5)]
        public string CodigoPostal { get; set; }

        [Display(Name = "Tratamiento")]
        public Guid? TreatmentId { get; set; }
        [Display(Name = "Tratamiento")]
        public Treatment Treatment { get; set; }

        public string Display => $"{Nombre} {Apellidos} {NIF}";

    }
}

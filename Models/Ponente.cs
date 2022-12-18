using Ferpuser.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ferpuser.Models
{
    public class Ponente
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Representa el id que viene importado de la base de datos externa (mysql)
        /// </summary>
        [Display(Name = "Id externo")]
        public string IdExterno { get; set; } = string.Empty;

        public string Login { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        public string NIF { get; set; }

        public string Localidad { get; set; }

        public string Provincia { get; set; }

        public string Cargo { get; set; }

        [Display(Name = "Centro de trabajo")]
        public string CentroTrabajo { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Display(Name = "Móvil")]
        public string Movil { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Mail { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Mail 2")]
        public string Mail2 { get; set; }

        public Tratamiento? Tratamiento { get; set; }

        [Display(Name = "Ámbito comité")]
        public AmbitoComite? AmbitoComite { get; set; }

        public bool Activo { get; set; } = true;

        public bool Visible { get; set; } = true;

        [Display(Name = "Último acceso")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? UltimoAcceso { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comentarios { get; set; }
       
        public bool Superevaluador { get; set; }
        public bool Visualizador { get; set; }

        [Display(Name = "Junta directiva")]
        public bool JuntaDirectiva { get; set; }

        [Display(Name = "Fecha de registro")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaRegistro { get; set; }

        [NotMapped]
        [Display(Name = "Tipo comité")]
        public int TipoComiteId { get; set; }
        [NotMapped]
        [Display(Name = "Tipo comité")]
        public TipoComite TipoComite { get; set; }

        [Display(Name = "Puesto comité")]
        [NotMapped]
        public int PuestoComiteId { get; set; }
        [Display(Name = "Puesto comité")]
        [NotMapped]
        public PuestoComite PuestoComite { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Congreso")]
        public Guid CongressId { get; set; }
        public Congress Congreso { get; set; }
    }   
}

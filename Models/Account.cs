using Ferpuser.Models.Enums;
using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Account : BaseModel
    {

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Contraseña")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Compare(nameof(Password), ErrorMessage = DataAnnotationsErrorMessages.CompareAttribute_MustMatch)]
        [Display(Name = "Repetir contraseña")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string Password2 { get; set; }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Enviar copia a este email")]
        public bool SendCopyTo { get; set; }

        [Display(Name = "Vendedor de Sage")]
        public string Vendedor { get; set; }

        [Display(Name = "Operario de Sage")]
        public string Operario { get; set; }

        [Display(Name = "Congreso")]
        public Guid? CongressId { get; set; }
        [Display(Name = "Congreso")]
        public Congress Congress { get; set; }

        [Display(Name = "Acceso a congresos")]
        public bool AccessCongress { get; set; }
        [Display(Name = "Acceso a colaboraciones")]
        public bool AccessCollaborations { get; set; }
        
        [Display(Name = "Servidor entrante")]
        public string IncomingMailServer { get; set; }
        [Display(Name = "Servidor saliente")]
        public string OutgoingMailServer { get; set; }
        [Display(Name = "Puerto entrante")]
        public int IncomingMailPort { get; set; }
        [Display(Name = "Puerto saliente")]
        public int OutgoingMailPort { get; set; }

        [Display(Name = "Usuario")]
        public string MailUser { get; set; }
        [Display(Name = "Contraseña")]
        public string MailPassword { get; set; }

        [Display(Name = "Pie de página antes imagen")]
        public string SignatureBefore { get; set; }

        [Display(Name = "Imagen de pie de página")]
        public string Signature { get; set; }

        [Display(Name = "Pie de página despues de imagen")]
        public string SignatureAfter { get; set; }

        #region PERMISOS DE USUARIO

        [Display(Name = "Administración")]
        public bool PermisoAdministracion { get; set; }

        [Display(Name = "Eventos")]
        public bool PermisoEventos { get; set; }

        [Display(Name = "Ventas")]
        public bool PermisoFacturacion { get; set; }

        [Display(Name = "Ventas (Nueva)")]
        public bool PermisoVentas { get; set; }

        [Display(Name = "Compras")]
        public bool PermisoCompras { get; set; }

        [Display(Name = "Control presupuestario")]
        public bool PermisoControlPresupuestario { get; set; }

        [Display(Name = "Control Almacen")]
        public bool PermisoControlAlmacen { get; set; }

        #endregion

        [Display(Name = "Perfil de usuario")]
        public PerfilUsuario PerfilUsuario { get; set; } = PerfilUsuario.Empleado;
    }

}

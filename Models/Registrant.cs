using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Registrant : BaseModel
    {

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        
        [Display(Name = "Apellidos")]
        public string Surnames { get; set; }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Tratamiento")]
        public Guid TreatmentId { get; set; }
        [Display(Name = "Tratamiento")]
        public Treatment Treatment { get; set; }

        [Display(Name = "Cargo")]
        public string Position { get; set; }
        [Display(Name = "Categoría profesional")]
        public string ProfessionalCategory { get; set; }
        
        [Display(Name = "Centro de trabajo")]
        public string Workplace { get; set; }

        [ForeignKey("Location")]
        [Display(Name = "Dirección principal")]
        public Guid? LocationId { get; set; }

        [Display(Name = "Dirección principal")]
        public RegistrantLocation Location { get; set; }
        
        [Display(Name = "NIF")]
        public string NIF { get; set; }
       
        [Display(Name = "Correo electrónico")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Correo electrónico alternativo")]
        [DataType(DataType.EmailAddress)]
        public string Email2 { get; set; }

        public string Laboratorio { get; set; }
        public string Especialidad { get; set; }


        [Display(Name = "Categoría")]
        public string Category { get; set; }

        [Display(Name = "Categoría")]
        public string CategoriaInscritoId { get; set; }

        [Display(Name = "Categoría")]
        public CategoriaInscrito CategoriaInscrito { get; set; }

        public string FullName
        {
            get
            {
                if (Name != null && Name.Length > 0 && Surnames != null && Surnames.Length > 0)
                {
                    return Surnames + ", " + Name;
                } else if (Name != null && Name.Length > 0)
                {
                    return Name;
                } else if (Surnames != null && Surnames.Length > 0)
                {
                    return Surnames;
                } else
                {
                    return "Sin nombre";
                }
            }
        }
    }
}
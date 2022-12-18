using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class DocumentoContratoObra
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Url del fichero")]
        [UIHint("File")]
        public string FicheroUrl { get; set; }

        [Display(Name = "Fichero")]
        public string FicheroNombre { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Contrato obra")]
        public Guid ContratoObraId { get; set; }
        [Display(Name = "Contrato obra")]
        public ContratoObra ContratoObra { get; set; }
    }
}

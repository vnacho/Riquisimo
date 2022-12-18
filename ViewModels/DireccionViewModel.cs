using System.ComponentModel.DataAnnotations;

namespace Ferpuser.ViewModels
{
    public class DireccionViewModel
    {
        [Display(Name = "Dirección")]
        public int? LineaEnvCli { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Código postal")]
        public string CodigoPostal { get; set; }

        [Display(Name = "Población")]
        public string Poblacion { get; set; }

        [Display(Name = "Provincia")]
        public string Provincia { get; set; }

        public override string ToString()
        {
            return $"{Direccion} {CodigoPostal} {Poblacion} {Provincia}";
        }
    }
}

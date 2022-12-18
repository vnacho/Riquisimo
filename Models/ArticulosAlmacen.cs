using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class ArticulosAlmacen : BaseModel
    {

        [Display(Name = "Código del artículo")]
        [MaxLength(20)]
        public string ProductCode { get; set; }

        [Display(Name = "Descripción del artículo")]
        [MaxLength(40)]
        public string ProductDescription { get; set; } = "";

        [Display(Name = "TTF_1")]
        [MaxLength(3)]
        public string Rate { get; set; } = "";

        [Display(Name = "TTF_2")]
        [MaxLength(3)]
        public string Rate2 { get; set; } = "";

        [Display( Name = "Descripción ampliada del artículo" )]
        public string ProductDescriptionExt { get; set; } = "";

        [Display(Name = "Precio")]
        public decimal Price { get; set; } = 0;

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Centro Coste")]
        public int CentroCosteId { get; set; } = 0;

        public CentroCoste CentroCoste { get; set; }

        public string Display => $"{ProductCode}-{ProductDescription}";

    }
}

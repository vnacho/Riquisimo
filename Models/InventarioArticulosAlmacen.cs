using Ferpuser.Migrations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class InventarioArticulosAlmacen : BaseModel
    {
        public ArticulosAlmacen ArticulosAlmacen { get; set; }

        [Display(Name = "Articulo Almacen")]
        public Guid ArticulosAlmacenId { get; set; }

        [Display(Name = "Fecha Última Actualización")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaUltiActu { get; set; }

        //[Display(Name = "Descripción del artículo")]
        //[MaxLength(40)]
        //public string ProductDescription { get; set; } = "";

        //[Display(Name = "TTF_1")]
        //[MaxLength(3)]
        //public string Rate { get; set; } = "";

        //[Display(Name = "TTF_2")]
        //[MaxLength(3)]
        //public string Rate2 { get; set; } = "";

        //[Display( Name = "Descripción ampliada del artículo" )]
        //public string ProductDescriptionExt { get; set; } = "";

        [Display(Name = "CANTIDAD")]
        public decimal Unidades { get; set; } = 0;

        //[Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        //[Display(Name = "Centro Coste")]
        //public int CentroCosteId { get; set; } = 0;

        //public CentroCoste CentroCoste { get; set; }

    }
}

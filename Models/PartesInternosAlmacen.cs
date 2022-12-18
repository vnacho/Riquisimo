using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class PartesInternosAlmacen : BaseModel
    {
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime fecha { get; set; }

        public ArticulosAlmacen ArticulosAlmacen { get; set; }

        [Display(Name = "Articulo Almacen")]
        public Guid ArticulosAlmacenId { get; set; }

        [Display(Name = "TTF_1")]
        public decimal TariffTypeUnits { get; set; }
        [Display(Name = "TTF_2")]
        public decimal TariffTypeUnits2 { get; set; }

        [Display(Name = "Precio")]
        public decimal Price { get; set; }

        [Display(Name = "Importe")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Destino")]
        public int DestinoId { get; set; }

        [Display(Name = "Destino")]
        public CentroCoste Destino { get; set; }

        public void Calcular()
        {
            if (TariffTypeUnits == 0 && TariffTypeUnits2 == 0)
                Amount = 0;
            else
                Amount = Math.Round((TariffTypeUnits == 0 ? 1 : TariffTypeUnits) * (TariffTypeUnits2 == 0 ? 1 : TariffTypeUnits2) * Price, 2);
        }
    }
}

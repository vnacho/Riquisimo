using Ferpuser.Migrations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models
{
    public class MovimientosArticulosAlmacen: BaseModel
    {
        public ArticulosAlmacen ArticulosAlmacen { get; set; }

        [Display(Name = "Articulo Almacen")]
        public Guid ArticulosAlmacenId { get; set; }

        [Display(Name = "Fecha Movimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaMovimiento { get; set; }

        [Display(Name = "Entrada/Salida")]
        [MaxLength(1)]
        public string movimiento { get; set; }

        [Display(Name = "CANTIDAD")]
        public decimal Unidades { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Centro Coste")]
        public int CentroCosteId { get; set; }

        public CentroCoste CentroCoste { get; set; }

        public TipoMovimiento TipoMovimiento => movimiento == "E" ? TipoMovimiento.Entrada : TipoMovimiento.Salida;

    }

    public enum TipoMovimiento
    {
        Entrada,
        Salida
    }
}


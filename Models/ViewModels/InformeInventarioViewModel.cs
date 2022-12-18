using Ferpuser.Models.PartialModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Ferpuser.Models.ViewModels
{
    public class InformeInventarioViewModel
    {
        //[Display(Name = "Mes")]
        //[Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = false)]
        //[Range(1, 12, ErrorMessage = "El campo '{0}' debe estar entre {1} y {2}")]
        //[UIHint("MonthSelector")]
        //public int? Month { get; set; }

        //[Display(Name = "Año")]
        //[Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = false)]
        //[Range(2000, 2100, ErrorMessage = "El campo '{0}' debe estar entre {1} y {2}")]
        //public int? Year { get; set; }

        [Display(Name = "Centro de coste")]
        public int? CentroCosteId { get; set; }

        [Display(Name = "Fecha desde")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = false)]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaDesde { get; set; }
        public IEnumerable<MovimientosInventarioModel> Items { get; set; }
        public IEnumerable<MovimientosArticulosAlmacen> ItemsMovimientos { get; set; }

        //public Expression<Func<CongressCarteraObraModel, bool>> ExpressionFilter()
        //{
        //    return f =>
        //        (!f.Finalizada &&
        //        f.TipoCongress.Contains("O"));
        //}

        //public IEnumerable<Origen> Origenes { get; set; }

        //Totales

        public void Calculate()
        {

            foreach (var item in Items)
            {
                item.Salidas = ItemsMovimientos.Where(f => f.movimiento.Contains('S')).Sum(f => f.Unidades);
                item.Entradas = ItemsMovimientos.Where(f => f.movimiento.Contains('E')).Sum(f => f.Unidades);
                item.ExFinal = item.ExInicial - item.Salidas + item.Entradas;
            }
        }

        //public string GetMonthName(int? month)
        //{
        //    if (!month.HasValue)
        //        return string.Empty;

        //    DateTime dt = new DateTime(2000, month.Value, 1);
        //    return dt.ToString("MMMM");
        //}


    }
}

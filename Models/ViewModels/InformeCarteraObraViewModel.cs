using Ferpuser.Models.PartialModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Ferpuser.Models.ViewModels
{
    public class InformeCarteraObraViewModel 
    {
        [Display(Name = "Mes")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = false)]
        [Range(1, 12, ErrorMessage = "El campo '{0}' debe estar entre {1} y {2}")]
        [UIHint("MonthSelector")]
        public int? Month { get; set; }

        [Display(Name = "Año")]
        [Required(ErrorMessage = "El campo '{0}' es obligatorio", AllowEmptyStrings = false)]
        [Range(2000, 2100, ErrorMessage = "El campo '{0}' debe estar entre {1} y {2}")]
        public int? Year { get; set; }
        public IEnumerable<CongressCarteraObraModel> Items { get; set; }
        public Expression<Func<CongressCarteraObraModel, bool>> ExpressionFilter()
        {
            return f =>
                (!f.Finalizada &&
                f.TipoCongress.Contains("O"));
        }

        public IEnumerable<Origen> Origenes { get; set; }

        //Totales
        public decimal TotalPresupuesto { get; set; }
        public decimal TotalEjecutado { get; set; }
        public decimal TotalResultado { get; set; }
        public decimal TotalPorcentaje { get; set; }
        public decimal TotalPendiente { get; set; }

        public void Calculate()
        {
            TotalEjecutado = 0;
            TotalPendiente = 0;
            TotalPorcentaje = 0;
            TotalPresupuesto = 0;
            TotalResultado = 0;
            decimal ImporteCostes = 0;
            string nivelAnalitico2 = "";

            foreach(var item in Items)
            {
                item.ImportePresupuesto = item.ContratosObra.Sum(f => f.ImporteContrato);
                nivelAnalitico2 = item.Number.ToString();
                item.ImporteEjecutado = Origenes.Where(f => f.NivelAnalitico2.Equals(nivelAnalitico2)).Sum(f => f.Ingresos);
                ImporteCostes = Origenes.Where(f => f.NivelAnalitico2.Equals(nivelAnalitico2)).Sum(f => f.Gastos);
                item.ImporteResultado = item.ImporteEjecutado - ImporteCostes;

                if (item.ImporteResultado != 0)
                    item.ImportePorcentaje = item.ImporteResultado / item.ImporteEjecutado * 100;
                else
                    item.ImportePorcentaje = 0;

                item.ImportePendiente = item.ImportePresupuesto - item.ImporteEjecutado;

                TotalPresupuesto += item.ImportePresupuesto;
                TotalEjecutado += item.ImporteEjecutado;
                TotalResultado += item.ImporteResultado;
                TotalPorcentaje += item.ImportePorcentaje;
                TotalPendiente += item.ImportePendiente;
                if (item.NombreCliente == null)
                    item.NombreCliente = "";
            }
        }

        public string GetMonthName(int? month)
        {
            if (!month.HasValue)
                return string.Empty;

            DateTime dt = new DateTime(2000, month.Value, 1);
            return dt.ToString("MMMM");
        }

    }


    //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //{
    //    List<ValidationResult> errores = new List<ValidationResult>();
    //    if (FechaDesde.HasValue && FechaDesde.HasValue)
    //    {
    //        int Dias = FechaHasta.Value.Subtract(FechaDesde.Value).Days;
    //        if (Dias > 31)
    //            errores.Add(new ValidationResult("No se pueden consultar más de 31 días de diferencia.", new string[] { "FechaDesde", "FechaHasta" }));

    //        if (FechaDesde.Value > FechaHasta.Value)
    //            errores.Add(new ValidationResult("La fecha hasta no puede ser menor que la fecha desde.", new string[] { "FechaHasta" }));
    //    }
    //    return errores;
    //}
}

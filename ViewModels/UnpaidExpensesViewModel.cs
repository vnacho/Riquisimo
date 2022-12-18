using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.ViewModels
{
    public class UnpaidExpensesViewModel
    {
        public Guid? Id { get; set; }
        public string ExpenseType { get; set; }
        public string Factura { get; set; }
        public DateTime? Fecha { get; set; }
        public string FechaString { get => Fecha?.ToString("yyyy-MM-dd"); }
        public string Cliente { get; set; }
        public string Almacen { get; set; }
        public string Descripcion { get; set; }
        public decimal Pending { get; set; }
        public string Nombre { get; internal set; }
        public bool Cobrada { get; set; }

        [Display(Name="Retención")]
        public bool TieneRetencion { get; set; }
        
        public string PendingDisplay => Pending.ToString("C");
    }
}

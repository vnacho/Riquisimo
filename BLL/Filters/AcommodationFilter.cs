using Ferpuser.Models;
using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class AcommodationFilter
    {
        [Display(Name = "Estado")]
        public EstadoCostCenterProduct? Estado { get; set; }

        [Display(Name = "Número de documento")]
        public int? Number { get; set; }

        [Display(Name = "Nº de factura")]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Persona inscrita")]
        public string Registrant { get; set; }

        [Display(Name = "Congreso")]
        public string Congress { get; set; }

        [Display(Name = "Evento")]
        public string CodigoEvento { get; set; }

        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Fecha { get; set; }

        //[Display(Name = "Fecha desde")]
        //public DateTime? FechaDesde { get; set; }

        //[Display(Name = "Fecha hasta")]
        //public DateTime? FechaHasta { get; set; }

        [Display(Name = "Nombre")]
        public string RoomType { get; set; }

        [Display(Name = "Número de ocupantes")]
        public int? NumeroOcupantes { get; set; }

        //[Display(Name = "Precio")]
        //public decimal? Fee { get; set; }

        [Display(Name = "Cliente")]
        public string Cliente { get; set; }

        [Display(Name = "Cliente")]
        public Guid? ClientId { get; set; }

        public Expression<Func<Accommodation, bool>> ExpressionFilter()
        {
            return f =>
                (
                    !Estado.HasValue ? true : 
                        (Estado == EstadoCostCenterProduct.Pagado ? f.Paid : true) &&
                        (Estado == EstadoCostCenterProduct.Facturado ? !f.Paid && f.Exported : true) &&
                        (Estado == EstadoCostCenterProduct.Revisado ? !f.Paid && !f.Exported && f.Reviewed : true) &&
                        (Estado == EstadoCostCenterProduct.Importado ? !f.Paid && !f.Exported && !f.Reviewed && f.Imported : true) &&
                        (Estado == EstadoCostCenterProduct.Creado ? !f.Paid && !f.Exported && !f.Reviewed && !f.Imported : true)
                ) &&
                (Number.HasValue ? f.Number == Number : true) &&
                (string.IsNullOrWhiteSpace(InvoiceNumber) ? true : f.InvoiceNumber.Contains(InvoiceNumber)) &&
                (string.IsNullOrWhiteSpace(Registrant) ? true : f.Registrant.Name.Contains(Registrant) || f.Registrant.Surnames.Contains(Registrant)) &&
                (string.IsNullOrWhiteSpace(Congress) ? true : 
                    f.Congress.Number.ToString().Contains(Congress) ||
                    f.Congress.Name.Contains(Congress) ||
                    f.Congress.Code.Contains(Congress)
                    ) &&
                (Fecha.HasValue ? f.StartDate <= Fecha.Value && f.EndDate >= Fecha.Value : true) &&
                (string.IsNullOrWhiteSpace(RoomType) ? true : f.RoomType.Name.Contains(RoomType)) &&
                (string.IsNullOrWhiteSpace(CodigoEvento) ? true : f.Congress.Number.ToString().Contains(CodigoEvento)) &&
                (ClientId.HasValue ? f.ClientId == ClientId : true) &&
                (NumeroOcupantes.HasValue ? f.RoomType.Occupants == NumeroOcupantes : true) &&
                (string.IsNullOrWhiteSpace(Cliente) ? true : f.Client.BusinessName.Contains(Cliente));
        }
    }
}

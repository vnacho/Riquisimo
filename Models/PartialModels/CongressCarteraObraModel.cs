using IbanNet;
using IbanNet.DataAnnotations;
using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.PartialModels
{
    public class CongressCarteraObraModel
    {

        [Display(Name = "Código")]
        public int Number { get; set; }

        [Display(Name = "Identificador interno")]
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Clave")]
        public string Code { get; set; }

        [Display(Name = "Lugar")]
        public string Place { get; set; }

        [Display(Name = "Fecha de inicio")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de finalización")]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);

        [Display(Name = "Tipo evento")]
        [StringLength(1)]
        public string TipoCongress { get; set; }

        [Display(Name = "Cliente")]
        public string NombreCliente { get; set; }

        [Display(Name = "Fecha inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaInicio { get; set; } = DateTime.Now;

        [Display(Name = "Finalizada")]
        public bool Finalizada { get; set; }
        public ICollection<ContratoObra> ContratosObra { get; set; }
        public decimal ImportePresupuesto { get; set; } = 0;
        public decimal ImporteEjecutado { get; set; } = 0;
        public decimal ImporteResultado { get; set; } = 0;
        public decimal ImportePorcentaje { get; set; } = 0;
        public decimal ImportePendiente { get; set; } = 0;

    }
}


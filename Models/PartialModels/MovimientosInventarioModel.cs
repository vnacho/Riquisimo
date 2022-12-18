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
    public class MovimientosInventarioModel
    {

        [Display(Name = "Código del artículo")]
        [MaxLength(20)]
        public string ProductCode { get; set; }

        [Display(Name = "Descripción del artículo")]
        [MaxLength(40)]
        public string ProductDescription { get; set; } = "";

        public decimal ExInicial { get; set; } = 0;
        public decimal Salidas { get; set; } = 0;
        public decimal Entradas { get; set; } = 0;
        public decimal ExFinal { get; set; } = 0;

    }
}

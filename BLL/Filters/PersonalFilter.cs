using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class PersonalFilter 
    {
        public string Nombre { get; set; }

        [Display(Name = "Categoría")]
        [MaxLength(40)]
        public string Categoria { get; set; }

        [Display(Name = "Fecha de alta")]
        public bool? TieneFechaAlta { get; set; }

        [Display(Name = "Fecha de baja")]
        public bool? TieneFechaBaja { get; set; }

        [Display(Name = "Fecha apto")]
        public bool? TieneFechaApto { get; set; }

        [Display(Name = "Fecha EPI")]
        public bool? TieneFechaEPI { get; set; }

        [Display(Name = "IBAN")]
        public bool? TieneIBAN { get; set; }

        public bool? CT { get; set; }

        [Display(Name = "Obra")]
        public int? ObraId { get; set; }

        [Display(Name = "Tipo último contrato")]
        public int? TipoUltimoContratoId { get; set; }

        [Display(Name = "Fecha desde")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaDesde { get; set; }

        [Display(Name = "Fecha hasta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaHasta { get; set; }
        [Display(Name = "Fecha EPI Des.")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaDesdeEntEPI { get; set; }
        [Display(Name = "Fecha EPI Has.")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FechaHastaEntEPI { get; set; }

        public Expression<Func<Personal, bool>> ExpressionFilter()
        {
            return f =>
               (TieneFechaAlta.HasValue ? f.FechaAlta.HasValue == TieneFechaAlta : true) &&
               (TieneFechaBaja.HasValue ? f.FechaBaja.HasValue == TieneFechaBaja : true) &&
               (TieneFechaApto.HasValue ? f.FechaApto.HasValue == TieneFechaApto : true) &&
               (TieneFechaEPI.HasValue ? f.FechaEntregaEpi.HasValue == TieneFechaEPI : true) &&
               (FechaDesde.HasValue ? f.FechaApto >= FechaDesde : true) &&
               (FechaHasta.HasValue ? f.FechaApto <= FechaHasta : true) &&
               (FechaDesdeEntEPI.HasValue ? f.FechaEntregaEpi >= FechaDesdeEntEPI : true) &&
               (FechaHastaEntEPI.HasValue ? f.FechaEntregaEpi <= FechaHastaEntEPI : true) &&
               (TieneIBAN.HasValue ? string.IsNullOrWhiteSpace(f.IBAN) != TieneIBAN : true) &&
               (CT.HasValue ? f.CT == CT : true) &&
               (ObraId.HasValue ? f.ObraId == ObraId : true) &&
               (TipoUltimoContratoId.HasValue ? f.TipoUltimoContratoId == TipoUltimoContratoId : true) &&
               (string.IsNullOrWhiteSpace(Nombre) ? true : f.Nombre.Contains(Nombre.Trim())) &&
               (string.IsNullOrWhiteSpace(Categoria) ? true : f.Categoria.Contains(Categoria.Trim())
               );

            //return f =>
            //    (TieneFechaAlta.HasValue ? (TieneFechaAlta.Value ? f.FechaAlta != null : f.FechaAlta == null) : true) &&
            //    (TieneFechaBaja.HasValue ? f.FechaBaja.HasValue == TieneFechaBaja : true) &&
            //    (TieneFechaApto.HasValue ? f.FechaApto.HasValue == TieneFechaApto : true) &&
            //    (TieneIBAN.HasValue ? string.IsNullOrWhiteSpace(f.IBAN) != TieneIBAN : true) &&
            //    (CT.HasValue ? f.CT == CT : true) &&
            //    (CentroCosteId.HasValue ? f.CentroCosteId == CentroCosteId : true) &&
            //    (string.IsNullOrWhiteSpace(Nombre) ? true : f.Nombre.Contains(Nombre.Trim())) &&
            //    (string.IsNullOrWhiteSpace(Categoria) ? true : f.Categoria.Contains(Categoria.Trim())
            //    );
        }

        //[Display(Name = "Fecha de alta desde")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //public DateTime? FechaAltaDesde { get; set; }

        //[Display(Name = "Fecha de alta hasta")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //public DateTime? FechaAltaHasta { get; set; }

        //[Display(Name = "Fecha de baja desde")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //public DateTime? FechaBajaDesde { get; set; }

        //[Display(Name = "Fecha de baja hasta")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //public DateTime? FechaBajaHasta { get; set; }

        //public Expression<Func<Personal, bool>> ExpressionFilter()
        //{

        //    return f =>
        //        (string.IsNullOrWhiteSpace(Nombre) ? true : f.Nombre.Contains(Nombre.Trim())) &&
        //        (string.IsNullOrWhiteSpace(Categoria) ? true : f.Categoria.Contains(Categoria.Trim()) &&
        //        (FechaAltaDesde.HasValue ? f.FechaAlta >= FechaAltaDesde.Value : true) &&
        //        (FechaAltaHasta.HasValue ? f.FechaAlta <= FechaAltaHasta.Value : true) &&
        //        (FechaBajaDesde.HasValue ? f.FechaBaja >= FechaBajaDesde.Value : true) &&
        //        (FechaBajaHasta.HasValue ? f.FechaBaja <= FechaBajaHasta.Value : true)
        //        );
        //}   

    }
}

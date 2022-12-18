using Ferpuser.Models.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.ViewModels
{
    public class InformeResumenEmpresaViewModel
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

        public IEnumerable<InformeResumenEmpresaDto> ItemsDirectos { get; set; }
        public IEnumerable<InformeResumenEmpresaDto> ItemsIndirectos { get; set; }
        public IEnumerable<InformeResumenEmpresaDto> ItemsEstructuras { get; set; }
        public IEnumerable<InformeResumenEmpresaDto> ItemsE9 { get; set; }

        //Directos
        public decimal DirectosHaberMesTotal { get; set; }
        public decimal DirectosDebeMesTotal { get; set; }
        public decimal DirectosRDTOMesTotal { get; set; }
        public decimal DirectosTasaMesTotal { get; set; }
        public decimal DirectosHaberAnioTotal { get; set; }
        public decimal DirectosDebeAnioTotal { get; set; }
        public decimal DirectosRDTOAnioTotal { get; set; }
        public decimal DirectosTasaAnioTotal { get; set; }

        //Indirectos
        public decimal IndirectosHaberMesTotal { get; set; }
        public decimal IndirectosDebeMesTotal { get; set; }
        public decimal IndirectosRDTOMesTotal { get; set; }
        public decimal IndirectosTasaMesTotal { get; set; }
        public decimal IndirectosHaberAnioTotal { get; set; }
        public decimal IndirectosDebeAnioTotal { get; set; }
        public decimal IndirectosRDTOAnioTotal { get; set; }
        public decimal IndirectosTasaAnioTotal { get; set; }

        //Estructuras
        public decimal EstructurasHaberMesTotal { get; set; }
        public decimal EstructurasDebeMesTotal { get; set; }
        public decimal EstructurasRDTOMesTotal { get; set; }
        public decimal EstructurasTasaMesTotal { get; set; }
        public decimal EstructurasHaberAnioTotal { get; set; }
        public decimal EstructurasDebeAnioTotal { get; set; }
        public decimal EstructurasRDTOAnioTotal { get; set; }
        public decimal EstructurasTasaAnioTotal { get; set; }

        //E9
        public decimal E9HaberMesTotal { get; set; }
        public decimal E9DebeMesTotal { get; set; }
        public decimal E9RDTOMesTotal { get; set; }
        public decimal E9TasaMesTotal { get; set; }
        public decimal E9HaberAnioTotal { get; set; }
        public decimal E9DebeAnioTotal { get; set; }
        public decimal E9RDTOAnioTotal { get; set; }
        public decimal E9TasaAnioTotal { get; set; }

        //Resultado Real
        public decimal ResultadoRealRDTOMesTotal { get; set; }
        public decimal ResultadoRealTasaMesTotal { get; set; }
        public decimal ResultadoRealRDTOAnioTotal { get; set; }
        public decimal ResultadoRealTasaAnioTotal { get; set; }

        //Resultado Contable
        public decimal ResultadoContableRDTOMesTotal { get; set; }
        public decimal ResultadoContableTasaMesTotal { get; set; }
        public decimal ResultadoContableRDTOAnioTotal { get; set; }
        public decimal ResultadoContableTasaAnioTotal { get; set; }

        public void Calculate()
        {
            //Directos
            DirectosHaberMesTotal = ItemsDirectos.Sum(f => f.HaberMes);
            DirectosDebeMesTotal = ItemsDirectos.Sum(f => f.DebeMes);
            DirectosRDTOMesTotal = ItemsDirectos.Sum(f => f.RTDOMes);
            DirectosTasaMesTotal = DirectosHaberMesTotal == 0 ? 0 : DirectosRDTOMesTotal / DirectosHaberMesTotal * 100;
            DirectosHaberAnioTotal = ItemsDirectos.Sum(f => f.HaberAnio);
            DirectosDebeAnioTotal = ItemsDirectos.Sum(f => f.DebeAnio);
            DirectosRDTOAnioTotal = ItemsDirectos.Sum(f => f.RTDOAnio);
            DirectosTasaAnioTotal = DirectosHaberAnioTotal == 0 ? 0 : DirectosRDTOAnioTotal / DirectosHaberAnioTotal * 100;

            //Indirectos
            IndirectosHaberMesTotal = ItemsIndirectos.Sum(f => f.HaberMes);
            IndirectosDebeMesTotal = ItemsIndirectos.Sum(f => f.DebeMes);
            IndirectosRDTOMesTotal = ItemsIndirectos.Sum(f => f.RTDOMes);
            IndirectosTasaMesTotal = IndirectosHaberMesTotal == 0 ? 0 : IndirectosRDTOMesTotal / IndirectosHaberMesTotal * 100;
            IndirectosHaberAnioTotal = ItemsIndirectos.Sum(f => f.HaberAnio);
            IndirectosDebeAnioTotal = ItemsIndirectos.Sum(f => f.DebeAnio);
            IndirectosRDTOAnioTotal = ItemsIndirectos.Sum(f => f.RTDOAnio);
            IndirectosTasaAnioTotal = IndirectosHaberAnioTotal == 0 ? 0 : IndirectosRDTOAnioTotal / IndirectosHaberAnioTotal * 100;

            //Estructuras
            EstructurasHaberMesTotal = ItemsEstructuras.Sum(f => f.HaberMes);
            EstructurasDebeMesTotal = ItemsEstructuras.Sum(f => f.DebeMes);
            EstructurasRDTOMesTotal = ItemsEstructuras.Sum(f => f.RTDOMes);
            EstructurasTasaMesTotal = EstructurasHaberMesTotal == 0 ? 0 : EstructurasRDTOMesTotal / EstructurasHaberMesTotal * 100;
            EstructurasHaberAnioTotal = ItemsEstructuras.Sum(f => f.HaberAnio);
            EstructurasDebeAnioTotal = ItemsEstructuras.Sum(f => f.DebeAnio);
            EstructurasRDTOAnioTotal = ItemsEstructuras.Sum(f => f.RTDOAnio);
            EstructurasTasaAnioTotal = EstructurasHaberAnioTotal == 0 ? 0 : EstructurasRDTOAnioTotal / EstructurasHaberAnioTotal * 100;

            //E9
            E9HaberMesTotal = ItemsE9.Sum(f => f.HaberMes);
            E9DebeMesTotal = ItemsE9.Sum(f => f.DebeMes);
            E9RDTOMesTotal = ItemsE9.Sum(f => f.RTDOMes);
            E9TasaMesTotal = E9HaberMesTotal == 0 ? 0 : E9RDTOMesTotal / E9HaberMesTotal * 100;
            E9HaberAnioTotal = ItemsE9.Sum(f => f.HaberAnio);
            E9DebeAnioTotal = ItemsE9.Sum(f => f.DebeAnio);
            E9RDTOAnioTotal = ItemsE9.Sum(f => f.RTDOAnio);
            E9TasaAnioTotal = E9HaberAnioTotal == 0 ? 0 : E9RDTOAnioTotal / E9HaberAnioTotal * 100;

            //Resultado real
            ResultadoRealRDTOMesTotal = DirectosRDTOMesTotal + IndirectosRDTOMesTotal + EstructurasRDTOMesTotal;
            ResultadoRealTasaMesTotal = DirectosHaberMesTotal == 0 ? 0 : ResultadoRealRDTOMesTotal / DirectosHaberMesTotal * 100;
            ResultadoRealRDTOAnioTotal = DirectosRDTOAnioTotal + IndirectosRDTOAnioTotal + EstructurasRDTOAnioTotal;
            ResultadoRealTasaAnioTotal = DirectosHaberAnioTotal == 0 ? 0 : ResultadoRealRDTOAnioTotal / DirectosHaberAnioTotal * 100;

            //Resultado contable
            ResultadoContableRDTOMesTotal = ResultadoRealRDTOMesTotal + E9RDTOMesTotal;
            ResultadoContableTasaMesTotal = DirectosHaberMesTotal == 0 ? 0 : ResultadoContableRDTOMesTotal / DirectosHaberMesTotal * 100;
            ResultadoContableRDTOAnioTotal = ResultadoRealRDTOAnioTotal + E9RDTOAnioTotal;
            ResultadoContableTasaAnioTotal = DirectosHaberAnioTotal == 0 ? 0 : ResultadoContableRDTOAnioTotal / DirectosHaberAnioTotal * 100;
        }

        public string GetMonthName(int? month)
        {
            if (!month.HasValue)
                return string.Empty;

            DateTime dt = new DateTime(2000, month.Value,1);
            return dt.ToString("MMMM");
        }

    }
}

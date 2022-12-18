using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Sage
{
    public partial class C_Albven
    {
        public string Usuario { get; set; }
        public string Empresa { get; set; }
        public string Numero { get; set; }
        public DateTime? Fecha { get; set; }
        public string Cliente { get; set; }
        public int? Env_Cli { get; set; }
        public string Presup { get; set; }
        public decimal? Pronto { get; set; }
        public string Vendedor { get; set; }
        public string Ruta { get; set; }
        public string Almacen { get; set; }
        public bool? Iva_Inc { get; set; }
        public string Factura { get; set; }
        public DateTime? Fecha_Fac { get; set; }
        public string Asi { get; set; }
        public string Fpag { get; set; }
        public decimal? Importe { get; set; }
        public string Observacio { get; set; }
        public int? Banc_Cli { get; set; }
        public string Divisa { get; set; }
        public decimal? Cambio { get; set; }
        public decimal? Impdivisa { get; set; }
        public decimal? Finan { get; set; }
        public bool? Vista { get; set; }
        public decimal? Coste { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Litros { get; set; }
        public string Obra { get; set; }
        public bool? Traspasado { get; set; }
        public bool? Recequiv { get; set; }
        public bool? Tag { get; set; }
        public string Operario { get; set; }
        public string Letra { get; set; }
        public bool? Facturable { get; set; }
        public string Pedido { get; set; }
        public decimal? Cot_Punt { get; set; }
        public decimal? Puntos { get; set; }
        public bool? Comms { get; set; }
        public bool? Send_Fra { get; set; }
        public string Clifinal { get; set; }
        public string Keycopy { get; set; }
        public bool? Impreso { get; set; }
        public string Libre_1 { get; set; }
        public string Libre_2 { get; set; }
        public string Libre_3 { get; set; }
        public int? Certific { get; set; }
        public decimal? Stock_Coef { get; set; }
        public decimal? Tpcretnofi { get; set; }
        public bool? Edi { get; set; }
        public bool? Gastos { get; set; }
        public int? Enviado { get; set; }
        public int? Libre_4 { get; set; }
        public DateTime? Fechastock { get; set; }
        public DateTime? Exportar { get; set; }
        public string Mandato { get; set; }
        public string Clienteerp { get; set; }
        public bool? Recc { get; set; }
        public string Guid_Exp { get; set; }
        public decimal? Totaldoc { get; set; }
        public string Codpost { get; set; }
        public string Canal { get; set; }
        public bool? Trasperp { get; set; }
        [Key]
        public string Guid_Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public decimal? Totaldiv { get; set; }
        public decimal? Porcen_Ret { get; set; }
        public int? Calculo { get; set; }
        public string Descfac { get; set; }
        public string Refercli { get; set; }
        public bool? Fradirecta { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.Models.Sage
{
    public partial class Asientos
    {
        public string Usuario { get; set; }
        [Key]
        public string Empresa { get; set; }
        [Key]
        public int Numero { get; set; }
        public string Cuenta { get; set; }
        public DateTime? Fecha { get; set; }
        public string Definicion { get; set; }
        public decimal? Debe { get; set; }
        public decimal? Haber { get; set; }
        public string Tipo { get; set; }
        public bool? Punteo { get; set; }
        public string Factura { get; set; }
        public string Proveedor { get; set; }
        public string Asi { get; set; }
        [Key]
        public int Linea { get; set; }
        public decimal? Importediv { get; set; }
        public string Divisa { get; set; }
        public bool? Vista { get; set; }
        public decimal? Debediv { get; set; }
        public decimal? Haberdiv { get; set; }
        public decimal? Cambio { get; set; }
        public int? Arqueo { get; set; }
        public string Referencia { get; set; }
        public string Archivo { get; set; }
        public bool? Cierre89 { get; set; }
        public string Libro { get; set; }
        //public string Libre1 { get; set; }
        //public decimal? Libre2 { get; set; }
        public string Guid { get; set; }
        public DateTime? Fcreado { get; set; }
        public int? Operacion { get; set; }
        public DateTime? Concifecha { get; set; }
        public bool? Conciaut { get; set; }
        //public string GuidId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public bool? Capture { get; set; }
        //public int? TipoMov { get; set; }
        public bool? Isv { get; set; }
        public DateTime? Exportar { get; set; }
        public string Conciconcep { get; set; }
        public bool? Suplido { get; set; }
        public int? Dts { get; set; }
        public bool? Manual { get; set; }
        public bool? Entradaman { get; set; }
    }
}

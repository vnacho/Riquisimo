using Ferpuser.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class DocumentoCompraVenta
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Url del fichero")]
        [UIHint("File")]
        public string FicheroUrl { get; set; }

        [Display(Name = "Fichero")]
        public string FicheroNombre { get; set; }
        //Tendrá como valor dos caracteres con la finalidad de agrupar archivos de distintas Tablas
        //Compra Factura (CF)
        //Compra Pedido (CP)
        //Venta Factura (VF)
        //Venta Pedido (VP)
        [MaxLength(2)]
        public string ClaveDoc { get; set; }
        //Id de la tabla que corresponda, de momento:
        //ComprasPedido, ComprasFacura, VentasPedido, VentasFactura
        public int IdTabla { get; set; }
    }
}

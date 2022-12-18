using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class ProveedorFilter
    {
        [Display(Name = "NIF/DNI")]
        [MaxLength(17)]        
        public string NIF { get; set; }        

        [Display(Name = "Razón Social")]
        [MaxLength(120)]
        public string NombreComercial { get; set; }

        [Display(Name = "Nombre Comercial")]
        [MaxLength(80)]
        public string Domicilio { get; set; }

        //public string CuentaContable { get; set; }
        public Expression<Func<Proveedor, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(NIF) ? true : f.NIF.Contains(NIF.Trim())) &&
                (string.IsNullOrWhiteSpace(NombreComercial) ? true : f.NOMBRECOMERCIAL.Contains(NombreComercial.Trim())) &&
                (string.IsNullOrWhiteSpace(Domicilio) ? true : f.RAZONSOCIAL.Contains(Domicilio.Trim()));
               // (string.IsNullOrWhiteSpace(CuentaContable) ? true : f.CUENTACONTABLE.Contains(CuentaContable.Trim())) &&
        }
    }
}

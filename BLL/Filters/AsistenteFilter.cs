using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Filters
{
    public class AsistenteFilter
    {
        [Display(Name = "NIF/DNI")]
        [MaxLength(20)]        
        public string NIF { get; set; }        

        [MaxLength(80)]
        public string Nombre { get; set; }

        [MaxLength(80)]
        public string Apellidos { get; set; }

        public Expression<Func<Asistente, bool>> ExpressionFilter()
        {
            return f =>
                (string.IsNullOrWhiteSpace(NIF) ? true : f.NIF.Contains(NIF.Trim())) &&
                (string.IsNullOrWhiteSpace(Nombre) ? true : f.Nombre.Contains(Nombre.Trim())) &&
                (string.IsNullOrWhiteSpace(Apellidos) ? true : f.Apellidos.Contains(Apellidos.Trim()));
        }
    }
}

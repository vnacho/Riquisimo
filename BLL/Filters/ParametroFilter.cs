using Ferpuser.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Ferpuser.BLL.Filters
{
    public class ParametroFilter
    {
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Excluir datos de empresa")]
        public bool ExcluirDatosEmpresa { get; set; }   

        public Expression<Func<Parametro, bool>> ExpressionFilter()
        {
            return f =>                
                (string.IsNullOrWhiteSpace(Codigo) ? true : f.Codigo.Contains(Codigo.Trim())) && 
                (ExcluirDatosEmpresa ? !f.Codigo.StartsWith("EMPRESA_") : true);
        }

    }
}

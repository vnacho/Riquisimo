using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ferpuser.BLL.BusinessValidators
{
    public class ParametroValidator
    {
        DateTime dtTemp;

        public ApplicationDbContext _db { get; set; }

        public ParametroValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Edit(Parametro model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            switch (model.Codigo)
            {
                case Consts.PARAMETRO_CODIGO_FECHA_LIMITE_INFERIOR_FACTURAS_COMPRA_VENTA:
                    list.AddRange(Validar_FECHA_LIMITE_INFERIOR_FACTURAS_COMPRA_VENTA(model));
                    break;
            }            
            return list;
        }

        private List<ValidationResult> Validar_FECHA_LIMITE_INFERIOR_FACTURAS_COMPRA_VENTA(Parametro model)
        {
            List<ValidationResult> errores = new List<ValidationResult>();            
            if (!string.IsNullOrWhiteSpace(model.Valor))
            {
                if (!DateTime.TryParse(model.Valor, out dtTemp))
                    errores.Add(new ValidationResult("El valor debe ser una fecha válida.",new string[] { nameof(model.Valor) }));
            }
            return errores;
        }
    }
}

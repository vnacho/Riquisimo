using Ferpuser.Data;
using Ferpuser.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ferpuser.BLL.BusinessValidators
{
    public class TipoDocumentoVentaValidator
    {
        public ApplicationDbContext _db { get; set; }

        public TipoDocumentoVentaValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(TipoDocumentoVenta model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.TiposDocumentoVenta.Any(f => f.Descripcion.Trim() == model.Descripcion.Trim()))
                list.Add(new ValidationResult("Ya existe un tipo de documento con ese nombre.", new string[] { nameof(model.Descripcion) }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(TipoDocumentoVenta model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.TiposDocumentoVenta.Any(f => f.Id != model.Id && f.Descripcion.Trim() == model.Descripcion.Trim()))
                list.Add(new ValidationResult("Ya existe un tipo de documento  con ese nombre.", new string[] { nameof(model.Descripcion) }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

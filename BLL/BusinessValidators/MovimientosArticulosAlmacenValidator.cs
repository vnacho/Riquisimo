using Ferpuser.Data;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class MovimientosArticulosAlmacenValidator
    {
        public ApplicationDbContext _db { get; set; }

        public MovimientosArticulosAlmacenValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(MovimientosArticulosAlmacen model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            //if (_db.MovimientosArticulosAlmacen.Any(f => f.ArticulosAlmacenId == model.ArticulosAlmacenId))
            //    list.Add(new ValidationResult("El articulo está existe en el inventario.", new string[] { "ArticulosAlmacenId" }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(MovimientosArticulosAlmacen model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

using Ferpuser.Data;
using Ferpuser.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ferpuser.BLL.BusinessValidators
{
    public class ProveedorValidator
    {
        public ApplicationDbContext _db { get; set; }

        public ProveedorValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(Proveedor model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            var proveedorLocal = _db.Proveedores.FirstOrDefault(pro=>pro.CUENTACONTABLE.Equals(model.CUENTACONTABLE) || pro.NIF.Equals(model.NIF));

            if (proveedorLocal != null)
            {
                if (proveedorLocal.CUENTACONTABLE.Equals(model.NIF))
                    list.Add(new ValidationResult("Ya existe un proveedor con ese NIF/CIF.", new string[] { nameof(Proveedor.NIF) }));
                if (proveedorLocal.CUENTACONTABLE.Equals(model.CUENTACONTABLE))
                    list.Add(new ValidationResult("Ya existe un proveedor con esa Cuenta contable.", new string[] { nameof(Proveedor.CUENTACONTABLE) }));
            }
            return list;
        }

        public IEnumerable<ValidationResult> Edit(Proveedor model)
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

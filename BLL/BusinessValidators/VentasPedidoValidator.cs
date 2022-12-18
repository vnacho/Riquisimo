using Ferpuser.Data;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class VentasPedidoValidator
    {
        public ApplicationDbContext _db { get; set; }

        public VentasPedidoValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(VentaPedido model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }

        public IEnumerable<ValidationResult> Edit(VentaPedido model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}

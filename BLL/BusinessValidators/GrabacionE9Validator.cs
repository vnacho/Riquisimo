using Ferpuser.Data;
using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.BusinessValidators
{
    public class GrabacionE9Validator
    {
        public ApplicationDbContext _db { get; set; }

        public GrabacionE9Validator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(GrabacionE9 model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }

        public IEnumerable<ValidationResult> Edit(GrabacionE9 model)
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

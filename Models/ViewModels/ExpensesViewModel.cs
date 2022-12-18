using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.ViewModels
{
    public class ExpensesViewModel
    {
        public Expense Expense { get; set; }
        public List<Product> Products { get; set; }
    }
}

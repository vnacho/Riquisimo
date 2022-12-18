using Ferpuser.BLL.Filters;
using System.Collections.Generic;

namespace Ferpuser.Models.ViewModels
{
    public class ComprasInformesFacturas
    {
        public CompraFacturaFilter Filter { get; set; } = new CompraFacturaFilter();
        public IEnumerable<CompraFactura> Items { get; set; } = new List<CompraFactura>();
    }
}

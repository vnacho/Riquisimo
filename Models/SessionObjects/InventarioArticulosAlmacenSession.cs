using Ferpuser.BLL.Filters;

namespace Ferpuser.Models.SessionObjects
{
    internal class InventarioArticulosAlmacenSession
    {
        public InventarioArticulosAlmacenSession()
        {
        }

        public InventarioArticulosAlmacenFilter filter { get; set; }
        public string sort { get; set; }
        public int page { get; set; }
    }
}
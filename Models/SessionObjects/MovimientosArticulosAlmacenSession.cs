using Ferpuser.BLL.Filters;

namespace Ferpuser.Models.SessionObjects
{
    internal class MovimientosArticulosAlmacenSession
    {
        public MovimientosArticulosAlmacenSession()
        {
        }

        public MovimientosArticulosAlmacenFilter filter { get; set; }
        public string sort { get; set; }
        public int page { get; set; }
    }
}
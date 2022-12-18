using Ferpuser.BLL.Filters;

namespace Ferpuser.Models.SessionObjects
{
    public class ArticulosAlmacenSession
    {
        public ArticulosAlmacenFilter filter { get; set; }
        public string sort { get; set; }
        public int page { get; set; }
    }
}

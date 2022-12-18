using Ferpuser.Data;
using Ferpuser.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ferpuser.BLL.Managers
{
    public class CongressManager
    {
        public ApplicationDbContext _db { get; set; }

        public CongressManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public bool PermiteImpresionFormatoA(string codigoEvento)
        {
            var congress = _db.Congresses.AsNoTracking().FirstOrDefault(f => f.Number.ToString() == codigoEvento);
            if (congress != null && !string.IsNullOrWhiteSpace(congress.LogoBase64) && !string.IsNullOrWhiteSpace(congress.TailBase64))
                return true;
            return false;
        }

        
    }
}

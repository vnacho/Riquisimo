using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Core
{
    public class Pager
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PagesDisplayOffset { get; set; }

        public int ResultadoDesde => ((Page - 1) * PageSize) + 1;
        public int ResultadoHasta => Page * PageSize > Total ? Total : Page * PageSize;
        public int LastPage => Total % PageSize == 0 ? Total / PageSize : Total / PageSize + 1;
        public int FirstPageDisplay => Page - PagesDisplayOffset < 1 ? 1 : Page - PagesDisplayOffset;
        public int LastPageDisplay => Page + PagesDisplayOffset > LastPage ? LastPage : Page + PagesDisplayOffset;

        public Pager(int total, int? page)
        {
            Total = total;
            Page = page.HasValue ? page.Value : 1;
            PageSize = 10;
            PagesDisplayOffset = 5;
        }

        public Pager(int total, int? page, int pagesize, int pagesoffset)
        {
            Total = total;
            Page = page.HasValue ? page.Value : 1;
            PageSize = pagesize;
            PagesDisplayOffset = pagesoffset;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Views.Shared
{
    public class ViewVariables
    {
        public bool FileUpload { get; set; } = false;
        public bool Back { get; set; } = true;
        public bool Validation;
        public bool TableSorter;
        public bool Create { get; set; } = true;
        public bool OneCard { get; set; } = true;
    }
}

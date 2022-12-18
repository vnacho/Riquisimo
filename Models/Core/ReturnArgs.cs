using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Core
{

    /// <summary>
    /// Esta clase es útil para devolver distintas llamadas de tipo partial result
    /// </summary>
    public class ReturnArgs
    {
        public ReturnArgs()
        {
        }

        public int Status { get; set; }
        public string ViewString { get; set; }
    }
}

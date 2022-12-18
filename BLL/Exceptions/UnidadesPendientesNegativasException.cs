using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Exceptions
{
    public class UnidadesPendientesNegativasException : Exception
    {
        public decimal Unidades { get; set; }
        public string CodigoPedido { get; set; }
        public int OrdenLineaPedido { get; set; }

        public UnidadesPendientesNegativasException(decimal uds, string codigopedido, int orden)
        {
            Unidades = uds;
            CodigoPedido = codigopedido;
            OrdenLineaPedido = orden;
        }

        public UnidadesPendientesNegativasException(decimal uds, int orden)
        {
            Unidades = uds;
            OrdenLineaPedido = orden;
        }
    }
}
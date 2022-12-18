using Ferpuser.Data;
using Ferpuser.Models.SageComu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class EjercicioManager
    {
        private readonly SageComuContext _sageComuContext;

        public EjercicioManager(SageComuContext context)
        {
            _sageComuContext = context;
        }

        public ejercici GetEjercicioActivo()
        {
            return _sageComuContext.Ejercicios.OrderBy(f => f.ANY).ToList().LastOrDefault();
        }

        public ejercici GetEjercicio(int anio)
        {
            string sAnio = anio.ToString().Trim();
            return _sageComuContext.Ejercicios.Where(f => f.ANY == sAnio).ToList().LastOrDefault();
        }

        public IEnumerable<ejercici> GetAll()
        {
            return _sageComuContext.Ejercicios;
        }
    }
}

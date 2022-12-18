using Ferpuser.Data;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Ferpuser.BLL.Managers
{
    public class SerieManager
    {
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComuContext;

        public SerieManager(SageContext sageContext, SageComuContext sageComuContext)
        {
            _sageContext = sageContext;
            _sageComuContext = sageComuContext;
        }

        public List<SerieDto> Get(string CodigoEmpresa, int CodigoTipoDocumento)
        {
            List<SerieDto> result = new List<SerieDto>();
            var letras = _sageComuContext.Letras;
            var series = _sageContext.Serie.Where(f => f.Empresa == CodigoEmpresa && f.Tipodoc == CodigoTipoDocumento).OrderBy(f => f.Serie);
            foreach (var item in series)
            {
                var letra = letras.SingleOrDefault(f => f.Codigo == item.Serie);
                result.Add(new SerieDto() { Codigo = item.Serie, Nombre = letra?.Nombre});
            }
            return result;
        }
    }
}

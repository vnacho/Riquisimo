using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models.SageComu;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Transfer
{
    public class SageContextFactoryHelper
    {
        private readonly SageComuContext _sageComuContext;
        private IConfiguration _config;

        public SageContextFactoryHelper(SageComuContext context, IConfiguration configuration)
        {
            _sageComuContext = context;
            _config = configuration;
        }

        public SageContext CreateSageContext(int ejercicio)
        {
            EjercicioManager manager = new EjercicioManager(_sageComuContext);
            ejercici model = manager.GetEjercicio(ejercicio);

            if (model == null)
                return null;
           
            var connectionstring = _config.GetConnectionString("Sage");
            connectionstring = string.Format(connectionstring, model.CONEXION.Trim());
            var optionsBuilder = new DbContextOptionsBuilder<SageContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            SageContext dbContext = new SageContext(optionsBuilder.Options);
            return dbContext;
        }
    }
}

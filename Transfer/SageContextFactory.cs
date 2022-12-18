using Ferpuser.Data;
using Ferpuser.Models.Consts;
using Ferpuser.Models.SageComu;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ferpuser.BLL.Managers;

namespace Ferpuser.Transfer
{
    public class SageContextFactory
    {
        private readonly SageComuContext _sageComuContext;
        private readonly HttpContext _httpContext;
        private IConfiguration _config;

        public SageContextFactory(SageComuContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _sageComuContext = context;
            _httpContext = httpContextAccessor?.HttpContext;
            _config = configuration;
        }

        public SageContext CreateSageContext()
        {
            EjercicioManager manager = new EjercicioManager(_sageComuContext);
            int? ejercicio = _httpContext.Session.GetInt32(Consts.SESSION_EJERCICIO);

            ejercici model = null;
            if (ejercicio.HasValue)
                model = manager.GetEjercicio(ejercicio.Value);
            
            if (model == null)
                model = manager.GetEjercicioActivo();

            if (!ejercicio.HasValue) //Aprovechamos para establecer el ejercicio en sesión
                _httpContext.Session.SetInt32(Consts.SESSION_EJERCICIO, Convert.ToInt32(model.ANY));

            //"Server=JAVI-PC\\SQLSERVER2019;Database=SAGE{0};Trusted_Connection=True;MultipleActiveResultSets=true;"
            var connectionstring = _config.GetConnectionString("Sage");
            connectionstring = string.Format(connectionstring, model.CONEXION.Trim());
            var optionsBuilder = new DbContextOptionsBuilder<SageContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            SageContext dbContext = new SageContext(optionsBuilder.Options);
            return dbContext;
        }
    }
}

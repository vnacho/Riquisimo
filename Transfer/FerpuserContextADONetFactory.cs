using Ferpuser.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Ferpuser.Transfer
{
    public class FerpuserContextADONetFactory
    {
        private readonly HttpContext _httpContext;
        private readonly ApplicationDbContext _context;

        public FerpuserContextADONetFactory(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor = null)
        {
            _context = context;
            _httpContext = httpContextAccessor?.HttpContext;
        }

        public FerpuserContextADONet CreateApplicationDbContext()
        {
            var user = _httpContext.User;
            var account = _context.Accounts.FirstOrDefault(a => a.Name.ToLower().Equals(user.Identity.Name));
            if (account == null)
                return null;

            var congress = _context.Congresses.Find(account.CongressId);
            if (congress == null)
                return null;
            if (string.IsNullOrWhiteSpace(congress.NewConnectionString) || string.IsNullOrWhiteSpace(congress.DatabasePrefix))
                return null;

            var connectionstring = congress.NewConnectionString + "AllowZeroDateTime=True;ConvertZeroDateTime=True";
            //var connectionstring = "Server=35enfermeriatraumatologia.com;Database=enftra35_35cot;User=enftra35_test;Password=O7BUdkcW5JE#0b9Qzr39*4DsI%Et$R;";
            //var connecionString = "Server=anecorm.org;Database=anecorm_db;User=anecorm_user;Password=mkp0y2NMrjKSRQanuHUe;";
            //Algunas cadenas de conexión pueden dar errores por ejemplo esta:
            //Server=36enfermeriatraumatologia.com;Database=cot36_bdatos;User=cot36_user;Password=g7RcU@$;Advk;
            //Tiene punto y coma ";" en el password

            FerpuserContextADONet dbContext = new FerpuserContextADONet(connectionstring, congress.DatabasePrefix);

            return dbContext;
        }

        /// <summary>
        /// Para algunas operaciones concretas (para comites) se necesita una conexión distinta.
        /// Está especificado en los valores de conexión para comités dentro de la entidad Congress.
        /// </summary>
        /// <param name="CongressId"></param>
        /// <returns></returns>
        public FerpuserContextADONet CreateApplicationDbContextComite(Guid CongressId)
        {            
            var congress = _context.Congresses.Find(CongressId);
            if (congress == null)
                return null;
            if (string.IsNullOrWhiteSpace(congress.NewConnectionStringComite) || string.IsNullOrWhiteSpace(congress.GetDatabasePrefixComite()))
                return null;

            var connectionstring = congress.NewConnectionStringComite + "AllowZeroDateTime=True;ConvertZeroDateTime=True";
            FerpuserContextADONet dbContext = new FerpuserContextADONet(connectionstring, congress.GetDatabasePrefixComite());

            return dbContext;
        }
    }
}

using Ferpuser.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Transfer
{
    public class FerpuserContextFactory
    {
        private readonly HttpContext _httpContext;
        private readonly ApplicationDbContext _context;

        public FerpuserContextFactory(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor = null)
        {
            _context = context;
            _httpContext = httpContextAccessor?.HttpContext;
        }

        public FerpuserContext CreateApplicationDbContext()
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

            var optionsBuilder = new DbContextOptionsBuilder<FerpuserContext>();
            optionsBuilder.UseMySql(connectionstring).AddInterceptors(new TablePrefixInterceptor(congress.DatabasePrefix));
            FerpuserContext dbContext = new FerpuserContext(optionsBuilder.Options);

            return dbContext;
        }
    }
}

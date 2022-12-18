using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Ferpuser.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using LazZiya.ExpressLocalization;
using Ferpuser.LocalizationResources;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Ferpuser.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using IbanNet.DependencyInjection.ServiceProvider;
using Ferpuser.BLL.Interfaces;
using Ferpuser.BLL.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Ferpuser.BLL.Managers;
using Rotativa.AspNetCore;
using Ferpuser.BLL.Helpers;
using Ferpuser.Models.Consts;
using Microsoft.AspNetCore.Http.Features;

namespace Ferpuser
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("ApplicationSettings"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIbanNet();

            services.AddTransient<FerpuserContextFactory>();
            services.AddTransient(provider => provider.GetService<FerpuserContextFactory>().CreateApplicationDbContext());

            services.AddTransient<FerpuserContextADONetFactory>();
            services.AddTransient(provider => provider.GetService<FerpuserContextADONetFactory>().CreateApplicationDbContext());

            //services.AddDbContext<SageContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("Sage")));
            services.AddTransient<SageContextFactory>();
            services.AddTransient(provider => provider.GetService<SageContextFactory>().CreateSageContext());

            services.AddDbContext<SageComuContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SageComu")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllers().AddNewtonsoftJson();

            //ptions.AddPolicy("Founders", policy =>
            //              policy.RequireClaim("EmployeeNumber", "1", "2", "3", "4", "5"));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddAuthorization(options =>
            {
                //Antiguas políticas de permisos.
                options.AddPolicy("Congress", policy => policy.RequireClaim("AccessCongress"));                
                options.AddPolicy("Collaborations", policy => policy.RequireClaim("AccessCollaborations"));

                //Nuevas políticas de permisos.
                options.AddPolicy("Admin", policy => policy.RequireClaim(Consts.CLAIM_PERMISO_ADMINISTRACION));
                options.AddPolicy("Facturacion", policy => policy.RequireClaim(Consts.CLAIM_PERMISO_FACTURACION));
                options.AddPolicy("BudgetControl", policy => policy.RequireClaim(Consts.CLAIM_PERMISO_CONTROL_PRESUPUESTARIO));
                options.AddPolicy("Compras", policy => policy.RequireClaim(Consts.CLAIM_PERMISO_COMPRAS));
                options.AddPolicy("Ventas", policy => policy.RequireClaim(Consts.CLAIM_PERMISO_VENTAS));
                options.AddPolicy("Almacen", policy => policy.RequireClaim(Consts.CLAIM_PERMISO_CONTROL_ALMACEN));
            });
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });
            var cultures = new CultureInfo[]
            {
                new CultureInfo("es"),
            };

            services.AddControllersWithViews()
                .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(exOps =>
                {
                    exOps.ResourcesPath = "LocalizationResources";
                    exOps.RequestLocalizationOptions = ops =>
                    {
                        ops.SupportedCultures = cultures;
                        ops.SupportedUICultures = cultures;
                        ops.DefaultRequestCulture = new RequestCulture("es");
                    };
                })
                .AddRazorRuntimeCompilation();
            

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60000);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.Configure<FormOptions>(options => {
                options.ValueCountLimit = int.MaxValue;
            });

            services.AddMvc(options => {
                options.MaxModelBindingCollectionSize = int.MaxValue;
            });

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddHttpContextAccessor();

            services.AddTransient<IFileUploader, LocalFileUploader>();
            services.AddTransient<SageContextFactoryHelper, SageContextFactoryHelper>();
            services.AddTransient<AcommodationManager, AcommodationManager>();
            services.AddTransient<RegistrationManager, RegistrationManager>();
            services.AddTransient<ControlPresupuestarioManager, ControlPresupuestarioManager>();            
            services.AddTransient<SecundarManager, SecundarManager>();
            services.AddTransient<VentaAlbaranManager, VentaAlbaranManager>();
            services.AddTransient<VentaFacturaManager, VentaFacturaManager>();
            services.AddTransient<SerieManager, SerieManager>();
            services.AddTransient<ParametroManager, ParametroManager>();
            services.AddTransient<VentasPedidoPrintHelper, VentasPedidoPrintHelper>();
            services.AddTransient<VentasAlbaranPrintHelper, VentasAlbaranPrintHelper>();
            //services.AddTransient<VentasFacturaPrintHelper, VentasFacturaPrintHelper>();
            services.AddTransient<VentaFacturaPrint, VentaFacturaPrint>();

            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.ViewLocationFormats.Add("/Views/Shared/Buscadores/{0}" + RazorViewEngine.ViewExtension);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRequestLocalization("es");
            app.UseResponseCompression();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            //Install-Package Rotativa.AspNetCore -Version 1.2.0-beta            
            //RotativaConfiguration.Setup(env.WebRootPath, "/usr/bin/");
            RotativaConfiguration.Setup(env.WebRootPath); //PATH está en wwwroot/Rotativa
        }
    }
}

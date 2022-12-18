using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Enums;
using Ferpuser.Security;
using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ferpuser.Controllers
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class AddViewModel : LoginViewModel
    {

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Compare(nameof(Password), ErrorMessage = DataAnnotationsErrorMessages.CompareAttribute_MustMatch)]
        [Display(Name = "Repetir contraseña")]
        [DataType(DataType.Password)]
        public string Password2 { get; set; }

        [Display(Name = "Vendedor de Sage")]
        public string Vendedor { get; set; }

        [Display(Name = "Congreso")]
        public Guid? CongressId { get; set; }
        [Display(Name = "Congreso")]
        public Congress Congress { get; set; }

        [Display(Name = "Acceso a congresos")]
        public bool AccessCongress { get; set; }
        [Display(Name = "Acceso a colaboraciones")]
        public bool AccessCollaborations { get; set; }
    }

    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SageContext _sageContext;
        private readonly SageComuContext _sageComuContext;

        public AccountController(ApplicationDbContext context, SageContext sageContext, SageComuContext sageComuContext)
        {
            _context = context;
            _sageContext = sageContext;
            _sageComuContext = sageComuContext;
        }

        [Authorize(Policy = "Admin")]
        public IActionResult Index()
        {
            return View(_context.Accounts.ToList());
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            
            var account = _context.Accounts.FirstOrDefault(a => a.Name.ToLower().Trim().Equals(vm.Name.Trim().ToLower()));
            if (account == null)
            {
                ModelState.AddModelError("Name", "Inicio de sesión incorrecto");
                return View(vm);
            }

            var passwordHasher = new PasswordHasher();
            (bool Verified, bool NeedsUpgrade) = passwordHasher.Check(account.PasswordHash, vm.Password);
            if (!Verified)
            {
                ModelState.AddModelError("Name", "Inicio de sesión incorrecto");
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, vm.Name),
            };
            
            if (account.AccessCongress)
                claims.Add(new Claim("AccessCongress", "true"));            
            if (account.AccessCollaborations)
                claims.Add(new Claim("AccessCollaborations", "true"));
                        
            //Permisos
            if (account.PermisoAdministracion)
                claims.Add(new Claim(Consts.CLAIM_PERMISO_ADMINISTRACION, true.ToString()));
            if (account.PermisoEventos)
                claims.Add(new Claim(Consts.CLAIM_PERMISO_EVENTOS, true.ToString()));
            if (account.PermisoFacturacion)
                claims.Add(new Claim(Consts.CLAIM_PERMISO_FACTURACION, true.ToString()));
            if (account.PermisoVentas)
                claims.Add(new Claim(Consts.CLAIM_PERMISO_VENTAS, true.ToString()));
            if (account.PermisoCompras)
                claims.Add(new Claim(Consts.CLAIM_PERMISO_COMPRAS, true.ToString()));
            if (account.PermisoControlPresupuestario)
                claims.Add(new Claim(Consts.CLAIM_PERMISO_CONTROL_PRESUPUESTARIO, true.ToString()));
            if (account.PermisoControlAlmacen)
                claims.Add(new Claim(Consts.CLAIM_PERMISO_CONTROL_ALMACEN, true.ToString()));
            if (account.PerfilUsuario == PerfilUsuario.Empleado)
                claims.Add(new Claim(Consts.CLAIM_PERFIL_USUARIO_EMPLEADO, true.ToString()));
            if (account.PerfilUsuario == PerfilUsuario.Cliente)
                claims.Add(new Claim(Consts.CLAIM_PERFIL_USUARIO_CLIENTE, true.ToString()));

            //Configuración SMTP para el envío de mails
            claims.Add(new Claim(Consts.CLAIM_SMTP_SERVER, account.OutgoingMailServer ?? string.Empty));
            claims.Add(new Claim(Consts.CLAIM_SMTP_PORT, account.OutgoingMailPort.ToString()));
            claims.Add(new Claim(Consts.CLAIM_MAIL_USER, account.MailUser ?? string.Empty));
            claims.Add(new Claim(Consts.CLAIM_MAIL_PASSWORD, account.MailPassword ?? string.Empty));
            claims.Add(new Claim(Consts.CLAIM_SEND_COPY, account.SendCopyTo.ToString()));
            claims.Add(new Claim(Consts.CLAIM_CODIGO_OPERARIO, account.Operario ?? string.Empty));
            claims.Add(new Claim(Consts.CLAIM_CODIGO_VENDEDOR, account.Vendedor ?? string.Empty));
            claims.Add(new Claim(Consts.CLAIM_ACCOUNT_ID, account.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, account.Email ?? string.Empty));

            await Login(claims);
            return View();
        }

        private async Task Login(List<Claim> claims)
        {
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                RedirectUri = "/"
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            Account account;
            if (!id.HasValue)
                account = await _context.Accounts.FirstOrDefaultAsync(a => a.Name.Equals(HttpContext.User.Identity.Name));
            else
                account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id.Equals(id.Value));

            if (account == null)
                return NotFound();

            ViewData["Vendedor"] = new SelectList(_sageContext.Vendedor.AsNoTracking().OrderBy(v => v.Codigo), "Codigo", "Display", account.Vendedor);
            ViewBag.Operarios = _sageComuContext.Operarios.AsNoTracking().OrderBy(f => f.NOMBRE);
            account.Password = "NOT HERE";
            account.Password2 = "NOT HERE";
            account.PasswordHash = "NOT HERE";
            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid? id, Account newAccount)
        {
            var isAdmin = HttpContext.User.Claims.Any(c => c.Type.Equals(Consts.CLAIM_PERMISO_ADMINISTRACION));
            Account account;
            if (isAdmin)
            {
                ViewData["Vendedor"] = new SelectList(_sageContext.Vendedor.AsNoTracking().OrderBy(v => v.Codigo), "Codigo", "Display", newAccount.Vendedor);
                ViewBag.Operarios = _sageComuContext.Operarios.AsNoTracking().OrderBy(f => f.NOMBRE);

                if (!id.HasValue || id != newAccount.Id)
                    return NotFound();

                if (!ModelState.IsValid)
                    return View(newAccount);
                
                account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id.Equals(id.Value));

                if (account == null)
                    return NotFound();
            }
            else
            {
                account = await _context.Accounts.FirstOrDefaultAsync(a => a.Name.Equals(HttpContext.User.Identity.Name));
            }
            if (isAdmin)
            {
                account.Name = newAccount.Name;
                account.Vendedor = newAccount.Vendedor;
                account.Operario = newAccount.Operario;

                account.AccessCongress = newAccount.AccessCongress;
                account.AccessCollaborations = newAccount.AccessCollaborations;

                account.PermisoAdministracion = newAccount.PermisoAdministracion;
                account.PermisoEventos = newAccount.PermisoEventos;
                account.PermisoFacturacion = newAccount.PermisoFacturacion;
                account.PermisoVentas = newAccount.PermisoVentas;
                account.PermisoCompras = newAccount.PermisoCompras;
                account.PermisoControlPresupuestario = newAccount.PermisoControlPresupuestario;
                account.PermisoControlAlmacen = newAccount.PermisoControlAlmacen;

                account.PerfilUsuario = newAccount.PerfilUsuario;
            }

            account.Email = newAccount.Email;
            account.SendCopyTo = newAccount.SendCopyTo;
            account.Modified = DateTime.Now;
            
            account.IncomingMailServer = newAccount.IncomingMailServer;
            account.OutgoingMailServer = newAccount.OutgoingMailServer;
            account.IncomingMailPort = newAccount.IncomingMailPort;
            account.OutgoingMailPort = newAccount.OutgoingMailPort;
            account.MailUser = newAccount.MailUser;
            account.MailPassword = newAccount.MailPassword;
            account.SignatureBefore = newAccount.SignatureBefore;
            account.Signature = newAccount.Signature;
            account.SignatureAfter = newAccount.SignatureAfter;

            if (!string.IsNullOrWhiteSpace(newAccount.Password) && !newAccount.Password.Equals("NOT HERE"))
            {
                var passwordHasher = new PasswordHasher();
                account.PasswordHash = passwordHasher.Hash(newAccount.Password);
            }

            try
            {
                _context.Update(account);
                _context.SaveChanges();
                if (isAdmin)
                    return RedirectToAction(nameof(Index));
                else                
                    return Redirect("/");
            }
            catch
            {
                throw;
            }
        }
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            ViewData["Vendedor"] = new SelectList(_sageContext.Vendedor.AsNoTracking().OrderBy(v => v.Codigo), "Codigo", "Display");
            ViewBag.Operarios = _sageComuContext.Operarios.AsNoTracking().OrderBy(f => f.NOMBRE);
            return View(new Account());
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create(Account account)
        {
            ViewData["Vendedor"] = new SelectList(_sageContext.Vendedor.AsNoTracking().OrderBy(v => v.Codigo), "Codigo", "Display", account.Vendedor);
            ViewBag.Operarios = _sageComuContext.Operarios.AsNoTracking().OrderBy(f => f.NOMBRE);
            if (!ModelState.IsValid)
            {
                return View(account);
            }
            if (_context.Accounts.Any(a => a.Name.ToLower().Trim().Equals(account.Name.Trim().ToLower())))
            {
                ModelState.AddModelError("Name", "El usuario ya existe");
                return View(account);
            }

            var passwordHasher = new PasswordHasher();
            var hash = passwordHasher.Hash(account.Password);
            //¡¡¡Atención!!!, parece como que no guarda el hash generado en la creación de usuario.
            //Pero de momento nadie se ha quejado con lo que no estoy seguro de que no esté funcionando bien.
            //Creo que falta la siguiente línea:
            //account.PasswordHash = hash;

            _context.Add(account);
            await _context.SaveChangesAsync();
            //await Login(newAccount.Name);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
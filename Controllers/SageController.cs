using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Sage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ferpuser.Controllers
{
    [Authorize]
    public class SageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SageContext _sage;

        public SageController(ApplicationDbContext context, SageContext sage)
        {
            _context = context;
            _sage = sage;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import()
        {
            foreach (var sageClient in _sage.Clientes)
            {
                await UpdateSageClient(_context, sageClient);
            }
            await _context.SaveChangesAsync();

            foreach (var g in _sage.Env_Cli.Where(dd => dd.Linea > 1).GroupBy(dd => dd.Cliente))
            {
                foreach (var sageLocation in g)
                {
                    await UpdateSageLocation(_context, sageLocation);
                }
            }

            return View();
        }

        public static async Task<ClientLocation> UpdateSageLocation(ApplicationDbContext context, Env_Cli sageLocation)
        {
            var location = context.ClientLocations.FirstOrDefault(d_ => d_.SageLine == sageLocation.Linea && d_.SageClient.Equals(sageLocation.Cliente));

            var client = context.Clients.FirstOrDefault(c => c.SageCode.Equals(sageLocation.Cliente));

            if (location == null)
            {
                location = new ClientLocation
                {
                    ClientId = client.Id,
                    SageClient = sageLocation.Cliente,
                    SageLine = sageLocation.Linea,
                    Address = sageLocation.Direccion.Trim() ?? "",
                    City = sageLocation.Poblacion.Trim() ?? "",
                    ZipCode = sageLocation.CodPos.Trim() ?? "",
                    Province = sageLocation.Provincia.Trim() ?? "",
                    Country = sageLocation.Pais.Trim() ?? "",
                    Phone = sageLocation.Telefono.Trim() ?? "",
                    Phone2 = sageLocation.Fax.Trim() ?? ""
                };

                context.Add(location);
            }
            else
            {
                location.ClientId = client.Id;
                location.SageLine = sageLocation.Linea;
                location.SageClient = sageLocation.Cliente;
                location.Address = sageLocation.Direccion.Trim() ?? "";
                location.City = sageLocation.Poblacion.Trim() ?? "";
                location.ZipCode = sageLocation.CodPos.Trim() ?? "";
                location.Province = sageLocation.Provincia.Trim() ?? "";
                location.Country = sageLocation.Pais.Trim() ?? "";
                location.Phone = sageLocation.Telefono.Trim() ?? "";
                location.Phone2 = sageLocation.Fax.Trim() ?? "";
                context.Update(location);
            }

            await context.SaveChangesAsync();
            return location;
        }

        public static async Task<Client> UpdateSageClient(ApplicationDbContext context, Clientes sageClient)
        {
            var client = context.Clients.FirstOrDefault(c_ => c_.SageCode.Equals(sageClient.Codigo));
            if (client == null)
            {
                client = new Client
                {
                    SageCode = sageClient.Codigo,
                    BusinessName = sageClient.Nombre.Trim() ?? "",
                    //Surnames = c.Nombre2.Trim() ?? "",
                    NIF = sageClient.CIF.Trim() ?? "",
                    Email = sageClient.Email.Trim() ?? "",
                    Email2 = sageClient.Email_f.Trim() ?? "",
                };
                var location = new ClientLocation
                {
                    ClientId = client.Id,
                    SageLine = 1,
                    SageClient = sageClient.Codigo,
                    Address = sageClient.Direccion.Trim() ?? "",
                    City = sageClient.Poblacion.Trim() ?? "",
                    ZipCode = sageClient.CodPost.Trim() ?? "",
                    Province = sageClient.Provincia.Trim() ?? "",
                    Country = sageClient.Pais.Trim() ?? "",
                    Phone = ""
                };
                context.Add(client);
                context.Add(location);
            }
            else
            {
                client.SageCode = sageClient.Codigo;
                client.BusinessName = sageClient.Nombre ?? "";
                //client.Surnames = c.Nombre2 ?? "";
                client.NIF = sageClient.CIF ?? "";
                client.Modified = DateTime.Now;
                client.Email = sageClient.Email.Trim() ?? client.Email;
                client.Email2 = sageClient.Email_f.Trim() ?? client.Email2;

                var location = context.ClientLocations.FirstOrDefault(d_ => d_.SageLine == 1 && d_.SageClient.Equals(sageClient.Codigo));
                if (location == null)
                {
                    location = new ClientLocation
                    {
                        ClientId = client.Id,
                        SageLine = 1,
                        SageClient = sageClient.Codigo.Trim() ?? "",
                        Address = sageClient.Direccion.Trim() ?? "",
                        City = sageClient.Poblacion.Trim() ?? "",
                        ZipCode = sageClient.CodPost.Trim() ?? "",
                        Province = sageClient.Provincia.Trim() ?? "",
                        Country = sageClient.Pais.Trim() ?? "",
                        Phone = ""
                    };

                    context.Add(location);
                }
                else
                {
                    location.ClientId = client.Id;
                    location.SageLine = 1;
                    location.SageClient = sageClient.Codigo.Trim() ?? "";
                    location.Address = sageClient.Direccion.Trim() ?? "";
                    location.City = sageClient.Poblacion.Trim() ?? "";
                    location.ZipCode = sageClient.CodPost.Trim() ?? "";
                    location.Province = sageClient.Provincia.Trim() ?? "";
                    location.Country = sageClient.Pais.Trim() ?? "";
                    location.Phone = "";
                    context.Update(location);
                }
                context.Update(client);
            }
            await context.SaveChangesAsync();
            return client;
        }
    }
}
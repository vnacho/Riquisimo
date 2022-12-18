using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Sage;
using Microsoft.AspNetCore.Authorization;
using Ferpuser.ViewModels;
using Newtonsoft.Json;
using Ferpuser.Models.Dtos;

namespace Ferpuser.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly SageContext sage;


        public ClientsController(ApplicationDbContext db, SageContext sage)
        {
            this.db = db;
            this.sage = sage;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await db.Clients.Where(c => c.Deleted == null).ToListAsync());
        }

        public IActionResult Unsent()
        {
            var newClients = db.Clients.Where(c => c.SageCode == null).AsNoTracking().ToList().Where(c =>
                (db.Registrations.Include(r => r.Congress).Any(r => r.Deleted == null && r.ClientId.Equals(c.Id) && !r.Congress.HideRegistrations) ||
                 db.Accommodations.Include(r => r.Congress).Any(r => r.Deleted == null && r.ClientId.Equals(c.Id) && !r.Congress.HideRegistrations))).ToList();

            var newClientRegistrations = db.Registrations
                .Include(r => r.Registrant)
                .Include(r => r.Client).Where(r => r.Deleted == null && newClients.Contains(r.Client)).Select(r => Selector.SelectRegistration(r)).ToList().GroupBy(r => r.ClientId);
            var newClientAccommodations = db.Accommodations
                .Include(r => r.Registrant)
                .Include(r => r.Client).Where(r => r.Deleted == null && newClients.Contains(r.Client)).Select(r => Selector.SelectAccommodation(r)).ToList().GroupBy(r => r.ClientId);

            var vm = new HomeViewModel
            {
                NewClients = newClients,
                NewClientRegistrations = newClientRegistrations.ToList(),
                NewClientAccommodations = newClientAccommodations.ToList()
            };
            return View(vm);
        }

        //public async Task<Client> FetchSageClient(string id)
        //{
        //    var client = await _context.Clients.FirstOrDefaultAsync(c => c.SageCode.Equals(id));
        //    if (client == null)
        //    {
        //        var sageClient = await _sage.Clientes.FirstOrDefaultAsync(m => m.Codigo.Equals(id));
        //        if (sageClient == null)
        //        {
        //            return null;
        //        }

        //        client = new Client
        //        {
        //            SageCode = sageClient.Codigo,
        //            BusinessName = sageClient.Nombre.Trim() ?? "",
        //            NIF = sageClient.CIF.Trim() ?? "",
        //            Email = sageClient.Email.Trim() ?? "",
        //            Email2 = sageClient.Email_f.Trim() ?? "",
        //        };
        //        var location = new ClientLocation
        //        {
        //            ClientId = client.Id,
        //            SageLine = 0,
        //            SageClient = sageClient.Codigo,
        //            Address = sageClient.Direccion.Trim() ?? "",
        //            City = sageClient.Poblacion.Trim() ?? "",
        //            ZipCode = sageClient.CodPost.Trim() ?? "",
        //            Province = sageClient.Provincia.Trim() ?? "",
        //            Country = sageClient.Pais.Trim() ?? "",
        //            Phone = ""
        //        };
        //        _context.Add(client);
        //        _context.Add(location);
        //    }
        //    return client;
        //}

        // GET: Clients/Details/5
        public async Task<IActionResult> Editor(Guid id, bool inline = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = await db.Clients.FirstOrDefaultAsync(c => c.Id.Equals(id));
            if (client == null)
            {
                return NotFound();
            }
            ViewData["Inline"] = inline;
            return View("EditorTemplates/Client", client);
        }
        public async Task<IActionResult> LocationAsync(Guid? id, bool inline = false)
        {
            ViewData["Inline"] = inline;
            ClientLocation clientLocation = null;

            if (id.HasValue)
            {
                clientLocation = await db.ClientLocations.FirstOrDefaultAsync(c => c.Id.Equals(id));

            }

            return View("EditorTemplates/ClientLocation", clientLocation);
        }
        //public async Task<IActionResult> Location(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var sageClient = await _sage.Clientes.FirstOrDefaultAsync(c => c.Codigo.Equals(id));
        //    if (sageClient == null)
        //    {
        //        return NotFound();
        //    }
        //    var client = await SageController.UpdateSageClient(_context, sageClient);
        //    if (client == null)
        //    {
        //        return NotFound();
        //    }

        //    var sageLocations = _sage.Env_Cli.Where(l => l.Linea > 0 && l.Cliente.Equals(id));
        //    var clientLocations = new List<ClientLocation>();

        //    foreach (var sageLocation in sageLocations)
        //    {
        //        clientLocations.Add(await SageController.UpdateSageLocation(_context, sageLocation));
        //    }

        //    return View("EditorTemplates/ClientLocation", clientLocations);
        //}
        public async Task<IActionResult> Locations(Guid id, bool inline = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await db.Clients.FirstOrDefaultAsync(c => c.Id.Equals(id));
            if (client == null)
            {
                return NotFound();
            }

            ViewData["Inline"] = inline;
            var sageLocations = sage.Env_Cli.Where(l => l.Cliente.Equals(client.SageCode));
            var clientLocations = new List<ClientLocation>();

            foreach (var sageLocation in sageLocations)
            {
                clientLocations.Add(await SageController.UpdateSageLocation(db, sageLocation));
            }

            //clientLocations = _context.ClientLocations.Where(cl => cl.ClientId.Equals(id) && cl.SageLine.HasValue && !string.IsNullOrWhiteSpace(cl.SageClient)).ToList();
            return View("Locations", clientLocations);
        }
        public async Task<IActionResult> Search(string id)
        {
            if (id == null)
            {
                return View(new List<Client>());
            }
            var text = id.Trim().ToLower();
            if (id.Length > 3)
            {
                List<Client> clients = new List<Client>();

                foreach(var sc in await sage.Clientes.Where(c => c.Nombre.ToLower().Trim().Contains(text) || c.CIF.ToLower().Trim().Contains(text) || c.Codigo.ToLower().Trim().Contains(text)).ToListAsync())
                {
                    clients.Add(await SageController.UpdateSageClient(db, sc));
                }
                //clients = clients.Union(_context.Clients.Where(c => c.BusinessName.ToLower().Trim().Contains(text) || c.NIF.ToLower().Trim().Contains(text))).OrderBy(c => c.BusinessName).ToList();
                return View(clients);
            }
            return View(new List<Clientes>());
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                client.Id = Guid.NewGuid();
                db.Add(client);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await db.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(client);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await db.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var client = await db.Clients.FindAsync(id);
            client.Deleted = DateTime.Now;
            db.Clients.Update(client);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<string> ClientNIFExists (string NIF)
        {
            var client = await sage.Clientes.FirstOrDefaultAsync(c => c.CIF.Trim().ToLower().Equals(NIF.ToLower().Trim()));
            if (client == null)
                return "false";
            var _client = await SageController.UpdateSageClient(db, client);
            return _client.Id.ToString();
        }

        private bool ClientExists(Guid id)
        {
            return db.Clients.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Buscador(string value)
        {
            IEnumerable<Client> model = null;
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.ToLower();
                model = await db.Clients.Where(f => f.BusinessName.ToLower().Contains(value)).OrderBy(f => f.BusinessName).ToListAsync();
            }
            return View(model);
        }


        [HttpPost]
        public string GetClientesJSON()
        {
            string resultado = "";
            try
            {
                Select2Request select2query = JsonConvert.DeserializeObject<Select2Request>(this.Request.Form.Keys.FirstOrDefault().ToString());

                IEnumerable<Clientes> query = sage.Clientes
                    .Where(f => f.Nombre.ToLower().Contains(select2query.q.ToLower()) 
                    || f.Codigo.ToLower().Contains(select2query.q.ToLower()))
                    .OrderBy(f => f.Codigo).ThenBy(f => f.Nombre);

                if (select2query.page_limit.HasValue)
                    query = query.Take(select2query.page_limit.Value);

                List<Select2Response> model = query
                    .Select(f => new Select2Response() { id = f.Codigo, text = f.DisplayName })
                    .ToList();
                
                resultado = System.Text.Json.JsonSerializer.Serialize(model);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return resultado;
        }
    }
}

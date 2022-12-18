using Ferpuser.Data;
using Ferpuser.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Ferpuser.Controllers
{
    public class EnvCliController : Controller
    {
        private readonly SageContext _sageContext;

        public EnvCliController(SageContext sageContext)
        {
            _sageContext = sageContext;
        }

        public PartialViewResult GetByIdCliente(string CodigoCliente, int? Linea)
        {
            CodigoCliente = CodigoCliente.Trim();

            var direcciones = _sageContext.Env_Cli.Where(f => f.Cliente.Trim() == CodigoCliente).ToList();
            ViewBag.Direcciones = direcciones;

            DireccionViewModel model = null;

            if (Linea.HasValue && direcciones.Any())
            {
                var direccion = direcciones.SingleOrDefault(f => f.Linea == Linea);
                if (direccion != null)
                {
                    model = new DireccionViewModel()
                    {
                        LineaEnvCli = Linea.Value,
                        Direccion = direccion.Direccion,
                        Provincia = direccion.Provincia,
                        CodigoPostal = direccion.CodPos,
                        Poblacion = direccion.Poblacion
                    };                    
                }
            }

            if (model == null) //Coger la dirección del cliente (tabla clientes)
            {
                var cliente = _sageContext.Clientes.SingleOrDefault(f => f.Codigo.Trim() == CodigoCliente);
                if (cliente != null)
                {
                    model = new DireccionViewModel()
                    {
                        Direccion = cliente.Direccion,
                        Provincia = cliente.Provincia,
                        CodigoPostal = cliente.CodPost,
                        Poblacion = cliente.Poblacion
                    };
                }
            }

            return PartialView("_DireccionesCliente", model);
        }
    }
}

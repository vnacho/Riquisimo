using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Ferpuser.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class ParametroManager
    {
        public ApplicationDbContext db { get; set; }

        public ParametroManager(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public async Task Edit(Parametro model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                transaction.Commit();
            }
        }        

        public DateTime? GetFechaLimite()
        {
            DateTime? dt = null;
            try
            {   
                var param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_FECHA_LIMITE_INFERIOR_FACTURAS_COMPRA_VENTA);
                if (!string.IsNullOrWhiteSpace(param.Valor))
                    dt = Convert.ToDateTime(param.Valor);
            }
            catch
            {
            }
            return dt;
        }

        public DatosEmpresaDto GetDatosEmpresa()
        {
            DatosEmpresaDto DatosEmpresa = new DatosEmpresaDto();
            
            Parametro param = null;

            param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_NOMBRE);
            DatosEmpresa.Nombre = param.Valor;

            param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_DIRECCION);
            DatosEmpresa.Direccion = param.Valor;

            param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_CP);
            DatosEmpresa.CodigoPostal = param.Valor;

            param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_POBLACION);
            DatosEmpresa.Poblacion = param.Valor;

            param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_PROVINCIA);
            DatosEmpresa.Provincia = param.Valor;

            param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_NIF_CIF);
            DatosEmpresa.NifCif = param.Valor;

            param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_LOGO);
            DatosEmpresa.Logo = param.Valor;

            param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_FIRMA);
            DatosEmpresa.Firma = param.Valor;

            param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_NOMBRE_REPRESENTANTE);
            DatosEmpresa.NombreRepresentante = param.Valor;

            param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_NIF_CIF_REPRESENTANTE);
            DatosEmpresa.NifCifRepresentante = param.Valor;

            return DatosEmpresa;
        }

        public async Task EditDatosEmpresa(DatosEmpresaDto DatosEmpresa)
        {
            Parametro param = null;
            using (var transaction = db.Database.BeginTransaction())
            {
                param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_NOMBRE);
                param.Valor = DatosEmpresa.Nombre;
                await db.SaveChangesAsync();

                param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_DIRECCION);
                param.Valor = DatosEmpresa.Direccion;
                await db.SaveChangesAsync();

                param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_CP);
                param.Valor = DatosEmpresa.CodigoPostal;
                await db.SaveChangesAsync();

                param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_POBLACION);
                param.Valor = DatosEmpresa.Poblacion;
                await db.SaveChangesAsync();

                param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_PROVINCIA);
                param.Valor = DatosEmpresa.Provincia;
                await db.SaveChangesAsync();

                param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_NIF_CIF);
                param.Valor = DatosEmpresa.NifCif;
                await db.SaveChangesAsync();

                param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_LOGO);
                param.Valor = DatosEmpresa.Logo;
                await db.SaveChangesAsync();

                param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_FIRMA);
                param.Valor = DatosEmpresa.Firma;
                await db.SaveChangesAsync();

                param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_NOMBRE_REPRESENTANTE);
                param.Valor = DatosEmpresa.NombreRepresentante;
                await db.SaveChangesAsync();

                param = db.Parametros.Find(Consts.PARAMETRO_CODIGO_EMPRESA_NIF_CIF_REPRESENTANTE);
                param.Valor = DatosEmpresa.NifCifRepresentante;
                await db.SaveChangesAsync();

                transaction.Commit();
            }
        }
    }
}

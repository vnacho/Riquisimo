using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Sage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class CentroCosteManager
    {
        public ApplicationDbContext _db { get; set; }
        private SageContext _sagedb;

        public CentroCosteManager(ApplicationDbContext dbContext, SageContext sagedb)
        {
            _db = dbContext;
            _sagedb = sagedb;
        }

        public async Task Create(CentroCoste model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                model.NivelAnalitico1 = model.NivelAnalitico1.Trim();
                model.NivelAnalitico2 = model.NivelAnalitico2.Trim();
                
                SageComprobacion(model);

                _db.CentrosCoste.Add(model);

                await _sagedb.SaveChangesAsync();
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task Edit(CentroCoste model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                model.NivelAnalitico1 = model.NivelAnalitico1.Trim();
                model.NivelAnalitico2 = model.NivelAnalitico2.Trim();

                SageComprobacion(model);

                _db.Entry(model).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }

        private void SageComprobacion(CentroCoste model)
        {
            bool bExisteNivel1 = _sagedb.secundar.Any(f => f.CODIGO == model.NivelAnalitico1);
            bool bExisteNivel2 = _sagedb.secundar.Any(f => f.CODIGO == model.NivelAnalitico2);

            string nombre50 = model.Nombre.Length > 50 ? model.Nombre.Substring(0,50) : model.Nombre;
            string nombre30 = model.Nombre.Length > 30 ? model.Nombre.Substring(0, 30) : model.Nombre;

            if (!bExisteNivel1)
            {
                string guid = Guid.NewGuid().ToString();
                _sagedb.secundar.Add(new secundar()
                {
                    CODIGO = model.NivelAnalitico1,
                    NOMBRE = nombre50,
                    NIVEL = 1,
                    CREATED = DateTime.Now,
                    MODIFIED = DateTime.Now,
                    GUID = guid,
                    GUID_ID = guid,
                    LIBRE_1 = string.Empty,
                    LIBRE_2 = string.Empty,
                    LIBRE_3 = string.Empty
                });
            }

            if (!bExisteNivel2)
            {
                string guid = Guid.NewGuid().ToString();
                _sagedb.secundar.Add(new secundar() { 
                    CODIGO = model.NivelAnalitico2, 
                    NOMBRE = nombre50, 
                    NIVEL = 2, 
                    CREATED = DateTime.Now, 
                    MODIFIED = DateTime.Now,
                    GUID = guid,
                    GUID_ID = guid,
                    LIBRE_1 = string.Empty,
                    LIBRE_2 = string.Empty,
                    LIBRE_3 = string.Empty
                });
                _sagedb.Almacen.Add(new Almacen() { Codigo = model.NivelAnalitico2, Nombre = nombre30 });

                TipoCentroCoste tipo = _db.TiposCentroCoste.Find(model.TipoCentroCosteId);
                _sagedb.alma_anali.Add(new alma_anali()
                {
                    ALMACEN = model.NivelAnalitico2,
                    PLANCONT = "01",
                    SECNIVEL1 = model.NivelAnalitico1,
                    SECNIVEL2 = model.NivelAnalitico2,
                    PORCENTAJE = tipo.PorcentajeDistribucion,
                    CREATED = DateTime.Now,
                    MODIFIED = DateTime.Now,
                    GUID_ID = Guid.NewGuid().ToString()
                });

                _db.Congresses.Add(new Congress() { 
                    Id = Guid.NewGuid(), 
                    Code = "XXX", //Requisito cliente 16/06/2021
                    Number = Convert.ToInt32(model.NivelAnalitico2), 
                    Name = nombre50,
                    TipoCongress = "Z"
                });
            }

            if (!bExisteNivel1 || !bExisteNivel2)
                _sagedb.plan_d.Add(new plan_d() { 
                    PLANCONT = "01", 
                    SECNIVEL1 = model.NivelAnalitico1, 
                    SECNIVEL2 = model.NivelAnalitico2, 
                    CREATED = DateTime.Now, 
                    MODIFIED = DateTime.Now,
                    GUID_ID = Guid.NewGuid().ToString()
                });
        }

        public async Task Delete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.Entry(_db.CentrosCoste.Find(id)).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}

using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Transfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Ferpuser.Models.Consts;

namespace Ferpuser.BLL.Managers
{
    public class VentaPedidoManager
    {
        private readonly object lockPedido = new object();

        public ApplicationDbContext _db { get; set; }
        public SageContextFactoryHelper _sageContextFactory { get; set; }

        public VentaPedidoManager(ApplicationDbContext dbContext, SageContextFactoryHelper sageContextFactory)
        {
            _db = dbContext;
            _sageContextFactory = sageContextFactory;
        }

        /// <summary>
        /// Creación de un pedido de venta
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void Create(VentaPedido model)
        {            
            using (var transaction = _db.Database.BeginTransaction())
            {
                //Buscar el número de pedido que corresponde
                SageContext sageContext = _sageContextFactory.CreateSageContext(model.Fecha.Year);

                if (sageContext == null)
                    throw new Exception($"{nameof(VentaPedidoManager)} : No se encuentra una base de datos para el año {model.Fecha.Year}.");

                var serie = sageContext.Serie.SingleOrDefault(f => 
                    f.Tipodoc == Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_PEDIDO &&
                    f.Empresa == Consts.CODIGO_EMPRESA && 
                    f.Serie == model.Serie
                );
                if (serie == null)
                    throw new Exception($"{nameof(VentaPedidoManager)} : No se encuentra la serie {model.Serie} para el Tipodoc {Consts.SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_PEDIDO}");

                model.CodigoPedido = (int)serie.Contador + 1;
                _db.VentaPedidos.Add(model);
                _db.SaveChanges();
                transaction.Commit();

                //Actualizar el contador en series
                serie.Contador = model.CodigoPedido;
                sageContext.Entry(serie).State = EntityState.Modified;
                sageContext.SaveChanges();                
            }            
        }        
    }
}

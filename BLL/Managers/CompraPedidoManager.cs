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

namespace Ferpuser.BLL.Managers
{
    public class CompraPedidoManager
    {
        private readonly object lockPedido = new object();

        public ApplicationDbContext _db { get; set; }
        public SageContextFactoryHelper _sageContextFactory { get; set; }

        public CompraPedidoManager(ApplicationDbContext dbContext, SageContextFactoryHelper sageContextFactory)
        {
            _db = dbContext;
            _sageContextFactory = sageContextFactory;
        }

        /// <summary>
        /// Creación de un pedido de compra
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void Create(CompraPedido model)
        {
            lock (lockPedido)
            {
                //Obtener el código de pedido correspondiente
                int codigoPedido = 1;
                SageContext sageContext = _sageContextFactory.CreateSageContext(model.Fecha.Year);
                var empresa = sageContext.empresa.FirstOrDefault();
                if (empresa != null)
                    codigoPedido = empresa.PEDICOM;

                string sCodigoPedido = $"{model.Fecha.ToString("yy")}{codigoPedido.ToString().PadLeft(6,'0')}";

                using (var transaction = _db.Database.BeginTransaction())
                {
                    model.CodigoPedido = sCodigoPedido;
                    _db.CompraPedidos.Add(model);
                    _db.SaveChanges();

                    transaction.Commit();
                }

                //Actualizar el código de pedido
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PEDICOM", codigoPedido + 1));
                parameters.Add(new SqlParameter("@CODIGO", empresa.CODIGO));
                sageContext.Database.ExecuteSqlRaw(
                    $"UPDATE empresa SET PEDICOM=@PEDICOM WHERE CODIGO=@CODIGO",
                    parameters);
            }
        }        
    }
}

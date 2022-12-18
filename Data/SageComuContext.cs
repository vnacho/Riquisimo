using Ferpuser.Models.Sage;
using Ferpuser.Models.SageComu;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Ferpuser.Data
{
    public class SageComuContext : DbContext
    {
        public SageComuContext(DbContextOptions<SageComuContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Letras>().HasKey(c => new { c.Codigo, c.Grupo });
            modelBuilder.Entity<ejercici>().HasKey(c => new { c.ANY, c.GRUPO });
            modelBuilder.Entity<previs>().HasKey(c => new { c.EMPRESA, c.PROVEEDOR, c.FACTURA, c.NUMEREB, c.PENDIENTE, c.EMISION, c.DOCCARGO });
            //modelBuilder.Entity<Previ_Cl>().HasKey(c => new { c.Factura });
        }

        public DbSet<Previ_Cl> Previ_Cl { get; set; }
        public DbSet<Letras> Letras { get; set; }
        public virtual DbSet<Operario> Operarios { get; set; }
        public DbSet<ejercici> Ejercicios { get; set; }
        public virtual DbSet<previs> previs { get; set; }
        public virtual DbSet<paises> paises { get; set; }
    }
}

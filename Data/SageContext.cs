using Ferpuser.Models.Sage;
using Microsoft.EntityFrameworkCore;

namespace Ferpuser.Data
{
    public class SageContext : DbContext
    {
        public SageContext(DbContextOptions<SageContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Env_Cli>().HasKey(c => new { c.Cliente, c.Linea });
            modelBuilder.Entity<Series>(entity =>
            {
                entity.HasKey(e => new { e.Empresa, e.Tipodoc, e.Serie })
                    .HasName("pk__2019sw__series__emptipser");

                entity.ToTable("series");

                entity.HasIndex(e => e.Serie)
                    .HasName("serie");

                entity.HasIndex(e => e.Tipodoc)
                    .HasName("tipodoc");

                entity.Property(e => e.Empresa)
                    .HasColumnName("EMPRESA")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tipodoc).HasColumnName("TIPODOC");

                entity.Property(e => e.Serie)
                    .HasColumnName("SERIE")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Contador)
                    .HasColumnName("CONTADOR")
                    .HasColumnType("numeric(10, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Created)
                    .HasColumnName("CREATED")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Guid_Id)
                    .IsRequired()
                    .HasColumnName("GUID_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('NEWID()')");

                entity.Property(e => e.Modified)
                    .HasColumnName("MODIFIED")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Vista)
                    .HasColumnName("VISTA")
                    .HasDefaultValueSql("((0))");
            });
            modelBuilder.Entity<Asientos>().HasKey(c => new { c.Empresa, c.Numero, c.Linea });
            modelBuilder.Entity<referpro>().HasKey(c => new { c.ARTICULO, c.PROVEEDOR, c.MONEDA, c.TALLA, c.COLOR });
            modelBuilder.Entity<c_albcom>().HasKey(c => new { c.EMPRESA, c.NUMERO, c.PROVEEDOR });
            modelBuilder.Entity<otrasien>().HasKey(c => new { c.EMPRESA, c.SECUNDAR, c.CODCUEN, c.ASI, c.SECNIVEL2, c.PLANCONT });
            modelBuilder.Entity<plan_d>().HasKey(c => new { c.PLANCONT, c.SECNIVEL1, c.SECNIVEL2 });
            modelBuilder.Entity<alma_anali>().HasKey(c => new { c.ALMACEN, c.PLANCONT, c.SECNIVEL1, c.SECNIVEL2 });
            modelBuilder.Entity<codpos>().HasKey(c => new { c.CODIGO, c.LINEA });
            modelBuilder.Entity<c_pedive>().HasKey(c => new { c.EMPRESA, c.NUMERO, c.LETRA });
            modelBuilder.Entity<pvp>().HasKey(c => new { c.ARTICULO, c.TARIFA, c.FECHAINI });
            modelBuilder.Entity<d_albven>().HasKey(c => new { c.EMPRESA, c.NUMERO, c.LINIA, c.LETRA });
            modelBuilder.Entity<ContlfPro>(entity =>
            {
                entity.HasKey(e => new { e.Proveedor, e.Linea })
                    .HasName("pk__2022sw__contlf_pro__prolin");

                entity.ToTable("contlf_pro");

                entity.HasIndex(e => e.GuidId)
                    .HasName("guid_id");

                entity.HasIndex(e => e.Linea)
                    .HasName("linea");

                entity.HasIndex(e => e.Persona)
                    .HasName("persona");

                entity.HasIndex(e => new { e.Proveedor, e.Observa })
                    .HasName("fax");

                entity.Property(e => e.Proveedor)
                    .HasColumnName("PROVEEDOR")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Linea).HasColumnName("LINEA");

                entity.Property(e => e.Cargo)
                    .IsRequired()
                    .HasColumnName("CARGO")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Created)
                    .HasColumnName("CREATED")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("EMAIL")
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Exportar)
                    .HasColumnName("EXPORTAR")
                    .HasColumnType("datetime");

                entity.Property(e => e.Facebook)
                    .IsRequired()
                    .HasColumnName("FACEBOOK")
                    .HasMaxLength(254)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Guid)
                    .IsRequired()
                    .HasColumnName("GUID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.GuidExp)
                    .IsRequired()
                    .HasColumnName("GUID_EXP")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.GuidId)
                    .IsRequired()
                    .HasColumnName("GUID_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Importar)
                    .HasColumnName("IMPORTAR")
                    .HasColumnType("datetime");

                entity.Property(e => e.Lincontpro).HasColumnName("LINCONTPRO");

                entity.Property(e => e.Lintelfpro).HasColumnName("LINTELFPRO");

                entity.Property(e => e.Modified)
                    .HasColumnName("MODIFIED")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Observa)
                    .IsRequired()
                    .HasColumnName("OBSERVA")
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Persona)
                    .IsRequired()
                    .HasColumnName("PERSONA")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Predet).HasColumnName("PREDET");

                entity.Property(e => e.Skype)
                    .IsRequired()
                    .HasColumnName("SKYPE")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasColumnName("TELEFONO")
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("TIPO")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Twitter)
                    .IsRequired()
                    .HasColumnName("TWITTER")
                    .HasMaxLength(254)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Vista)
                    .IsRequired()
                    .HasColumnName("VISTA")
                    .HasDefaultValueSql("((1))");
            });

        }

        public DbSet<Tipo_IVA> Tipo_IVA { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Env_Cli> Env_Cli { get; set; }
        public DbSet<C_Albven> C_Albven { get; set; }
        public DbSet<Series> Serie { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Almacen> Almacen { get; set; }
        public DbSet<Asientos> Asientos { get; set; }
        public DbSet<FPag> FPag { get; set; }
        public DbSet<Vendedor> Vendedor { get; set; }
        public virtual DbSet<ProveedorSage> Proveedores { get; set; }
        public virtual DbSet<referpro> PreciosProveed { get; set; }
        public virtual DbSet<empresa> empresa { get; set; }
        public virtual DbSet<c_albcom> c_albcom { get; set; }
        public virtual DbSet<tipo_ret> tipo_ret { get; set; }
        public virtual DbSet<secundar> secundar { get; set; }
        public virtual DbSet<otrasien> otrasien { get; set; }
        public virtual DbSet<plans> plans { get; set; }
        public virtual DbSet<cuentas> cuentas { get; set; }
        public virtual DbSet<plan_d> plan_d { get; set; }
        public virtual DbSet<alma_anali> alma_anali { get; set; }
        public virtual DbSet<codpos> codpos { get; set; }
        public virtual DbSet<c_pedive> c_pedive { get; set; }
        public virtual DbSet<pvp> pvp { get; set; }
        public virtual DbSet<d_albven> d_albven { get; set; }
        public virtual DbSet<modconfi> modconfi { get; set; }
        public virtual DbSet<ContlfPro> ContlfPro { get; set; }


    }
}

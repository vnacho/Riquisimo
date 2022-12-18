using System;
using System.Collections.Generic;
using System.Text;
using Ferpuser.Models;
using Ferpuser.Models.Consts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ferpuser.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parametro>().HasData(
                new Parametro
                {
                    Codigo = Consts.PARAMETRO_CODIGO_FECHA_LIMITE_INFERIOR_FACTURAS_COMPRA_VENTA,                    
                    Descripcion = "Indica la fecha límite inferior bajo la que no se permitirá introducir facturas de compra o venta (Fecha de factura). Si se deja vacía no se producirá tal restricción.",
                    Formato = "dd/MM/yyyy"
                },
                new Parametro
                {
                    Codigo = Consts.PARAMETRO_CODIGO_EMPRESA_NOMBRE,
                    Descripcion = "Indica el nombre de la empresa."
                },
                new Parametro
                {
                    Codigo = Consts.PARAMETRO_CODIGO_EMPRESA_DIRECCION,
                    Descripcion = "Indica la dirección de la empresa."
                },
                new Parametro
                {
                    Codigo = Consts.PARAMETRO_CODIGO_EMPRESA_CP,
                    Descripcion = "Indica el código postal de la empresa.",
                },
                new Parametro
                {
                    Codigo = Consts.PARAMETRO_CODIGO_EMPRESA_POBLACION,
                    Descripcion = "Indica la población de la empresa."
                },
                new Parametro
                {
                    Codigo = Consts.PARAMETRO_CODIGO_EMPRESA_PROVINCIA,
                    Descripcion = "Indica la provincia de la empresa."
                },
                new Parametro
                {
                    Codigo = Consts.PARAMETRO_CODIGO_EMPRESA_NIF_CIF,
                    Descripcion = "Indica el NIF/CIF de la empresa."
                },
                new Parametro
                {
                    Codigo = Consts.PARAMETRO_CODIGO_EMPRESA_LOGO,
                    Descripcion = "Respresenta el logo de la empresa.",
                    Formato = "Base64"
                },
                new Parametro
                {
                    Codigo = Consts.PARAMETRO_CODIGO_EMPRESA_FIRMA,
                    Descripcion = "Respresenta la firma de la empresa.",
                    Formato = "Base64"
                },
                new Parametro
                {
                    Codigo = Consts.PARAMETRO_CODIGO_EMPRESA_NOMBRE_REPRESENTANTE,
                    Descripcion = "Indica el nombre del representante de la empresa."
                },
                new Parametro
                {
                    Codigo = Consts.PARAMETRO_CODIGO_EMPRESA_NIF_CIF_REPRESENTANTE,
                    Descripcion = "Indica el NIF/CIF del representante de la empresa."
                }
            );

            modelBuilder.Entity<CategoriaInscrito>().HasData(
                new CategoriaInscrito
                {
                    Id = "1",
                    Nombre = "Asistente"
                },
                new CategoriaInscrito
                {
                    Id = "2",
                    Nombre = "Ponente"
                },
                new CategoriaInscrito
                {
                    Id = "3",
                    Nombre = "Moderador"
                },
                new CategoriaInscrito
                {
                    Id = "4",
                    Nombre = "Comité Científico"
                },
                new CategoriaInscrito
                {
                    Id = "5",
                    Nombre = "Comité Organizador"
                },
                new CategoriaInscrito
                {
                    Id = "6",
                    Nombre = "Junta Directiva"
                }
            );

            modelBuilder.Entity<TipoComite>().HasData(
                new TipoComite
                {
                    Id = 1,
                    Nombre = ""
                },
                new TipoComite
                {
                    Id = 2,
                    Nombre = "Junta Directiva"
                },
                new TipoComite
                {
                    Id = 3,
                    Nombre = "Comité de Honor"
                },
                new TipoComite
                {
                    Id = 4,
                    Nombre = "Comité Organizador"
                },
                new TipoComite
                {
                    Id = 5,
                    Nombre = "Comité Científico"
                },
                new TipoComite
                {
                    Id = 6,
                    Nombre = "Comité Asesor"
                }
            );

            modelBuilder.Entity<PuestoComite>().HasData(
               new PuestoComite
               {
                   Id = 1,
                   Nombre = ""
               },
               new PuestoComite
               {
                   Id = 2,
                   Nombre = "Presidencia de Honor"
               },
               new PuestoComite
               {
                   Id = 3,
                   Nombre = "Vicepresidencia de Honor"
               },
               new PuestoComite
               {
                   Id = 4,
                   Nombre = "Miembros de Honor"
               },
               new PuestoComite
               {
                   Id = 5,
                   Nombre = "Presidente"
               },
               new PuestoComite
               {
                   Id = 6,
                   Nombre = "Presidenta"
               },
               new PuestoComite
               {
                   Id = 7,
                   Nombre = "Vicepresidente"
               },
               new PuestoComite
               {
                   Id = 8,
                   Nombre = "Vicepresidenta"
               },
               new PuestoComite
               {
                   Id = 9,
                   Nombre = "Coordinadora"
               },
               new PuestoComite
               {
                   Id = 10,
                   Nombre = "Secretario"
               },
               new PuestoComite
               {
                   Id = 11,
                   Nombre = "Secretaria"
               },
               new PuestoComite
               {
                   Id = 12,
                   Nombre = "Tesorero"
               },
               new PuestoComite
               {
                   Id = 13,
                   Nombre = "Tesorera"
               },
               new PuestoComite
               {
                   Id = 14,
                   Nombre = "Coordinadora Área Científica"
               },
               new PuestoComite
               {
                   Id = 15,
                   Nombre = "Vocales"
               },
               new PuestoComite
               {
                   Id = 16,
                   Nombre = "Presidenta de las Jornadas"
               }
            );

            modelBuilder.Entity<DocumentType>().HasData(
                new DocumentType
                {
                    Id = new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"),
                    Name = "Factura",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                },
                new DocumentType
                {
                    Id = new Guid("043d73bf-1516-4672-affe-4f0836048f40"),
                    Name = "Factura proforma",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                },
                new DocumentType
                {
                    Id = new Guid("fd4d4c0d-53cb-4844-a520-cba0dac861c4"),
                    Name = "Presupuesto",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                }
            );

            modelBuilder.Entity<Registration>().Property(p => p.DocumentTypeId).HasDefaultValue(new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"));
            modelBuilder.Entity<Accommodation>().Property(p => p.DocumentTypeId).HasDefaultValue(new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"));
            modelBuilder.Entity<Expense>().Property(p => p.DocumentTypeId).HasDefaultValue(new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39"));

            modelBuilder.Entity<Registration>().Property(p => p.Product).HasDefaultValue("70100");
            modelBuilder.Entity<Registration>().Property(p => p.Serie).HasDefaultValue("I ");

            modelBuilder.Entity<Accommodation>().Property(p => p.Product).HasDefaultValue("70800");
            modelBuilder.Entity<Accommodation>().Property(p => p.Serie).HasDefaultValue("A ");

            modelBuilder.Entity<Registration>().Property(p => p.ShowCostCenterInfoOnInvoice).HasDefaultValue(true);
            modelBuilder.Entity<Accommodation>().Property(p => p.ShowCostCenterInfoOnInvoice).HasDefaultValue(true);
            modelBuilder.Entity<Expense>().Property(p => p.ShowCostCenterInfoOnInvoice).HasDefaultValue(true);

            modelBuilder.Entity<CongressEmailAccounts>().HasIndex(p => new { p.CongressId, p.AccountId }).IsUnique();

            modelBuilder.Entity<TipoCentroCoste>().Property(f => f.PorcentajeDistribucion).HasColumnType("decimal(5,2)");
            modelBuilder.Entity<Personal>().Property(f => f.CosteEstandar).HasColumnType("decimal(13,2)");
            modelBuilder.Entity<Personal>().Property(f => f.Venta).HasColumnType("decimal(13,2)");
            modelBuilder.Entity<Estructura>().Property(f => f.PorcentajeReparto).HasColumnType("decimal(5,2)");
            modelBuilder.Entity<Personal>().Property(f => f.PrecioHora).HasColumnType("decimal(5,2)");


            modelBuilder.Entity<PartePersonal>()
                .HasOne(f => f.Personal)
                .WithMany()
                .HasForeignKey(f => f.PersonalId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GrabacionE9>()
                .HasOne(f => f.Destino)
                .WithMany()
                .HasForeignKey(f => f.DestinoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Encuentro>()
                .HasOne(f => f.Congress)
                .WithMany(f => f.Encuentros)
                .HasForeignKey(f => f.CongressId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Congress> Congresses { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ClientLocation> ClientLocations { get; set; }
        public DbSet<RegistrantLocation> RegistrantLocations { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<RegistrationType> RegistrationTypes { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Registrant> Registrant { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<CongressEmailAccounts> CongressEmailAccounts { get; set; }
        public DbSet<CompraPedido> CompraPedidos { get; set; }
        public DbSet<CompraPedidoLinea> CompraPedidoLineas { get; set; }
        public DbSet<CompraAlbaran> CompraAlbaranes { get; set; }
        public DbSet<CompraAlbaranLinea> CompraAlbaranLineas { get; set; }
        public DbSet<CompraFactura> CompraFacturas { get; set; }
        public DbSet<CompraFacturaLinea> CompraFacturaLineas { get; set; }
        public DbSet<TipoCentroCoste> TiposCentroCoste { get; set; }
        public DbSet<CentroCoste> CentrosCoste { get; set; }
        public DbSet<TipoTarifa> TiposTarifa { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Estructura> Estructura { get; set; }
        public DbSet<PartePersonal> PartePersonal { get; set; }
        public DbSet<GrabacionE9> GrabacionE9 { get; set; }
        public DbSet<Documento> Documento { get; set; }
        public DbSet<Origen> Origen { get; set; }
        public DbSet<Asistente> Asistente { get; set; }
        public DbSet<VerOrigen> VerOrigen { get; set; }
        public DbSet<TipoContrato> TipoContrato { get; set; }
        public DbSet<VentaPedido> VentaPedidos { get; set; }
        public DbSet<VentaPedidoLinea> VentaPedidoLineas { get; set; }
        public DbSet<VentaAlbaran> VentaAlbaranes { get; set; }
        public DbSet<VentaAlbaranLinea> VentaAlbaranLineas { get; set; }
        public DbSet<VentaFactura> VentaFacturas { get; set; }
        public DbSet<VentaFacturaLinea> VentaFacturaLineas { get; set; }
        public DbSet<TipoDocumentoVenta> TiposDocumentoVenta { get; set; }
        public DbSet<ArticulosAlmacen> ArticulosAlmacen { get; set; }
        public DbSet<PartesInternosAlmacen> PartesInternosAlmacen { get; set; }
        public DbSet<ContratoObra> ContratoObras { get; set; }
        public DbSet<DocumentoObra> DocumentoObras { get; set; }
        public DbSet<DocumentoContratoObra> DocumentoContratoObras { get; set; }
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<InventarioArticulosAlmacen> InventarioArticulosAlmacen { get; set; }
        public DbSet<MovimientosArticulosAlmacen> MovimientosArticulosAlmacens { get; set; }
        public DbSet<CategoriaInscrito> CategoriasInscritos { get; set; }
        public DbSet<TipoComite> TiposComite { get; set; }
        public DbSet<PuestoComite> PuestosComite { get; set; }
        public DbSet<Ponente> Ponentes { get; set; }
        public DbSet<Proveedor> Proveedores{ get; set; }
        public DbSet<Encuentro> Encuentros { get; set; }
        public DbSet<Restauracion> Restauraciones { get; set; }     
        public DbSet<DocumentoCompraVenta> DocumentoCompraVenta { get; set; }
        public DbSet<SociedadCientifica> SociedadesCientificas { get; set; }
        public DbSet<CargoJuntaDirectivaSociedadCientifica> CargosJuntaDirectivaSociedadCientifica { get; set; }
        public DbSet<SocioSociedadCientifica> SociosSociedadCientifica { get; set; }
        public DbSet<Tienda> Tiendas { get; set; }
        public DbSet<Tikect> Tikects { get; set; }

    }
}

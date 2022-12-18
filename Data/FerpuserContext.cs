using Ferpuser.Models.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ferpuser.Data
{
    public class FerpuserContext : DbContext
    {
        private readonly HttpContext _httpContext;
        public static readonly string _prefix = "__default_ferpuser_table_prefix__";

        public FerpuserContext(DbContextOptions<FerpuserContext> options, IHttpContextAccessor httpContextAccessor = null)
            : base(options)
        {
            _httpContext = httpContextAccessor?.HttpContext;
        }

        public DbSet<RegistrationTransfer> RegistrationTransfers { get; set; }
        public DbSet<RegistrationTypeTransfer> RegistrationTypeTransfers { get; set; }
        public DbSet<ProvinceTransfer> ProvinceTransfers { get; set; }
        public DbSet<WorkplaceTransfer> WorkplaceTransfers { get; set; }
        public DbSet<AccommodationTransfer> AccommodationTransfers { get; set; }
        public DbSet<HotelTransfer> HotelTransfers { get; set; }
        public DbSet<PonenteTransfer> PonenteTransfers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkplaceTransfer>(entity =>
            {
                entity.ToTable("centros_trabajo");

                entity.HasIndex(e => e.CodCentro)
                    .HasName("cod_centro")
                    .IsUnique();

                entity.HasIndex(e => e.Nombre)
                    .HasName("nombre_2");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CodCentro)
                    .HasColumnName("cod_centro")
                    .HasColumnType("varchar(10)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CodPostal)
                    .HasColumnName("cod_postal")
                    .HasColumnType("varchar(5)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Fax)
                    .HasColumnName("fax")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Mail)
                    .HasColumnName("mail")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(150)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Provincia)
                    .HasColumnName("provincia")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Telefono2)
                    .HasColumnName("telefono2")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<ProvinceTransfer>(entity =>
            {
                entity.ToTable("provincias");

                entity.HasIndex(e => e.Nombre)
                    .HasName("nombre");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ccaa)
                    .HasColumnName("ccaa")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(30)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<RegistrationTypeTransfer>(entity =>
            {
                entity.ToTable(_prefix + "inscripcion_cuotas");

                entity.HasIndex(e => e.Nombre)
                    .HasName("nombre");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasColumnType("enum('Si','No')")
                    .HasDefaultValueSql("'''Si'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Orden)
                    .HasColumnName("orden")
                    .HasColumnType("tinyint(2)");

                entity.Property(e => e.PrecioAntes)
                    .HasColumnName("precio_antes")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PrecioDespues)
                    .HasColumnName("precio_despues")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<RegistrationTransfer>(entity =>
            {

                entity.ToTable(_prefix + "inscripciones");


                entity.HasIndex(e => e.Apellidos)
                    .HasName("apellido1");

                entity.HasIndex(e => e.Nombre)
                    .HasName("nombre");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasColumnName("apellidos")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Cargo)
                    .HasColumnName("cargo")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");                

                entity.Property(e => e.Categoria)
                    .IsRequired()
                    .HasColumnName("categoria")
                    .HasColumnType("enum('Asistente','Ponente','Moderador','Comité')")
                    .HasDefaultValueSql("'''Asistente'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CategoriaProfesional)
                    .HasColumnName("categoria_profesional")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CentroTrabajo)
                    .HasColumnName("centro_trabajo")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.CentroTrabajo2)
                    .HasColumnName("centro_trabajo2")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("'NULL'")
                    .HasComment("por si no existe el centro_trabajo")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CodigoPostal)
                    .HasColumnName("codigo_postal")
                    .HasColumnType("varchar(5)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Comentarios)
                    .HasColumnName("comentarios")
                    .HasColumnType("text")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
                              
                entity.Property(e => e.FacCodigoPostal)
                    .HasColumnName("fac_codigo_postal")
                    .HasColumnType("varchar(5)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacDireccion)
                    .HasColumnName("fac_direccion")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacLocalidad)
                    .HasColumnName("fac_localidad")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacMail)
                    .HasColumnName("fac_mail")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacNif)
                    .HasColumnName("fac_nif")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacNombre)
                    .HasColumnName("fac_nombre")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
                
                entity.Property(e => e.FacPais)
                    .HasColumnName("fac_pais")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacProvincia)
                    .HasColumnName("fac_provincia")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.FechaInscripcion)
                    .HasColumnName("fecha_inscripcion")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'''0000-00-00 00:00:00'''");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasColumnName("mail")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Mail2)
                    .HasColumnName("mail2")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Movil)
                    .HasColumnName("movil")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nif)
                    .IsRequired()
                    .HasColumnName("nif")
                    .HasColumnType("varchar(9)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Pais)
                //    .IsRequired()
                //    .HasColumnName("pais")
                //    .HasColumnType("varchar(50)")
                //    .HasDefaultValueSql("''''''")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Pais)
                    .HasColumnName("pais")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Poblacion)
                    .HasColumnName("poblacion")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PrecioCuota)
                    .HasColumnName("precio_cuota")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Provincia)
                    .HasColumnName("provincia")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");


                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TipoCuota)
                    .HasColumnName("tipo_cuota")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'")
                    .HasComment("el valor es el id de la tabla inscripcion_cuotas");

                entity.Property(e => e.Uid)
                    .HasColumnName("uid")
                    .HasColumnType("varchar(36)")
                    .HasDefaultValueSql("'" + Guid.Empty + "'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                //------------------------------------------------------------

                //entity.Property(e => e.Activo)
                //    .IsRequired()
                //    .HasColumnName("activo")
                //    .HasColumnType("enum('Si','No')")
                //    .HasDefaultValueSql("'''Si'''")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Asistencia0)
                //    .HasColumnName("asistencia0")
                //    .HasColumnType("varchar(100)")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Asistencia1)
                //    .HasColumnName("asistencia1")
                //    .HasColumnType("varchar(100)")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Asistente)
                //    .IsRequired()
                //    .HasColumnName("asistente")
                //    .HasColumnType("enum('Si','No')")
                //    .HasDefaultValueSql("'''Si'''")
                //    .HasComment("indica que el inscrito ha asistido y puede descargarse el diploma ente otras cosas")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Cancelado)
                //    .IsRequired()
                //    .HasColumnName("cancelado")
                //    .HasColumnType("enum('Si','No')")
                //    .HasDefaultValueSql("'''No'''")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");
                //entity.Property(e => e.Cena)
                //    .IsRequired()
                //    .HasColumnName("cena")
                //    .HasColumnType("enum('Si','No')")
                //    .HasDefaultValueSql("'''No'''")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");
                //entity.Property(e => e.Certificado)
                //    .IsRequired()
                //    .HasColumnName("certificado")
                //    .HasColumnType("enum('Si','No')")
                //    .HasDefaultValueSql("'''No'''")
                //    .HasComment("indicamos si el inscrito puede descargarse el certificado")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.ClaveApp)
                //    .HasColumnName("clave_app")
                //    .HasColumnType("int(4)")
                //    .HasDefaultValueSql("'NULL'");
                //entity.Property(e => e.ProvinciaCt)
                //    .HasColumnName("provincia_ct")
                //    .HasColumnType("int(11)")
                //    .HasDefaultValueSql("'NULL'");
                //entity.Property(e => e.Pagado)
                //    .IsRequired()
                //    .HasColumnName("pagado")
                //    .HasColumnType("enum('Si','No')")
                //    .HasDefaultValueSql("'''No'''")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");
                //entity.Property(e => e.FechaCertificado)
                //    .HasColumnName("fecha_certificado")
                //    .HasColumnType("datetime")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasComment("guardamos la fecha en que se ha descargado el certificado");

                //entity.Property(e => e.FechaDiploma)
                //    .HasColumnName("fecha_diploma")
                //    .HasColumnType("datetime")
                //    .HasDefaultValueSql("'NULL'");
                //entity.Property(e => e.Fac1Concepto)
                //    .HasColumnName("fac1_concepto")
                //    .HasColumnType("text")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Fac1Fecha)
                //    .HasColumnName("fac1_fecha")
                //    .HasColumnType("date")
                //    .HasDefaultValueSql("'NULL'");

                //entity.Property(e => e.Fac1Iva)
                //    .HasColumnName("fac1_iva")
                //    .HasColumnType("int(11)")
                //    .HasDefaultValueSql("'NULL'");

                //entity.Property(e => e.Fac1Num)
                //    .HasColumnName("fac1_num")
                //    .HasColumnType("varchar(4)")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Fac1Precio)
                //    .HasColumnName("fac1_precio")
                //    .HasColumnType("int(11)")
                //    .HasDefaultValueSql("'NULL'");

                //entity.Property(e => e.Fac2Cif)
                //    .HasColumnName("fac2_cif")
                //    .HasColumnType("varchar(15)")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Fac2Ciudad)
                //    .HasColumnName("fac2_ciudad")
                //    .HasColumnType("varchar(75)")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Fac2Codigopostal)
                //    .HasColumnName("fac2_codigopostal")
                //    .HasColumnType("varchar(5)")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Fac2Concepto)
                //    .HasColumnName("fac2_concepto")
                //    .HasColumnType("text")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Fac2Direccion)
                //    .HasColumnName("fac2_direccion")
                //    .HasColumnType("varchar(75)")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Fac2Empresa)
                //    .HasColumnName("fac2_empresa")
                //    .HasColumnType("varchar(50)")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Fac2Fecha)
                //    .HasColumnName("fac2_fecha")
                //    .HasColumnType("date")
                //    .HasDefaultValueSql("'NULL'");

                //entity.Property(e => e.Fac2Iva)
                //    .HasColumnName("fac2_iva")
                //    .HasColumnType("int(11)")
                //    .HasDefaultValueSql("'NULL'");

                //entity.Property(e => e.Fac2Num)
                //    .HasColumnName("fac2_num")
                //    .HasColumnType("varchar(4)")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.Fac2Precio)
                //    .HasColumnName("fac2_precio")
                //    .HasColumnType("int(11)")
                //    .HasDefaultValueSql("'NULL'");

                //entity.Property(e => e.Fac2Provincia)
                //    .HasColumnName("fac2_provincia")
                //    .HasColumnType("int(11)")
                //    .HasDefaultValueSql("'NULL'");

                //entity.Property(e => e.FacAbonoConcepto)
                //    .HasColumnName("fac_abono_concepto")
                //    .HasColumnType("text")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.FacAbonoFecha)
                //    .HasColumnName("fac_abono_fecha")
                //    .HasColumnType("date")
                //    .HasDefaultValueSql("'NULL'");

                //entity.Property(e => e.FacAbonoIva)
                //    .HasColumnName("fac_abono_iva")
                //    .HasColumnType("int(11)")
                //    .HasDefaultValueSql("'NULL'");

                //entity.Property(e => e.FacAbonoNum)
                //    .HasColumnName("fac_abono_num")
                //    .HasColumnType("varchar(4)")
                //    .HasDefaultValueSql("'NULL'")
                //    .HasCharSet("utf8")
                //    .HasCollation("utf8_general_ci");

                //entity.Property(e => e.FacAbonoPrecio)
                //    .HasColumnName("fac_abono_precio")
                //    .HasColumnType("int(11)")
                //    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<AccommodationTransfer>(entity =>
            {
                entity.ToTable(_prefix + "alojamientos");

                entity.HasIndex(e => e.Apellidos)
                    .HasName("apellido1");

                entity.HasIndex(e => e.Nombre)
                    .HasName("nombre");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasColumnType("enum('Si','No')")
                    .HasDefaultValueSql("'''Si'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasColumnName("apellidos")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ApellidosAcompanya)
                    .HasColumnName("apellidos_acompanya")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Cancelado)
                    .IsRequired()
                    .HasColumnName("cancelado")
                    .HasColumnType("enum('Si','No')")
                    .HasDefaultValueSql("'''No'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Cargo)
                    .HasColumnName("cargo")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CategoriaProfesional)
                    .HasColumnName("categoria_profesional")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CentroTrabajo)
                    .HasColumnName("centro_trabajo")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.CentroTrabajo2)
                    .HasColumnName("centro_trabajo2")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("'NULL'")
                    .HasComment("por si no existe el centro_trabajo")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CodigoPostal)
                    .HasColumnName("codigo_postal")
                    .HasColumnType("varchar(10)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CodigoPostalAcompanya)
                    .HasColumnName("codigo_postal_acompanya")
                    .HasColumnType("varchar(10)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Comentarios)
                    .HasColumnName("comentarios")
                    .HasColumnType("text")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DireccionAcompanya)
                    .HasColumnName("direccion_acompanya")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacCodigoPostal)
                    .HasColumnName("fac_codigo_postal")
                    .HasColumnType("varchar(10)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacDireccion)
                    .HasColumnName("fac_direccion")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacLocalidad)
                    .HasColumnName("fac_localidad")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacMail)
                    .HasColumnName("fac_mail")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacNif)
                    .HasColumnName("fac_nif")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacNombre)
                    .HasColumnName("fac_nombre")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacPais)
                    .IsRequired()
                    .HasColumnName("fac_pais")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FacProvincia)
                    .HasColumnName("fac_provincia")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.FechaInscripcion)
                    .HasColumnName("fecha_inscripcion")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'''0000-00-00 00:00:00'''");

                entity.Property(e => e.FechaLlegada)
                    .HasColumnName("fecha_llegada")
                    .HasColumnType("date")
                    .HasDefaultValueSql("'''0000-00-00'''");

                entity.Property(e => e.FechaSalida)
                    .HasColumnName("fecha_salida")
                    .HasColumnType("date")
                    .HasDefaultValueSql("'''0000-00-00'''");

                entity.Property(e => e.IdHotel)
                    .HasColumnName("id_hotel")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'")
                    .HasComment("el valor es el id de la tabla hoteles");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasColumnName("mail")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Mail2)
                    .HasColumnName("mail2")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MailAcompanya)
                    .HasColumnName("mail_acompanya")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Movil)
                    .HasColumnName("movil")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nif)
                    .IsRequired()
                    .HasColumnName("nif")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NifAcompanya)
                    .HasColumnName("nif_acompanya")
                    .HasColumnType("varchar(9)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NombreAcompanya)
                    .HasColumnName("nombre_acompanya")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Pais)
                    .IsRequired()
                    .HasColumnName("pais")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Poblacion)
                    .HasColumnName("poblacion")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PoblacionAcompanya)
                    .HasColumnName("poblacion_acompanya")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Precio)
                    .HasColumnName("precio")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Provincia)
                    .HasColumnName("provincia")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ProvinciaAcompanya)
                    .HasColumnName("provincia_acompanya")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ProvinciaCt)
                    .HasColumnName("provincia_ct")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TelefonoAcompanya)
                    .HasColumnName("telefono_acompanya")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TipoHabitacion)
                    .IsRequired()
                    .HasColumnName("tipo_habitacion")
                    .HasColumnType("enum('Doble Uso Individual','Doble')")
                    .HasDefaultValueSql("'''Doble Uso Individual'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<HotelTransfer>(entity =>
            {
                entity.ToTable(_prefix + "hoteles");

                entity.HasIndex(e => e.Orden)
                    .HasName("orden");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("tinyint(10) unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasColumnType("enum('Si','No')")
                    .HasDefaultValueSql("'''Si'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CompletoDoble)
                    .IsRequired()
                    .HasColumnName("completo_doble")
                    .HasColumnType("enum('Si','No')")
                    .HasDefaultValueSql("'''No'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CompletoIndividual)
                    .IsRequired()
                    .HasColumnName("completo_individual")
                    .HasColumnType("enum('Si','No')")
                    .HasDefaultValueSql("'''No'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CoordenadasX).HasColumnName("coordenadasX");

                entity.Property(e => e.CoordenadasY).HasColumnName("coordenadasY");

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasColumnType("varchar(150)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Estrellas)
                    .IsRequired()
                    .HasColumnName("estrellas")
                    .HasColumnType("enum('0','1','2','3','4','5')")
                    .HasDefaultValueSql("'''0'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Localidad)
                    .HasColumnName("localidad")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Mail)
                    .HasColumnName("mail")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(70)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Observaciones)
                    .HasColumnName("observaciones")
                    .HasColumnType("text")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Orden)
                    .HasColumnName("orden")
                    .HasColumnType("tinyint(2) unsigned");

                entity.Property(e => e.PrecioDoble)
                    .HasColumnName("precio_doble")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.PrecioIndividual)
                    .HasColumnName("precio_individual")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Provincia)
                    .HasColumnName("provincia")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasColumnType("varchar(15)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<PonenteTransfer>(entity =>
            {         
                //Tabla
                entity.ToTable(_prefix + "miembros_comites");

                //Índices
                entity.HasIndex(e => e.Id)
                    .HasName("id");
                entity.HasIndex(e => e.Login)
                   .HasName("login");
                entity.HasIndex(e => e.Nombre)
                   .HasName("nombre");
                entity.HasIndex(e => e.Apellidos)
                   .HasName("apellidos");

                //Columnas
                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("id")
                    .HasColumnType("tinyint(10) unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Login)
                   .IsRequired()
                   .HasColumnName("login")
                   .HasColumnType("varchar(15)")
                   .HasDefaultValueSql("''''''")
                   .HasCharSet("utf8")
                   .HasCollation("utf8_general_ci");

                entity.Property(e => e.Password)
                   .IsRequired()
                   .HasColumnName("password")
                   .HasColumnType("varchar(15)")
                   .HasDefaultValueSql("''''''")
                   .HasCharSet("utf8")
                   .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasColumnName("apellidos")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''''''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Localidad)
                    .HasColumnName("localidad")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Provincia)
                    .HasColumnName("provincia")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Cargo)
                    .HasColumnName("cargo")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CentroTrabajo)
                    .HasColumnName("centro_trabajo")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasColumnType("varchar(30)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Movil)
                    .HasColumnName("movil")
                    .HasColumnType("varchar(30)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Mail)
                    .HasColumnName("mail")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Mail2)
                    .HasColumnName("mail2")
                    .HasColumnType("varchar(75)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Tratamiento1)
                  .IsRequired()
                  .HasColumnName("tratamiento1")
                  .HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.Tratamiento2)
                    .HasColumnName("tratamiento2")
                    .HasColumnType("enum('D.', 'Dª.', 'Dr.', 'Dra.', 'Prof.')")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.AmbitoComite)
                   .IsRequired()
                   .HasColumnName("ambito_comite")
                   .HasColumnType("enum('Local', 'Nacional')")
                   .HasDefaultValueSql("'NULL'")
                   .HasCharSet("utf8")
                   .HasCollation("utf8_general_ci");

                entity.Property(e => e.TipoComite)
                  .IsRequired()
                  .HasColumnName("tipo_comite")
                  .HasColumnType("tinyint(3) unsigned")
                  .HasDefaultValueSql("1");

                entity.Property(e => e.TipoComite2)
                  .IsRequired()
                  .HasColumnName("tipo_comite2")
                  .HasColumnType("tinyint(3) unsigned")
                  .HasDefaultValueSql("1");

                entity.Property(e => e.PuestoComite)
                  .IsRequired()
                  .HasColumnName("puesto_comite")
                  .HasColumnType("int(10) unsigned")
                  .HasDefaultValueSql("1");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasColumnType("enum('Si','No')")
                    .HasDefaultValueSql("'''Si'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasColumnName("visible")
                    .HasColumnType("enum('Si','No')")
                    .HasDefaultValueSql("'''Si'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UltimoAcceso)
                    .HasColumnName("ultimo_acceso")
                    .HasColumnType("date")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Comentarios)
                    .HasColumnName("comentarios")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'NULL'")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SuperEvaluador)
                    .IsRequired()
                    .HasColumnName("superevaluador")
                    .HasColumnType("enum('Si','No')")
                    .HasDefaultValueSql("'''No'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Visualizador)
                    .IsRequired()
                    .HasColumnName("visualizador")
                    .HasColumnType("enum('Si','No')")
                    .HasDefaultValueSql("'''No'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.JuntaDirectiva)
                    .IsRequired()
                    .HasColumnName("junta_directiva")
                    .HasColumnType("enum('Si','No')")
                    .HasDefaultValueSql("'''No'''")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Transfer
{

    public partial class RegistrationTransfer
    {
        public Guid? Uid { get; set; }
        [Key]
        public int Id { get; set; }
        public string Nif { get; set; }
        public string Mail { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Poblacion { get; set; }
        public int? Provincia { get; set; }
        public string CodigoPostal { get; set; }
        public string Pais { get; set; }
        public string Movil { get; set; }
        public string Telefono { get; set; }
        public string Mail2 { get; set; }
        public string Cargo { get; set; }
        public string CategoriaProfesional { get; set; }
        public int? CentroTrabajo { get; set; }
        public string CentroTrabajo2 { get; set; }
        public int? TipoCuota { get; set; }
        public int? PrecioCuota { get; set; }
        public string FacNombre { get; set; }
        public string FacNif { get; set; }
        public string FacDireccion { get; set; }
        public string FacCodigoPostal { get; set; }
        public int? FacProvincia { get; set; }
        public string FacLocalidad { get; set; }
        public string FacMail { get; set; }
        public string FacPais { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string Comentarios { get; set; }
        public string Categoria { get; set; }

        [NotMapped]
        public string Laboratorio { get; set; }

        [NotMapped]
        public string Especialidad { get; set; }

        /// <summary>
        /// Método auxiliar para parsear desde una consulta de MySQL con ADO.Net sin Entity Framework
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static RegistrationTransfer Parse(DataRow dr)
        {
            RegistrationTransfer item = new RegistrationTransfer();
            item.Id = Convert.ToInt32(dr["id"]);
            item.Apellidos = dr["apellidos"].ToString();
            item.Cargo = dr["cargo"] == DBNull.Value ? null : dr["cargo"].ToString();
            item.Categoria = dr["categoria"].ToString();
            item.CategoriaProfesional = dr["categoria_profesional"] == DBNull.Value ? null : dr["categoria_profesional"].ToString();
            item.CentroTrabajo = dr["centro_trabajo"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["centro_trabajo"]);
            item.CentroTrabajo2 = dr["centro_trabajo2"] == DBNull.Value ? null : dr["centro_trabajo2"].ToString();
            item.CodigoPostal = dr["codigo_postal"] == DBNull.Value ? null : dr["codigo_postal"].ToString();
            item.Comentarios = dr["comentarios"] == DBNull.Value ? null : dr["comentarios"].ToString();
            item.Direccion = dr["direccion"] == DBNull.Value ? null : dr["direccion"].ToString();
            item.FacCodigoPostal = dr["fac_codigo_postal"] == DBNull.Value ? null : dr["fac_codigo_postal"].ToString();
            item.FacDireccion = dr["fac_direccion"] == DBNull.Value ? null : dr["fac_direccion"].ToString();
            item.FacLocalidad = dr["fac_localidad"] == DBNull.Value ? null : dr["fac_localidad"].ToString();
            item.FacMail = dr["fac_mail"] == DBNull.Value ? null : dr["fac_mail"].ToString();
            item.FacNif = dr["fac_nif"] == DBNull.Value ? null : dr["fac_nif"].ToString();
            item.FacNombre = dr["fac_nombre"] == DBNull.Value ? null : dr["fac_nombre"].ToString();
            item.FacPais = dr["fac_pais"] == DBNull.Value ? null : dr["fac_pais"].ToString();
            item.FacProvincia = dr["fac_provincia"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["fac_provincia"]);
            //item.FechaInscripcion = Convert.ToDateTime(dr["fecha_inscripcion"]);
            item.FechaInscripcion = DateTime.Parse(dr["fecha_inscripcion"].ToString());
            item.Mail = dr["mail"].ToString();
            item.Mail2 = dr["mail2"] == DBNull.Value ? null : dr["mail2"].ToString();
            item.Movil = dr["movil"] == DBNull.Value ? null : dr["movil"].ToString();
            item.Nif = dr["nif"].ToString();
            item.Nombre = dr["nombre"].ToString();
            item.Pais = dr["pais"] == DBNull.Value ? null : dr["pais"].ToString();
            item.Poblacion = dr["poblacion"] == DBNull.Value ? null : dr["poblacion"].ToString();
            item.PrecioCuota = dr["precio_cuota"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["precio_cuota"]);
            item.Provincia = dr["provincia"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["provincia"]);
            item.Telefono = dr["telefono"] == DBNull.Value ? null : dr["telefono"].ToString();
            item.TipoCuota = dr["tipo_cuota"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["tipo_cuota"]);
            item.Uid = Guid.Parse(dr["uid"].ToString());

            //Hay algunos campos que no van a estar en todos los eventos, por eso hay que comprobar antes de que se introduzcan
            if (dr.Table.Columns.Contains("laboratorio"))
                item.Laboratorio = dr["laboratorio"] == DBNull.Value ? null : dr["laboratorio"].ToString();
            if (dr.Table.Columns.Contains("especialidad"))
                item.Especialidad = dr["especialidad"] == DBNull.Value ? null : dr["especialidad"].ToString();

            return item;
        }
    }
}

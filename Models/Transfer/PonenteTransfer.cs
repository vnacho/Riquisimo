using System;
using System.Data;

namespace Ferpuser.Models.Transfer
{
    public class PonenteTransfer
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Nif { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public string Cargo { get; set; }
        public string CentroTrabajo { get; set; }
        public string Telefono { get; set; }
        public string Movil { get; set; }
        public string Mail { get; set; }
        public string Mail2 { get; set; }
        public int? Tratamiento1 { get; set; }
        public string Tratamiento2 { get; set; }
        public string AmbitoComite { get; set; }
        public int TipoComite { get; set; }
        public int TipoComite2 { get; set; }
        public int PuestoComite { get; set; }
        public string Activo { get; set; }
        public string Visible { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public string Comentarios { get; set; }
        public string SuperEvaluador { get; set; }
        public string Visualizador { get; set; }
        public string JuntaDirectiva { get; set; }
        public DateTime? FechaRegistro { get; set; }

        /// <summary>
        /// Método auxiliar para parsear desde una consulta de MySQL con ADO.Net sin Entity Framework
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static PonenteTransfer Parse(DataRow dr)
        {
            PonenteTransfer item = new PonenteTransfer();
            item.Id = Convert.ToInt32(dr["id"]);
            item.Login = dr["login"].ToString();
            item.Password = dr["password"].ToString();
            item.Nombre = dr["nombre"].ToString();
            item.Apellidos = dr["apellidos"].ToString();
            item.Localidad = dr["localidad"] == DBNull.Value ? string.Empty : dr["localidad"].ToString();
            item.Provincia = dr["provincia"] == DBNull.Value ? string.Empty : dr["provincia"].ToString();
            item.Cargo = dr["cargo"] == DBNull.Value ? string.Empty : dr["cargo"].ToString();
            item.CentroTrabajo = dr["centro_trabajo"] == DBNull.Value ? string.Empty : dr["centro_trabajo"].ToString();
            item.Telefono = dr["telefono"] == DBNull.Value ? string.Empty : dr["telefono"].ToString();
            item.Movil = dr["movil"] == DBNull.Value ? string.Empty : dr["movil"].ToString();
            item.Mail = dr["mail"] == DBNull.Value ? string.Empty : dr["mail"].ToString();
            item.Mail2 = dr["mail2"] == DBNull.Value ? string.Empty : dr["mail2"].ToString();
            item.Tratamiento1 = Convert.ToInt32(dr["tratamiento1"]);
            item.Tratamiento2 = dr["tratamiento2"] == DBNull.Value ? string.Empty : dr["tratamiento2"].ToString();
            item.AmbitoComite = dr["ambito_comite"].ToString();
            item.TipoComite = Convert.ToInt32(dr["tipo_comite"]);
            item.TipoComite2 = Convert.ToInt32(dr["tipo_comite2"]);
            item.PuestoComite = Convert.ToInt32(dr["puesto_comite"]);
            item.Activo = dr["activo"].ToString();
            item.Visible = dr["visible"].ToString();
            item.UltimoAcceso = dr["ultimo_acceso"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(dr["ultimo_acceso"].ToString());
            item.Comentarios = dr["comentarios"] == DBNull.Value ? string.Empty : dr["comentarios"].ToString();
            item.SuperEvaluador = dr["superevaluador"].ToString();
            item.Visualizador = dr["visualizador"].ToString();
            item.JuntaDirectiva = dr["junta_directiva"].ToString();

            //Hay algunos campos que no van a estar en todas las bases de datos, por eso hay que comprobar antes de que se parseen
            if (dr.Table.Columns.Contains("nif"))
                item.Nif = dr["nif"].ToString();
            if (dr.Table.Columns.Contains("fecha_registro"))
                item.FechaRegistro = dr["fecha_registro"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(dr["fecha_registro"].ToString());

            return item;
        }
    }
}

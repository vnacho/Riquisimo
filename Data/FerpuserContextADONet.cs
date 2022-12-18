using Ferpuser.Models.Transfer;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Ferpuser.Data
{
    /// <summary>
    /// Esta clase se utiliza para realizar algunas consultas y acciones sobre la base de 
    /// datos que no comparten la estructura común de todas las bases de datos MySQL.
    /// Por ejemplo campos Laboratorio y Especialidad
    /// </summary>
    public class FerpuserContextADONet
    {
        private string ConnectionString { get; set; }
        private string Prefix { get; set; }

        public FerpuserContextADONet(string ConnectionString, string Prefix)
        {
            this.ConnectionString = ConnectionString;
            this.Prefix = Prefix;
        }

        public List<RegistrationTransfer> GetRegistrationTransfer()
        {
            List<RegistrationTransfer> list = new List<RegistrationTransfer>();
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(ConnectionString);
                conn.Open();

                string query = $"SELECT * FROM {Prefix}inscripciones";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();

                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    RegistrationTransfer item = RegistrationTransfer.Parse(dr);
                    list.Add(item);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return list;
        }

        public List<PonenteTransfer> GetPonenteTransfer()
        {
            List<PonenteTransfer> list = new List<PonenteTransfer>();
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(ConnectionString);
                conn.Open();

                string query = $"SELECT * FROM {Prefix}miembros_comites";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();

                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    PonenteTransfer item = PonenteTransfer.Parse(dr);
                    list.Add(item);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return list;
        }
    }
}

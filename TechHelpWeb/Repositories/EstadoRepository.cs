using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using TechHelpWeb.Models;

namespace TechHelpWeb.Repositories
{
    public static class EstadoRepository
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["TechHelpDB"].ConnectionString;

        public static List<EstadoSolicitud> ObtenerTodos()
        {
            var lista = new List<EstadoSolicitud>();
            string query = "SELECT Id, Nombre FROM EstadosSolicitud ORDER BY Id";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new EstadoSolicitud { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() });
                }
            }
            return lista;
        }
    }
}
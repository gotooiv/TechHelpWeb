using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using TechHelpWeb.Models;

namespace TechHelpWeb.Repositories
{
    public static class AreaRepository
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["TechHelpDB"].ConnectionString;

        public static List<AreaSolicitante> ObtenerTodos()
        {
            var lista = new List<AreaSolicitante>();
            string query = "SELECT Id, Nombre FROM AreasSolicitantes ORDER BY Nombre";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new AreaSolicitante { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() });
                }
            }
            return lista;
        }
    }
}
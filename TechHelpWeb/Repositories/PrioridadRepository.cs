using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using TechHelpWeb.Models;

namespace TechHelpWeb.Repositories
{
    public static class PrioridadRepository
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["TechHelpDB"].ConnectionString;

        public static List<Prioridad> ObtenerTodos()
        {
            var lista = new List<Prioridad>();
            string query = "SELECT Id, Nombre FROM Prioridades ORDER BY Id";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Prioridad { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() });
                }
            }
            return lista;
        }
    }
}
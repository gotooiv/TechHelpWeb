using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using TechHelpWeb.Models;

namespace TechHelpWeb.Repositories
{
    public static class TipoProblemaRepository
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["TechHelpDB"].ConnectionString;

        public static List<TipoProblema> ObtenerTodos()
        {
            var lista = new List<TipoProblema>();
            string query = "SELECT Id, Nombre FROM TiposProblema ORDER BY Nombre";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new TipoProblema { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() });
                }
            }
            return lista;
        }
    }
}
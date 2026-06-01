using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TechHelpWeb.Models;

namespace TechHelpWeb.Repositories
{
    public static class UsuarioRepository
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["TechHelpDB"].ConnectionString;

        public static Usuario ValidarCredenciales(string correo, string contrasena)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT u.Id, u.NombreCompleto, u.Correo, u.RolId, r.Nombre AS NombreRol, u.Activo, u.FechaRegistro
                                 FROM Usuarios u INNER JOIN Roles r ON u.RolId = r.Id
                                 WHERE u.Correo = @Correo AND u.Contrasena = @Contrasena";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = correo;
                cmd.Parameters.Add("@Contrasena", SqlDbType.NVarChar, 255).Value = contrasena;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Usuario
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        NombreCompleto = reader["NombreCompleto"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        RolId = Convert.ToInt32(reader["RolId"]),
                        NombreRol = reader["NombreRol"].ToString(),
                        Activo = Convert.ToBoolean(reader["Activo"]),
                        FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"])
                    };
                }
                return null;
            }
        }

        public static Usuario ObtenerPorId(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT u.Id, u.NombreCompleto, u.Correo, u.RolId, r.Nombre AS NombreRol, u.Activo, u.FechaRegistro
                                 FROM Usuarios u INNER JOIN Roles r ON u.RolId = r.Id WHERE u.Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Usuario
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        NombreCompleto = reader["NombreCompleto"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        RolId = Convert.ToInt32(reader["RolId"]),
                        NombreRol = reader["NombreRol"].ToString(),
                        Activo = Convert.ToBoolean(reader["Activo"]),
                        FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"])
                    };
                }
                return null;
            }
        }

        public static bool ActualizarPerfil(int id, string nombreCompleto, string correo)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Usuarios SET NombreCompleto = @Nombre, Correo = @Correo WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = nombreCompleto;
                cmd.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = correo;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool VerificarContrasena(int id, string contrasena)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM Usuarios WHERE Id = @Id AND Contrasena = @Contrasena";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@Contrasena", SqlDbType.NVarChar, 255).Value = contrasena;
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public static bool CambiarContrasena(int id, string nuevaContrasena)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Usuarios SET Contrasena = @Contrasena WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@Contrasena", SqlDbType.NVarChar, 255).Value = nuevaContrasena;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
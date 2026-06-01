using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TechHelpWeb.Models;

namespace TechHelpWeb.Repositories
{
    public static class SolicitudSoporteRepository
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["TechHelpDB"].ConnectionString;

        public static List<SolicitudSoporte> ObtenerTodas()
        {
            var lista = new List<SolicitudSoporte>();
            string query = @"
                SELECT s.Id, s.UsuarioId, s.NombreCompleto, s.Correo, s.Descripcion, s.FechaSolicitud, s.FechaRegistro,
                       s.AreaSolicitanteId, s.TipoProblemaId, s.PrioridadId, s.EstadoSolicitudId,
                       a.Nombre AS AreaSolicitanteNombre, 
                       t.Nombre AS TipoProblemaNombre, 
                       p.Nombre AS PrioridadNombre, 
                       e.Nombre AS EstadoNombre
                FROM SolicitudesSoporte s
                INNER JOIN AreasSolicitantes a ON s.AreaSolicitanteId = a.Id
                INNER JOIN TiposProblema t ON s.TipoProblemaId = t.Id
                INNER JOIN Prioridades p ON s.PrioridadId = p.Id
                INNER JOIN EstadosSolicitud e ON s.EstadoSolicitudId = e.Id
                ORDER BY s.Id DESC";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new SolicitudSoporte
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        UsuarioId = Convert.ToInt32(reader["UsuarioId"]),
                        NombreCompleto = reader["NombreCompleto"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        FechaSolicitud = Convert.ToDateTime(reader["FechaSolicitud"]),
                        FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]),
                        AreaSolicitanteId = Convert.ToInt32(reader["AreaSolicitanteId"]),
                        TipoProblemaId = Convert.ToInt32(reader["TipoProblemaId"]),
                        PrioridadId = Convert.ToInt32(reader["PrioridadId"]),
                        EstadoSolicitudId = Convert.ToInt32(reader["EstadoSolicitudId"]),
                        AreaSolicitanteNombre = reader["AreaSolicitanteNombre"].ToString(),
                        TipoProblemaNombre = reader["TipoProblemaNombre"].ToString(),
                        PrioridadNombre = reader["PrioridadNombre"].ToString(),
                        EstadoNombre = reader["EstadoNombre"].ToString()
                    });
                }
            }
            return lista;
        }

        public static SolicitudSoporte ObtenerPorId(int id)
        {
            string query = @"
                SELECT s.Id, s.UsuarioId, s.NombreCompleto, s.Correo, s.Descripcion, s.FechaSolicitud, s.FechaRegistro,
                       s.AreaSolicitanteId, s.TipoProblemaId, s.PrioridadId, s.EstadoSolicitudId,
                       a.Nombre AS AreaSolicitanteNombre, 
                       t.Nombre AS TipoProblemaNombre, 
                       p.Nombre AS PrioridadNombre, 
                       e.Nombre AS EstadoNombre
                FROM SolicitudesSoporte s
                INNER JOIN AreasSolicitantes a ON s.AreaSolicitanteId = a.Id
                INNER JOIN TiposProblema t ON s.TipoProblemaId = t.Id
                INNER JOIN Prioridades p ON s.PrioridadId = p.Id
                INNER JOIN EstadosSolicitud e ON s.EstadoSolicitudId = e.Id
                WHERE s.Id = @Id";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new SolicitudSoporte
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        UsuarioId = Convert.ToInt32(reader["UsuarioId"]),
                        NombreCompleto = reader["NombreCompleto"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        FechaSolicitud = Convert.ToDateTime(reader["FechaSolicitud"]),
                        FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]),
                        AreaSolicitanteId = Convert.ToInt32(reader["AreaSolicitanteId"]),
                        TipoProblemaId = Convert.ToInt32(reader["TipoProblemaId"]),
                        PrioridadId = Convert.ToInt32(reader["PrioridadId"]),
                        EstadoSolicitudId = Convert.ToInt32(reader["EstadoSolicitudId"]),
                        AreaSolicitanteNombre = reader["AreaSolicitanteNombre"].ToString(),
                        TipoProblemaNombre = reader["TipoProblemaNombre"].ToString(),
                        PrioridadNombre = reader["PrioridadNombre"].ToString(),
                        EstadoNombre = reader["EstadoNombre"].ToString()
                    };
                }
                return null;
            }
        }

        public static SolicitudSoporte Agregar(SolicitudSoporte solicitud)
        {
            string query = @"
                INSERT INTO SolicitudesSoporte (UsuarioId, NombreCompleto, Correo, AreaSolicitanteId, TipoProblemaId, PrioridadId, EstadoSolicitudId, Descripcion, FechaSolicitud, FechaRegistro)
                VALUES (@UsuarioId, @Nombre, @Correo, @AreaId, @TipoId, @PrioridadId, 1, @Descripcion, @FechaSolicitud, @FechaRegistro);
                SELECT SCOPE_IDENTITY();";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = solicitud.UsuarioId;
                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = solicitud.NombreCompleto;
                cmd.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = solicitud.Correo;
                cmd.Parameters.Add("@AreaId", SqlDbType.Int).Value = solicitud.AreaSolicitanteId;
                cmd.Parameters.Add("@TipoId", SqlDbType.Int).Value = solicitud.TipoProblemaId;
                cmd.Parameters.Add("@PrioridadId", SqlDbType.Int).Value = solicitud.PrioridadId;
                cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 500).Value = solicitud.Descripcion;
                cmd.Parameters.Add("@FechaSolicitud", SqlDbType.Date).Value = solicitud.FechaSolicitud;
                cmd.Parameters.Add("@FechaRegistro", SqlDbType.DateTime).Value = DateTime.Now;
                con.Open();
                int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
                solicitud.Id = nuevoId;
                solicitud.EstadoSolicitudId = 1;
                return solicitud;
            }
        }

        public static bool ActualizarEstado(int id, int nuevoEstadoId)
        {
            string query = "UPDATE SolicitudesSoporte SET EstadoSolicitudId = @EstadoId WHERE Id = @Id";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@EstadoId", SqlDbType.Int).Value = nuevoEstadoId;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool ActualizarSolicitud(SolicitudSoporte solicitud)
        {
            string query = @"
                UPDATE SolicitudesSoporte SET 
                    NombreCompleto = @Nombre,
                    Correo = @Correo,
                    AreaSolicitanteId = @AreaId,
                    TipoProblemaId = @TipoId,
                    PrioridadId = @PrioridadId,
                    Descripcion = @Descripcion,
                    FechaSolicitud = @FechaSolicitud
                WHERE Id = @Id";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = solicitud.Id;
                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = solicitud.NombreCompleto;
                cmd.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = solicitud.Correo;
                cmd.Parameters.Add("@AreaId", SqlDbType.Int).Value = solicitud.AreaSolicitanteId;
                cmd.Parameters.Add("@TipoId", SqlDbType.Int).Value = solicitud.TipoProblemaId;
                cmd.Parameters.Add("@PrioridadId", SqlDbType.Int).Value = solicitud.PrioridadId;
                cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 500).Value = solicitud.Descripcion;
                cmd.Parameters.Add("@FechaSolicitud", SqlDbType.Date).Value = solicitud.FechaSolicitud;
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool Eliminar(int id)
        {
            string query = "DELETE FROM SolicitudesSoporte WHERE Id = @Id";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
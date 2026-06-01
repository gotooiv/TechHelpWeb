using System;
namespace TechHelpWeb.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public int RolId { get; set; }
        public string NombreRol { get; set; }   // para mostrar
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
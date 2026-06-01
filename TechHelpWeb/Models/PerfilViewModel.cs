using System.ComponentModel.DataAnnotations;

namespace TechHelpWeb.Models
{
    public class PerfilViewModel
    {
        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        public string NombreRol { get; set; }
    }
}
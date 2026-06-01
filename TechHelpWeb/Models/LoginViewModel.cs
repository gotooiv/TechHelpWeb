using System.ComponentModel.DataAnnotations;

namespace TechHelpWeb.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Correo obligatorio")]
        [EmailAddress]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Contraseña obligatoria")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
    }
}
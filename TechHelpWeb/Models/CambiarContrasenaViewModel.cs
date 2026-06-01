using System.ComponentModel.DataAnnotations;

namespace TechHelpWeb.Models
{
    public class CambiarContrasenaViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string ContrasenaActual { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string NuevaContrasena { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NuevaContrasena", ErrorMessage = "No coincide")]
        public string ConfirmacionContrasena { get; set; }
    }
}
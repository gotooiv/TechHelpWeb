using System;
using System.ComponentModel.DataAnnotations;

namespace TechHelpWeb.Models
{
    public class SolicitudSoporte
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Nombre completo obligatorio")]
        [StringLength(100)]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "Correo obligatorio")]
        [EmailAddress]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Área solicitante obligatoria")]
        public int AreaSolicitanteId { get; set; }

        [Required(ErrorMessage = "Tipo de problema obligatorio")]
        public int TipoProblemaId { get; set; }

        [Required(ErrorMessage = "Prioridad obligatoria")]
        public int PrioridadId { get; set; }

        [Required(ErrorMessage = "Descripción obligatoria")]
        [MinLength(10, ErrorMessage = "Mínimo 10 caracteres")]
        [MaxLength(500)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Fecha de solicitud obligatoria")]
        public DateTime FechaSolicitud { get; set; }

        public int EstadoSolicitudId { get; set; } = 1; // Pendiente por defecto

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Propiedades para mostrar nombre (no se guardan)
        public string AreaSolicitanteNombre { get; set; }
        public string TipoProblemaNombre { get; set; }
        public string PrioridadNombre { get; set; }
        public string EstadoNombre { get; set; }
    }
}
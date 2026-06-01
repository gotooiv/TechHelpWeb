using System.Web.Http;
using TechHelpWeb.Models;
using TechHelpWeb.Repositories;

namespace TechHelpWeb.Controllers
{
    [RoutePrefix("api/solicitudes")]
    public class SolicitudesApiController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult ObtenerTodas()
        {
            return Ok(SolicitudSoporteRepository.ObtenerTodas());
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult ObtenerPorId(int id)
        {
            var solicitud = SolicitudSoporteRepository.ObtenerPorId(id);
            if (solicitud == null) return NotFound();
            return Ok(solicitud);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Crear([FromBody] SolicitudSoporte solicitud)
        {
            if (solicitud == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            if (solicitud.UsuarioId == 0)
                return BadRequest("UsuarioId requerido");
            var nueva = SolicitudSoporteRepository.Agregar(solicitud);
            return Ok(nueva);
        }

        [HttpPut]
        [Route("{id:int}/estado")]
        public IHttpActionResult ActualizarEstado(int id, [FromBody] int estadoId)
        {
            bool ok = SolicitudSoporteRepository.ActualizarEstado(id, estadoId);
            if (!ok) return NotFound();
            return Ok(new { mensaje = "Estado actualizado" });
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult ActualizarSolicitud(int id, [FromBody] SolicitudSoporte solicitud)
        {
            if (solicitud == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != solicitud.Id)
                return BadRequest("ID no coincide");
            bool ok = SolicitudSoporteRepository.ActualizarSolicitud(solicitud);
            if (!ok) return NotFound();
            return Ok(new { mensaje = "Solicitud actualizada" });
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Eliminar(int id)
        {
            bool ok = SolicitudSoporteRepository.Eliminar(id);
            if (!ok) return NotFound();
            return Ok(new { mensaje = "Eliminada" });
        }
    }
}
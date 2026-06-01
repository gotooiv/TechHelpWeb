using System.Web.Mvc;
using TechHelpWeb.Repositories;

namespace TechHelpWeb.Controllers
{
    public class SolicitudesController : Controller
    {
        [HttpGet]
        public ActionResult Crear()
        {
            if (Session["UsuarioId"] == null)
                return RedirectToAction("Login", "Auth");

            // Autocompletar desde sesión
            ViewBag.NombreCompleto = Session["NombreCompleto"]?.ToString();
            ViewBag.Correo = Session["Correo"]?.ToString();

            // Cargar listas para los dropdowns (necesarias para la vista)
            ViewBag.Areas = AreaRepository.ObtenerTodos();
            ViewBag.Tipos = TipoProblemaRepository.ObtenerTodos();
            ViewBag.Prioridades = PrioridadRepository.ObtenerTodos();

            return View();
        }
    }
}
using System.Web.Mvc;
using TechHelpWeb.Models;
using TechHelpWeb.Repositories;

namespace TechHelpWeb.Controllers
{
    public class PerfilController : Controller
    {
        private bool SesionActiva() => Session["UsuarioId"] != null;

        [HttpGet]
        public ActionResult Index()
        {
            if (!SesionActiva()) return RedirectToAction("Login", "Auth");
            int id = (int)Session["UsuarioId"];
            var u = UsuarioRepository.ObtenerPorId(id);
            if (u == null) return RedirectToAction("Login", "Auth");
            var model = new PerfilViewModel { NombreCompleto = u.NombreCompleto, Correo = u.Correo, NombreRol = u.NombreRol };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PerfilViewModel model)
        {
            if (!SesionActiva()) return RedirectToAction("Login", "Auth");
            if (!ModelState.IsValid) return View(model);
            int id = (int)Session["UsuarioId"];
            if (UsuarioRepository.ActualizarPerfil(id, model.NombreCompleto, model.Correo))
            {
                Session["NombreCompleto"] = model.NombreCompleto;
                Session["Correo"] = model.Correo;
                ViewBag.Exito = "Perfil actualizado";
            }
            else
            {
                ModelState.AddModelError("", "Error al actualizar");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult CambiarContrasena()
        {
            if (!SesionActiva()) return RedirectToAction("Login", "Auth");
            return View(new CambiarContrasenaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarContrasena(CambiarContrasenaViewModel model)
        {
            if (!SesionActiva()) return RedirectToAction("Login", "Auth");
            if (!ModelState.IsValid) return View(model);
            int id = (int)Session["UsuarioId"];
            if (!UsuarioRepository.VerificarContrasena(id, model.ContrasenaActual))
            {
                ModelState.AddModelError("ContrasenaActual", "Contraseña actual incorrecta");
                return View(model);
            }
            if (UsuarioRepository.CambiarContrasena(id, model.NuevaContrasena))
                ViewBag.Exito = "Contraseña cambiada";
            else
                ModelState.AddModelError("", "Error al cambiar");
            return View(new CambiarContrasenaViewModel());
        }
    }
}
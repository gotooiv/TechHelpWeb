using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using TechHelpWeb.Models;
using TechHelpWeb.Repositories;

namespace TechHelpWeb.Controllers
{
    public class AuthController : Controller
    {
        private bool VerificarConexion(out string mensajeError)
        {
            mensajeError = "";
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["TechHelpDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                mensajeError = "Error de SQL: " + ex.Message;
                return false;
            }
            catch (System.Exception ex)
            {
                mensajeError = "Error general: " + ex.Message;
                return false;
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            bool conexionOK = VerificarConexion(out string errorMsg);
            if (!conexionOK)
                ViewBag.ErrorConexion = "⚠️ No se pudo conectar a la base de datos. " + errorMsg;
            else
                ViewBag.SuccessConexion = "✅ Conexión a la base de datos establecida correctamente.";

            if (Session["UsuarioId"] != null)
                return RedirectToAction("Crear", "Solicitudes");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = UsuarioRepository.ValidarCredenciales(model.Correo, model.Contrasena);
            if (usuario == null || !usuario.Activo)
            {
                ModelState.AddModelError("", "Credenciales incorrectas o cuenta inactiva");
                return View(model);
            }

            Session["UsuarioId"] = usuario.Id;
            Session["NombreCompleto"] = usuario.NombreCompleto;
            Session["Correo"] = usuario.Correo;
            Session["Rol"] = usuario.NombreRol;

            return RedirectToAction("Crear", "Solicitudes");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}
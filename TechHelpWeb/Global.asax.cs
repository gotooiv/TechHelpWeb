using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace TechHelpWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register); // Registra rutas de API
            AreaRegistration.RegisterAllAreas();                  // Registra áreas (si las hay)
            RouteConfig.RegisterRoutes(RouteTable.Routes);        // Registra rutas de MVC
        }
    }
}
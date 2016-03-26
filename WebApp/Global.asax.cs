using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Vincente.WebApp.App_Start;
using WebApp.App_Start;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AutofaqConfig.Configure();
        }
    }
}

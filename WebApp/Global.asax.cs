using System.Web.Mvc;
using System.Web.Routing;
using WebApp.App_Start;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AutofaqConfig.Configure();
        }
    }
}

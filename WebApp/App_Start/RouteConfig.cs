using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Error",
                url: "Error/{action}",
                defaults: new { controller = "Error", action = "Summary" }
            );

            routes.MapRoute(
                name: "Wip",
                url: "Wip",
                defaults: new { controller = "Wip", action = "ByList" }
            );

            routes.MapRoute(
                name: "WipDetail",
                url: "Wip/{list}",
                defaults: new { controller = "Wip", action = "Detail" }
            );

            routes.MapRoute(
                name: "InvoiceList",
                url: "Invoice",
                defaults: new { controller = "Invoice", action = "List" }
            );

            routes.MapRoute(
                name: "InvoiceByEpic",
                url: "Invoice/{year}/{month}",
                defaults: new { controller = "Invoice", action = "ByEpic" }
            );

            routes.MapRoute(
                name: "InvoiceDetail",
                url: "Invoice/{year}/{month}/{epic}",
                defaults: new { controller = "Invoice", action = "Detail" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Default", action = "Index" }
            );
        }
    }
}

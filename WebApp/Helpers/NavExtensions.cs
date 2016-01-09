using Gothandy.Tree.Extensions;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Vincente.WebApp.Models;

namespace Vincente.WebApp.Helpers
{
    public static class NavExtensions
    {
        public static NavTree GetRoot(this HtmlHelper helper)
        {
            NavTree root = helper.ViewBag.SiteMapRoot;
            return root;
        }

        public static bool IsCurrent(this HtmlHelper helper, NavLink item)
        {
            NavTree root = helper.ViewBag.SiteMapRoot;
            NavTree current = helper.ViewBag.SiteMapCurrent;
            return (item.ActionName == current.Item.ActionName) && (item.ControllerName == current.Item.ControllerName);
        }

        public static string GetTitle (this HtmlHelper helper)
        {
            NavTree root = helper.ViewBag.SiteMapRoot;
            NavTree current = helper.ViewBag.SiteMapCurrent;
            return current.Item.LinkText;
        }

        public static List<NavLink> GetAncestors(this HtmlHelper helper)
        {
            NavTree root = helper.ViewBag.SiteMapRoot;
            NavTree current = helper.ViewBag.SiteMapCurrent;
            return current.GetAncestors();
        }

        private static bool IsActive(this HtmlHelper html, string actionName, string controllerName)
        {
            var routeData = html.ViewContext.RouteData;
            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];
            var returnActive = (controllerName == routeControl) && (actionName == routeAction);

            return returnActive;
        }

    
    }
}
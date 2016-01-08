using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Vincente.WebApp.Helpers
{
    public static class CssClassExtensions
    {
        public static HtmlString NavItem(this HtmlHelper html, string linkText, string actionName)
        {
            return NavItem(html, linkText, actionName, linkText);
        }

        public static HtmlString NavItem(this HtmlHelper html, string linkText, string actionName, string controllerName)
        {
            string actionLink = html.ActionLink(linkText, actionName, controllerName).ToString();

            string fullLink;

            if (IsActive(html, actionName, controllerName))
            {
                fullLink = string.Format("<li class=\"active\">{0}</li>", actionLink);
            }
            else
            {
                fullLink = string.Format("<li>{0}</li>", actionLink);
            }

            return new HtmlString(fullLink);
        }


        public static bool IsActive(this HtmlHelper html, string actionName, string controllerName)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];
            var returnActive = (controllerName == routeControl) && (actionName == routeAction);

            return returnActive;
        }

        public static string IsDanger(this HtmlHelper html, bool isDanger)
        {
            return isDanger ? "danger" : string.Empty;
        }

        public static string IsDanger(this HtmlHelper html, bool? isDanger)
        {
            return IsDanger(html, isDanger.GetValueOrDefault());
        }

        public static string IsDanger(this HtmlHelper html, decimal? isDanger)
        {
            return IsDanger(html, isDanger.GetValueOrDefault() > 0);
        }
    }
}
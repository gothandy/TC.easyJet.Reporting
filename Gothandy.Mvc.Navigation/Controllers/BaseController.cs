using Gothandy.Tree.Extensions;
using System.Collections.Generic;
using System.Web.Mvc;
using Vincente.WebApp.Models;

namespace Gothandy.Mvc.Navigation.Controllers
{
    public class BaseController : Controller
    {
        public NavTree SiteMapRoot { get; set; }

        protected string action;
        protected string controller;
        protected List<NavLink> ancestors;
        protected string defaultAction;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            controller = (string)filterContext.RouteData.Values["controller"];
            action = (string)filterContext.RouteData.Values["action"];

            var current = GetNavTree(action, controller);

            if (current == null)
            {
                current = GetNavTree(defaultAction, controller);
                ancestors = current.GetAncestors();
                ancestors.Add(current.Item);
            }
            else
            {
                ViewBag.Title = current.Item.LinkText;
                ancestors = current.GetAncestors();
            }

            ViewBag.SiteMap = this.SiteMapRoot;
            ViewBag.Ancestors = ancestors;

            base.OnActionExecuting(filterContext);
        }

        private NavTree GetNavTree(string action, string controller)
        {
            return (NavTree)this.SiteMapRoot.GetDescendants().Find(d => (d.Item.ActionName == action) && (d.Item.ControllerName == controller));
        }
    }
}
using Gothandy.Tree.Extensions;
using System.Collections.Generic;
using System.Web.Mvc;
using Vincente.WebApp.Models;
using System.Linq;

namespace Gothandy.Mvc.Navigation.Controllers
{
    public class BaseController : Controller
    {
        // Initialize sitemap via this property.
        public NavTree SiteMapRoot { get; set; }

        protected List<NavLink> ancestors;
        protected string action;
        protected string controller;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            controller = ((string)filterContext.RouteData.Values["controller"]).ToLower();
            action = ((string)filterContext.RouteData.Values["action"]).ToLower();

            var current = GetNavTree(action, controller);

            if (current == null)
            {
                current = GetParentFromTree();

                if (current != null)
                {
                    ancestors = current.GetAncestors();
                    ancestors.Add(current.Item);
                }
            }
            else
            {
                ViewBag.Title = current.Item.LinkText;
                ancestors = current.GetAncestors();
            }

            // For use in Layout to build Sitemap, Navigation and Breadcrumbs
            ViewBag.SiteMap = this.SiteMapRoot;
            ViewBag.Ancestors = ancestors;

            base.OnActionExecuting(filterContext);
        }

        private NavTree GetParentFromTree()
        {
            var results = this.SiteMapRoot.GetDescendants().FindAll(d => d.Item.ControllerName.ToLower() == controller);

            if (results.Count == 1) return (NavTree)results.Last();

            return null;
        }

        private NavTree GetNavTree(string action, string controller)
        {
            return (NavTree)this.SiteMapRoot.GetDescendants().Find(d => (d.Item.ActionName.ToLower() == action) && (d.Item.ControllerName.ToLower() == controller));
        }
    }
}
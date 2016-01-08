using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincente.WebApp.Models
{
    public class NavModel
    {

        public NavModel (string title)
        {
            BaseConstructor(title, new List<NavLink>());
        }

        public NavModel (string title, NavLink link)
        {
            BaseConstructor(title, new List<NavLink>() { link });
        }

        public NavModel(string title, List<NavLink> links)
        {
            BaseConstructor(title, links);
        }

        private void BaseConstructor(string title, List<NavLink> links)
        {
            this.Title = title;
            this.Links = new List<NavLink>();

            if (title != "Home")
            {
                this.Links.Add(new NavLink()
                    {
                        LinkText = "Home",
                        ActionName = "Index",
                        ControllerName = "Default"
                    });
            };

            this.Links.AddRange(links);
        }

        public string Title { get; set; }

        public List<NavLink> Links { get; set; }
    }

    public class NavLink
    {
        public string LinkText { get; set; }
        public     string ActionName { get; set; }
        public    string ControllerName { get; set;}
        public    object RouteValues { get; set; }
    }
}
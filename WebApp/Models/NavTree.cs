using Gothandy.Tree;

namespace Vincente.WebApp.Models
{
    public class NavTree : BaseTree<NavLink>
    {

        public NavTree(string linkText, string actionName, string controllerName) :
            base(new NavLink()
            {
                LinkText = linkText,
                ActionName = actionName,
                ControllerName = controllerName
            })
        { }

        public NavTree(string linkText, string actionName, string controllerName, string groupName) :
            base(new NavLink()
            {
                LinkText = linkText,
                ActionName = actionName,
                ControllerName = controllerName,
                GroupName = groupName
            })
        { }
    }
}
using Gothandy.StartUp;
using System.Web.Mvc;
using Vincente.Data.Interfaces;
using Vincente.WebApp.Models;

namespace WebApp.Controllers
{
    public class DefaultController : Controller
    {
        private DefaultModel model;

        public DefaultController(ICardRead cards, ITimeEntryRead timeEntries)
        {
            model = new DefaultModel(cards, timeEntries);
        }

        // GET: Default
        public ActionResult Index()
        {
            ViewBag.BuildDateTime = Tools.GetBuildDateTime(typeof(WebApp.MvcApplication));
            return View(model);
        }
    }
}
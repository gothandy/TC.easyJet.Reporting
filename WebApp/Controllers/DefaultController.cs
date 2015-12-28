using System.Web.Mvc;
using Vincente.Data.Interfaces;
using Vincente.WebApp.Models;
using WebApp.Models;

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
            return View(model);
        }
    }
}
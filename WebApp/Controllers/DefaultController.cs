using Azure.Tables;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Trello(string key)
        {
            CardTable table = new CardTable("tceasyjetreporting2", key);

            return View();
        }

        public ActionResult Toggl(string key)
        {
            TimeEntryTable table = new TimeEntryTable("tceasyjetreporting2", key);

            return View(table.Query());
        }
    }
}

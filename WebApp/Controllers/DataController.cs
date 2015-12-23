using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApp.Models;

namespace Vincente.WebApp.Controllers
{
    public class DataController : Controller
    {
        private JoinClient joinClient;

        public DataController(JoinClient joinClient)
        {
            this.joinClient = joinClient;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllByMonth()
        {
            IEnumerable<JoinModel> stories = joinClient.GetStories();
            IEnumerable<JoinModel> housekeeping = joinClient.GetHousekeeping();

            var result = stories.Concat(housekeeping);

            return View(result);
        }

        public ActionResult Trello()
        {
            var result = joinClient.GetCards();

            return View(result);
        }

        public ActionResult Toggl()
        {
            var result = joinClient.GetTimeEntriesByMonth();

            return View(result);
        }

        public ActionResult Orphans()
        {
            var orphans = joinClient.GetOrphans();

            return View("Toggl", orphans);
        }
    }
}

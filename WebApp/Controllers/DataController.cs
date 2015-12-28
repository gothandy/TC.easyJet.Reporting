using System.Linq;
using System.Web.Mvc;
using Vincente.Data.Tables;
using Vincente.WebApp.Models;

namespace Vincente.WebApp.Controllers
{
    public class DataController : Controller
    {
        private ModelParameters p;

        public DataController(ModelParameters modelParameters)
        {
            p = modelParameters;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Trello()
        {
            return View(p.Card.Query());
        }

        public ActionResult Toggl()
        {
            var timeEntryByMonth = new TimeEntriesByMonth(p.TimeEntry);

            return View(timeEntryByMonth.Query());
        }

        public ActionResult AllByMonth()
        {
            var timeEntriesByMonth = new TimeEntriesByMonth(p.TimeEntry);
            var housekeeping = new Housekeeping(timeEntriesByMonth);
            var cardsWithTime = new CardsWithTime(p.Card, timeEntriesByMonth);

            return View(cardsWithTime.Query().Concat(housekeeping.Query()));
        }
    }
}

using System.Web.Mvc;
using Vincente.Data.Interfaces;
using Vincente.Data.Tables;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.WebApp.Models;

namespace Vincente.WebApp.Controllers
{
    public class DataController : Controller
    {
        private ICardRead cards;
        private ITaskRead tasks;
        private ITimeEntryRead timeEntries;
        private InvoiceData invoiceData;
        private NavLink dataIndexLink;

        public DataController(ICardRead cards, ITaskRead tasks, ITimeEntryRead timeEntries, InvoiceData invoiceData)
        {
            this.cards = cards;
            this.tasks = tasks;
            this.timeEntries = timeEntries;
            this.invoiceData = invoiceData;

            // Sitemap
            this.dataIndexLink = new NavLink()
            {
                LinkText = "Data",
                ActionName = "Index",
                ControllerName = "Data"
            };
        }

        public ActionResult Index()
        {
            ViewBag.Nav = new NavModel("Data");
            return View();
        }

        public ActionResult Cards()
        {
            ViewBag.Nav = new NavModel("Cards", dataIndexLink);
            return View(cards.Query());
        }

        public ActionResult Tasks()
        {
            ViewBag.Nav = new NavModel("Tasks", dataIndexLink);
            return View(tasks.Query());
        }

        public ActionResult TimeEntries()
        {
            ViewBag.Nav = new NavModel("Time Entries", dataIndexLink);
            return View(timeEntries.Query());
        }

        public ActionResult SDN()
        {
            ViewBag.Nav = new NavModel("SDN", dataIndexLink);
            return View(invoiceData.Query());
        }

        public ActionResult BudgetControl()
        {
            ViewBag.Nav = new NavModel("Budget Control", dataIndexLink);

            var result =
                from c in invoiceData.Query()
                group c by new
                {
                    c.Month,
                    c.Epic,
                    c.TeamName,
                    c.ReuseDA,
                    c.ReuseFCP
                } into g
                orderby g.Key.Month, g.Key.Epic, g.Key.TeamName
                select new CardWithTime()
                {
                    Month = g.Key.Month,
                    Epic = g.Key.Epic,
                    TeamName = g.Key.TeamName,
                    ReuseDA = g.Key.ReuseDA.GetValueOrDefault(),
                    ReuseFCP = g.Key.ReuseFCP.GetValueOrDefault(),
                    Billable = g.Sum(c => c.Billable)
                };

            return View(result);
        }
    }
}

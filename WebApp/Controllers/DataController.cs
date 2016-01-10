using Gothandy.Mvc.Navigation.Controllers;
using System.Linq;
using System.Web.Mvc;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;
using Vincente.Data.Tables;

namespace Vincente.WebApp.Controllers
{
    public class DataController : BaseController
    {
        private ICardRead cards;
        private ITaskRead tasks;
        private ITimeEntryRead timeEntries;
        private InvoiceData invoiceData;

        public DataController(ICardRead cards, ITaskRead tasks, ITimeEntryRead timeEntries, InvoiceData invoiceData)
        {
            this.cards = cards;
            this.tasks = tasks;
            this.timeEntries = timeEntries;
            this.invoiceData = invoiceData;

            defaultAction = "Index";
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cards()
        {
            return View(cards.Query());
        }

        public ActionResult Tasks()
        {
            return View(tasks.Query());
        }

        public ActionResult TimeEntries()
        {
            return View(timeEntries.Query());
        }

        public ActionResult SDN()
        {
            return View(invoiceData.Query());
        }

        public ActionResult BudgetControl()
        {
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

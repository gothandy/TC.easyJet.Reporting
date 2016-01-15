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
                select new Activity()
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

        public ActionResult TasksFromTrello()
        {
            var result =
                from c in cards.Query()
                where c.DomId != null && c.Epic != null
                orderby c.DomId
                select new Task()
                {
                    DomId = c.DomId,
                    Name = GetName(c),
                    ProjectName = string.Concat("eJ ", c.Epic),
                    Active = (c.Invoice == null)
                };

            return View("TasksFrom", result);
        }

        public ActionResult TasksFromToggl()
        {
            var result =
                from t in tasks.Query()
                orderby t.DomId
                select new Task()
                {
                    DomId = t.DomId,
                    Name = t.Name,
                    ProjectName = t.ProjectName,
                    Active = t.Active
                };

            return View("TasksFrom", result);
        }

        private string GetName(Card c)
        {
            string invoice = string.Empty;

            if (c.Invoice.HasValue) invoice = string.Format("X {0} {1} ", c.Invoice.Value.Year - 2000, c.Invoice.Value.ToString("MMM").ToUpper());

            string domId = c.DomId.Substring(1); //Take off the D.

            return string.Concat(invoice, domId, " - ", c.Name);
        }
    }
}

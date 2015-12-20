using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Vincente.Azure;
using Vincente.Azure.Entities;
using Vincente.Azure.Tables;
using Vincente.WebApp.Models;

namespace Vincente.WebApp.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Join()
        {
            TableClient client = GetNewTableClient();

            TimeEntryTable timeEntryTable = new TimeEntryTable(client);
            CardTable cardTable = new CardTable(client);

            var stories =
                from timeEntry in GroupByMonth(timeEntryTable.Query())
                join card in cardTable.Query()
                on timeEntry.DomId equals card.DomId
                orderby timeEntry.Month, card.Epic, card.List, card.Name, timeEntry.UserName
                select new JoinModel()
                {
                    Month = timeEntry.Month,
                    Epic = card.Epic,
                    List = card.List,
                    DomId = timeEntry.DomId,
                    Name = card.Name,
                    UserName = timeEntry.UserName,
                    Billable = timeEntry.Billable,
                    Invoice = card.Invoice
                };

            var housekeeping =
                from timeEntry in GroupByMonth(timeEntryTable.Query())
                where timeEntry.Housekeeping != null && timeEntry.Month > new System.DateTime(2015, 6, 30)
                select new JoinModel()
                {
                    Month = timeEntry.Month,
                    Epic = "Housekeeping",
                    List = null,
                    DomId = null,
                    Name = timeEntry.Housekeeping,
                    UserName = timeEntry.UserName,
                    Billable = timeEntry.Billable,
                    Invoice = timeEntry.Month
                };



            var result = stories.Concat(housekeeping);

            return View(result);
        }

        public ActionResult Trello()
        {
            TableClient client = GetNewTableClient();

            CardTable table = new CardTable(client);

            var result = table.Query();

            return View(result);
        }

        public ActionResult Toggl()
        {
            TableClient client = GetNewTableClient();

            TimeEntryTable table = new TimeEntryTable(client);

            var result = GroupByMonth(table.Query());

            return View(result);
        }

        public ActionResult Orphans()
        {
            TableClient client = GetNewTableClient();

            TimeEntryTable table = new TimeEntryTable(client);

            var orphans =
                from timeEntry in table.Query()
                where
                    timeEntry.Housekeeping == null &&
                    timeEntry.DomId == null &&
                    timeEntry.Month > new System.DateTime(2015, 6, 30)
                orderby timeEntry.Month
                select new TimeEntryEntity()
                {
                    Month = timeEntry.Month,
                    Housekeeping = timeEntry.Housekeeping,
                    UserName = timeEntry.UserName,
                    Billable = timeEntry.Billable,
                    TaskId = timeEntry.TaskId,
                };

            return View("Toggl", orphans);
        }

        private static IEnumerable<TimeEntryEntity> GroupByMonth(IEnumerable<TimeEntryEntity> query)
        {
            var result =
                from e in query
                group e by new
                {
                    e.Month,
                    e.UserName,
                    e.DomId,
                    e.Housekeeping

                } into g
                select new TimeEntryEntity()
                {
                    Month = g.Key.Month,
                    UserName = g.Key.UserName,
                    DomId = g.Key.DomId,
                    Housekeeping = g.Key.Housekeeping,
                    Billable = g.Sum(e => e.Billable)
                };

            return result;
        }

        private static TableClient GetNewTableClient()
        {
            var azureAccountName = ConfigurationManager.AppSettings["azureAccountName"];
            var azureAccountKey = ConfigurationManager.AppSettings["azureAccountKey"];

            return new TableClient(azureAccountName, azureAccountKey);
        }
    }
}

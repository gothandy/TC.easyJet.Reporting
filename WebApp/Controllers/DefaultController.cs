using Azure;
using Azure.Entities;
using Azure.Tables;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
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

            var stories = from timeEntry in GroupByMonth(timeEntryTable.Query())
                        join card in cardTable.Query()
                        on timeEntry.DomId equals card.DomId
                        orderby timeEntry.Month, card.Epic, card.List, card.Name, timeEntry.UserName
                        select new JoinModel()
                        {
                            Type = "Story",
                            Month = timeEntry.Month,
                            Epic = card.Epic,
                            List = card.List,
                            DomId = timeEntry.DomId,
                            HouseKeeping = timeEntry.HouseKeeping,
                            Name = card.Name,
                            UserName = timeEntry.UserName,
                            Billable = timeEntry.Billable,
                            Invoice = card.Invoice
                        };

            return View(stories);
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

        private static IEnumerable<TimeEntryEntity> GroupByMonth(IEnumerable<TimeEntryEntity> query)
        {
            var result =
                from e in query
                group e by new
                {
                    e.Month,
                    e.UserName,
                    e.DomId,
                    e.HouseKeeping

                } into g
                select new TimeEntryEntity()
                {
                    Month = g.Key.Month,
                    UserName = g.Key.UserName,
                    DomId = g.Key.DomId,
                    HouseKeeping = g.Key.HouseKeeping,
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

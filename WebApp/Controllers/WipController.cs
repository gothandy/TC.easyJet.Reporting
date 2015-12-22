using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Vincente.Azure;
using Vincente.Azure.Tables;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class WipController : Controller
    {
        // GET: Wip
        public ActionResult Index()
        {
            var azureConnectionString = ConfigurationManager.AppSettings["azureConnectionString"];

            var client = new TableClient(azureConnectionString);

            CardTable cardTable = new CardTable(client);
            TimeEntryTable timeEntryTable = new TimeEntryTable(client);

            return View(AllWip(cardTable, timeEntryTable));
        }

        public ActionResult ByList(int? list)
        {
            var azureConnectionString = ConfigurationManager.AppSettings["azureConnectionString"];

            var client = new TableClient(azureConnectionString);

            CardTable cardTable = new CardTable(client);
            TimeEntryTable timeEntryTable = new TimeEntryTable(client);

            var data =
                from timeEntry in timeEntryTable.Query()
                join card in cardTable.Query()
                on timeEntry.DomId equals card.DomId
                where card.Invoice == null && card.ListIndex == list
                select new JoinModel()
                {
                    ListName = card.ListName,
                    Epic = card.Epic,
                    DomId = card.DomId,
                    Name = card.Name,
                    Billable = ((decimal)timeEntry.Billable) / 100
                };

            var result =
                from e in data
                group e by new
                {
                    e.ListName,
                    e.Epic,
                    e.DomId,
                    e.Name
                } into g
                select new JoinModel()
                {
                    ListName = g.Key.ListName,
                    Epic = g.Key.Epic,
                    DomId = g.Key.DomId,
                    Name = g.Key.Name,
                    Billable = g.Sum(e => e.Billable)
                };

            return View(result);
        }

        private IEnumerable<JoinModel> AllWip(CardTable cardTable, TimeEntryTable timeEntryTable)
        {
            var data =
                from timeEntry in timeEntryTable.Query()
                join card in cardTable.Query()
                on timeEntry.DomId equals card.DomId
                where card.Invoice == null
                select new JoinModel()
                {
                    ListIndex = card.ListIndex,
                    ListName = card.ListName,
                    Billable = ((decimal)timeEntry.Billable) / 100
                };

            var result =
                from e in data
                group e by new
                {
                    e.ListIndex,
                    e.ListName
                } into g
                orderby g.Key.ListIndex
                select new JoinModel()
                {
                    ListIndex = g.Key.ListIndex,
                    ListName = g.Key.ListName,
                    Billable = g.Sum(e => e.Billable)
                };

            return result;
        }
    }
}
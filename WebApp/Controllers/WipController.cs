using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Vincente.Azure;
using Vincente.Azure.Tables;
using Vincente.WebApp.Models;
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

            return View();
        }

        private IEnumerable<ListModel> AllWip(CardTable cardTable, TimeEntryTable timeEntryTable)
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
                select new ListModel()
                {
                    ListIndex = g.Key.ListIndex,
                    ListName = g.Key.ListName,
                    Billable = g.Sum(e => e.Billable)
                };

            return result;
        }
    }
}
using Azure;
using Azure.Entities;
using Azure.Tables;
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

            var join = from timeEntry in timeEntryTable.Query()
                        join card in cardTable.Query()
                        on timeEntry.DomId equals card.DomId into g
                        from card2 in g.DefaultIfEmpty()
                        select new JoinModel()
                        {
                            DomId = timeEntry.DomId,
                            HouseKeeping = timeEntry.HouseKeeping,
                            List = (card2 == null ? null: card2.List),
                            Name = (card2 == null ? null: card2.Name),
                            Epic = (card2 == null ? null: card2.Epic),
                            Invoice = (card2 == null ? null: card2.Invoice),
                            Month = timeEntry.Month,
                            UserName = timeEntry.UserName,
                            Billable = timeEntry.Billable
                        };

            return View(join);
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

            var query = table.Query();

            var result =
                from e in query
                group e by new {
                    e.Month,
                    e.UserName,
                    e.DomId,
                    e.HouseKeeping

                } into g
                select new TimeEntryEntity() {
                    Month = g.Key.Month,
                    UserName = g.Key.UserName,
                    DomId = g.Key.DomId,
                    HouseKeeping = g.Key.HouseKeeping,
                    Billable = g.Sum(e => e.Billable)
                };

            return View(result);
        }

        private static TableClient GetNewTableClient()
        {
            var azureAccountName = ConfigurationManager.AppSettings["azureAccountName"];
            var azureAccountKey = ConfigurationManager.AppSettings["azureAccountKey"];

            return new TableClient(azureAccountName, azureAccountKey);
        }
    }
}

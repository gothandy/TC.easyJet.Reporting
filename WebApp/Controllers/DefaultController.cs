using Azure;
using Azure.Entities;
using Azure.Tables;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
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

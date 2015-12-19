using Azure;
using Azure.Entities;
using Azure.Tables;
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

        public ActionResult Trello(string key)
        {
            TableClient client = new TableClient("tceasyjetreporting", key);

            CardTable table = new CardTable(client);

            var result = table.Query();

            return View(result);
        }

        public ActionResult Toggl(string key)
        {
            TableClient client = new TableClient("tceasyjetreporting", key);

            TimeEntryTable table = new TimeEntryTable(client);

            var result = table.Query();

            var groupBy =
                from e in result
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

            return View(table.Query());
        }
    }
}

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

            var results =
                from card in cardTable.Query()
                group card by new
                {
                    card.ListIndex,
                    card.ListName
                }
                into list
                orderby list.Key.ListIndex
                select new ListModel()
                {
                    ListIndex = list.Key.ListIndex,
                    ListName = list.Key.ListName,
                    Billable = 0
                };
                

            return View(results);
        }
    }
}
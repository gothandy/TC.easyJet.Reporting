using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Vincente.Azure;
using Vincente.Azure.Tables;

namespace WebApp.Controllers
{
    public class DefaultController : Controller
    {
        private TableClient tableClient;
        public DefaultController(TableClient tableClient)
        {
            this.tableClient = tableClient;
        }

        // GET: Default
        public ActionResult Index()
        {
            ViewBag.BuildDateTime = GetBuildDateTime();
            ViewBag.LatestCard = GetCardLatestTimestamp();
            ViewBag.LatestTimeEntry = GetTimeEntryLatestTimestamp();

            return View();
        }

        private DateTime GetCardLatestTimestamp()
        {
            var table = new CardTable(tableClient);

            var latest =
                from e in table.Query()
                group e by 1 into g
                select new
                {
                    Latest = g.Max(e => e.Timestamp)
                };

            return latest.First().Latest.LocalDateTime;
        }

        private DateTime GetTimeEntryLatestTimestamp()
        {
            var table = new TimeEntryTable(tableClient);

            var latest =
                from e in table.Query()
                group e by 1 into g
                select new
                {
                    Latest = g.Max(e => e.Timestamp)
                };

            return latest.First().Latest.LocalDateTime;
        }

        private DateTime GetBuildDateTime()
        {
            var entryAssembly = Assembly.GetAssembly(typeof(WebApp.MvcApplication));
            var assemblyName = entryAssembly.GetName();
            var version = assemblyName.Version;
            var timeSpan =
                new TimeSpan(
                    TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
                    TimeSpan.TicksPerSecond * 2 * version.Revision); // seconds since midnight, (multiply by 2 to get original)

            return new DateTime(2000, 1, 1).Add(timeSpan); 
        }
    }
}
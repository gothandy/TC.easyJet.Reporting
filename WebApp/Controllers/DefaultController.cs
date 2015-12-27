using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using WebApp.Models;
using Vincente.Data.Interfaces;
using Vincente.Data.Entities;

namespace WebApp.Controllers
{
    public class DefaultController : Controller
    {
        private ITableRead<Card> cardTable;
        private ITableRead<TimeEntry> timeEntryTable;

        public DefaultController(ITableRead<Card> cardTable, ITableRead<TimeEntry> timeEntryTable)
        {
            this.cardTable = cardTable;
            this.timeEntryTable = timeEntryTable;
        }

        // GET: Default
        public ActionResult Index()
        {
            ViewBag.BuildDateTime = GetTimeSpanFromNow(GetBuildDateTime()).ToLower();
            ViewBag.LatestCard = GetTimeSpanFromNow(GetCardLatestTimestamp()).ToLower();
            ViewBag.LatestTimeEntry = GetTimeSpanFromNow(GetTimeEntryLatestTimestamp()).ToLower();

            return View();
        }

        private string GetTimeSpanFromNow(DateTime past)
        {
            TimeSpan ts = DateTime.Now.Subtract(past);

            if (ts.Days != 0) return GetPeriod(ts.Days, "Day");
            if (ts.Hours != 0) return GetPeriod(ts.Hours, "Hour");
            if (ts.Minutes != 0) return GetPeriod(ts.Minutes, "Minute");
            return GetPeriod(ts.Seconds, "Second");
        }

        private static string GetPeriod(int count, string period)
        {
            var format = (count == 1) ? "{0} {1}" : "{0} {1}s";

            return string.Format(format, count, period);
        }

        private DateTime GetCardLatestTimestamp()
        {
            var latest =
                from e in cardTable.Query()
                group e by 1 into g
                select new
                {
                    Latest = g.Max(e => e.Timestamp)
                };

            return latest.First().Latest;
        }

        private DateTime GetTimeEntryLatestTimestamp()
        {
            var latest =
                from e in timeEntryTable.Query()
                group e by 1 into g
                select new
                {
                    Latest = g.Max(e => e.Timestamp)
                };

            return latest.First().Latest;
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
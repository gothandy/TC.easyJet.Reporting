using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class DefaultController : Controller
    {
        private JoinClient joinClient;

        public DefaultController(JoinClient joinClient)
        {
            this.joinClient = joinClient;
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
            if (count == 1)
            {
                return string.Format("1 {0}",period);
            }
            else
            {
                return string.Format("{0} {1}s", count, period);
            }
        }

        private DateTime GetCardLatestTimestamp()
        {
            var latest =
                from e in joinClient.GetCards()
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
                from e in joinClient.GetTimeEntries()
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
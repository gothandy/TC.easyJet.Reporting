using System;
using System.Reflection;
using System.Web.Mvc;
using Vincente.Data.Interfaces;
using Vincente.WebApp.Models;

namespace WebApp.Controllers
{
    public class DefaultController : Controller
    {
        private DefaultModel model;

        public DefaultController(ICardRead cards, ITimeEntryRead timeEntries)
        {
            model = new DefaultModel(cards, timeEntries);
        }

        // GET: Default
        public ActionResult Index()
        {
            ViewBag.BuildDateTime = GetBuildDateTime();
            return View(model);
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
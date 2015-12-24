using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            var entryAssembly = Assembly.GetAssembly(typeof(WebApp.MvcApplication));

            var assemblyName = entryAssembly.GetName();

            var version = assemblyName.Version;

            ViewBag.BuildDateTime = new DateTime(2000, 1, 1).Add(
                new TimeSpan(
                    TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
                    TimeSpan.TicksPerSecond * 2 * version.Revision)); // seconds since midnight, (multiply by 2 to get original)
            
            return View();
        }
    }
}
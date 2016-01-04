using System.Web.Mvc;
using Vincente.Data.Interfaces;
using Vincente.Data.Tables;
using System.Linq;
using Vincente.Data.Entities;
using System.Collections.Generic;
using System;

namespace Vincente.WebApp.Controllers
{
    public class UserController : Controller
    {
        private IEnumerable<TimeEntry> timeEntries;

        public UserController(ITimeEntryRead timeEntries)
        {
            this.timeEntries = timeEntries.Query();
        }
        // GET: User
        public ActionResult Summary()
        {
            return View();
        }
    }
}
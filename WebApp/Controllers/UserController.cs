using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;
using Vincente.WebApp.Models;

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
            var result =
               from e in timeEntries
               where e.Start > new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1)
            group e by new
               {
                   e.UserName
               }
               into g
               orderby g.Key.UserName
               select new UserModel()
               {
                   UserName = g.Key.UserName,
                   LastMonth = g.Sum(e => sumMonth(e.Start, e.Billable, -1)),
                   ThisMonth = g.Sum(e => sumMonth(e.Start, e.Billable, 0)),
                   Yesterday = g.Sum(e => sumDay(e.Start, e.Billable, -1)),
                   Today = g.Sum(e => sumDay(e.Start, e.Billable, 0)),
                   Total = g.Sum(e => e.Billable)
               };

            return View(result);
        }

        private decimal sumMonth(DateTime start, decimal billable, int months)
        {
            return (getMonth(start) == getMonth(DateTime.Now).AddMonths(months)) ? billable :  0;
        }

        private decimal sumDay(DateTime start, decimal billable, int days)
        {
            return (getDay(start) == getDay(DateTime.Now).AddDays(days)) ? billable : 0;
        }

        private DateTime getMonth(DateTime d) { return new DateTime(d.Year, d.Month, 1); }

        private DateTime getDay(DateTime d) { return new DateTime(d.Year, d.Month, d.Day); }

    }
}
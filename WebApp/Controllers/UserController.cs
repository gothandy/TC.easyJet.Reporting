using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;
using Vincente.WebApp.Models;

namespace Vincente.WebApp.Controllers
{
    public class UserController : BaseController
    {
        private IEnumerable<TimeEntry> timeEntries;

        public UserController(ITimeEntryRead timeEntries)
        {
            this.timeEntries = timeEntries.Query();

            defaultAction = "Summary";
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
                   LastMonth = g.Sum(e => SumIfMonth(e.Start, e.Billable, -1)),
                   ThisMonth = g.Sum(e => SumIfMonth(e.Start, e.Billable, 0)),
                   LastWeek = g.Sum(e => SumIfWeek(e.Start, e.Billable, -1)),
                   ThisWeek = g.Sum(e => SumIfWeek(e.Start, e.Billable, 0)),
                   Yesterday = g.Sum(e => SumIfDay(e.Start, e.Billable, -1)),
                   Today = g.Sum(e => SumIfDay(e.Start, e.Billable, 0)),
                   Total = g.Sum(e => e.Billable)
               };

            return View(result);
        }

        private decimal SumIfMonth(DateTime start, decimal billable, int months)
        {
            return (StartOfMonth(start)
                == StartOfMonth(DateTime.Now).AddMonths(months)) ? billable :  0;
        }

        private decimal SumIfDay(DateTime start, decimal billable, int days)
        {
            return (StartOfDay(start)
                == StartOfDay(DateTime.Now).AddDays(days)) ? billable : 0;
        }

        private decimal SumIfWeek(DateTime start, decimal billable, int weeks)
        {
            return (StartOfWeek(start)
                == StartOfWeek(DateTime.Now).AddDays(weeks * 7)) ? billable : 0;
        }

        // Move these into extension methods for DateTime?

        private static DateTime StartOfMonth(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        private static DateTime StartOfDay(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        private static DateTime StartOfWeek(DateTime dt)
        {
            DayOfWeek startOfWeek = DayOfWeek.Monday;

            int diff = dt.DayOfWeek - startOfWeek;

            if (diff < 0) diff += 7;

            return dt.AddDays(-1 * diff).Date;
        }


    }
}
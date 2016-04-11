using Gothandy.Tables.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;

namespace Vincente.Data.Tables
{
    public class HousekeepingByMonth : ITableRead<Activity>
    {
        private TimeEntriesByMonth timeEntriesByMonth;

        public HousekeepingByMonth(TimeEntriesByMonth timeEntriesByMonth)
        {
            this.timeEntriesByMonth = timeEntriesByMonth;
        }

        public IEnumerable<Activity> Query()
        {
            return
                from timeEntry in timeEntriesByMonth.Query()
                where timeEntry.Housekeeping != null && timeEntry.Month > new System.DateTime(2015, 6, 30)
                select new Activity()
                {
                    Month = timeEntry.Month,
                    Epic = "Housekeeping",
                    ListIndex = null,
                    ListName = null,
                    DomId = null,
                    Name = timeEntry.Housekeeping,
                    UserName = timeEntry.UserName,
                    TeamName = timeEntry.TeamName,
                    ReuseDA = false,
                    ReuseFCP = false,
                    Billable = timeEntry.Billable,
                    Invoice = timeEntry.Month
                };
        }
    }
}

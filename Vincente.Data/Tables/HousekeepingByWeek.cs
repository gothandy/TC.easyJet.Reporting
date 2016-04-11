using Gothandy.Tables.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;

namespace Vincente.Data.Tables
{
    public class HousekeepingByWeek : ITableRead<Activity>
    {
        private TimeEntriesByWeek timeEntriesByWeek;

        public HousekeepingByWeek(TimeEntriesByWeek timeEntriesByWeek)
        {
            this.timeEntriesByWeek = timeEntriesByWeek;
        }

        public IEnumerable<Activity> Query()
        {
            return
                from timeEntry in timeEntriesByWeek.Query()
                where timeEntry.Housekeeping != null && timeEntry.Month > new System.DateTime(2015, 6, 30)
                select new Activity()
                {
                    Week = timeEntry.Week,
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

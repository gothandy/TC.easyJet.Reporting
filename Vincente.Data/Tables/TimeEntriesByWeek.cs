using Gothandy.Tables.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Data.Tables
{
    public class TimeEntriesByWeek : ITableRead<TimeEntry>
    {
        private ITimeEntryRead timeEntryTable;

        public TimeEntriesByWeek(ITimeEntryRead timeEntryTable)
        {
            this.timeEntryTable = timeEntryTable;
        }

        public IEnumerable<TimeEntry> Query()
        {
            return
                from e in timeEntryTable.Query()
                group e by new
                {
                    e.Month,
                    e.Week,
                    e.UserName,
                    e.TeamName,
                    e.DomId,
                    e.TaskId,
                    e.Housekeeping
                } into g
                select new TimeEntry()
                {
                    Month = g.Key.Month,
                    Week = g.Key.Week,
                    UserName = g.Key.UserName,
                    TeamName = g.Key.TeamName,
                    DomId = g.Key.DomId,
                    TaskId = g.Key.TaskId,
                    Housekeeping = g.Key.Housekeeping,
                    Billable = g.Sum(e => e.Billable)
                };
        }
    }
}

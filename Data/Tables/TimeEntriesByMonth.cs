using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;
using Vincente.Data.Interfaces.ViewInterfaces;

namespace Vincente.Data.Tables
{
    public class TimeEntriesByMonth : ITimeEntriesByMonth
    {
        private ITableRead<TimeEntry> timeEntryTable;

        public TimeEntriesByMonth(ITableRead<TimeEntry> timeEntryTable)
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
                    e.UserName,
                    e.DomId,
                    e.Housekeeping

                } into g
                select new TimeEntry()
                {
                    Month = g.Key.Month,
                    UserName = g.Key.UserName,
                    DomId = g.Key.DomId,
                    Housekeeping = g.Key.Housekeeping,
                    Billable = g.Sum(e => e.Billable)
                };
        }
    }
}

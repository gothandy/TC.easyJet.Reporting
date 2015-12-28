using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace WebApp.Models
{
    public class JoinClient
    {
        private IEnumerable<Card> cardTableQuery;
        private IEnumerable<TimeEntry> timeEntryTableQuery;

        public JoinClient(ITableRead<Card> cardTable, ITableRead<TimeEntry> timeEntryTable)
        {
            this.cardTableQuery = Cache<Card>("cardTableQuery", cardTable.Query());
            this.timeEntryTableQuery = Cache<TimeEntry>("timeEntryTableQuery", timeEntryTable.Query());
        }

        public IEnumerable<TimeEntry> GetOrphans()
        {
            var result =
                from timeEntry in GroupByMonth(timeEntryTableQuery)
                where
                    timeEntry.Housekeeping == null &&
                    timeEntry.DomId == null &&
                    timeEntry.Month > new System.DateTime(2015, 6, 30)
                orderby timeEntry.Start
                select new TimeEntry()
                {
                    Start = timeEntry.Start,
                    Housekeeping = timeEntry.Housekeeping,
                    UserName = timeEntry.UserName,
                    Billable = timeEntry.Billable,
                    TaskId = timeEntry.TaskId,
                };

            return result;
        }

    }
}
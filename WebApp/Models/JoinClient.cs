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

        public JoinClient(ITable<Card> cardTable, ITable<TimeEntry> timeEntryTable)
        {
            this.cardTableQuery = Cache<Card>("cardTableQuery", cardTable.Query());
            this.timeEntryTableQuery = Cache<TimeEntry>("timeEntryTableQuery", timeEntryTable.Query());
        }

        public IEnumerable<JoinModel> GetWipData()
        {

            var result =
                from e in GetStories()
                where e.Invoice == null
                select new JoinModel()
                {
                    ListIndex = e.ListIndex,
                    ListName = e.ListName,
                    Epic = e.Epic,
                    CardId = e.CardId,
                    DomId = e.DomId,
                    Name = e.Name,
                    Month = e.Month,
                    UserName = e.UserName,
                    Billable = e.Billable,
                };

            return Cache<JoinModel>("GetJoinedData", result);
        }

        public IEnumerable<TimeEntry> GetTimeEntriesByMonth()
        {
            var data = timeEntryTableQuery;

            return GroupByMonth(data);
        }

        public IEnumerable<Card> GetCards()
        {
            return cardTableQuery;
        }

        public IEnumerable<TimeEntry> GetTimeEntries()
        {
            return timeEntryTableQuery;
        }

        public IEnumerable<JoinModel> GetHousekeeping()
        {
            var result =
                from timeEntry in GroupByMonth(timeEntryTableQuery)
                where timeEntry.Housekeeping != null && timeEntry.Month > new System.DateTime(2015, 6, 30)
                select new JoinModel()
                {
                    Month = timeEntry.Month,
                    Epic = "Housekeeping",
                    ListIndex = null,
                    ListName = null,
                    DomId = null,
                    Name = timeEntry.Housekeeping,
                    UserName = timeEntry.UserName,
                    Billable = timeEntry.Billable,
                    Invoice = timeEntry.Month
                };

            return Cache<JoinModel>("GetHousekeeping", result);
        }

        public IEnumerable<JoinModel> GetStories()
        {
            var result =
                from timeEntry in GroupByMonth(timeEntryTableQuery)
                join card in cardTableQuery
                on timeEntry.DomId equals card.DomId
                orderby timeEntry.Month, card.Epic, card.ListIndex, card.Name, timeEntry.UserName
                select new JoinModel()
                {
                    Month = timeEntry.Month,
                    Epic = card.Epic,
                    ListIndex = card.ListIndex,
                    ListName = card.ListName,
                    CardId = card.Id,
                    DomId = timeEntry.DomId,
                    Name = card.Name,
                    UserName = timeEntry.UserName,
                    Billable = timeEntry.Billable,
                    Invoice = card.Invoice
                };

            return Cache<JoinModel>("GetStories", result);
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

            return Cache<TimeEntry>("GetOrhpans", result);
        }

        private static IEnumerable<TimeEntry> GroupByMonth(IEnumerable<TimeEntry> query)
        {
            var result =
                from e in query
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

            return Cache<TimeEntry>("GroupByMonth", result);
        }

        private static IEnumerable<T> Cache<T>(string key, IEnumerable<T> query)
        {
            if (HttpRuntime.Cache[key] == null)
            {
                var list = query.ToList<T>();

                HttpRuntime.Cache.Insert(key, list, null, System.DateTime.UtcNow.AddMinutes(1), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            return (IEnumerable<T>)HttpRuntime.Cache[key];
        }
    }
}
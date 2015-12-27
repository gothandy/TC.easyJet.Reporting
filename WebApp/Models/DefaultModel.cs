using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace WebApp.Models
{
    public class DefaultModel
    {
        private ITableRead<Card> cardTable;
        private ITableRead<TimeEntry> timeEntryTable;

        public DefaultModel(ITableRead<Card> cardTable, ITableRead<TimeEntry> timeEntryTable)
        {
            this.cardTable = cardTable;
            this.timeEntryTable = timeEntryTable;
        }

        public DateTime CardLatestTimestamp
        {
            get
            {
                var latest =
                    from e in cardTable.Query()
                    group e by 1 into g
                    select new
                    {
                        Latest = g.Max(e => e.Timestamp)
                    };

                return latest.First().Latest;
            }
        }

        public DateTime TimeEntryLatestTimestamp
        {
            get
            {
                var latest =
                    from e in timeEntryTable.Query()
                    group e by 1 into g
                    select new
                    {
                        Latest = g.Max(e => e.Timestamp)
                    };

                return latest.First().Latest;
            }
        }

        public DateTime BuildDateTime
        {
            get
            {
                var entryAssembly = Assembly.GetAssembly(typeof(WebApp.MvcApplication));
                var assemblyName = entryAssembly.GetName();
                var version = assemblyName.Version;
                var timeSpan =
                    new TimeSpan(
                        TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
                        TimeSpan.TicksPerSecond * 2 * version.Revision); // seconds since midnight, (multiply by 2 to get original)

                return new DateTime(2000, 1, 1).Add(timeSpan);
            }
        }
    }
}
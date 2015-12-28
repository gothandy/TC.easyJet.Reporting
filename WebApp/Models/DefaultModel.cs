using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;
using Vincente.WebApp.Models;

namespace WebApp.Models
{
    public class DefaultModel
    {
        private ICardRead cards;
        private ITimeEntryRead timeEntries;

        public DefaultModel(ICardRead cards, ITimeEntryRead timeEntries)
        {
            this.cards = cards;
            this.timeEntries = timeEntries;
        }

        public DateTime CardLatestTimestamp
        {
            get
            {
                var latest =
                    from e in cards.Query()
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
                    from e in timeEntries.Query()
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
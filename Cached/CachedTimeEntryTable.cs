using System;
using System.Collections.Generic;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Cached
{
    public class CachedTimeEntryTable : BaseCachedTable<TimeEntry>, ITimeEntryRead
    {
        public CachedTimeEntryTable(ITimeEntryRead table) : base(table) { }

        public IEnumerable<TimeEntry> Query()
        {
            return Cache<TimeEntry>(table.Query(), "TimeEntryTable", new TimeSpan(0,1,0));
        }
    }
}

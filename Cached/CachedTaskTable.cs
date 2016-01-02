using Gothandy.Tables.Cache;
using System;
using System.Collections.Generic;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Cached
{
    public class CachedTaskTable : BaseCachedTable<Task>, ITaskRead
    {
        public CachedTaskTable(ITaskRead table) : base(table) { }

        public IEnumerable<Task> Query()
        {
            return Cache(table.Query(), "TaskTable", new TimeSpan(0,1,0));
        }
    }
}

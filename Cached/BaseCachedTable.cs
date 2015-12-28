using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vincente.Data.Interfaces;

namespace Vincente.Cached
{
    public class BaseCachedTable<T>
    {
        protected ITableRead<T> table;

        public BaseCachedTable (ITableRead<T> table)
        {
            this.table = table;
        }

        internal IEnumerable<T> Cache<T>(IEnumerable<T> query, string key, TimeSpan period)
        {
            if (HttpRuntime.Cache[key] == null)
            {
                var list = query.ToList<T>();

                HttpRuntime.Cache.Insert(key, list, null, DateTime.UtcNow.Add(period), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            return (IEnumerable<T>)HttpRuntime.Cache[key];
        }
    }
}

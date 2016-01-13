using Gothandy.Tables.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gothandy.Tables.Cache
{
    public class BaseCachedTable<T>
    {
        protected ITableRead<T> table;

        public BaseCachedTable (ITableRead<T> table)
        {
            this.table = table;
        }

        public IEnumerable<T> Cache(IEnumerable<T> query, string key, TimeSpan period)
        {
            if (HttpRuntime.Cache[key] == null)
            {
                var list = query.ToList<T>();

                HttpRuntime.Cache.Insert(key, list, null,
                    DateTime.UtcNow.Add(period),
                    System.Web.Caching.Cache.NoSlidingExpiration);
            }

            return (IEnumerable<T>)HttpRuntime.Cache[key];
        }
    }
}

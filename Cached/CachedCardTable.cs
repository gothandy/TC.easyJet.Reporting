using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Cached
{
    public class CachedCardTable : ITableRead<Card>
    {
        private ITableRead<Card> cardTable;
        private TimeSpan period;

        public CachedCardTable (ITableRead<Card> cardTable, TimeSpan period)
        {
            this.cardTable = cardTable;
            this.period = period;
        }

        public IEnumerable<Card> Query()
        {
            return Cache("CardTable", cardTable.Query(), period);
        }

        private static IEnumerable<T> Cache<T>(string key, IEnumerable<T> query, TimeSpan period)
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

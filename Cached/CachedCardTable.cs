using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Cached
{
    public class CachedCardTable : ITable<Card>
    {
        private ITable<Card> cardTable;
        private TimeSpan period;

        public CachedCardTable (ITable<Card> cardTable, TimeSpan period)
        {
            this.cardTable = cardTable;
            this.period = period;
        }

        public void BatchInsertOrReplace(Card item)
        {
            cardTable.BatchInsertOrReplace(item);
        }

        public void ExecuteBatch()
        {
            cardTable.ExecuteBatch();
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

using System;
using System.Collections.Generic;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Cached
{
    public class CachedCardTable : BaseCachedTable<Card>, ICardRead
    {
        public CachedCardTable(ICardRead table) : base(table) { }

        public IEnumerable<Card> Query()
        {
            return Cache(table.Query(), "CardTable", new TimeSpan(0,1,0));
        }
    }
}

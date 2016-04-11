using Gothandy.Tables.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;

namespace Vincente.Data.Tables
{
    public class InvoiceByWeek : ITableRead<Activity>
    {
        private CardsByWeek cardsByWeek;
        private HousekeepingByWeek housekeepingByWeek;

        public InvoiceByWeek(CardsByWeek cardsByWeek, HousekeepingByWeek housekeepingByWeek)
        {
            this.cardsByWeek = cardsByWeek;
            this.housekeepingByWeek = housekeepingByWeek;
        }

        public IEnumerable<Activity> Query()
        {
            var includeForecast = (from a in cardsByWeek.Query()
                                   select AddForecastInvoiceDate(a));
                                   
            return includeForecast.Concat(housekeepingByWeek.Query());
        }

        private static Activity AddForecastInvoiceDate(Activity a)
        {
            if (a.Invoice == null)
            {
                var index = 10 - a.ListIndex.Value;
                if (index < 0) index = 0;
                var now = DateTime.Now;
                var rfi = now.AddDays(index * 5);
                a.Invoice = new DateTime(rfi.Year, rfi.Month, 1);
                a.Wip = a.Billable;
                a.IsWip = true;
            }
            else
            {
                a.IsWip = false;
            }

            return a;
        }
    }
}

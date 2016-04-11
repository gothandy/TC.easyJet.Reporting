using Gothandy.Tables.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;

namespace Vincente.Data.Tables
{
    public class InvoiceByMonth : ITableRead<Activity>
    {
        private CardsByMonth cardsByMonth;
        private HousekeepingByMonth housekeeping;

        public InvoiceByMonth(CardsByMonth cardsByMonth, HousekeepingByMonth housekeeping)
        {
            this.cardsByMonth = cardsByMonth;
            this.housekeeping = housekeeping;
        }

        public IEnumerable<Activity> Query()
        {
            var includeForecast = (from a in cardsByMonth.Query()
                                   select AddForecastInvoiceDate(a));
                                   
            return includeForecast.Concat(housekeeping.Query());
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

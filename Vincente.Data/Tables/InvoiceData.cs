using Gothandy.Tables.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;

namespace Vincente.Data.Tables
{
    public class InvoiceData : ITableRead<Activity>
    {
        private CardsByMonth cardsWithTime;
        private Housekeeping housekeeping;

        public InvoiceData(CardsByMonth cardsWithTime, Housekeeping housekeeping)
        {
            this.cardsWithTime = cardsWithTime;
            this.housekeeping = housekeeping;
        }

        public IEnumerable<Activity> Query()
        {
            return cardsWithTime.Query().Concat(housekeeping.Query());
        }
    }
}

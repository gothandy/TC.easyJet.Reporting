using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Data.Tables
{
    public class InvoiceData : ITableRead<CardWithTime>
    {
        private CardsWithTime cardsWithTime;
        private Housekeeping housekeeping;

        public InvoiceData(CardsWithTime cardsWithTime, Housekeeping housekeeping)
        {
            this.cardsWithTime = cardsWithTime;
            this.housekeeping = housekeeping;
        }

        public IEnumerable<CardWithTime> Query()
        {
            return cardsWithTime.Query().Concat(housekeeping.Query());
        }
    }
}

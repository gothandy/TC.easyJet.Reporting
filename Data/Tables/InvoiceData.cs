using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;
using Vincente.Data.Interfaces.ViewInterfaces;

namespace Vincente.Data.Tables
{
    public class InvoiceData : IInvoiceData
    {
        private ICardsWithTime cardsWithTime;
        private IHousekeeping housekeeping;

        public InvoiceData(ICardsWithTime cardsWithTime, IHousekeeping housekeeping)
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

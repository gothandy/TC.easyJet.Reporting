using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vincente.Data.Entities;

namespace Vincente.Data.Interfaces.ViewInterfaces
{
    public interface ITimeEntriesByMonth : ITableRead<TimeEntry> { }
    public interface ICardsWithTime : ITableRead<CardWithTime> { }
    public interface IHousekeeping : ITableRead<CardWithTime> { }
    public interface IInvoiceData : ITableRead<CardWithTime> { }
}

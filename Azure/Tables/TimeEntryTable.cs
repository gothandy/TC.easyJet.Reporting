using Gothandy.Tables.Azure;
using Microsoft.WindowsAzure.Storage.Table;
using Vincente.Azure.Converters;
using Vincente.Azure.Entities;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Azure.Tables
{
    public class TimeEntryTable : AzureTable<TimeEntry, TimeEntryEntity>, ITimeEntryRead, ITimeEntryWrite
    {
        public TimeEntryTable(CloudTable table) : base(table, new TimeEntryConverter()) { }
    }
}

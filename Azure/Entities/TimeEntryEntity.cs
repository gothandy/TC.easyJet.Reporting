using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Vincente.Azure.Entities
{
    public class TimeEntryEntity : TableEntity
    {
        public long? TaskId { get; set; }
        public string DomId { get; set; }
        public DateTime? Start { get; set; }
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public long Billable { get; set; }
        public string Housekeeping { get; set; }
        public string Description { get; set; }
    }
}

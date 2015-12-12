using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace TC.easyJet.Reporting
{
    public class TimeEntryEntity : TableEntity
    {

        public TimeEntryEntity(long? taskId, long? id)
        {

            this.PartitionKey = taskId.ToString();
            this.RowKey = id.ToString();
        }

        public TimeEntryEntity() { }

        public long? ProjectId { get; set; }
        public DateTime? Start { get; set; }
        public string TaskName { get; set; }
        public string UserName { get; set; }
        public long? Billable { get; set; }
        
    }
}

using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Vincente.Azure.Entities
{
    public class TaskEntity : TableEntity
    {
        public string Name { get; set; }
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool Active { get; set; }
        public long TrackedSeconds { get; set; }
        public string CardId { get; set; }
        public string DomId { get; set; }
    }
}

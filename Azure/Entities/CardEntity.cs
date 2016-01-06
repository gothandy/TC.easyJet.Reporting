using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Vincente.Azure.Entities
{
    public class CardEntity : TableEntity
    {
        public string DomId { get; set; }
        public int ListIndex { get; set; }
        public string ListName { get; set; }
        public string Name { get; set; }
        public string Epic { get; set; }
        public bool? Blocked { get; set; }
        public string BlockedReason { get; set; }
        public bool? ReuseDA { get; set; }
        public bool? ReuseFCP { get; set; }
        public DateTime? Invoice { get; set; }
        public string TaskIds { get; set; }
    }
}

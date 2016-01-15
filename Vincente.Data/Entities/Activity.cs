using System;

namespace Vincente.Data.Entities
{
    public class Activity
    {
        public string CardId { get; set; }
        public long? TaskId { get; set; }
        public string DomId { get; set; }

        public string Epic { get; set; }
        public int? ListIndex { get; set; }
        public string ListName { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string TeamName { get; set; }

        // Blocked
        public bool? IsBlocked { get; set; }
        public string BlockedReason { get; set; }
        public decimal? Blocked { get; set; }

        // Wip
        public bool? IsWip { get; set; }
        public decimal? Wip { get; set; }

        //Reuse
        public bool? ReuseDA { get; set; }
        public bool? ReuseFCP { get; set; }

        //Dates
        public DateTime? Invoice { get; set; }
        public DateTime? Month { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public TimeSpan? Duration { get; set; }

        public decimal? Billable { get; set; }
        
    }
}

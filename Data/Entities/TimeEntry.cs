using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vincente.Data.Entities
{
    public class TimeEntry
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public string DomId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Month { get; set; }
        public string UserName { get; set; }
        public decimal Billable { get; set; }
        public string Housekeeping { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

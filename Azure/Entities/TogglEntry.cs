using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Entities
{
    public class TogglEntry
    {
        public long TaskId { get; set; }
        public string DomId { get; set; }
        public DateTime Start { get; set; }
        public string UserName { get; set; }
        public decimal Billable { get; set; }
        public string Housekeeping { get; set; }
    }
}

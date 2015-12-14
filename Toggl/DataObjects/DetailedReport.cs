using System.Collections.Generic;
using Newtonsoft.Json;

namespace Toggl.DataObjects
{
    public class DetailedReport
    {
		[JsonProperty(PropertyName = "data")]
		public List<ReportTimeEntry> Data { get; set; }

        [JsonProperty(PropertyName = "total_grand")]
        public long? TotalGrand { get; set; }

        [JsonProperty(PropertyName = "total_billable")]
        public long? TotalBillable { get; set; }

        [JsonProperty(PropertyName = "total_count")]
        public long? TotalCount { get; set; }

        [JsonProperty(PropertyName = "per_page")]
        public long? PerPage { get; set; }

        public bool LastPage { get; internal set; }
    }
}

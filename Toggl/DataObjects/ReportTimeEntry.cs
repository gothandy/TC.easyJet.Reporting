using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Toggl.DataObjects
{
    public class ReportTimeEntry
	{
		[JsonProperty(PropertyName = "id")]
		public long? Id { get; set; }

		[JsonProperty(PropertyName = "pid")]
		public long? ProjectId { get; set; }

		[JsonProperty(PropertyName = "project")]
		public string ProjectName { get; set; }

		[JsonProperty(PropertyName = "client")]
		public string ClientName { get; set; }

		[JsonProperty(PropertyName = "tid")]
		public long? TaskId { get; set; }

		[JsonProperty(PropertyName = "task")]
		public string TaskName { get; set; }

		[JsonProperty(PropertyName = "uid")]
		public long? UserId { get; set; }

		[JsonProperty(PropertyName = "user")]
		public string UserName { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "start")]
		[JsonConverter(typeof(IsoDateTimeConverter))]
		public DateTime? Start { get; set; }
		//public string Start { get; set; }

		[JsonProperty(PropertyName = "end")]
		//[JsonConverter(typeof(IsoDateTimeConverter))]
		//public DateTime? Stop { get; set; }
		public string Stop { get; set; }

		[JsonProperty(PropertyName = "dur")]
		public long? Duration { get; set; }

		[JsonProperty(PropertyName = "updated")]
		//[JsonConverter(typeof(IsoDateTimeConverter))]
		//public DateTime? Stop { get; set; }
		public string Updated { get; set; }

		[JsonProperty(PropertyName = "use_stop")]
		public bool? UseStop { get; set; }

		[JsonProperty(PropertyName = "is_billable")]
		public bool? IsBillable { get; set; }

		[JsonProperty(PropertyName = "billable")]
		public long? Billable { get; set; }

	}
}

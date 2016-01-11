using Newtonsoft.Json;

namespace Gothandy.Toggl.DataObjects
{
    public class Task
    {
        /*
        {
            "id":714xxxx,
            "name":"201xxxxx.7 â€“ Rebooking â€“ Rebooking API functionality",
            "wid":605xxx,
            "pid":833xxxx,
            "active":true,
            "at":"2015-09-24T10:53:52+00:00",
            "estimated_seconds":0,
            "tracked_seconds":613xxx
        }
        */

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "id")]
        public long? Id { get; set; }

        [JsonProperty(PropertyName = "wid")]
        public long WorkspaceId { get; set; }

        [JsonProperty(PropertyName = "pid")]
        public long? ProjectId { get; set; }

        [JsonProperty(PropertyName = "active")]
        public bool? Active { get; set; }

        [JsonProperty(PropertyName = "estimated_seconds")]
        public long? EstimatedSeconds { get; set; }

        [JsonProperty(PropertyName = "tracked_seconds")]
        public long? TrackedSeconds { get; set; }

    }
}

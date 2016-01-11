using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Gothandy.Toggl.DataObjects
{
    public class Project
    {
        /*
            "id":121xxxxx,
            "wid":605xxx,
            "cid":152xxxxx,
            "name":"XX XX Xxxxxxx",
            "billable":true,
            "is_private":true,
            "active":true,
            "template":false,
            "at":"2015-12-26T16:02:47+00:00",
            "created_at":"2015-11-06T15:42:38+00:00",
            "color":"3",
            "auto_estimates":false,
            "actual_hours":1400,
            "rate":15.00
        */

        [JsonProperty(PropertyName = "id")]
        public long? Id { get; set; }

        [JsonProperty(PropertyName = "wid")]
        public long WorkspaceId { get; set; }

        [JsonProperty(PropertyName = "cid")]
        public long? ClientId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "billable")]
        public bool? Billable { get; set; }

        [JsonProperty(PropertyName = "is_private")]
        public bool? IsPrivate { get; set; }

        [JsonProperty(PropertyName = "active")]
        public bool? Active { get; set; }

        [JsonProperty(PropertyName = "template")]
        public bool? Template { get; set; }

        [JsonProperty(PropertyName = "at")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? Timestamp { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? Created { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "auto_estimates")]
        public bool? AutoEstimates { get; set; }

        [JsonProperty(PropertyName = "actual_hours")]
        public long? ActualHours { get; set; }

        [JsonProperty(PropertyName = "rate")]
        public double? Rate { get; set; }
    }
}
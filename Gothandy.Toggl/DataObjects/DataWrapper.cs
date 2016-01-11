using Newtonsoft.Json;

namespace Gothandy.Toggl.DataObjects
{
    public class DataWrapper<T>
    {
        [JsonProperty(PropertyName = "data")]
        public Project Data { get; set; }
    }
}

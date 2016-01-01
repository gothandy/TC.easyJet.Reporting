using Newtonsoft.Json;

namespace Vincente.Toggl.DataObjects
{
    public class DataWrapper<T>
    {
        [JsonProperty(PropertyName = "data")]
        public Project Data { get; set; }
    }
}

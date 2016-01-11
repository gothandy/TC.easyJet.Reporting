using Newtonsoft.Json;

namespace Gothandy.Toggl.DataObjects
{
    public class DataWrapper<T>
    {
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
    }
}

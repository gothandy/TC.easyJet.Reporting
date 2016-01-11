using Newtonsoft.Json;

namespace Gothandy.Toggl.DataObjects
{
    public class TaskWrapper
    {
        [JsonProperty(PropertyName = "task")]
        public Task Task { get; set; }
    }
}

using Newtonsoft.Json;

namespace Gothandy.Toggl.DataObjects
{
    public class ProjectWrapper
    {
        [JsonProperty(PropertyName = "project")]
        public Project Project { get; set; }
    }
}

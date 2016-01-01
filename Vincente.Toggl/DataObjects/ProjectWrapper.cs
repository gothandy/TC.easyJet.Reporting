using Newtonsoft.Json;

namespace Vincente.Toggl.DataObjects
{
    public class ProjectWrapper
    {
        [JsonProperty(PropertyName = "project")]
        public Project Project { get; set; }
    }
}

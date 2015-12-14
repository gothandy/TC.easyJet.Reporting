using Newtonsoft.Json;

namespace TrelloToAzure.Trello.DataObjects
{
    public class Label
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
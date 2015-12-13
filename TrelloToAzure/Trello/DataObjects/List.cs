using Newtonsoft.Json;

namespace TrelloToAzure.Trello.DataObjects
{
    public class List
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "pos")]
        public long Position { get; set; }

        [JsonProperty(PropertyName = "closed")]
        public bool Closed { get; set; }
    }
}
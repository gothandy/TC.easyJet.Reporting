using Newtonsoft.Json;
using System.Collections.Generic;

namespace Vincente.Trello.DataObjects
{
    public class TrelloCard
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "idList")]
        public string IdList { get; set; }

        [JsonProperty(PropertyName = "idLabels")]
        public List<string> IdLabels { get; set; }      

    }
}
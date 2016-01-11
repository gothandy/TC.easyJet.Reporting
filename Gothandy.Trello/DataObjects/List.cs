using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gothandy.Trello.DataObjects
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

        public static List GetList(string idList, List<List> lists)
        {

            foreach (List list in lists)
            {
                if (idList == list.Id) return list;
            }

            return null;
        }
    }
}
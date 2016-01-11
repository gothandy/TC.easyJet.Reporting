using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gothandy.Trello.DataObjects
{
    public class Label
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public static List<string> GetNameLabels(List<string> idLabels, List<Label> labels)
        {
            var nameLabels = new List<String>();

            foreach (string idLabel in idLabels)
            {
                var nameLabel = GetNameLabel(idLabel, labels);

                nameLabels.Add(nameLabel);
            }

            return nameLabels;
        }

        private static string GetNameLabel(string idLabel, List<Label> labels)
        {
            foreach (Label label in labels)
            {
                if (label.Id == idLabel) return label.Name;
            }

            throw (new Exception("No Id Label match."));
        }
    }
}
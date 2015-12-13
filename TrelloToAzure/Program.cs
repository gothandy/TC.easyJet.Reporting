using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TrelloToAzure
{
    class Program
    {
        static void Main(string[] args)
        {
            var accountKey = args[0];

            var connectionString = String.Format(
                "DefaultEndpointsProtocol=https;AccountName=tceasyjetreporting;AccountKey={0}",
                accountKey);

            var trelloKey = "3ba00ca224256611c3ccbac183364259";
            var trelloToken = args[1];
            var boardId = "5596a7b7ac88c077383d281c";
            var fields = "name,idList,idLabels";

            var url = String.Format(
                "https://api.trello.com/1/boards/{0}/cards?fields={1}&key={2}&token={3}",
                boardId, fields, trelloKey, trelloToken);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var json = webClient.DownloadString(url);

                var cards = JsonConvert.DeserializeObject(json);
            }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using TrelloToAzure.Trello.DataObjects;

namespace Trello
{
    public class Workspace
    {
        private string key;
        private string token;
        private string boardId;

        public Workspace(string key, string token, string boardId)
        {
            this.key = key;
            this.token = token;
            this.boardId = boardId;
        }

        public List<Card> GetCards()
        {
            List<Card> cards;
            var fields = "name,idList,idLabels";

            var url = String.Format(
                "https://api.trello.com/1/boards/{0}/cards/all?fields={1}&key={2}&token={3}",
                boardId, fields, key, token);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var json = webClient.DownloadString(url);

                cards = JsonConvert.DeserializeObject<List<Card>>(json);
            }

            return cards;
        }

        public List<Label> GetLabels()
        {
            List<Label> labels;
            var fields = "name";

            var url = String.Format(
                "https://api.trello.com/1/boards/{0}/labels?fields={1}&key={2}&token={3}",
                boardId, fields, key, token);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var json = webClient.DownloadString(url);

                labels = JsonConvert.DeserializeObject<List<Label>>(json);
            }

            return labels;
        }

        public List<List> GetLists()
        {
            List<List> lists;
            var fields = "name,pos,closed";

            var url = String.Format(
                "https://api.trello.com/1/boards/{0}/lists/all?fields={1}&key={2}&token={3}",
                boardId, fields, key, token);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var json = webClient.DownloadString(url);

                lists = JsonConvert.DeserializeObject<List<List>>(json);
            }

            return lists;
        }
    }
}

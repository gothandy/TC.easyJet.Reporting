using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using Gothandy.Trello.DataObjects;

namespace Gothandy.Trello
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

        public List<TrelloCard> GetCards()
        {
            List<TrelloCard> cards;
            var fields = "name,idList,idLabels";

            var url = String.Format(
                "https://api.trello.com/1/boards/{0}/cards/all?fields={1}&key={2}&token={3}",
                boardId, fields, key, token);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var json = webClient.DownloadString(url);

                cards = JsonConvert.DeserializeObject<List<TrelloCard>>(json);
            }

            return cards;
        }

        public string GetBackupJson()
        {
            var url = String.Format(
                "https://api.trello.com/1/boards/{0}?{1}&key={2}&token={3}",
                boardId,
                "fields=all&actions=all&action_fields=all&actions_limit=1000&cards=all&card_fields=all&card_attachments=true&labels=all&lists=all&list_fields=all&members=all&member_fields=all&checklists=all&checklist_fields=all&organization=false",
                key, token);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                return webClient.DownloadString(url);
            }
        }

        public List<Label> GetLabels()
        {
            var count = 250;
            List<Label> labels;
            var fields = "name";

            var url = String.Format(
                "https://api.trello.com/1/boards/{0}/labels?fields={1}&key={2}&token={3}&limit={4}",
                boardId, fields, key, token, count);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var json = webClient.DownloadString(url);

                labels = JsonConvert.DeserializeObject<List<Label>>(json);

                if (labels.Count == count)
                {
                    throw (new Exception("Too many Trello labels, increase limit or delete labels."));
                }
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrelloToAzure.Trello;
using TrelloToAzure.Trello.DataObjects;

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

            var workspace = new Workspace(trelloKey, trelloToken);

            List<Card> cards = workspace.GetCards(boardId);
            List<Label> labels = workspace.GetLabels(boardId);
            List<List> lists = workspace.GetLists(boardId);

            foreach(Card card in cards)
            {
                var list = GetList(card.IdList, lists);
                var nameLabels = GetLabels(card.IdLabels, labels);

            }
        }

        private static object GetLabels(List<string> idLabels, List<Label> labels)
        {
            var nameLabels = "";

            foreach(string idLabel in idLabels)
            {
                var label = GetNameLabel(idLabel, labels);

                if (nameLabels != "") nameLabels = String.Concat(nameLabels,",");

                nameLabels = String.Concat(nameLabels,label);
            }

            return nameLabels;
        }

        private static string GetNameLabel(string idLabel, List<Label> labels)
        {
            foreach(Label label in labels)
            {
                if (label.Id == idLabel) return label.Name;
            }

            throw (new Exception("No Id Label match."));
        }

        private static object GetList(string idList, List<List> lists)
        {
            foreach(List list in lists)
            {
                if (idList == list.Id) return list;
            }

            return null;
        }
    }
}

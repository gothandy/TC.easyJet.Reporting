using Azure.Tables;
using System;
using System.Collections.Generic;
using TrelloToAzure.Azure;
using TrelloToAzure.Trello;
using TrelloToAzure.Trello.DataObjects;

namespace TrelloToAzure
{
    class Program
    {
        static void Main(string[] args)
        {
            var accountKey = args[0];
            var trelloKey = "3ba00ca224256611c3ccbac183364259";
            var trelloToken = args[1];
            var boardId = "5596a7b7ac88c077383d281c";

            var workspace = new Workspace(trelloKey, trelloToken);

            List<Card> cards = workspace.GetCards(boardId);
            List<Label> labels = workspace.GetLabels(boardId);
            List<List> lists = workspace.GetLists(boardId);

            CardTable table = new CardTable(accountKey);

            foreach (Card card in cards)
            {
                var entity = new CardEntity(boardId, card.Id);

                var listName = List.GetList(card.IdList, lists).Name;
                var nameLabels = GetNameLabels(card.IdLabels, labels);

                entity.Name = GetCardName(card.Name);
                entity.DomId = GetDomIdFromName(card.Name);
                entity.List = listName;
                entity = GetLabels(nameLabels, entity);

                table.BatchInsertOrReplace(entity);
            }

            table.ExecuteBatch();

        }

        private static List<string> GetNameLabels(List<string> idLabels, List<Label> labels)
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

        private static CardEntity GetLabels(List<string> labels, CardEntity entity)
        {

            foreach (string label in labels)
            {
                var epicMatch = "eJ ";
                var invoiceMatch = "Invoice ";
                var reuseMatch = "Reuse ";

                if (label.StartsWith(epicMatch))
                {
                    entity.EpicLabels = LabelConcat(entity.EpicLabels, label.Substring(epicMatch.Length));
                }
                else if (label.StartsWith(invoiceMatch))
                {
                    entity.InvoiceLabels = LabelConcat(entity.InvoiceLabels, label.Substring(invoiceMatch.Length));
                }
                else if (label.StartsWith(reuseMatch))
                {
                    entity.ReuseLabels = LabelConcat(entity.ReuseLabels, label.Substring(reuseMatch.Length));
                }
                else
                {
                    entity.OtherLabels = LabelConcat(entity.OtherLabels, label);
                }
            }

            return entity;
        }

        private static string LabelConcat(string nameLabels, string label)
        {
            if (nameLabels != null) nameLabels = String.Concat(nameLabels, ",");
            return String.Concat(nameLabels, label);
        }

        private static string GetCardName(string fullName)
        {
            var domId = GetDomIdFromName(fullName);

            if (domId == null) return fullName;

            var pos = fullName.IndexOf(domId);

            var name = fullName.Substring(pos + domId.Length + 1);

            name = name.Replace("â€“", "-");

            if (name.StartsWith("- ")) name = name.Substring(2);

            name = name.Trim();

            return name;
        }

        private static string GetDomIdFromName(string taskName)
        {
            string[] words = taskName.Split(' ');

            foreach (string word in words)
            {

                if (word.StartsWith("20") && word.Contains(".") && word.Length > 9)
                {
                    return word;
                }
            }

            return null;
        }


    }
}

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            var tableName = "Cards";

            var workspace = new Workspace(trelloKey, trelloToken);

            List<Card> cards = workspace.GetCards(boardId);
            List<Label> labels = workspace.GetLabels(boardId);
            List<List> lists = workspace.GetLists(boardId);

            CloudTable table;

            table = GetTable(accountKey, tableName);

            TableBatchOperation batchOperation = new TableBatchOperation();

            foreach (Card card in cards)
            {
                var entity = new CardEntity(boardId, card.Id);
                var list = GetList(card.IdList, lists);

                entity.Name = GetCardName(card.Name);
                entity.DomId = GetDomIdFromName(card.Name);
                entity.List = list.Name;
                entity = GetLabels(labels, card, entity);

                batchOperation.InsertOrReplace(entity);

                if (batchOperation.Count == 100)
                {
                    table.ExecuteBatch(batchOperation);
                    batchOperation = new TableBatchOperation();
                }
            }

            table.ExecuteBatch(batchOperation);

        }

        private static string GetLabels(List<string> idLabels, List<Label> labels)
        {
            var nameLabels = "";

            foreach (string idLabel in idLabels)
            {
                var label = GetNameLabel(idLabel, labels);

                nameLabels = LabelConcat(nameLabels, label);
            }

            return nameLabels;
        }

        private static string LabelConcat(string nameLabels, string label)
        {
            if (nameLabels != null) nameLabels = String.Concat(nameLabels, ",");
            return String.Concat(nameLabels, label);
        }

        private static CardEntity GetLabels(List<Label> labels, Card card, CardEntity entity)
        {

            foreach (string idLabel in card.IdLabels)
            {
                var epicMatch = "eJ ";
                var invoiceMatch = "Invoice ";
                var reuseMatch = "Reuse ";

                var label = GetNameLabel(idLabel, labels);

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

        private static CloudTable GetTable(string accountKey, string tableName)
        {
            var connectionString = String.Format(
                "DefaultEndpointsProtocol=https;AccountName=tceasyjetreporting;AccountKey={0}",
                accountKey);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);

            //table.Delete();
            table.CreateIfNotExists();

            return table;
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

        private static string GetNameLabel(string idLabel, List<Label> labels)
        {
            foreach(Label label in labels)
            {
                if (label.Id == idLabel) return label.Name;
            }

            throw (new Exception("No Id Label match."));
        }

        private static List GetList(string idList, List<List> lists)
        {
            foreach(List list in lists)
            {
                if (idList == list.Id) return list;
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace TrelloToAzure.Azure
{
    public class CardEntity : TableEntity
    {

        public string DomId { get; set; }
        public string List { get; set; }
        public string Name { get; set; }
        public string EpicLabels { get; set; }
        public string InvoiceLabels { get; set; }
        public string ReuseLabels { get; set; }
        public string OtherLabels { get; set; }

        public CardEntity(string boardId, string cardId)
        {

            this.PartitionKey = boardId;
            this.RowKey = cardId;
        }

        public CardEntity() { }

        public CardEntity(string boardId, string cardId, string listName, List<string> nameLabels, string cardName) : this(boardId, cardId)
        {

            this.Name = CardEntity.GetCardName(cardName);
            this.DomId = CardEntity.GetDomIdFromName(cardName);
            this.List = listName;
            CardEntity.GetLabels(nameLabels, this);

        }

        public static void GetLabels(List<string> labels, CardEntity entity)
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
        }

        private static string LabelConcat(string nameLabels, string label)
        {
            if (nameLabels != null) nameLabels = String.Concat(nameLabels, ",");
            return String.Concat(nameLabels, label);
        }

        public static string GetCardName(string fullName)
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

        public static string GetDomIdFromName(string taskName)
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

using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace Vincente.Azure.Entities
{
    public class CardEntity : TableEntity
    {

        public string DomId { get; set; }
        public int ListIndex { get; set; }
        public string ListName { get; set; }
        public string Name { get; set; }
        public string Epic { get; set; }
        public DateTime? Invoice { get; set; }

        public CardEntity(string cardId)
        {

            this.PartitionKey = "SingleKey";
            this.RowKey = cardId;
        }

        public CardEntity() { }

        public CardEntity(string cardId, int listIndex, string listName, List<string> nameLabels, string cardName) : this(cardId)
        {
            DomId = Formula.FromName.GetDomID(cardName);
            ListIndex = listIndex;
            ListName = listName;
            Name = Formula.FromName.GetShortName(cardName);
            Epic = Formula.FromLabels.GetEpic(nameLabels);
            Invoice = Formula.FromLabels.GetInvoice(nameLabels, listName);
        }
    }
}

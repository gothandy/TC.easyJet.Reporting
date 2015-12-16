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
        public string Epic { get; set; }
        public DateTime? Invoice { get; set; }

        public CardEntity(string boardId, string cardId)
        {

            this.PartitionKey = boardId;
            this.RowKey = cardId;
        }

        public CardEntity() { }

        public CardEntity(string boardId, string cardId, string listName, List<string> nameLabels, string cardName) : this(boardId, cardId)
        {
            DomId = Formula.FromName.GetDomID(cardName);
            List = listName;
            Name = Formula.FromName.GetShortName(cardName);
            Epic = Formula.FromLabels.GetEpic(nameLabels);
            Invoice = Formula.FromLabels.GetInvoice(nameLabels, listName);
        }
    }
}

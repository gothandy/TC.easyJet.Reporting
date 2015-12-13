using System;
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
        
    }
}

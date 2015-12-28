using Microsoft.WindowsAzure.Storage.Table;
using Vincente.Azure.Converters;
using Vincente.Azure.Entities;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Azure.Tables
{
    public class CardTable : AzureTable<Card, CardEntity>, ICardRead
    {
        public CardTable(CloudTable table) : base(table, new CardConverter()) { }
    }
}

using Gothandy.Tables.Azure.Interfaces;
using System.Linq;
using Vincente.Azure.Entities;
using Vincente.Data.Entities;
using System;
using System.Collections.Generic;

namespace Vincente.Azure.Converters
{
    internal class CardConverter : IConverter<Card, CardEntity>
    {
        public CardEntity Write(Card dataEntity)
        {
            return new CardEntity()
            {
                PartitionKey = "SingleKey",
                RowKey = dataEntity.Id,
                DomId = dataEntity.DomId,
                Epic = dataEntity.Epic,
                Invoice = dataEntity.Invoice,
                ListIndex = dataEntity.ListIndex,
                ListName = dataEntity.ListName,
                Name = dataEntity.Name,
                TaskIds = WriteTaskIds(dataEntity.TaskIds)
            };
        }

        public Card Read(CardEntity azureEntity)
        {
            return new Card()
            {
                Id = azureEntity.RowKey,
                DomId = azureEntity.DomId,
                Epic = azureEntity.Epic,
                Invoice = azureEntity.Invoice,
                ListIndex = azureEntity.ListIndex,
                ListName = azureEntity.ListName,
                Name = azureEntity.Name,
                Timestamp = azureEntity.Timestamp.LocalDateTime,
                TaskIds = ReadTaskIds(azureEntity.TaskIds)
            };
        }

        private string WriteTaskIds(List<long> taskIds)
        {
            if (taskIds == null) return null;

            return string.Join(",", taskIds);
        }

        private List<long> ReadTaskIds(string taskIds)
        {
            if (taskIds == null || taskIds == "") return null;

            return
                (from s in taskIds.Split(',')
                 select long.Parse(s)).ToList();
        }
    }
}

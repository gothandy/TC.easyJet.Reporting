﻿using Vincente.Azure.Entities;
using Vincente.Azure.Interfaces;
using Vincente.Data.Entities;

namespace Vincente.Azure.Converters
{
    public class CardConverter : IConverter<Card, CardEntity>
    {
        public CardEntity AzureFromData(Card dataEntity)
        {
            return new CardEntity(dataEntity.Id)
            {
                DomId = dataEntity.DomId,
                Epic = dataEntity.Epic,
                Invoice = dataEntity.Invoice,
                ListIndex = dataEntity.ListIndex,
                ListName = dataEntity.ListName,
                Name = dataEntity.Name
            };
        }

        public Card DataFromAzure(CardEntity azureEntity)
        {
            return new Card()
            {
                DomId = azureEntity.DomId,
                Epic = azureEntity.Epic,
                Invoice = azureEntity.Invoice,
                ListIndex = azureEntity.ListIndex,
                ListName = azureEntity.ListName,
                Name = azureEntity.Name,
                Timestamp = azureEntity.Timestamp.LocalDateTime
            };
        }
    }
}

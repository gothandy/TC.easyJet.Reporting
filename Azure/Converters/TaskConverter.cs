using Gothandy.Tables.AzureTables.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Vincente.Azure.Entities;
using Vincente.Data.Entities;
using System;

namespace Vincente.Azure.Converters
{
    public class TaskConverter : IConverter<Task, TaskEntity>
    {
        public Task Read(TaskEntity azureEntity)
        {
            return new Task()
            {
                ProjectId = azureEntity.ProjectId,
                Id = long.Parse(azureEntity.RowKey),
                Name = azureEntity.Name,
                ProjectName = azureEntity.ProjectName,
                Active = azureEntity.Active,
                TrackedSeconds = azureEntity.TrackedSeconds,
                CardId = azureEntity.CardId
            };
        }

        public TaskEntity Write(Task dataEntity)
        {
            return new TaskEntity
            {
                PartitionKey = "SingleKey",
                RowKey = dataEntity.Id.ToString(),
                Name = dataEntity.Name,
                ProjectId = dataEntity.ProjectId,
                ProjectName = dataEntity.ProjectName,
                Active = dataEntity.Active,
                TrackedSeconds = dataEntity.TrackedSeconds,
                CardId = dataEntity.CardId
            };
        }
    }
}

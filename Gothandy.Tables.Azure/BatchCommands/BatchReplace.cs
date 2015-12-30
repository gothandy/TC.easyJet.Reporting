﻿using Gothandy.Tables.Azure.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;

namespace Gothandy.Tables.Azure.BatchCommands
{
    public class BatchReplace : IBatchCommand
    {
        public void Execute(TableBatchOperation batchOperation, TableEntity entity)
        {
            entity.ETag = "*"; // Always overwrite (ignore concurrency).
            batchOperation.Replace(entity);
        }
    }
}

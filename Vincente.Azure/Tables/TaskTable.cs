using Gothandy.Tables.AzureTables;
using Microsoft.WindowsAzure.Storage.Table;
using Vincente.Azure.Converters;
using Vincente.Azure.Entities;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Azure.Tables
{
    public class TaskTable : AzureTable<Task, TaskEntity>, ITaskRead, ITaskWrite
    {
        public TaskTable(CloudTable table) : base(table, new TaskConverter()) { }
    }
}

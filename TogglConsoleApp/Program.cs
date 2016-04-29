using Gothandy.StartUp;
using Microsoft.WindowsAzure.Storage;
using System;
using Vincente.Azure.Tables;
using Vincente.Config;
using Vincente.WebJobs.TogglToTime;

namespace TogglConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine("Build {0}", Tools.GetBuildDateTime(typeof(Program)));

            var config = ConfigBuilder.Build();

            #region Dependancies
            var azureStorageAccount = CloudStorageAccount.Parse(config.azureConnectionString);

            var azureTableClient = azureStorageAccount.CreateCloudTableClient();
            var azureTimeEntryTable = azureTableClient.GetTableReference(config.azureTimeEntriesTableName);

            var azureBlobClient = azureStorageAccount.CreateCloudBlobClient();
            var azureBlobContainer = azureBlobClient.GetContainerReference(config.azureBlobContainerName);
            var azureTeamListBlob = azureBlobContainer.GetBlockBlobReference(config.azureTeamListPath);
            var azureTeamTable = new ReplaceTable(azureTeamListBlob);

            var togglWorkspace = new Gothandy.Toggl.Workspace(config.togglApiKey, config.togglWorkspaceId);

            var togglToTime = new TogglToTime(config.togglClientId, azureTimeEntryTable, azureTeamTable, togglWorkspace);
            #endregion

            togglToTime.Execute();
        }
    }
}


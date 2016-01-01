using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.Data.Entities;
using Vincente.Formula;
using Vincente.Toggl.DataObjects;
using Vincente.Toggl.Tables;

namespace Vincente.TogglSync
{
    class Program
    {
        const int togglWorkspaceId = 605632;
        const int togglClientId = 15242883;
        const int toggleProjectTemplateId = 12577727;

        const string trelloKey = "3ba00ca224256611c3ccbac183364259";
        const string trelloBoardId = "5596a7b7ac88c077383d281c";

        static void Main(string[] args)
        {
            var azureTableClient = GetAzureTableClient();
            var trelloWorkspace = GetTrelloWorkspace();
            var togglWorkspace = GetTogglWorkspace();

            SyncTogglProjects.Execute(trelloWorkspace, togglWorkspace);
            UpdateAzureTaskIds.Execute(togglWorkspace, azureTableClient, togglClientId);
        }

        private static CloudTableClient GetAzureTableClient()
        {
            var azureConnectionString = CheckAndGetAppSettings("azureConnectionString");
            var azureStorageAccount = CloudStorageAccount.Parse(azureConnectionString);
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();
            return azureTableClient;
        }

        private static Toggl.Workspace GetTogglWorkspace()
        {
            Toggl.Workspace togglWorkspace;
            var togglApiKey = CheckAndGetAppSettings("togglApiKey");
            togglWorkspace = new Toggl.Workspace(togglApiKey, togglWorkspaceId);
            return togglWorkspace;
        }

        private static Trello.Workspace GetTrelloWorkspace()
        {
            Trello.Workspace trelloWorkspace;
            var trelloToken = CheckAndGetAppSettings("trelloToken");

            trelloWorkspace = new Trello.Workspace(trelloKey, trelloToken, trelloBoardId);
            return trelloWorkspace;
        }

        private static string CheckAndGetAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (value == null) throw new ArgumentNullException(name);
            return value;
        }
    }
}

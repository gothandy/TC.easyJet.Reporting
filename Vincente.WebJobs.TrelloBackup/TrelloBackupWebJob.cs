using Gothandy.Trello;
using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace Vincente.WebJobs.TrelloBackup
{
    public class TrelloBackupWebJob
    {
        private CloudBlockBlob azureTrelloBackupBlob;
        private Workspace trelloWorkspace;

        public TrelloBackupWebJob(Workspace trelloWorkspace, Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob azureTrelloBackupBlob)
        {
            this.trelloWorkspace = trelloWorkspace;
            this.azureTrelloBackupBlob = azureTrelloBackupBlob;
        }

        public void Execute()
        {
            var json = trelloWorkspace.GetBackupJson();
            azureTrelloBackupBlob.UploadText(json);
        }
    }
}

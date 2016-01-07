using Gothandy.Tables.AzureBlobs;
using Microsoft.WindowsAzure.Storage.Blob;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Azure.Tables
{
    public class TeamTable : AzureBlob<Team>, ITeamRead
    {
        public TeamTable(CloudBlockBlob blob) : base(blob) { }
    }
}

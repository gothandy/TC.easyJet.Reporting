using Gothandy.Tables.AzureBlobs;
using Microsoft.WindowsAzure.Storage.Blob;
using Vincente.Data.Entities;

namespace Vincente.Azure.Tables
{
    public class ReplaceTable : AzureBlob<Team>
    {
        public ReplaceTable(CloudBlockBlob blob) : base(blob) { }
    }
}

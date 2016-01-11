using Gothandy.Tables.AzureBlobs;
using Microsoft.WindowsAzure.Storage.Blob;
using Vincente.Data.Entities;

namespace Vincente.Azure.Tables
{
    public class ListNameTable : AzureBlob<Replace>
    {
        public ListNameTable(CloudBlockBlob blob) : base(blob) { }
    }
}

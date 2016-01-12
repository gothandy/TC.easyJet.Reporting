using Gothandy.Tables.Interfaces;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Gothandy.Tables.AzureBlobs
{
    public abstract class AzureBlob <T> : ITableRead<T>
    {
        private CloudBlockBlob blob;

        public AzureBlob(CloudBlockBlob blob)
        {
            this.blob = blob;
        }

        public IEnumerable<T> Query()
        {
            if (!blob.Exists()) return EmptyTable();

            var json = GetJson(blob);

            return JsonConvert.DeserializeObject<List<T>>(json) as IEnumerable<T>;
        }

        private IEnumerable<T> EmptyTable()
        {
            return new List<T>() as IEnumerable<T>;
        }

        private string GetJson(CloudBlockBlob blob)
        {
            string json;

            using (var memoryStream = new MemoryStream())
            {
                blob.DownloadToStream(memoryStream);
                json = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            return json;
        }
    }
}

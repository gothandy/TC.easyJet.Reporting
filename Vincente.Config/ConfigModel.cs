using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vincente.Config
{
    public class ConfigModel
    {
        public int togglWorkspaceId { get; set; }
        public int togglClientId { get; set; }
        public string azureTimeEntriesTableName { get; set; }
        public string azureBlobContainerName { get; set; }
        public string azureTeamListPath { get; set; }
        public string azureConnectionString { get; set; }
        public string togglApiKey { get; set; }
    }
}

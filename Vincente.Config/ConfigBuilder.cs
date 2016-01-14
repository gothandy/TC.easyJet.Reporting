using Gothandy.StartUp;

namespace Vincente.Config
{
    public class ConfigBuilder
    {
        public static ConfigModel Create()
        {
            return new ConfigModel()
            {
                azureTimeEntriesTableName = "TimeEntries",
                azureBlobContainerName = "vincente",
                azureTeamListPath = "TeamList.json",
                azureConnectionString = Tools.CheckAndGetAppSettings("azureConnectionString"),
                togglApiKey = Tools.CheckAndGetAppSettings("togglApiKey")
            };
        }
    }
}

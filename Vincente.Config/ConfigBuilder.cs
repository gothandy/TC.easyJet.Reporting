using Gothandy.StartUp;

namespace Vincente.Config
{
    public class ConfigBuilder
    {
        public static ConfigModel Build()
        {
            return new ConfigModel()
            {
                azureTimeEntriesTableName = "TimeEntries",
                azureBlobContainerName = "vincente",
                azureTeamListPath = "TeamList.json",
                azureConnectionString = Tools.CheckAndGetAppSettings("azureConnectionString"),
                togglApiKey = Tools.CheckAndGetAppSettings("togglApiKey"),
                azureReplacePath = "Replaces.json",
                trelloToken = Tools.CheckAndGetAppSettings("trelloToken"),
                trelloKey = "3ba00ca224256611c3ccbac183364259",
                trelloBoardId = "5596a7b7ac88c077383d281c"
        };
        }
    }
}

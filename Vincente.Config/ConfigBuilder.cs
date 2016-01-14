using Gothandy.StartUp;

namespace Vincente.Config
{
    public class ConfigBuilder
    {
        public static ConfigModel Build()
        {
            return new ConfigModel()
            {
                azureBlobContainerName = "vincente",
                azureConnectionString = Tools.CheckAndGetAppSettings("azureConnectionString"),
                azureReplacePath = "Replaces.json",
                azureTeamListPath = "TeamList.json",
                azureTimeEntriesTableName = "TimeEntries",
                azureCardsTableName = "Cards",
                azureTasksTableName = "Tasks",

                togglApiKey = Tools.CheckAndGetAppSettings("togglApiKey"),
                togglClientId = 15242883,
                togglProjectTemplateId = 12577727,
                togglWorkspaceId = 605632,

                trelloBoardId = "5596a7b7ac88c077383d281c",
                trelloKey = "3ba00ca224256611c3ccbac183364259",
                trelloToken = Tools.CheckAndGetAppSettings("trelloToken")
            };
        }
    }
}

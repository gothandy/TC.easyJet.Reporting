namespace Vincente.Config
{
    public class ConfigModel
    {
        public string trelloToken { get; internal set; }
        public int togglWorkspaceId { get; internal set; }
        public int togglClientId { get; internal set; }
        public string azureTimeEntriesTableName { get; internal set; }
        public string azureBlobContainerName { get; internal set; }
        public string azureTeamListPath { get; internal set; }
        public string azureConnectionString { get; internal set; }
        public string togglApiKey { get; internal set; }
        public string azureReplacePath { get; internal set; }
        public string trelloKey { get; internal set; }
        public string trelloBoardId { get; internal set; }
        public int togglProjectTemplateId { get; internal set; }
        public string azureCardsTableName { get; internal set; }
        public string azureTasksTableName { get; internal set; }
    }
}

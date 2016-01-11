namespace Gothandy.Toggl
{
    public class Workspace
    {
        public string ApiKey { get; set; }
        public int WorkspaceId { get; set; }

        public Workspace(string apiKey, int workspaceId)
        {
            this.ApiKey = apiKey;
            this.WorkspaceId = workspaceId;
        }
    }
}
using Gothandy.Toggl.Tables;
using Vincente.Azure.Tables;

namespace Vincente.WebJobs.TogglToTask
{
    public class TogglToTaskData
    {
        public int togglClientId { get; set; }
            public int togglProjectTemplateId { get; set; }
            public Azure.Tables.TaskTable azureTaskTable { get; set; }
            public CardTable azureCardTable { get; set; }
            public Gothandy.Trello.Workspace trelloWorkspace { get; set; }
            public Gothandy.Toggl.Tables.TaskTable togglTaskTable { get; set; }
            public ProjectTable togglProjectTable { get; set; }
    }
}

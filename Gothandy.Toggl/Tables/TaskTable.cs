using System.Collections.Generic;
using Vincente.Toggl.DataObjects;

namespace Vincente.Toggl.Tables
{
    public class TaskTable : BaseTable
    {
        public TaskTable(Workspace workspace) : base(workspace) { }

        public List<Task> GetTasks(int projectId)
        {
            var url = string.Format(
                "https://www.toggl.com/api/v8/projects/{0}/tasks?active=both",
                projectId);

            return Get<List<Task>>(url, true);
        }
    }
}

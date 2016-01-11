using System;
using System.Collections.Generic;
using Gothandy.Toggl.DataObjects;

namespace Gothandy.Toggl.Tables
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

        public Task Create(Task task)
        {
            var url = "https://www.toggl.com/api/v8/tasks";

            var request = new TaskWrapper() { Task = task };
            var response = Post<DataWrapper<Task>>(url, request);

            return response.Data;
        }

        public object Delete(long id)
        {
            var url = string.Format("https://www.toggl.com/api/v8/tasks/{0}", id);

            var response = Delete<List<int>>(url);

            return response[0];
        }
    }
}

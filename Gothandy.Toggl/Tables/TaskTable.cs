using System;
using System.Collections.Generic;
using Gothandy.Toggl.DataObjects;

namespace Gothandy.Toggl.Tables
{
    public class TaskTable : BaseTable
    {
        private const string apiRoot = "https://www.toggl.com/api/v8";

        public TaskTable(Workspace workspace) : base(workspace) { }

        public List<Task> GetTasks(int projectId)
        {
            var url = string.Format("/projects/{0}/tasks?active=both", projectId);

            return workspace.Client.Get<List<Task>>(url);
        }

        public Task Create(Task task)
        {
            var url = "/tasks";

            var request = new TaskWrapper() { Task = task };
            var response = workspace.Client.Post<DataWrapper<Task>>(url, request);

            return response.Data;
        }

        public Task Update(Task task)
        {
            var url = string.Format("/tasks/{0}", task.Id);
            var request = new TaskWrapper() { Task = task };
            var response = workspace.Client.Put<DataWrapper<Task>>(url, request);

            return response.Data;
        }

        public object Delete(long id)
        {
            var url = string.Format("/tasks/{0}", id);

            var response = workspace.Client.Delete<List<int>>(url);

            return response[0];
        }

        public Task GetTask(long id)
        {
            var url = string.Format("/tasks/{0}", id);
            var response = workspace.Client.Get<DataWrapper<Task>>(url);

            return response.Data;
        }
    }
}

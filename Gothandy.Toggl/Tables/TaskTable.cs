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
            var url = string.Format(
                "{0}/projects/{1}/tasks?active=both",
                apiRoot, projectId);

            return Get<List<Task>>(url, true);
        }

        public Task Create(Task task)
        {
            var url = string.Format("{0}/tasks", apiRoot);

            var request = new TaskWrapper() { Task = task };
            var response = Post<DataWrapper<Task>>(url, request);

            return response.Data;
        }

        public Task Update(Task task)
        {
            var url = string.Format("{0}/tasks/{1}", apiRoot, task.Id);
            var request = new TaskWrapper() { Task = task };
            var response = Put<DataWrapper<Task>>(url, request);

            return response.Data;
        }

        public object Delete(long id)
        {
            var url = string.Format("{0}/tasks/{1}", apiRoot, id);

            var response = Delete<List<int>>(url);

            return response[0];
        }

        public Task GetTask(long id)
        {
            var url = string.Format("{0}/tasks/{1}", apiRoot, id);

            var response = Get<DataWrapper<Task>>(url, true);

            return response.Data;
        }
    }
}

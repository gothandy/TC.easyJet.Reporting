using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gothandy.Toggl.DataObjects;
using Gothandy.StartUp;
using Gothandy.Toggl.Tables;
using System.Diagnostics;

namespace Gothandy.Toggl.Test
{
    [TestClass]
    public class TogglTask
    {
        const int togglWorkspaceId = 605632;
        const int togglClientId = 18068101;
        const int togglProjectA = 13119308;
        const int togglProjectB = 13119310;
        const int togglTaskA = 8083834;

        private Workspace workspace;
        private TaskTable taskTable;

        public TogglTask()
        {
            var togglApiKey = Tools.CheckAndGetAppSettings("togglApiKey");

            workspace = new Workspace(togglApiKey, togglWorkspaceId);
            taskTable = new TaskTable(workspace);
        }

        [TestMethod]
        public void TogglTaskGet()
        {
            var task = GetTask();
        }

        [TestMethod]
        public void TogglTaskCreate()
        {
            var task = CreateTask();

            DeleteTask(task);
        }

        [TestMethod]
        public void TogglTaskUpdateName()
        {
            var task = CreateTask();

            try
            {
                task.Name = "Gothandy.Toggl.Test.Task.Updated";
                var response = UpdateTask(task);
                Assert.AreEqual(task.Name, response.Name);
            }
            finally
            {
                DeleteTask(task);
            }
        }

        [TestMethod]
        public void TogglTaskUpdateActive()
        {
            var task = CreateTask();

            try
            {
                task.Active = false;
                var response = UpdateTask(task);
                Assert.AreEqual(task.Active, response.Active);
            }
            finally
            {
                DeleteTask(task);
            }
        }

        [TestMethod]
        public void TogglTaskUpdateProject()
        {
            var task = CreateTask();

            try
            {
                task.ProjectId = togglProjectB;
                var response = UpdateTask(task);
                Assert.AreEqual(task.ProjectId, response.ProjectId);
            }
            finally
            {
                DeleteTask(task);
            }
        }

        [TestMethod]
        public void TogglTaskUpdateProjectWithTime()
        {
            var task = GetTask();

            if (task.ProjectId == togglProjectA)
            {
                UpdateProject(task, togglProjectB);
            }
            else
            {
                UpdateProject(task, togglProjectA);
            }
        }

        private void UpdateProject(Task task, long id)
        {
            task.ProjectId = id;
            var response = UpdateTask(task);
            Assert.AreEqual(task.ProjectId, response.ProjectId);
        }

        private Task GetTask()
        {
            var response = taskTable.GetTask(togglTaskA);
            Trace.WriteLine(string.Format("Task {0} read.", response.Id.Value));

            return response;
        }

        private Task CreateTask()
        {
            Task task = new Task()
            {
                WorkspaceId = togglWorkspaceId,
                ProjectId = togglProjectA,
                Name = "Gothandy.Toggl.Test.Task.Created",
            };

            var response = taskTable.Create(task);
            Trace.WriteLine(string.Format("Task {0} created.", response.Id.Value));

            return response;
        }

        private Task UpdateTask(Task task)
        {
            var response = taskTable.Update(task);
            Trace.WriteLine(string.Format("Task {0} updated.", response.Id.Value));

            return response;
        }

        private void DeleteTask(Task createResponse)
        {
            var id = taskTable.Delete(createResponse.Id.Value);
            Trace.WriteLine(string.Format("Task {0} deleted.", id));
        }
    }
}

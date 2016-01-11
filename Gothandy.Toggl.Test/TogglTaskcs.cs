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
        const int togglClientId = 15242883;
        const int toggleProjectTemplateId = 12577727;

        private Workspace workspace;
        private TaskTable taskTable;

        public TogglTask()
        {
            var togglApiKey = Tools.CheckAndGetAppSettings("togglApiKey");

            workspace = new Workspace(togglApiKey, togglWorkspaceId);
            taskTable = new TaskTable(workspace);
        }

        [TestMethod]
        public void TogglTaskCreate()
        {
            DataObjects.Task task = new DataObjects.Task()
            {
                WorkspaceId = togglWorkspaceId,
                ProjectId = toggleProjectTemplateId,
                Name = "Gothandy.Toggl.Test.Task",
            };

            var response = taskTable.Create(task);

            Trace.WriteLine(string.Format("Task {0} created.", response.Id.Value));

            var id = taskTable.Delete(response.Id.Value);

            Trace.WriteLine(string.Format("Task {0} deleted.", id));
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gothandy.Toggl.DataObjects;
using Gothandy.StartUp;
using Gothandy.Toggl.Tables;
using System.Diagnostics;

namespace Gothandy.Toggl.Test
{
    [TestClass]
    public class TogglProject
    {
        const int togglWorkspaceId = 605632;
        const int togglClientId = 15242883;
        const int toggleProjectTemplateId = 12577727;

        private Workspace workspace;
        private ProjectTable projectTable;

        public TogglProject()
        {
            var togglApiKey = Tools.CheckAndGetAppSettings("togglApiKey");

            workspace = new Workspace(togglApiKey, togglWorkspaceId);
            projectTable = new ProjectTable(workspace);
        }

        [TestMethod]
        public void TogglProjectGet()
        {
            projectTable.GetProject(toggleProjectTemplateId);
        }

        [TestMethod]
        public void TogglProjectCreate()
        {
            DataObjects.Project project = new DataObjects.Project()
            {
                WorkspaceId = togglWorkspaceId,
                ClientId = togglClientId,
                Name = "Gothandy.Toggl.Test.Project",
                IsPrivate = true,
                Color = 5,
                Rate = 10.00
            };

            var response = projectTable.Create(project);

            Trace.WriteLine(string.Format("Project {0} created.", response.Id.Value));

            var id = projectTable.Delete(response.Id.Value);

            Trace.WriteLine(string.Format("Project {0} deleted.", id));
        }

        [TestMethod]
        public void TogglCreateFromProjectTemplate()
        {
            // Copies all the tasks without time into new project.
            DataObjects.Project project = new DataObjects.Project()
            {
                WorkspaceId = togglWorkspaceId,
                ClientId = togglClientId,
                Name = "Gothandy.Toggl.Test.Project",
                TemplateId = toggleProjectTemplateId
            };

            var response = projectTable.Create(project);
            Trace.WriteLine(string.Format("Project {0} created.", response.Id.Value));

            var id = projectTable.Delete(response.Id.Value);
            Trace.WriteLine(string.Format("Project {0} deleted.", id));
        }
    }
}

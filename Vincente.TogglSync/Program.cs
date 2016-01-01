using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Vincente.Formula;
using Vincente.Toggl.DataObjects;
using Vincente.Toggl.Tables;

namespace Vincente.TogglSync
{
    class Program
    {
        const int togglWorkspaceId = 605632;
        const int togglClientId = 15242883;
        const int toggleProjectTemplateId = 12577727;

        const string trelloKey = "3ba00ca224256611c3ccbac183364259";
        const string trelloBoardId = "5596a7b7ac88c077383d281c";

        static void Main(string[] args)
        {
            Trello.Workspace trelloWorkspace = GetTrelloWorkspace();
            Toggl.Workspace togglWorkspace = GetTogglWorkspace();

            SyncTogglProjects(trelloWorkspace, togglWorkspace);

            SyncTogglTasks(trelloWorkspace, togglWorkspace);

        }

        private static void SyncTogglTasks(Trello.Workspace trelloWorkspace, Toggl.Workspace togglWorkspace)
        {
            var togglProjectTable = new ProjectTable(togglWorkspace);
            var togglTaskTable = new TaskTable(togglWorkspace);
            var togglProjects = togglProjectTable.GetProjects(togglClientId);
            var togglTasks = new List<Task>();

            foreach(Project project in togglProjects)
            {
                var tasks = togglTaskTable.GetTasks((int)project.Id);

                if (tasks != null) togglTasks.AddRange(tasks);
            }

            Console.Out.WriteLine("{0} Tasks.", togglTasks.Count);
        }

        private static void SyncTogglProjects(Trello.Workspace trelloWorkspace, Toggl.Workspace togglWorkspace)
        {
            var trelloLabels =
                (from l in trelloWorkspace.GetLabels()
                 select l.Name).ToList();

            var trelloEpics = FromLabels.GetEpics(trelloLabels);

            var togglProjectTable = new ProjectTable(togglWorkspace);

            var togglProjects =
                (from p in togglProjectTable.GetProjects(togglClientId)
                 select p.Name).ToList();

            Project togglTemplate = togglProjectTable.GetProject(toggleProjectTemplateId);

            var createdCount = 0;

            foreach (string epic in trelloEpics)
            {
                if (!togglProjects.Contains(epic))
                {
                    Project project = new Project()
                    {
                        WorkspaceId = togglWorkspaceId,
                        ClientId = togglClientId,
                        Name = epic,
                        IsPrivate = togglTemplate.IsPrivate,
                        Color = togglTemplate.Color,
                        Rate = togglTemplate.Rate
                    };

                    ProjectTable projectTable = new ProjectTable(togglWorkspace);

                    projectTable.Create(project);

                    Console.Out.WriteLine("\"{0}\" toggl Project created.", project.Name);

                    createdCount++;
                }
            }

            if (createdCount == 0) Console.Out.WriteLine("All toggl projects in sync.");
        }

        private static Toggl.Workspace GetTogglWorkspace()
        {
            Toggl.Workspace togglWorkspace;
            var togglApiKey = CheckAndGetAppSettings("togglApiKey");
            togglWorkspace = new Toggl.Workspace(togglApiKey, togglWorkspaceId);
            return togglWorkspace;
        }

        private static Trello.Workspace GetTrelloWorkspace()
        {
            Trello.Workspace trelloWorkspace;
            var trelloToken = CheckAndGetAppSettings("trelloToken");

            trelloWorkspace = new Trello.Workspace(trelloKey, trelloToken, trelloBoardId);
            return trelloWorkspace;
        }

        private static string CheckAndGetAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (value == null) throw new ArgumentNullException(name);
            return value;
        }
    }
}

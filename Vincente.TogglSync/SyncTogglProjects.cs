using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.Data.Entities;
using Vincente.Formula;
using Vincente.Toggl.DataObjects;
using Vincente.Toggl.Tables;

namespace Vincente.TogglSync
{
    public static class SyncTogglProjects
    {
        const int togglWorkspaceId = 605632;
        const int togglClientId = 15242883;
        const int toggleProjectTemplateId = 12577727;

        const string trelloKey = "3ba00ca224256611c3ccbac183364259";
        const string trelloBoardId = "5596a7b7ac88c077383d281c";

        public static void Execute(Trello.Workspace trelloWorkspace, Toggl.Workspace togglWorkspace)
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
    }
}

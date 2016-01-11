using System.Collections.Generic;
using System.Linq;
using Vincente.Formula;
using Gothandy.Toggl.DataObjects;
using Gothandy.Toggl.Tables;
using Gothandy.Trello.DataObjects;

namespace Vincente.TogglSync.Operations
{
    public static class SyncTogglProjects
    {
        public static List<string> Execute(ProjectTable togglProjectTable, List<Project> togglProjects, Project togglTemplate, List<Label> trelloLabels)
        {
            var togglProjectNames = (from p in togglProjects select p.Name).ToList();
            var trelloLabelNames = (from l in trelloLabels select l.Name).ToList();
            var trelloEpics = FromLabels.GetEpics(trelloLabelNames);
            var createdProjects = new List<string>();

            foreach (string epic in trelloEpics)
            {
                if (!togglProjectNames.Contains(epic))
                {
                    Project project = new Project()
                    {
                        WorkspaceId = togglTemplate.WorkspaceId,
                        ClientId = togglTemplate.ClientId,
                        Name = epic,
                        IsPrivate = togglTemplate.IsPrivate,
                        Color = togglTemplate.Color,
                        Rate = togglTemplate.Rate
                    };

                    togglProjectTable.Create(project);

                    createdProjects.Add(project.Name);
                }
            }

            return createdProjects;
        }
    }
}

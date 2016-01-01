using System.Collections.Generic;
using Vincente.Toggl.DataObjects;

namespace Vincente.Toggl.Tables
{
    public class ProjectTable : BaseTable
    {
        public ProjectTable (Workspace workspace) : base (workspace) { }

        public Project Create(Project project)
        {
            var url = "https://www.toggl.com/api/v8/projects";

            ProjectWrapper request = new ProjectWrapper() { Project = project };
            ProjectWrapper response = Post<ProjectWrapper>(url, request);

            return response.Data;
        }

        public Project GetProject(int toggleProjectTemplateId)
        {
            var url = string.Format(
                "https://www.toggl.com/api/v8/projects/{0}",
                toggleProjectTemplateId);

            return Get<ProjectWrapper>(url, true).Data;
        }

        public List<Project> GetProjects(int clientId)
        {
            var url = string.Format(
                "https://www.toggl.com/api/v8/clients/{0}/projects?active=both",
                clientId);

            return Get<List<Project>>(url, true);
        }
    }
}

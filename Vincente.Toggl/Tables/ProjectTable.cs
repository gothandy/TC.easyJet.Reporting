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
    }
}

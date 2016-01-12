using System;
using System.Collections.Generic;
using Gothandy.Toggl.DataObjects;

namespace Gothandy.Toggl.Tables
{
    public class ProjectTable : BaseTable
    {
        public ProjectTable (Workspace workspace) : base (workspace) { }

        public Project Create(Project project)
        {
            var url = "/projects";

            var request = new ProjectWrapper() { Project = project };
            var response = workspace.Client.Post<DataWrapper<Project>>(url, request);

            return response.Data;
        }

        public long Delete(long id)
        {
            var url = string.Format("/projects/{0}", id);

            var response = workspace.Client.Delete<List<int>>(url);

            return response[0];
        }

        public Project GetProject(int toggleProjectTemplateId)
        {
            var url = string.Format("/projects/{0}", toggleProjectTemplateId);

            return workspace.Client.Get<DataWrapper<Project>>(url).Data;
        }

        public List<Project> GetProjects(int clientId)
        {
            var url = string.Format("/clients/{0}/projects?active=both", clientId);

            return workspace.Client.Get<List<Project>>(url);
        }
    }
}

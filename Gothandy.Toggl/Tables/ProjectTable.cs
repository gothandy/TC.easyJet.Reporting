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
            var url = "https://www.toggl.com/api/v8/projects";

            var request = new ProjectWrapper() { Project = project };
            var response = Post<DataWrapper<Project>>(url, request);

            return response.Data;
        }
        public long Delete(long id)
        {
            var url = string.Format("https://www.toggl.com/api/v8/projects/{0}", id);

            var response = Delete<List<int>>(url);

            return response[0];
        }


        public Project GetProject(int toggleProjectTemplateId)
        {
            var url = string.Format(
                "https://www.toggl.com/api/v8/projects/{0}",
                toggleProjectTemplateId);

            return Get<DataWrapper<Project>>(url, true).Data;
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

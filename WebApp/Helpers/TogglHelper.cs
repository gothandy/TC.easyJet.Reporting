using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vincente.Config;

namespace Vincente.WebApp.Helpers
{
    public class TogglHelper
    {

        public static HtmlString SummaryReport(DateTime from, string text)
        {
            var config = ConfigBuilder.Build();
            var to = from.AddMonths(1).AddDays(-1);

            return new HtmlString(string.Format(
                "<a target=\"blank\" href=\"{0}/clients/{1}/billable/both\">{2}</a>",
                BaseUrl(config.togglWorkspaceId, "summary", from, to), config.togglClientId, text));
        }
        
        public static HtmlString SummaryReport(List<long> taskIds)
        {
            var config = ConfigBuilder.Build();
            if (taskIds == null) return null;

            return new HtmlString(string.Format(
                "<a target=\"blank\" href=\"{0}/clients/{1}/tasks/{2}/billable/both\">{0}</a>",
                BaseUrlYear(config.togglWorkspaceId), config.togglClientId, string.Join(",", taskIds)));
        }

        public static HtmlString DetailedReport(long? taskId, string text)
        {
            var config = ConfigBuilder.Build();

            if (!taskId.HasValue) return new HtmlString(text);

            return new HtmlString(string.Format(
                "<a target=\"blank\" href=\"{0}/clients/{1}/tasks/{2}/billable/both\">{3}</a>",
                BaseUrlYear(config.togglWorkspaceId), config.togglClientId, taskId, text));
        }

        public static HtmlString EditProject(long projectId, string text)
        {
            var config = ConfigBuilder.Build();

            return new HtmlString(string.Format(
                "<a target=\"blank\" href=\"https://www.toggl.com/app/projects/{0}/edit/{1}\">{2}</a>",
                config.togglWorkspaceId, projectId, text));
        }

        // /users/960313/clients/15242883/tasks/0/billable/both
        public static HtmlString DetailedUserNoTask(long? userId, string text)
        {
            var config = ConfigBuilder.Build();
            if (!userId.HasValue) return new HtmlString(text);

            return new HtmlString(string.Format(
                "<a target=\"blank\" href=\"{0}/users/{1}/clients/{2}/tasks/0/billable/both\">{3}</a>",
                BaseUrlYear(config.togglWorkspaceId), userId, config.togglClientId, text));
        }

        //www.toggl.com/app/reports/detailed/605632/from/2015-12-17/to/2016-01-21
        private static string BaseUrlYear(int togglWorkspaceId)
        {
            return BaseUrl(togglWorkspaceId, "detailed", DateTime.Now.AddYears(-1), DateTime.Now);
        }

        private static string BaseUrl(int togglWorkspaceId, string type, DateTime from, DateTime to)
        {
            return string.Format(
                "https://www.toggl.com/app/reports/{0}/{1}/from/{2:yyyy-MM-dd}/to/{3:yyyy-MM-dd}",
                type, togglWorkspaceId, from, to);
        }
    }
}
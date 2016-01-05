using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincente.WebApp.Helpers
{
    public class TogglHelper
    {
        private const long togglWorkspaceId = 605632;
        private const long togglClientId = 15242883;

        public static HtmlString SummaryReport(List<long> taskIds)
        {
            if (taskIds == null) return null;

            return new HtmlString(string.Format(
                "<a target=\"blank\" href=\"https://www.toggl.com/app/reports/summary/605632/period/prevYear/clients/15242883/tasks/{0}/billable/both\">{0}</a>",
                string.Join(",", taskIds)));
        }

        public static HtmlString DetailedReport(long? taskId, string text)
        {
            if (!taskId.HasValue) return new HtmlString(text);

            return new HtmlString(string.Format(
                "<a target=\"blank\" href=\"https://www.toggl.com/app/reports/detailed/605632/period/prevYear/clients/15242883/tasks/{0}/billable/both\">{1}</a>",
                taskId, text));
        }

        public static HtmlString EditProject(long projectId, string text)
        {
            return new HtmlString(string.Format(
                "<a target=\"blank\" href=\"https://www.toggl.com/app/projects/605632/edit/{0}\">{1}</a>",
                projectId, text));
        }

        // /users/960313/clients/15242883/tasks/0/billable/both
        public static HtmlString DetailedUserNoTask(long? userId, string text)
        {
            if (!userId.HasValue) return new HtmlString(text);

            return new HtmlString(string.Format(
                "<a target=\"blank\" href=\"{0}/users/{1}/clients/{2}/tasks/0/billable/both\">{3}</a>",
                BaseUrl(), userId, togglClientId, text));
        }

        //www.toggl.com/app/reports/detailed/605632/from/2015-12-17/to/2016-01-21
        private static string BaseUrl()
        {
            return string.Format(
                "https://www.toggl.com/app/reports/detailed/{0}/from/{1:yyyy-MM-dd}/to/{2:yyyy-MM-dd}",
                togglWorkspaceId,
                DateTime.Now.AddYears(-1),
                DateTime.Now);
        }
    }
}
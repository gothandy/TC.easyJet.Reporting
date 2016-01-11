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

        //https://www.toggl.com/app/reports/summary/605632/period/prevMonth /clients/15242883/billable/both

        public static HtmlString SummaryReport(DateTime from, string text)
        {
            var to = from.AddMonths(1).AddDays(-1);

            return new HtmlString(string.Format(
                "<a target=\"blank\" href=\"{0}/clients/{1}/billable/both\">{2}</a>",
                BaseUrl("summary", from, to), togglClientId, text));
        }
        
        public static HtmlString SummaryReport(List<long> taskIds)
        {
            if (taskIds == null) return null;

            return new HtmlString(string.Format(
                "<a target=\"blank\" href=\"{0}/clients/{1}/tasks/{2}/billable/both\">{0}</a>",
                BaseUrlYear(), togglClientId, string.Join(",", taskIds)));
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
                BaseUrlYear(), userId, togglClientId, text));
        }

        //www.toggl.com/app/reports/detailed/605632/from/2015-12-17/to/2016-01-21
        private static string BaseUrlYear()
        {
            return BaseUrl("detailed", DateTime.Now.AddYears(-1), DateTime.Now);
        }

        private static string BaseUrl(string type, DateTime from, DateTime to)
        {
            return string.Format(
                "https://www.toggl.com/app/reports/{0}/{1}/from/{2:yyyy-MM-dd}/to/{3:yyyy-MM-dd}",
                type, togglWorkspaceId, from, to);
        }
    }
}
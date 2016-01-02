using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vincente.WebApp.Helpers
{
    public class TogglHelper
    {
        public static HtmlString TaskIds(List<long> taskIds)
        {
            if (taskIds == null) return null;

            return new HtmlString(string.Format(
                "<a target=\"blank\" href=\"https://www.toggl.com/app/reports/summary/605632/period/prevYear/clients/15242883/tasks/{0}/billable/both\">{0}</a>",
                string.Join(",", taskIds)));
        }
    }
}
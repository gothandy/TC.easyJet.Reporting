using System;
using System.Collections.Generic;
using Gothandy.Toggl.DataObjects;

namespace Gothandy.Toggl.Tables
{
    public class TimeEntryTable : BaseTable
    {
        public TimeEntryTable(Workspace workspace) : base (workspace) { }

        public List<ReportTimeEntry> GetReportTimeEntries(int clientId, DateTime since, DateTime until)
        {
            List<ReportTimeEntry> list = new List<ReportTimeEntry>();

            var page = 1;
            while (true)
            {
                var detailedReport = this.DetailedReport(clientId, since, until, page);

                list.AddRange(detailedReport.Data);

                if (detailedReport.LastPage) break;

                page++;

                System.Threading.Thread.Sleep(1000);
            }

            return list;
        }

        private DetailedReport DetailedReport(int clientId, DateTime since, DateTime until, int page)
        {
            var url = String.Format(
                "{0}?user_agent={1}&workspace_id={2}&client_ids={3}&since={4:yyyy-MM-dd}&until={5:yyyy-MM-dd}&page={6}&order_field=date&rounding=off",
                "https://toggl.com/reports/api/v2/details",
                "andy@tcuk.com",
                workspace.WorkspaceId, clientId, since, until, page);

            var detailedReport = Get<DetailedReport>(url, false);

            detailedReport.LastPage = (detailedReport.PerPage * page) > detailedReport.TotalCount;

            return detailedReport;
        }
    }
}

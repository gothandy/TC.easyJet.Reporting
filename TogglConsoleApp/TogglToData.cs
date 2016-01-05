using System;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;
using Vincente.Formula;
using Vincente.Toggl.DataObjects;

namespace TogglConsoleApp
{
    public class TogglToData
    {
        public static List<TimeEntry> Execute(ITimeEntryWrite timeEntryTable, List<ReportTimeEntry> togglTimeEntries)
        {
            return
                (from togglTimeEntry in togglTimeEntries
                select GetTimeEntry(togglTimeEntry)).ToList();
        }

        public static TimeEntry GetTimeEntry(ReportTimeEntry togglTimeEntry)
        {
            if (togglTimeEntry.Start == null) throw new ArgumentNullException("Start");
            if (togglTimeEntry.Id == null) throw new ArgumentException("Id");
            if (togglTimeEntry.Billable == null) throw new ArgumentException("Billable");

            return new TimeEntry()
            {
                Id = togglTimeEntry.Id.GetValueOrDefault(),
                Start = togglTimeEntry.Start.GetValueOrDefault(),
                TaskId = togglTimeEntry.TaskId,
                UserId = togglTimeEntry.UserId,
                UserName = togglTimeEntry.UserName,
                Billable = togglTimeEntry.Billable.GetValueOrDefault(),
                DomId = FromName.GetDomID(togglTimeEntry.TaskName),
                Housekeeping = FromProject.IfHouseKeepingReturnTaskName(togglTimeEntry.ProjectId, togglTimeEntry.TaskName),
                Description = togglTimeEntry.Description
            };
        }

        private static DateTime MonthFromStart(DateTime? start)
        {
            return new DateTime(start.GetValueOrDefault().Year, start.GetValueOrDefault().Month, 1);
        }
    }
}

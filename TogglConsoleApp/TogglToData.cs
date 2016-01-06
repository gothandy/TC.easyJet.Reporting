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
        public static List<TimeEntry> Execute(ITimeEntryWrite timeEntryTable, List<ReportTimeEntry> togglTimeEntries, List<Team> teams)
        {
            return
                (from togglTimeEntry in togglTimeEntries
                select GetTimeEntry(togglTimeEntry, teams)).ToList();
        }

        public static TimeEntry GetTimeEntry(ReportTimeEntry togglTimeEntry, List<Team> teams)
        {
            if (togglTimeEntry.Start == null) throw new ArgumentNullException("Start");
            if (togglTimeEntry.Id == null) throw new ArgumentException("Id");
            if (togglTimeEntry.Billable == null) throw new ArgumentException("Billable");

            var selectTeam =
                (from t in teams
                 where t.UserName == togglTimeEntry.UserName
                 select t.TeamName).ToList();

            string team = null;

            if (selectTeam.Count == 1) team = selectTeam.First();

            var newTimeEntry = new TimeEntry()
            {
                Id = togglTimeEntry.Id.GetValueOrDefault(),
                Start = togglTimeEntry.Start.GetValueOrDefault(),
                TaskId = togglTimeEntry.TaskId,
                UserId = togglTimeEntry.UserId,
                UserName = togglTimeEntry.UserName,
                TeamName = team,
                Billable = togglTimeEntry.Billable.GetValueOrDefault(),
                DomId = FromName.GetDomID(togglTimeEntry.TaskName),
                Housekeeping = FromProject.IfHouseKeepingReturnTaskName(togglTimeEntry.ProjectId, togglTimeEntry.TaskName),
                Description = togglTimeEntry.Description
            };

            return newTimeEntry;
        }

        private static DateTime MonthFromStart(DateTime? start)
        {
            return new DateTime(start.GetValueOrDefault().Year, start.GetValueOrDefault().Month, 1);
        }
    }
}

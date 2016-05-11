using System;
using System.Linq;
using Vincente.WebJobs.TogglToTask.Operations;

namespace Vincente.WebJobs.TogglToTask
{
    public class TogglToTaskWebJob
    {
        private TogglToTaskData togglToTaskData;

        public TogglToTaskWebJob(TogglToTaskData togglToTaskData)
        {
            this.togglToTaskData = togglToTaskData;
        }

        public void Execute()
        {
            var azureCards = togglToTaskData.azureCardTable.Query().ToList();
            var trelloLabels = togglToTaskData.trelloWorkspace.GetLabels();
            var togglProjects = togglToTaskData.togglProjectTable.GetProjects(togglToTaskData.togglClientId);
            var togglTemplate = togglToTaskData.togglProjectTable.GetProject(togglToTaskData.togglProjectTemplateId);

            

            var togglTasks = GetAllTogglTasks.Execute(togglToTaskData.togglTaskTable, togglProjects);
            Console.Out.WriteLine("{0} Toggl Tasks Returned", togglTasks.Count);

            var created = SyncTogglProjects.Execute(togglToTaskData.togglProjectTable, togglProjects, togglTemplate, trelloLabels);
            Console.Out.WriteLine("{0} Toggl Projects Created", created.Count);

            var results = UpdateAzureTasks.Execute(togglToTaskData.azureTaskTable, azureCards, togglProjects, togglTasks);
            Console.Out.WriteLine("{0} Tasks Inserted", results.Inserted);
            Console.Out.WriteLine("{0} Tasks Ignored", results.Ignored);
            Console.Out.WriteLine("{0} Tasks Replaced", results.Replaced);
            Console.Out.WriteLine("{0} Tasks Deleted", results.Deleted);
        }
    }
}

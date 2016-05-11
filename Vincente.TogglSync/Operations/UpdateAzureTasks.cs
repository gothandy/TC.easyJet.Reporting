using Gothandy.Tables.Bulk;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Gothandy.Toggl.DataObjects;
using System.Diagnostics;

namespace Vincente.WebJobs.TogglToTask.Operations
{
    internal class UpdateAzureTasks
    {

        internal static Results Execute(Azure.Tables.TaskTable azureTaskTable, List<Card> azureCards, List<Project> togglProjects, List<Gothandy.Toggl.DataObjects.Task> togglTasks)
        {
            var newTasks = GetNewTasks(azureCards, togglProjects, togglTasks);
            var oldTasks = azureTaskTable.Query().ToList();
            var bulkOperation = new Operations<Data.Entities.Task>(azureTaskTable);

            return bulkOperation.BatchCompare(oldTasks, newTasks);
        }

        private static List<Data.Entities.Task> GetNewTasks(List<Card> azureCards, List<Project> togglProjects, List<Gothandy.Toggl.DataObjects.Task> togglTasks)
        {
            var newTasks = new List<Data.Entities.Task>();
            var emptyDuplicates = new List<long>();

            foreach (Card card in azureCards)
            {
                if (card.DomId != null)
                {
                    // Ignore duplicates
                    var count = (from c in azureCards where c.DomId == card.DomId select c).Count();

                    if (count == 1)
                    {
                        emptyDuplicates.AddRange(
                                AddNewTasks(togglProjects, togglTasks, newTasks, card));
                    }
                }
            }

            

            return newTasks;
        }

        private static List<long> AddNewTasks(List<Project> togglProjects, List<Gothandy.Toggl.DataObjects.Task> togglTasks, List<Data.Entities.Task> newTasks, Card card)
        {
            var originalDomId = string.Concat(card.DomId.Substring(1), " ");
            var emptyDuplicates = new List<long>();

            var tasks =
                (from t in togglTasks
                 where t.Name.Contains(originalDomId)
                 select t).ToList();

            foreach (Gothandy.Toggl.DataObjects.Task task in tasks)
            {
                if (tasks.Count == 1 || task.TrackedSeconds.GetValueOrDefault() != 0)
                {
                    // Bring back unique tasks with no time
                    AddNewTask(togglProjects, newTasks, card, task);
                }
                else
                {
                    // Ignore duplicates duplicate tasks with no time (keep the one with time). 
                    emptyDuplicates.Add(task.Id.Value);

                    // Need to refactor so can access emptyDuplicates
                    Trace.WriteLine(task.Id.Value);
                }
            }

            return emptyDuplicates;
        }

        private static void AddNewTask(List<Project> togglProjects, List<Data.Entities.Task> newTasks, Card card, Gothandy.Toggl.DataObjects.Task task)
        {
            var project =
                (from p in togglProjects
                 where p.Id == task.ProjectId
                 select p).First();

            var newTask = new Data.Entities.Task()
            {
                ProjectId = task.ProjectId.GetValueOrDefault(),
                Id = task.Id.GetValueOrDefault(),
                Name = task.Name,
                ProjectName = project.Name,
                Active = task.Active.GetValueOrDefault(),
                TrackedSeconds = task.TrackedSeconds.GetValueOrDefault(),
                CardId = card.Id,
                DomId = card.DomId
            };

            newTasks.Add(newTask);
        }
    }
}

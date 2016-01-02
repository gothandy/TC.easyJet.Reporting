using Gothandy.Tables.Bulk;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Toggl.DataObjects;

namespace Vincente.TogglSync.Operations
{
    internal class UpdateAzureTasks
    {
        internal static Results Execute(Azure.Tables.TaskTable azureTaskTable, List<Card> azureCards, List<Project> togglProjects, List<Toggl.DataObjects.Task> togglTasks)
        {
            var newTasks = GetNewTasks(azureCards, togglProjects, togglTasks);
            var oldTasks = azureTaskTable.Query().ToList();
            var bulkOperation = new Operations<Data.Entities.Task>(azureTaskTable);

            return bulkOperation.BatchCompare(oldTasks, newTasks);
        }

        private static List<Data.Entities.Task> GetNewTasks(List<Card> azureCards, List<Project> togglProjects, List<Toggl.DataObjects.Task> togglTasks)
        {
            var newTasks = new List<Data.Entities.Task>();

            foreach (Card card in azureCards)
            {
                if (card.DomId != null)
                {
                    // Ignore duplicates
                    var count = (from c in azureCards where c.DomId == card.DomId select c).Count();

                    if (count == 1)
                    {
                        AddNewTasks(togglProjects, togglTasks, newTasks, card);
                    }
                }
            }

            return newTasks;
        }

        private static void AddNewTasks(List<Project> togglProjects, List<Toggl.DataObjects.Task> togglTasks, List<Data.Entities.Task> newTasks, Card card)
        {
            var originalDomId = string.Concat(card.DomId.Substring(1), " ");

            var tasks =
                (from t in togglTasks
                 where t.Name.Contains(originalDomId)
                 select t).ToList();

            var trackedCount =
                (from t in togglTasks
                 where t.Name.Contains(originalDomId) && t.TrackedSeconds != null
                 select t).Count();


            foreach (Toggl.DataObjects.Task task in tasks)
            {
                if (tasks.Count == 1 || task.TrackedSeconds.GetValueOrDefault() != 0)
                {
                    AddNewTask(togglProjects, newTasks, card, task);
                }
            }
        }

        private static void AddNewTask(List<Project> togglProjects, List<Data.Entities.Task> newTasks, Card card, Toggl.DataObjects.Task task)
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
                CardId = card.Id
            };

            newTasks.Add(newTask);
        }
    }
}

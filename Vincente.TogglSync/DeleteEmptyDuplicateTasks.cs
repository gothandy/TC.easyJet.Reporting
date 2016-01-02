using System;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Toggl.DataObjects;

namespace Vincente.TogglSync
{
    internal class DeleteEmptyDuplicateTasks
    {
        internal static List<string> Execute(List<Card> azureCards, List<Project> togglProjects, List<Task> togglTasks)
        {
            var deleted = new List<string>();

            foreach (Card card in azureCards)
            {
                if (card.DomId != null)
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

                    if (tasks.Count > 1 && trackedCount > 0)
                    {
                        foreach (Task task in tasks)
                        {

                            var project =
                                (from p in togglProjects
                                 where p.Id == task.ProjectId
                                 select p).First();

                            if (task.TrackedSeconds == null)
                            {
                                deleted.Add(string.Format("{0} {1} {2}", project.Name, card.DomId, card.Name));
                            }
                        }
                    }
                }
            }
            return deleted;
        }
    }
}

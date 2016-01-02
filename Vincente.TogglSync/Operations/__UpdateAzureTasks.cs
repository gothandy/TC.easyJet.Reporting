using System.Collections.Generic;
using System.Linq;
using Vincente.Azure.Tables;
using Vincente.Data.Entities;
using Vincente.Toggl.DataObjects;

namespace Vincente.TogglSync.Operations
{
    internal static class UpdateAzureTasks
    {
        internal static List<string> Execute(TaskTable taskTable, List<Card> cards, List<Toggl.DataObjects.Task> togglTasks)
        {
            var updated = new List<string>();

            if (togglTasks != null)
            {
                foreach (Card card in cards)
                {
                    if (card.DomId != null)
                    {
                        var originalDomId = string.Concat(card.DomId.Substring(1), " ");

                        var taskIds =
                            (from t in togglTasks
                             where t.Name.Contains(originalDomId) && t.TrackedSeconds.GetValueOrDefault() > 0
                             select t.Id.GetValueOrDefault()).ToList();

                        if (card.TaskIds == null) card.TaskIds = new List<long>();

                        if (!card.TaskIds.SequenceEqual(taskIds))
                        {
                            card.TaskIds = taskIds;
                            taskTable.BatchReplace(card);
                            updated.Add(string.Format("{0} {1}", card.DomId, card.Name));
                        }
                    }
                }
                taskTable.BatchComplete();
            }
            return updated;
        }
    }
}

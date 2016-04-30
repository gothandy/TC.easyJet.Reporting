using Gothandy.Trello.DataObjects;
using System.Collections.Generic;
using Vincente.Data.Entities;
using Gothandy.Trello;
using Vincente.Azure.Tables;
using System.Linq;

namespace Vincente.WebJobs.TrelloToCard
{
    public class TrelloData
    {
        private List<TrelloCard> cards;
        private List<Label> labels;
        private List<List> lists;
        private List<Replace> replaces;

        public TrelloData(Workspace trelloWorkspace, ListNameTable azureReplaceTable)
        {
            cards = trelloWorkspace.GetCards();
            labels = trelloWorkspace.GetLabels();
            lists = trelloWorkspace.GetLists();
            replaces = azureReplaceTable.Query().ToList();
        }

        public List<TrelloCard> Cards { get { return cards; } }
        public List<Label> Labels { get { return labels; } }
        public List<List> Lists { get { return lists; } }
        public List<Replace> Replaces { get { return replaces; } }
    }
}

using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.WebApp.Models
{
    /// <summary>
    /// To simplify/standardise Autofac config and Controller constructor method.
    /// </summary>
    public class ModelParameters
    {
        private ITableRead<Card> cardTable;
        private ITableRead<TimeEntry> timeEntryTable;

        public ModelParameters(ITableRead<Card> cardTable, ITableRead<TimeEntry> timeEntryTable)
        {
            this.cardTable = cardTable;
            this.timeEntryTable = timeEntryTable;
        }

        public ITableRead<Card> Card { get { return cardTable; } }
        public ITableRead<TimeEntry> TimeEntry { get { return timeEntryTable; } }

    }
}
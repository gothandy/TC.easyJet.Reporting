using System;

namespace Vincente.WebJob
{
    public class LastRunTimes
    {
        public LastRunTimes() { }

        public DateTime TogglToTask { get; set; }
        public DateTime TogglToTimeEntry { get; set; }
        public DateTime TrelloToCard { get; set; }
    }
}

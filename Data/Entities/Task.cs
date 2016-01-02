using Gothandy.Tables.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vincente.Data.Entities
{
    public class Task : ICompare<Task>
    {
        public long ProjectId { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public bool Active { get; set; }
        public long TrackedSeconds { get; set; }
        public string CardId { get; set; }

        public bool KeyEquals(Task other)
        {
            if (this.Id != other.Id) return false;

            return true;
        }

        public bool ValueEquals(Task other)
        {
            if (this.ProjectId != other.ProjectId) return false;
            if (this.Id != other.Id) return false;
            if (this.Name != other.Name) return false;
            if (this.ProjectName != other.ProjectName) return false;
            if (this.Active != other.Active) return false;
            if (this.TrackedSeconds != other.TrackedSeconds) return false;
            if (this.CardId != other.CardId) return false;

            return true;
        }
    }
}

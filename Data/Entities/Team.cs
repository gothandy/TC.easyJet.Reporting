using Gothandy.Tables.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vincente.Data.Entities
{
    public class Team : ICompare<Team>
    {
        public string UserName { get; set; }
        public string TeamName { get; set; }

        public bool ValueEquals(Team other)
        {
            if (this.UserName != other.UserName) return false;
            if (this.TeamName != other.TeamName) return false;

            return true;
        }

        public bool KeyEquals(Team other)
        {
            if (this.UserName != other.UserName) return false;

            return true;
        }
    }
}

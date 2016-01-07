using Gothandy.Tables.Interfaces;

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

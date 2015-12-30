using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vincente.Data.Entities 
{
    public class Card : IEquatable<Card>
    {
        public string Id { get; set; }
        public string DomId { get; set; }
        public int ListIndex { get; set; }
        public string ListName { get; set; }
        public string Name { get; set; }
        public string Epic { get; set; }
        public DateTime? Invoice { get; set; }
        public DateTime Timestamp { get; set; }

        public bool Equals(Card other)
        {
            if (this.Id != other.Id) return false;
            if (this.DomId != other.DomId) return false;
            if (this.ListIndex != other.ListIndex) return false;
            if (this.ListName != other.ListName) return false;
            if (this.Name != other.Name) return false;
            if (this.Epic != other.Epic) return false;
            if (this.Invoice != other.Invoice) return false;

            return true;
        }
    }
}

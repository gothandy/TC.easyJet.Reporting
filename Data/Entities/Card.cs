using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vincente.Data.Entities
{
    public class Card
    {
        public string Id { get; set; }
        public string DomId { get; set; }
        public int ListIndex { get; set; }
        public string ListName { get; set; }
        public string Name { get; set; }
        public string Epic { get; set; }
        public DateTime? Invoice { get; set; }
        public DateTime Timestamp { get; set; }

        public override bool Equals(object obj)
        {
            Card that = (Card)obj;

            if (this.Id != that.Id) return false;
            if (this.DomId != that.DomId) return false;
            if (this.ListIndex != that.ListIndex) return false;
            if (this.ListName != that.ListName) return false;
            if (this.Name != that.Name) return false;
            if (this.Epic != that.Epic) return false;
            if (this.Invoice != that.Invoice) return false;

            return true;
        }



    }
}

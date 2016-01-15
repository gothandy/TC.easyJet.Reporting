using Vincente.Data.Entities;

namespace Vincente.WebApp.Models
{
    public class TopXModel : Activity
    {
        public int? Count { get; set; }
        public int? Months { get; set; }
        public decimal? BillableBlocked { get; set; }
        public decimal? BillableWip { get; set; }
    }
}
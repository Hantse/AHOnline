using Infrastructure.Core.Persistence;
using System;

namespace Infrastructure.Core.Entities
{
    public class Alert : CoreEntity
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemNameFr { get; set; }

        public int Quantity { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public long UserId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

using Dapper.Contrib.Extensions;
using Infrastructure.Core.Persistence;

namespace Infrastructure.Core.Entities
{
    public class Auction : CoreEntity
    {
        [Key]
        public long Id { get; set; }
        public long AuctionId { get; set; }
        public string RealmName { get; set; }
        public int RealmFaction { get; set; }

        public string ItemName { get; set; }
        public string ItemNameFr { get; set; }

        public string Quality { get; set; }
        public string InventoryType { get; set; }
        public string ItemClass { get; set; }
        public string ItemSubclass { get; set; }

        public int Level { get; set; }
        public int ItemId { get; set; }
        public int ItemRand { get; set; }
        public int ItemSeed { get; set; }

        public int Bid { get; set; }
        public int Buyout { get; set; }
        public int Quantity { get; set; }

        public string TimeLeft { get; set; }
    }
}

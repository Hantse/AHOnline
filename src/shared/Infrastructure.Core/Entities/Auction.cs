using Infrastructure.Core.Persistence;
using System;

namespace Infrastructure.Core.Entities
{
    public class Auction : CoreEntity
    {
        public long AuctionId { get; set; }
        public string RealmName { get; set; }
        public int RealmFaction { get; set; }
        public string ItemName { get; set; }
        public int ItemId { get; set; }
        public int ItemRand { get; set; }
        public int ItemSeed { get; set; }
        public int Bid { get; set; }
        public int Buyout { get; set; }
        public int Quantity { get; set; }
        public string TimeLeft { get; set; }
    }
}

using Infrastructure.Core.Entities;

namespace FrontendApp.Models
{
    public class ItemDisplay
    {
        public ItemDisplay()
        {

        }

        public ItemDisplay(Auction s)
        {
            ItemName = s.ItemName;
            ItemNameFr = s.ItemNameFr;
            ItemId = s.ItemId;
            Quantity = s.Quantity;
            AuctionId = s.AuctionId;
            Bid = s.Bid;
            Buyout = s.Buyout;
            Quality = s.Quality;
            Id = s.Id;
            Level = s.Level;
            ItemClass = s.ItemClass;
            ItemRand = s.ItemRand;
            ItemSeed = s.ItemSeed;
            ItemSubclass = s.ItemSubclass;
            TimeLeft = s.TimeLeft;
        }

        public long Id { get; set; }
        public long AuctionId { get; set; }
        public string RealmName { get; set; }
        public int RealmFaction { get; set; }

        public string ItemName { get; set; }
        public string ItemNameFr { get; set; }

        public string ItemClass { get; set; }
        public string ItemSubclass { get; set; }
        public string Quality { get; set; }

        public int ItemId { get; set; }
        public int ItemRand { get; set; }
        public int ItemSeed { get; set; }

        public int Bid { get; set; }
        public int Buyout { get; set; }
        public int Level { get; set; }
        public int Quantity { get; set; }

        public string TimeLeft { get; set; }
    }
}

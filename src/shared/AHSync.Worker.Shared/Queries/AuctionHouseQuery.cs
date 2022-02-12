namespace AHSync.Worker.Shared.Queries
{
    public class AuctionHouseQuery
    {
        public string ItemName { get; set; }
        public string ItemNameFr { get; set; }

        public string Quality { get; set; }
        public string ItemClass { get; set; }
        public string ItemSubclass { get; set; }

        public string RealmName { get; set; }
        public int RealmFaction { get; set; }

        public int MinLevel { get; set; } = 0;
        public int MaxLevel { get; set; } = 70;
    }
}

INSERT INTO [Auction](AuctionId, [RealmName], [RealmFaction], [ItemName], [ItemId], [ItemRand], [ItemSeed], [Bid], [Buyout], [Quantity], [TimeLeft], CreateAt, CreateBy)
VALUES
(100, 'TestRealm', 2, 'My test item', 1000, 0, 0, 1500, 2500, 10, 'SHORT', GETUTCDATE(), 'Tester'),
(101, 'TestRealm', 2, 'My test item', 1001, 0, 0, 1500, 2500, 10, 'SHORT', GETUTCDATE(), 'Tester');
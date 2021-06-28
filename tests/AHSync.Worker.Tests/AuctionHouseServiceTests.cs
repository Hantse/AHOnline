using AHSync.Worker.Shared.Interfaces;
using AHSync.Worker.Shared.Repositories;
using AHSync.Worker.Shared.Services;
using AHSync.Worker.Tests.Fixtures;
using Blizzard.WoWClassic.ApiClient.Contracts;
using Infrastructure.Core.Persistence;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AHSync.Worker.Tests
{
    public class AuctionHouseServiceTests : IClassFixture<LocalDbFixture>
    {

        private readonly LocalDbFixture guildDatabaseFixture;
        private readonly LocalDatabaseConnectionFactory dbConnectionFactory;
        private readonly ILogger<CoreRepository<Infrastructure.Core.Entities.Auction>> logger;

        private AuctionHouseAuction mockData = new AuctionHouseAuction()
        {
            Auctions = new Auction[] {
                new Auction()
                {
                    Id = 100,
                    Bid = 10000,
                    Buyout = 15000,
                    Item = new AuctionItem()
                    {
                        Id = 1000,
                        Rand = 0,
                        Seed = 0
                    },
                    Quantity = 50,
                    TimeLeft = "LONG"
                },
                new Auction()
                {
                    Id = 200,
                    Bid = 10000,
                    Buyout = 15000,
                    Item = new AuctionItem()
                    {
                        Id = 1000,
                        Rand = 0,
                        Seed = 0
                    },
                    Quantity = 50,
                    TimeLeft = "LONG"
                },
                new Auction()
                {
                    Id = 300,
                    Bid = 10000,
                    Buyout = 15000,
                    Item = new AuctionItem()
                    {
                        Id = 1000,
                        Rand = 0,
                        Seed = 0
                    },
                    Quantity = 50,
                    TimeLeft = "LONG"
                }
            }
        };

        public AuctionHouseServiceTests(LocalDbFixture guildDatabaseFixture)
        {
            this.guildDatabaseFixture = guildDatabaseFixture;
            this.dbConnectionFactory = new LocalDatabaseConnectionFactory(guildDatabaseFixture.localDb.ConnectionString);
            this.logger = new Mock<ILogger<CoreRepository<Infrastructure.Core.Entities.Auction>>>().Object;
        }

        [Fact]
        public async Task WhenIShouldExecute_TryProcessAsync()
        {
            var wowApiServiceMock = new Mock<IWoWApiService>();
            wowApiServiceMock.Setup(s => s.GetRealmAuctionsAsync(It.IsAny<int>(), It.IsAny<int>()))
                                .ReturnsAsync(() => mockData);

            var auctionRepositoryMock = new AuctionHouseRepository(dbConnectionFactory, logger);
            var auctionHouseServiceLoggerMock = new Mock<ILogger<AuctionHouseService>>().Object;
            var auctionHouseService = new AuctionHouseService(auctionRepositoryMock, auctionHouseServiceLoggerMock, wowApiServiceMock.Object);


            var processResult = await auctionHouseService.TryProcessAsync(1, "TestRealm", 2);

            Assert.Equal(1, processResult.deleteResult);
            Assert.Equal(1, processResult.updateResult);
            Assert.Equal(2, processResult.insertResult);
        }
    }
}

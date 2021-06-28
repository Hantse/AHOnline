using Infrastructure.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AHSync.Worker.Tests.Fixtures
{
    public class LocalDbFixture : IAsyncLifetime
    {
        public readonly LocalDb localDb;

        public LocalDbFixture()
        {
            localDb = new LocalDb("LocalDbFixture");
        }

        public async Task DisposeAsync()
        {
            await localDb.Destroy();
        }

        public async Task InitializeAsync()
        {
            await localDb.Create();
            await localDb.ExecuteScript("./Scripts/V1__Auctions__Table.sql");
            await localDb.ExecuteScript("./Scripts/InsertData.sql");
        }
    }
}

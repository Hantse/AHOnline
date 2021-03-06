using AHSync.Item.Worker.Shared.Interfaces;
using AHSync.Item.Worker.Shared.Services;
using AHSync.Worker.Shared.Filters;
using AHSync.Worker.Shared.Interfaces;
using AHSync.Worker.Shared.Repositories;
using AHSync.Worker.Shared.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Infrastructure.Core.Interfaces;
using Infrastructure.Core.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AHSync.Worker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(Environment.GetEnvironmentVariable("Database_Hangfire"));

            var services = new ServiceCollection();
            var serilogLogger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddSerilog(logger: serilogLogger, dispose: true);
            });

            var builder = new ContainerBuilder();
            builder.RegisterType<SqlDbConnectionFactory>().As<IDatabaseConnectionFactory>().InstancePerDependency();
            builder.RegisterType<AuctionHouseRepository>().As<IAuctionHouseRepository>().InstancePerDependency();
            builder.RegisterType<ItemRepository>().As<IItemRepository>().InstancePerDependency();
            builder.RegisterType<OperationHistoryRepository>().As<IOperationHistoryRepository>().InstancePerDependency();
            builder.RegisterType<WoWApiService>().As<IWoWApiService>().InstancePerDependency();
            builder.RegisterType<AuctionHouseService>().As<IAuctionHouseService>().InstancePerDependency();
            builder.RegisterType<ItemSyncService>().As<IItemSyncService>().InstancePerDependency();
            builder.Populate(services);

            var options = new BackgroundJobServerOptions
            {
                Queues = new[] { Environment.GetEnvironmentVariable("QueueName") },
                WorkerCount = 1,
                Activator = new AutofacJobActivator(builder.Build(), false)
            };

            using (var server = new BackgroundJobServer(options))
            {
                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                await Task.Delay(-1);
            }
        }
    }
}

using AHSync.Item.Worker.Shared.Interfaces;
using AHSync.Item.Worker.Shared.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AHSync.Item.Worker
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("Server=tcp:51.178.185.60,1433;Initial Catalog=AH_SYNC_DATABASE;Persist Security Info=False;User ID=sa;Password=xxQb6FVes;MultipleActiveResultSets=True;Connection Timeout=30;");

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
            builder.RegisterType<ItemSyncService>().As<IItemSyncService>().InstancePerDependency();
            builder.Populate(services);

            var options = new BackgroundJobServerOptions
            {
                Queues = new[] { "item-sync" },
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

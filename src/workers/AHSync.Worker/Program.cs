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
            builder.RegisterType<AuctionHouseService>().As<IAuctionHouseService>().InstancePerDependency();
            builder.Populate(services);

            var options = new BackgroundJobServerOptions
            {
                Queues = new[] { "ah-sync" },
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

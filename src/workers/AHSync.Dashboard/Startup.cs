using AHSync.Worker.Shared.Interfaces;
using AHSync.Worker.Shared.Repositories;
using AHSync.Worker.Shared.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blizzard.WoWClassic.ApiClient;
using Blizzard.WoWClassic.ApiClient.Helpers;
using Hangfire;
using Infrastructure.Core.Interfaces;
using Infrastructure.Core.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AHSync.Dashboard
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddLogging();

            var builder = new ContainerBuilder();
            builder.RegisterType<SqlDbConnectionFactory>().As<IDatabaseConnectionFactory>().InstancePerDependency();
            builder.RegisterType<WoWApiService>().As<IWoWApiService>().InstancePerDependency();
            builder.RegisterType<AuctionHouseRepository>().As<IAuctionHouseRepository>().InstancePerDependency();
            builder.RegisterType<OperationHistoryRepository>().As<IOperationHistoryRepository>().InstancePerDependency();
            builder.RegisterType<AuctionHouseService>().As<IAuctionHouseService>().InstancePerDependency();
            builder.Populate(services);

            services.AddHangfire(x => x.UseSqlServerStorage(Environment.GetEnvironmentVariable("Database_Hangfire")));

            services.AddHangfireServer(options =>
            {
                options.ServerName = "Dashboard Host";
                options.WorkerCount = 1;
                options.ServerTimeout = TimeSpan.FromMinutes(1);
                options.Activator = new AutofacJobActivator(builder.Build(), false);
                options.Queues = new string[] { "admin", "system" };
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHangfireDashboard("");
            app.UseHttpsRedirection();

            InitJobs();
        }

        public async Task InitJobs()
        {
            var clientWow = new WoWClassicApiClient("bxSvhNNHJwI0kgNvKy6Z91oMEOpwgjmv", "2b136112d3064b11b19c5ea275846996");
            clientWow.SetDefaultValues(RegionHelper.Europe, NamespaceHelper.Dynamic, LocaleHelper.French);

            var realms = await clientWow.GetConnectedRealmsAsync();
            foreach (var realm in realms.ConnectedRealms)
            {
                var realmId = int.Parse(realm.Href.Replace("https://eu.api.blizzard.com/data/wow/connected-realm/", "")
                                        .Replace("?namespace=dynamic-classic-eu", ""));

                var realmInformations = await clientWow.GetConnectedRealmAsync(realmId);
                var realmName = realmInformations.RealmDetails[0].Name;

                RecurringJob.AddOrUpdate<IAuctionHouseService>($"{realmName}-ally", (auctionHouseService) => auctionHouseService.TryProcessAsync(realmId, realmName, 2), "*/30 * * * *", TimeZoneInfo.Utc);
                RecurringJob.AddOrUpdate<IAuctionHouseService>($"{realmName}-horde", (auctionHouseService) => auctionHouseService.TryProcessAsync(realmId, realmName, 6), "*/30 * * * *", TimeZoneInfo.Utc);
            }
        }
    }
}

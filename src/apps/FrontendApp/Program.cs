using AHSync.Worker.Shared.Interfaces;
using AHSync.Worker.Shared.Repositories;
using AHSync.Worker.Shared.Services;
using FrontendApp.Security.Discord;
using Infrastructure.Core.Interfaces;
using Infrastructure.Core.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using MudBlazor.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAuctionHouseRepository, AuctionHouseRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IDiscordSyncRepository, DiscordSyncRepository>();
builder.Services.AddScoped<IDatabaseConnectionFactory, SqlDbConnectionFactory>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = DiscordDefaults.AuthenticationScheme;
})
.AddCookie()
.AddDiscord(x =>
{
    x.AppId = builder.Configuration["Discord:AppId"];
    x.AppSecret = builder.Configuration["Discord:AppSecret"];
    x.SaveTokens = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.MapBlazorHub();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.MapFallbackToPage("/_Host");

app.Run();
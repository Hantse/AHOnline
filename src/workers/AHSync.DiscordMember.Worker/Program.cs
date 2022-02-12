// See https://aka.ms/new-console-template for more information
using Discord;
using Discord.Rest;
using Discord.WebSocket;

Console.WriteLine("Hello, World!");
var discordToken = "ODg4NDE5NzE2MDc3NTg4NTAx.YUSbVg.QjZZuVlsmKZbeSJkL__6nVbN-gc";
ulong guildId = 940995100169896006;
DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig()
{
    GatewayIntents = GatewayIntents.All
});

DiscordRestClient restClient = new DiscordRestClient();
client.GuildMembersDownloaded += Client_GuildMembersDownloaded;
client.UserJoined += Client_UserJoined;
client.UserLeft += Client_UserLeft;

await client.LoginAsync(TokenType.Bot, discordToken);
await restClient.LoginAsync(TokenType.Bot, discordToken);
await client.StartAsync();
await client.SetGameAsync("Scanning AH");

await Task.Delay(5000);
await client.GetGuild(guildId).DownloadUsersAsync();

await Task.Delay(-1);

async Task Client_GuildMembersDownloaded(SocketGuild arg)
{
    var downloadCount = client.GetGuild(guildId).DownloadedMemberCount;
    var member = client.GetGuild(guildId).GetUser(195861708248449024);
}


async Task Client_UserJoined(SocketGuildUser arg)
{

}

async Task Client_UserLeft(SocketGuild arg1, SocketUser arg2)
{

}

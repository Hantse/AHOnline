using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AHAlert.Worker
{
    class Program
    {
        public static void Main(string[] args)
                => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            var token = "NzkyNTE1MjU1NzE5NjkwMjcw.X-e1WQ.MZ5DymZep--2YVKn8Nt_C32MwQA";

            _client.Connected += Client_Connected;
            _client.GuildMembersDownloaded += Client_GuildMembersDownloaded;
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task Client_GuildMembersDownloaded(SocketGuild arg)
        {
            var user = _client.GetUser(351827485920329738);
            while (true)
            {
                await Task.Delay(1500);
                await user.SendMessageAsync("TEST");
            }
        }

        private async Task Client_Connected()
        {
            var guild = _client.GetGuild(763438478330953728);
            await guild.DownloadUsersAsync();
        }
    }
}

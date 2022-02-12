using AHSync.Worker.Shared.Interfaces;
using FrontendApp.Datas;
using FrontendApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace FrontendApp.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        public IDiscordSyncRepository DiscordSyncRepository { get; set; }

        private bool isRegisterDiscord = true;

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthenticationStateTask).User;
            isRegisterDiscord = (await DiscordSyncRepository.QueryOneAsync(new Infrastructure.Core.Entities.DiscordSync()
            {
                UserId = long.Parse(user.Claims.First(f => f.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value)
            })) != null;
        }

        private async Task OpenDiscordJoin()
        {
            await JsRuntime.InvokeAsync<object>("open", "https://discord.gg/DGNVjHKtb6", "_blank");
        }
    }
}

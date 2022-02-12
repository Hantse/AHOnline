using AHSync.Worker.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace FrontendApp.Security
{
    public class AppAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IProfileRepository profileRepository;
        private readonly IDiscordSyncRepository discordSyncRepository;

        public AppAuthStateProvider(IProfileRepository profileRepository, IDiscordSyncRepository discordSyncRepository)
        {
            this.profileRepository = profileRepository;
            this.discordSyncRepository = discordSyncRepository;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            


            throw new NotImplementedException();
        }
    }
}

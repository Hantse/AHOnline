using AHSync.Worker.Shared.Interfaces;
using AHSync.Worker.Shared.Services;
using Infrastructure.Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Stripe;

namespace FrontendApp.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        public ICurrentUserService CurrentUserService { get; set; }

        [Inject]
        public IProfileRepository ProfileRepository { get; set; }

        private MudTheme AhOnlineTheme = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = new MudBlazor.Utilities.MudColor("#333b4d"),
                TextSecondary = new MudBlazor.Utilities.MudColor("#FFF"),
                ActionDefault = new MudBlazor.Utilities.MudColor("#FFF"),
                ActionDisabled = new MudBlazor.Utilities.MudColor(255, 255, 255, 0.6),
                Surface = new MudBlazor.Utilities.MudColor("#FFF")
            }
        };

        protected override async Task OnInitializedAsync()
        {
            StripeConfiguration.ApiKey = "sk_test_51JBFXAEexqdz4HCkfKLneGoQt94twYNFhLKIrIk84CV7KUPCe8rvKHdWlRmUgeV7HuZaNih19vm35uG1M3TJTfb400cG9q8nf4";

            var user = (await AuthenticationStateTask).User;
            var profile = await ProfileRepository.QueryOneAsync(new Profile()
            {
                UserId = long.Parse(user.Claims.First(f => f.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value)
            });

            if (profile == null)
            {
                var customerService = new CustomerService();
                var cutomerOptions = new CustomerCreateOptions()
                {
                    Email = user.Claims.First(f => f.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value,
                    Metadata = new Dictionary<string, string>()
                };

                cutomerOptions.Metadata.Add("userId", user.Claims.First(f => f.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
                cutomerOptions.Metadata.Add("username", user.Claims.First(f => f.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value);
                cutomerOptions.Metadata.Add("discriminator", user.Claims.First(f => f.Type == "urn:discord:discriminator").Value);

                var createdCustomer = await customerService.CreateAsync(cutomerOptions);

                profile = new Profile()
                {
                    CreateAt = DateTime.UtcNow,
                    CreateBy = user.Claims.First(f => f.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value,
                    UserId = long.Parse(user.Claims.First(f => f.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value),
                    Email = user.Claims.First(f => f.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value,
                    Discriminator = int.Parse(user.Claims.First(f => f.Type == "urn:discord:discriminator").Value),
                    Username = user.Claims.First(f => f.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value,
                    CustomerId = createdCustomer.Id
                };

                await ProfileRepository.InsertSingleAsync(profile);
            }

            CurrentUserService.SetProfile(profile);
        }
    }
}

using AHSync.Worker.Shared.Services;
using Infrastructure.Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Globalization;
using System.Security.Claims;

namespace FrontendApp.Components.Shared
{
    public partial class TopNavBar : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public NavigationManager Nav { get; set; }

        [Inject]
        public ICurrentUserService CurrentUserService { get; set; }

        private Profile profile;
        private ClaimsPrincipal user;

        private CultureInfo Culture
        {
            get => CultureInfo.CurrentCulture;
            set
            {
                if (CultureInfo.CurrentCulture != value)
                {
                    var uri = new Uri(Nav.Uri)
                        .GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
                    var cultureEscaped = Uri.EscapeDataString(value.Name);
                    var uriEscaped = Uri.EscapeDataString(uri);
                    Nav.NavigateTo($"Culture/Set?culture={cultureEscaped}&redirectUri={uriEscaped}", forceLoad: true);
                }
            }
        }

        public async Task OnScrollTo(string id)
        {
            await JSRuntime.InvokeVoidAsync("navigateToDiv", id);
        }

        protected override async Task OnInitializedAsync()
        {
            Culture = CultureInfo.CurrentCulture;
            user = (await AuthenticationStateTask).User;
            profile = CurrentUserService.GetProfile();
        }

        public string GetAvatarUri()
        {
            var id = user.Claims.FirstOrDefault(f => f.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var avatar = user.Claims.FirstOrDefault(f => f.Type == "urn:discord:avatar")?.Value;

            return $"https://cdn.discordapp.com/avatars/{id}/{avatar}";
        }
    }
}

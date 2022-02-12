using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;

namespace FrontendApp.Pages.Account
{
    public partial class Logout : ComponentBase
    {
        [Inject]
        NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            NavigationManager.NavigateTo("/Account/Logout", true);
        }
    }
}

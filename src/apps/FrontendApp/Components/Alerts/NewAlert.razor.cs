using Infrastructure.Core.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FrontendApp.Components.Alerts
{
    public partial class NewAlert : ComponentBase
    {
        [CascadingParameter] 
        MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public Alert AlertModel { get; set; } = new Alert();
    }
}

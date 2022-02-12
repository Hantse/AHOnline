using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Stripe;
using Stripe.Checkout;

namespace FrontendApp.Components.Shared
{
    public partial class PricingButton : ComponentBase
    {

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Parameter]
        public string PriceId { get; set; }

        protected override void OnInitialized()
        {
            StripeConfiguration.ApiKey = "sk_test_51JBFXAEexqdz4HCkfKLneGoQt94twYNFhLKIrIk84CV7KUPCe8rvKHdWlRmUgeV7HuZaNih19vm35uG1M3TJTfb400cG9q8nf4";
            base.OnInitialized();
        }

        public async Task GetUriAsync()
        {
            var hostedService = new SessionService();
            var options = new SessionCreateOptions
            {
                SuccessUrl = "https://example.com/success",
                CancelUrl = "https://example.com/cancel",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                      Price = PriceId,
                      Quantity = 1
                    },
                },
                Mode = "subscription"
            };

            var createdSession = await hostedService.CreateAsync(options);
            await JsRuntime.InvokeAsync<object>("open", createdSession.Url, "_blank");
        }
    }
}

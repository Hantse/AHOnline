using Microsoft.AspNetCore.Components;

namespace FrontendApp.Components.Shared
{
    public partial class MoneyDisplay : ComponentBase
    {
        [Parameter]
        public long Value { get; set; }

        private string FormatCurrency()
        {
            string valueAsString = Value.ToString();
            if (Value < 100)
            {
                return $"{Value}<img src=\"img/money/money-copper.gif\" />";
            }
            else if (Value < 10000 && Value >= 100)
            {
                var copper = valueAsString.Substring(valueAsString.Length - 2, 2);
                var silver = valueAsString.Substring(0, valueAsString.Length - 2);
                return $"{silver}<img src=\"img/money/money-silver.gif\" /> {copper}<img src=\"img/money/money-copper.gif\" />";
            }
            else if (Value >= 10000)
            {
                var copper = valueAsString.Substring(valueAsString.Length - 2, 2);
                var silver = valueAsString.Substring(valueAsString.Length - 4, 2);
                var gold = valueAsString.Substring(0, valueAsString.Length - 4);
                return $"{gold}<img src=\"img/money/money-gold.gif\" /> {silver}<img src=\"img/money/money-silver.gif\" /> {copper}<img src=\"img/money/money-copper.gif\" />";
            }

            return $"{Value}";
        }
    }
}

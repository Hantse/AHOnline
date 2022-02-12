using AHSync.Worker.Shared.Interfaces;
using AHSync.Worker.Shared.Queries;
using AHSync.Worker.Shared.Services;
using FrontendApp.Components.Alerts;
using FrontendApp.Datas;
using FrontendApp.Models;
using Infrastructure.Core.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FrontendApp.Components.AuctionHouse
{
    public partial class AuctionHouse : ComponentBase
    {
        [Parameter]
        public int Faction { get; set; }

        [Inject]
        public ICurrentUserService CurrentUserService { get; set; }
        [Inject]
        public IDialogService DialogService { get; set; }

        [Inject]
        public IAuctionHouseRepository AuctionHouseRepository { get; set; }

        private List<CategoryModel> categories = AuctionHouseData.Categories;
        private List<ItemDisplay> items = new List<ItemDisplay>();

        private AuctionHouseQuery formQueryModel = new AuctionHouseQuery()
        {
            RealmName = "Sulfuron"
        };

        private bool isLoading;
        private string category;
        private string subCategory;

        private MudTable<ItemDisplay> table;

        private async Task ClearAndReloadAsync()
        {
            formQueryModel = new AuctionHouseQuery()
            {
                RealmFaction = Faction,
                RealmName = "Sulfuron"
            };

            await table.ReloadServerData();
        }

        private async Task ReloadDataAsync()
        {
            await table.ReloadServerData();
        }

        public string GetQualityColor(string quality)
        {
            return quality?.ToLower();
        }

        private async Task<TableData<ItemDisplay>> ServerReload(TableState state)
        {
            isLoading = true;
            StateHasChanged();
            formQueryModel.RealmFaction = Faction;
            var data = await AuctionHouseRepository.QueryFilterAsync(formQueryModel);
            var mappedData = data.Select(s => new ItemDisplay(s)).ToArray();
            mappedData = OrderBy(state, mappedData);
            mappedData = mappedData.Skip(state.Page * state.PageSize)
                                    .Take(state.PageSize)
                                    .ToArray();
            isLoading = false;
            return new TableData<ItemDisplay>() { TotalItems = data.Count(), Items = mappedData };
        }

        private static ItemDisplay[] OrderBy(TableState state, ItemDisplay[] mappedData)
        {
            switch (state.SortLabel)
            {
                case "name":
                    mappedData = mappedData.OrderByDirection(state.SortDirection, o => o.ItemName).ToArray();
                    break;

                case "quantity":
                    mappedData = mappedData.OrderByDirection(state.SortDirection, o => o.Quantity).ToArray();
                    break;

                case "level":
                    mappedData = mappedData.OrderByDirection(state.SortDirection, o => o.Level).ToArray();
                    break;

                case "timeleft":
                    mappedData = mappedData.OrderByDirection(state.SortDirection, o => o.TimeLeft).ToArray();
                    break;

                case "bid":
                    mappedData = mappedData.OrderByDirection(state.SortDirection, o => o.Bid).ToArray();
                    break;

                default:
                    break;
            }

            return mappedData;
        }

        public async Task SetCategory(string category)
        {
            Console.WriteLine(category);
        }

        public async Task SetSubCategory(string category)
        {
            Console.WriteLine(category);
        }

        public async Task AddAlert(int itemId)
        {
            var parameters = new DialogParameters { ["AlertModel"] = new Alert() };
            var options = new DialogOptions { CloseOnEscapeKey = true, CloseButton = true };
            var reference = DialogService.Show<NewAlert>("Create new alerte", parameters, options);
        }
    }
}

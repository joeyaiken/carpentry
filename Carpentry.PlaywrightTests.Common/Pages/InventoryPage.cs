#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Carpentry.PlaywrightTests.Common.Pages
{
    public class InventoryPage : NavigationPage
    {
        private readonly IPage _page;
        private readonly AppType _appType;

        public InventoryPage(string appUrl, IPage page, AppType appType) : base(page, appType, $"{appUrl}inventory", "Inventory")
        {
            _page = page;
            _appType = appType;
        }

        private async Task WaitForBusy()
        {
            await _page.WaitForSelectorAsync("#progress-bar", new PageWaitForSelectorOptions()
            {
                State = WaitForSelectorState.Hidden
            });
        }
        
        public async Task SetGroupBy(string value)
        {
            await SelectOption("group-by-filter", value);
        }
        
        public async Task SetSortBy(string value)
        {
            await SelectOption("sort-by-filter", value);
        }
        
        public async Task SetMinValue(int value)
        {
            await SetInputValue("min-count-filter", value.ToString());
        }
        
        public async Task SetTakeValue(int value)
        {
            await SetInputValue("take-filter", value.ToString());
        }

        public async Task ClickSearch()
        {
            await _page.ClickAsync("button:has-text(\"Search\")");
            await WaitForBusy();
        }

        public async Task<List<InventorySearchResult>> GetSearchResults()
        {
            var elements = await _page.QuerySelectorAllAsync(".card-result");
            return elements.Select(e => new InventorySearchResult(e, _appType)).ToList();
        }
    }

    public class InventorySearchResult
    {
        private readonly IElementHandle _element;
        private readonly AppType _appEnvironment;

        public InventorySearchResult(IElementHandle element, AppType appEnvironment)
        {
            _element = element;
            _appEnvironment = appEnvironment;
        }

        public async Task<string?> GetName()
        {
            var element = await _element.QuerySelectorAsync(".card-result-image");
            var title = await element!.GetAttributeAsync("title");
            return title;
        }

        public async Task<int?> GetTotal()
        {

            var totalStr = "";
            if (_appEnvironment == AppType.Angular)
            {
                var total = await _element.QuerySelectorAsync(".card-total");
                totalStr = await total.TextContentAsync();
            }
            else
            {
                var row = await _element.QuerySelectorAsync("tr:has(td:text-is(\"Total\"))");
                totalStr = await (await row!.QuerySelectorAllAsync("td")).Last().TextContentAsync();
            }
            if (totalStr != null && int.TryParse(totalStr, out var parsedInt))
            {
                return parsedInt;
            }
            return null;
        }
    }
}
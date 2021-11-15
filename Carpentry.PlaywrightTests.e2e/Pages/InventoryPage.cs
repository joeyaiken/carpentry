#nullable enable
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Carpentry.PlaywrightTests.e2e.Pages
{
    public class InventoryPage : NavigationPage
    {
        private readonly IPage _page;
        private readonly AppType _appEnvironment;

        public InventoryPage(string appUrl, IPage page, AppType appEnvironment) : base(page, $"{appUrl}inventory", "Inventory")
        {
            _page = page;
            _appEnvironment = appEnvironment;
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

        private async Task SetInputValue(string elementId, string value)
        {
            await _page.FillAsync($"#{elementId}", value);
        }
        
        private async Task SelectOption(string elementId, string value)
        {
            await _page.ClickAsync($"#{elementId}");
            
            if (_appEnvironment == AppType.Angular)
            {
                await Task.Delay(100);
                var selector = await _page.WaitForSelectorAsync($"mat-option:has(span:text-is(\"{value}\"))");
                await selector!.ClickAsync();
            }
            else
            {
                var selector = await _page.WaitForSelectorAsync($"li:text-is(\"{value}\")");
                await selector!.ClickAsync();
            }
        }

        public async Task ClickSearch()
        {
            await _page.ClickAsync("button:has-text(\"Search\")");
            await WaitForBusy();
        }

        public async Task<List<InventorySearchResult>> GetSearchResults()
        {
            var elements = await _page.QuerySelectorAllAsync(".card-result");
            return elements.Select(e => new InventorySearchResult(e, _appEnvironment)).ToList();
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
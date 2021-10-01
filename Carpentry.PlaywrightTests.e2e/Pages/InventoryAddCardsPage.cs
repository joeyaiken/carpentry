#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Carpentry.PlaywrightTests.e2e.Pages
{
    public class InventoryAddCardsPage
    {
        private readonly string _pageUrl;
        private readonly IPage _page;
        public InventoryAddCardsPage(string appUrl, IPage page)
        {
            _pageUrl = $"{appUrl}inventory/add-cards";
            _page = page;
        }

        public async Task NavigateTo()
        {
            if (_pageUrl == _page.Url) return;
            await _page.ClickAsync("app-nav-menu a:has-text(\"Inventory\")");
            await _page.ClickAsync("app-inventory-overview button:has-text(\"Add Cards\")");
        }
        
        public async Task WaitForBusy()
        {
            await _page.WaitForSelectorAsync("mat-progress-bar", new PageWaitForSelectorOptions()
            {
                State = WaitForSelectorState.Hidden
            });
        }
        
        public async Task ApplySetFilter(string setName)
        {
            await SelectOption("set-select", setName);
            // await _page.ClickAsync("mat-select#set-select");
            // var selector = await _page.WaitForSelectorAsync($"mat-option:has(span:text-is(\"{setName}\"))");
            // await selector.ClickAsync();
        }

        public async Task ApplySearchGroupFilter(string searchGroup)
        {
            await SelectOption("search-group-select", searchGroup);
        }

        private async Task SelectOption(string elementId, string value)
        {
            await _page.ClickAsync($"mat-select#{elementId}");
            var selector = await _page.WaitForSelectorAsync($"mat-option:has(span:text-is(\"{value}\"))");
            await selector.ClickAsync();
        }

        public async Task ClickSearch()
        {
            await _page.ClickAsync("button:has-text(\"Search\")");
            await WaitForBusy();
        }

        public async Task<SearchResultRow?> GetSearchResultByName(string cardName)
        {
            var element = await _page.QuerySelectorAsync($"div#search-results tr:has(td:text-is(\"{cardName}\"))");
            return element == null ? null : new SearchResultRow(element);
        }

        public async Task<int?> GetPendingCardCount(string cardName)
        {
            var element = await _page.QuerySelectorAsync($".pending-card:has(h5:text-is(\"{cardName}\"))");
            var value = await (await element!.QuerySelectorAsync("h6"))!.TextContentAsync();
            if (value != null && int.TryParse(value, out var parsedInt))
                return parsedInt;
            return null;
        }
        
        // public async Task<Dictionary<string, int>> GetPendingCardCounts()
        // {
        //     var elements = await _page.QuerySelectorAllAsync(".pending-card");
        //     var test = elements.Select(async element => new
        //     {
        //         Name = await (await element.QuerySelectorAsync("h5"))!.TextContentAsync(),
        //         Count = await (await element.QuerySelectorAsync("h4"))!.TextContentAsync(),
        //     }); 
        //
        //     return null;
        // }
        public async Task ClickSave()
        {
            await _page.ClickAsync("button:has-text(\"Save\")");
        }
    }

    public class SearchResultRow
    {
        private readonly IElementHandle _element;
        public SearchResultRow(IElementHandle element)
        {
            _element = element;
        }

        public async Task ClickAddButton()
        {
            //var addButton = await _element.QuerySelectorAsync("a:has-text(\"add\")");
            var addButton = await _element.QuerySelectorAsync("button:has(span:text-is(\"+\"))");
            await addButton!.ClickAsync();
        }
    }
}
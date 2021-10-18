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
        // private readonly string _pageUrl;
        private readonly IPage _page;
        private readonly AppType _appEnvironment;

        public InventoryPage(string appUrl, IPage page, AppType appEnvironment) : base(page, $"{appUrl}inventory", "Inventory")
        {
            // _pageUrl = $"{appUrl}inventory";
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
            await _page.FillAsync($"#{elementId} input", value);
        }
        
        private async Task SelectOption(string elementId, string value)
        {
            await _page.ClickAsync($"#{elementId}");
            
            if (_appEnvironment == AppType.Angular)
            {
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
            return elements.Select(e => new InventorySearchResult(e)).ToList();
        }
        
        //TODO - Consider refactoring to using properties that are <IElementHandle?> ?
        // public string? TitleText => _page.TextContentAsync("app-landing h2").Result; //TODO - this selector may not work in react
        // public string? SubtitleText => _page.TextContentAsync("app-landing h3").Result; //TODO - this selector may not work in react

        //public Task<IElementHandle?> Test()
        // public async Task Test()
        // {
        //     //var test = await _page.QuerySelectorAsync("app-landing h2");
        //     //return await _page.QuerySelectorAsync("app-landing h2");
        //     var element = await _page.QuerySelectorAsync("app-landing h2");
        //
        //     var test = await element.TextContentAsync();
        //
        //
        // }
        //
        //public async Task<string?> TitleText()
        //{
        //    return await _page.TextContentAsync("app-landing h2"); //TODO - this selector may not work in react
        //}

        //public async Task<string?> SubtitleText()
        //{
        //    return await _page.TextContentAsync("app-landing h3"); //TODO - this selector may not work in react
        //}
    }

    public class InventorySearchResult
    {
        private readonly IElementHandle _element;
        public InventorySearchResult(IElementHandle element)
        {
            _element = element;
            // Name = GetName().Result;
        }
        //card-result-image
        // public string? Name { get; set; }

        public async Task<string?> GetName()
        {
            var element = await _element.QuerySelectorAsync(".card-result-image");
            var title = await element!.GetAttributeAsync("title");
            return title;
        }

        public async Task<int?> GetTotal()
        {
            var row = await _element.QuerySelectorAsync("tr:has(td:text-is(\"Total\"))");
            var totalStr = await (await row!.QuerySelectorAllAsync("td")).Last().TextContentAsync();
            if (totalStr != null && int.TryParse(totalStr, out var parsedInt))
            {
                return parsedInt;
            }
            return null;
        }
        
        // public int? Count { get; set; }
    }
}
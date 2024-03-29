﻿#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpentry.PlaywrightTests.Common.Pages
{
    public class InventoryAddCardsPage : PageBase
    {
        private readonly string _pageUrl;
        
        public InventoryAddCardsPage(string appUrl, IPage page, AppType appEnvironment) : base(page, appEnvironment)
        {
            _pageUrl = $"{appUrl}inventory/add-cards";
        }

        public async Task NavigateTo()
        {
            if (_pageUrl == Page.Url) return;
            await Page.ClickAsync("#app-nav-menu button:has-text(\"Inventory\")");
            await Page.ClickAsync("button:has-text(\"Add Cards\")");
        }
        
        private async Task WaitForBusy()
        {
            await Page.WaitForSelectorAsync("#progress-bar", new PageWaitForSelectorOptions()
            {
                State = WaitForSelectorState.Hidden
            });
        }
        
        public async Task ApplySetFilter(string setName)
        {
            await SelectOption("set-select", setName);
        }

        public async Task ApplySearchGroupFilter(string searchGroup)
        {
            await SelectOption("search-group-select", searchGroup);
        }

        public async Task ClickSearch()
        {
            await Page.ClickAsync("button:has-text(\"Search\")");
            await WaitForBusy();
        }

        public async Task<SearchResultRow?> GetSearchResultByName(string cardName)
        {
            var element = await Page.QuerySelectorAsync($"div#search-results tr:has(td:text-is(\"{cardName}\"))");
            return element == null ? null : new SearchResultRow(cardName, element);
        }
        
        public async Task<List<SearchResultCardDetail>> GetCardDetails()
        {
            var elements = await Page.QuerySelectorAllAsync(".search-result-card");
            return elements.Select(e => new SearchResultCardDetail(e)).ToList();
        }

        public async Task<PendingCard?> GetPendingCard(string cardName)
        {
            var element = await Page.QuerySelectorAsync($".pending-card:has(h5:text-is(\"{cardName}\"))");
            return element == null ? null : new PendingCard(element);
        }
        
        public async Task<List<PendingCard>> GetAllPendingCards()
        {
            var elements = await Page.QuerySelectorAllAsync(".pending-card");
            return elements.Select(e => new PendingCard(e)).ToList();
        }
        
        public async Task ClickSave()
        {
            await Page.ClickAsync("button:has-text(\"Save\")");
        }
        public async Task ClickCancel()
        {
            await Page.ClickAsync("button:has-text(\"Cancel\")");
        }
    }

    public class SearchResultRow
    {
        private readonly string _cardName;
        private readonly IElementHandle _element;
        
        public SearchResultRow(string cardName, IElementHandle element)
        {
            _cardName = cardName;
            _element = element;
        }

        public async Task Click()
        {
            await (await _element.QuerySelectorAsync($"td:text-is(\"{_cardName}\")"))!.ClickAsync();
        }
        
        public async Task ClickAddButton()
        {
            var addButton = await _element.QuerySelectorAsync(".quick-add-button");
            await addButton!.ClickAsync();
        }

        public async Task ClickRemoveButton()
        {
            var addButton = await _element.QuerySelectorAsync(".quick-remove-button");
            await addButton!.ClickAsync();
        }
    }

    public class SearchResultCardDetail
    {
        private readonly IElementHandle _element;

        public SearchResultCardDetail(IElementHandle element)
        {
            _element = element;
        }

        public async Task ClickAddNormal()
        {
            await (await _element.QuerySelectorAsync(".add-button-normal"))!.ClickAsync();
        }
        
        public async Task ClickRemoveNormal()
        {
            await (await _element.QuerySelectorAsync(".remove-button-normal"))!.ClickAsync();
        }
        
        public async Task ClickAddFoil()
        {
            await (await _element.QuerySelectorAsync(".add-button-foil"))!.ClickAsync();
        }
        
        public async Task ClickRemoveFoil()
        {
            await (await _element.QuerySelectorAsync(".remove-button-foil"))!.ClickAsync();
        }
    }

    public class PendingCard
    {
        private readonly IElementHandle _element;

        public PendingCard(IElementHandle element)
        {
            _element = element;
        }
        
        public async Task<string> GetCardName()
        {
            var value = await (await _element.QuerySelectorAsync("h5"))!.TextContentAsync();
            Assert.IsNotNull(value);
            return value;
        }
        
        public async Task<int> GetPendingCount()
        {
            var value = await (await _element.QuerySelectorAsync("h6"))!.TextContentAsync();
            if (value != null && int.TryParse(value, out var parsedInt))
                return parsedInt;
            return 0;
        }
    }
}
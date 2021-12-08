#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Carpentry.PlaywrightTests.Common.Pages
{
    public class HomePage : PageBase
    {
        private readonly string _pageUrl;
        
        public HomePage(string appUrl, IPage page, AppType appType) : base(page, appType)
        {
            _pageUrl = $"{appUrl}";
        }

        public async Task NavigateTo()
        {
            if (_pageUrl == Page.Url) return;
            await Page.GotoAsync(_pageUrl);
        }

        public async Task<string?> GetTitleText()
        {
            return await GetElementText("#home-container #title");
        }

        public async Task<string?> GetSubtitleText()
        {
            return await GetElementText("#home-container #subtitle");
        }

        private async Task<string?> GetElementText(string selector)
        {
            var element = await Page.QuerySelectorAsync(selector);
            if (element == null) return null;
            return await element.TextContentAsync();
        }

        public async Task ClickAddDeck()
        {
            await Page.ClickAsync(".add-deck-button");
        }

        public async Task SetNewDeckTitle(string title)
        {
            await SetInputValue("deck-name-input", title);
        }

        public async Task SetNewDeckFormat(string format)
        {
            await SelectOption("deck-format-select", format);
        }

        public async Task SetNewDeckNotes(string description)
        {
            await SetInputValue("deck-notes-input", description);
        }
        
        public async Task ClickSaveNewDeck()
        {
            await Page.ClickAsync("#save-new-deck-button");
        }

        public async Task<DeckListRow> GetDeckByName(string deckName)
        {
            throw new NotImplementedException();
        }
    }

    public class DeckListRow
    {
        private readonly IElementHandle _element;

        public DeckListRow(IElementHandle element)
        {
            _element = element;
        }

        public async Task<List<string>> GetDeckColors()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetValidity()
        {
            throw new NotImplementedException();
        }
    }
}

#nullable enable

using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.PlaywrightTests.e2e.Pages
{
    public class HomePage
    {
        private readonly string _pageUrl;
        private readonly IPage _page;
        public HomePage(string appUrl, IPage page)
        {
            _pageUrl = $"{appUrl}";
            _page = page;
        }

        public async Task NavigateTo()
        {
            if (_pageUrl == _page.Url) return;
            await _page.GotoAsync(_pageUrl);
        }

        //TODO - Consider refactoring to using properties that are <IElementHandle?> ?
        // public string? TitleText => _page.TextContentAsync("app-landing h2").Result; //TODO - this selector may not work in react
        // public string? SubtitleText => _page.TextContentAsync("app-landing h3").Result; //TODO - this selector may not work in react

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
            var element = await _page.QuerySelectorAsync(selector);
            if (element == null) return null;
            return await element.TextContentAsync();
        }
    }
}

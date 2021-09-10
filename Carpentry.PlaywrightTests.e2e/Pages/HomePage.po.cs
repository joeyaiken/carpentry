
#nullable enable

using Microsoft.Playwright;
using System;
using System.Collections.Generic;
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

        public string? TitleText => _page.TextContentAsync("app-landing h2").Result; //TODO - this selector may not work in react
        public string? SubtitleText => _page.TextContentAsync("app-landing h3").Result; //TODO - this selector may not work in react

        //public async Task<string?> TitleText()
        //{
        //    return await _page.TextContentAsync("app-landing h2"); //TODO - this selector may not work in react
        //}

        //public async Task<string?> SubtitleText()
        //{
        //    return await _page.TextContentAsync("app-landing h3"); //TODO - this selector may not work in react
        //}
    }
}

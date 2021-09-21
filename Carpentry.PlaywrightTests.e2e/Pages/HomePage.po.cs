
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

        //TODO - Consider refactoring to using properties that are <IElementHandle?> ?
        public string? TitleText => _page.TextContentAsync("app-landing h2").Result; //TODO - this selector may not work in react
        public string? SubtitleText => _page.TextContentAsync("app-landing h3").Result; //TODO - this selector may not work in react

        //public Task<IElementHandle?> Test()
        public async Task Test()
        {
            //var test = await _page.QuerySelectorAsync("app-landing h2");
            //return await _page.QuerySelectorAsync("app-landing h2");
            var element = await _page.QuerySelectorAsync("app-landing h2");

            var test = await element.TextContentAsync();


        }
        
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

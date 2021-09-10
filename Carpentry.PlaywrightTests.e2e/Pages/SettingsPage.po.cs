using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.PlaywrightTests.e2e.Pages
{
    public class SettingsPage
    {
        private readonly string _pageUrl;
        private readonly IPage _page;
        public SettingsPage(string appUrl, IPage page)
        {
            _pageUrl = $"{appUrl}settings/";
            _page = page;
        }

        public async Task NavigateTo()
        {
            if (_pageUrl == _page.Url) return;

            //Click Settings nav button
            //app-nav-menu button:text(""
            //wait for the page to load
            await _page.ClickAsync("app-nav-menu a:has-text(\"Settings\")");

            //await _page.GotoAsync(_pageUrl);
        }

        //public string? TitleText => _page.TextContentAsync("app-landing h2").Result; //TODO - this selector may not work in react
        //public string? SubtitleText => _page.TextContentAsync("app-landing h3").Result; //TODO - this selector may not work in react
    }
}

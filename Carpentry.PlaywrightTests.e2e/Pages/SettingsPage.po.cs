#nullable enable
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carpentry.PlaywrightTests.e2e.Pages
{
    public class SettingsPage : NavigationPage
    {
        // private readonly string _pageUrl;
        private readonly IPage _page;
        public SettingsPage(string appUrl, IPage page) : base(page, $"{appUrl}settings/", "Settings")
        {
            // _pageUrl = $"{appUrl}settings/";
            _page = page;
        }

        // public async Task NavigateTo()
        // {
        //     if (_pageUrl == _page.Url) return;
        //     await _page.ClickAsync("#app-nav-menu a:has-text(\"Settings\")");
        // }

        public async Task WaitForBusy()
        {
            //await _page.WaitForSelectorAsync("mat-progress-bar", new PageWaitForSelectorOptions()
            await _page.WaitForSelectorAsync("#progress-bar", new PageWaitForSelectorOptions()
            {
                State = WaitForSelectorState.Hidden
            });
        }

        public async Task ClickShowUntrackedToggle()
        {
            await _page.ClickAsync("#show-untracked-toggle");
            await WaitForBusy();
        }

        public async Task ClickRefreshButton()
        {
            await _page.ClickAsync("#refresh-button");
            await WaitForBusy();
        }

        public async Task<IReadOnlyList<IElementHandle>> GetTrackedSetRows()
        {
            return await _page.QuerySelectorAllAsync(".set-row");
        }

        public async Task<TrackedSetRow?> GetRowByCode(string setCode)
        {
            var element = await _page.QuerySelectorAsync($".set-row:has(td:text-is(\"{setCode}\"))");
            return element == null ? null : new TrackedSetRow(element);
        }

        public async Task TryAddTrackedSet(string setCode)
        {
            var row = await GetRowByCode(setCode);
            Assert.IsNotNull(row);
            
            //TODO - Determine if the set is tracked or not
            //If not, add it
            //(don't worry about updating the set if it's already tracked)
            
            await row!.ClickAddButton();
            await WaitForBusy();
        }
    }

    public class TrackedSetRow
    {
        private readonly IElementHandle _element;
        public TrackedSetRow(IElementHandle element)
        {
            _element = element;
        }

        public async Task ClickAddButton()
        {
            // var addButton = await _element.QuerySelectorAsync("a:has-text(\"add\")");
            var addButton = await _element.QuerySelectorAsync(".add-button");
            await addButton!.ClickAsync();
        }
    }
}

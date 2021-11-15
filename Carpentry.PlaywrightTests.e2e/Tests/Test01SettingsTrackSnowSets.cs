using System.Threading.Tasks;
using Carpentry.PlaywrightTests.e2e.Pages;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;

namespace Carpentry.PlaywrightTests.e2e.Tests
{
    public class Test01SettingsTrackSnowSets : IRunnableTest
    {
        private readonly IPage _page;
        private readonly string _appUrl;
        private readonly ILogger _logger;
        public Test01SettingsTrackSnowSets(IPage page, string appUrl, ILogger logger)
        {
            _page = page;
            _appUrl = appUrl;
            _logger = logger;
        }
        
        public async Task Run()
        {
            var settingsPage = new SettingsPage(_appUrl, _page);
            await settingsPage.NavigateTo();
            await settingsPage.WaitForBusy();
            
            //I want this test to be able to pass even if it's already partially populated
            
            //TODO - Eventually, check if individual sets exist locally, and only add untracked
            //(for now, just assume if any sets exist, then all expected sets should exist)
            //Assert no tracked sets
            var trackedSets = await settingsPage.GetTrackedSetRows();
            if(trackedSets.Count > 0) return;
            
            //show untracked sets
            await settingsPage.ClickShowUntrackedToggle();
            
            //refresh list
            await settingsPage.ClickRefreshButton();

            //add Kaldheim, MH1, and Coldsnap
            await settingsPage.TryAddTrackedSet(nameof(SetCodes.khm));
            await settingsPage.TryAddTrackedSet(nameof(SetCodes.mh1));
            await settingsPage.TryAddTrackedSet(nameof(SetCodes.csp));

            //Hide untracked sets
            await settingsPage.ClickShowUntrackedToggle();

            //Assert 3 tracked sets (& details ?)
            var updatedTrackedSets = await settingsPage.GetTrackedSetRows();
            Assert.AreEqual(3, updatedTrackedSets.Count);

            Assert.IsNotNull(await settingsPage.GetRowByCode(nameof(SetCodes.khm)));
            Assert.IsNotNull(await settingsPage.GetRowByCode(nameof(SetCodes.mh1)));
            Assert.IsNotNull(await settingsPage.GetRowByCode(nameof(SetCodes.csp)));
            
            _logger.Information($"{nameof(Test01SettingsTrackSnowSets)} completed successfully");
        }
    }
}
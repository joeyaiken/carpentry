using System;
using System.Threading.Tasks;
using Carpentry.PlaywrightTests.e2e.Pages;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;

namespace Carpentry.PlaywrightTests.e2e.Tests
{
    public class Test00HomePageLoads : IRunnableTest
    {
        private readonly IPage _page;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;
        public Test00HomePageLoads(IPage page, IOptions<AppSettings> appSettings, ILogger logger)
        {
            _page = page;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public async Task Run()
        {
            _logger.Information($"starting {nameof(Test00HomePageLoads)}");

            var homePage = new HomePage(_appSettings.AngularUrl, _page);

            await homePage.NavigateTo();

            Assert.AreEqual("Carpentry", homePage.TitleText);

            Assert.AreEqual("A deck & inventory management tool for Magic the Gathering", homePage.SubtitleText);

            _logger.Information($"{nameof(Test00HomePageLoads)} completed successfully");
        }
    }
}
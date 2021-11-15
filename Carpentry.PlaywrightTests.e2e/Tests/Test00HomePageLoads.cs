using System.Threading.Tasks;
using Carpentry.PlaywrightTests.e2e.Pages;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;

namespace Carpentry.PlaywrightTests.e2e.Tests
{
    public class Test00HomePageLoads : IRunnableTest
    {
        private readonly IPage _page;
        private readonly string _appUrl;
        private readonly ILogger _logger;
        public Test00HomePageLoads(IPage page, string appUrl, ILogger logger)
        {
            _page = page;
            _appUrl = appUrl;
            _logger = logger;
        }

        public async Task Run()
        {
            _logger.Information($"starting {nameof(Test00HomePageLoads)}");

            var homePage = new HomePage(_appUrl, _page);

            await homePage.NavigateTo();

            Assert.AreEqual("Carpentry", await homePage.GetTitleText());

            Assert.AreEqual("A deck & inventory management tool for Magic the Gathering", await homePage.GetSubtitleText());

            _logger.Information($"{nameof(Test00HomePageLoads)} completed successfully");
        }
    }
}
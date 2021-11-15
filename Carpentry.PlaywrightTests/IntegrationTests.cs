using System.Threading.Tasks;
using Carpentry.PlaywrightTests.e2e;
using Carpentry.PlaywrightTests.e2e.Tests;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Serilog;

namespace Carpentry.PlaywrightTests
{
    [TestClass]
    public class Integration
    {


        [TestMethod]
        public async Task TestSuite_Angular_Works()
        {
            var angularUrl = "https://localhost:44335/";
            var appSettings = new AppSettings()
            {
                AngularUrl = angularUrl, 
                AppEnvironment = AppType.Angular,
                AppUrl = angularUrl
            };
            var mockLogger = new Mock<ILogger>();

            var testSuite = new TestSuite(appSettings, mockLogger.Object);

            await testSuite.RunTestSuite();
        }
        

        // //Assumes an app is actively running
        // [TestMethod]
        // public async Task Test02InventoryAddSnowCards_Works()
        // {
        //     using var playwright = await Playwright.CreateAsync();
        //     await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        //     {
        //         Headless = false,
        //         //SlowMo = 50,
        //     });
        //     var page = await browser.NewPageAsync();
        //
        //     var seedData = new SeedData();
        //     var options = Options.Create(new AppSettings() { AppUrl = "https://localhost:44335/"});
        //     //Create a Test02InventoryAddSnowCards
        //     var runnable = new Test02InventoryAddSnowCards(page, options, seedData);
        //
        //     await runnable.Run();
        // }
    }
}